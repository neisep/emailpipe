using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Emailpipe.Common.Models;
using Emailpipe.Crypto;

namespace emailpipe
{
    public class SettingsPage : IPage
    {
        private Grid _mainGrid;
        public Settings Settings { get; private set; }
        private byte[] _key;

        private ComboBox _apiselectComboBox;
        private TextBox _txtBoxApiKey;
        private TextBox _txtBoxApiAdress;
        private TextBox _txtBoxMailadress;
        private TextBox _txtBoxMailPassword;
        private TextBox _txtBoxMailServerAdress;

        public SettingsPage(byte[] keyBytes)
        {
            _key = keyBytes;
            _mainGrid = new Grid();
            _mainGrid.SetValue(Grid.ColumnProperty, 1);
            _mainGrid.SetValue(Grid.RowProperty, 2);
            Settings = new Settings();
            LoadFromFile();
        }

        public FrameworkElement GenerateContent()
        {
            if (_mainGrid.Children.Count > 0)
                return _mainGrid;

            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            _mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (var i = 0; i < 10; i++)
            {
                _mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            }

            var txtApiModeLabel = new Label {Content = "Api mode:"};
            txtApiModeLabel.SetValue(Grid.ColumnProperty, 1);

            _apiselectComboBox = new ComboBox();
            _apiselectComboBox.SetValue(Grid.ColumnProperty, 2);
            _apiselectComboBox.ItemsSource = Enum.GetValues(typeof(Settings.ApiTypes)).Cast<Settings.ApiTypes>();

            var txtApiKeyLabel = new Label { Content = "Api key:" };
            txtApiKeyLabel.SetValue(Grid.ColumnProperty, 1);
            txtApiKeyLabel.SetValue(Grid.RowProperty, 1);
            _txtBoxApiKey = new TextBox();
            _txtBoxApiKey.SetValue(Grid.ColumnProperty, 2);
            _txtBoxApiKey.SetValue(Grid.RowProperty, 1);

            var txtApiAdressLabel = new Label { Content = "Adress:(=>tickets.json)" };
            txtApiAdressLabel.SetValue(Grid.ColumnProperty, 1);
            txtApiAdressLabel.SetValue(Grid.RowProperty, 2);
            _txtBoxApiAdress = new TextBox();
            _txtBoxApiAdress.SetValue(Grid.ColumnProperty, 2);
            _txtBoxApiAdress.SetValue(Grid.RowProperty, 2);

            var txtMailAdressLabel = new Label { Content = "Email adress:" };
            txtMailAdressLabel.SetValue(Grid.ColumnProperty, 1);
            txtMailAdressLabel.SetValue(Grid.RowProperty, 4);
            _txtBoxMailadress = new TextBox();
            _txtBoxMailadress.SetValue(Grid.ColumnProperty, 2);
            _txtBoxMailadress.SetValue(Grid.RowProperty, 4);

            var txtMailPasswordLabel = new Label { Content = "Password:" };
            txtMailPasswordLabel.SetValue(Grid.ColumnProperty, 1);
            txtMailPasswordLabel.SetValue(Grid.RowProperty, 5);
            _txtBoxMailPassword = new TextBox();
            _txtBoxMailPassword.SetValue(Grid.ColumnProperty, 2);
            _txtBoxMailPassword.SetValue(Grid.RowProperty, 5);

            var txtMailServerLabel = new Label { Content = "Mail server adress:" };
            txtMailServerLabel.SetValue(Grid.ColumnProperty, 1);
            txtMailServerLabel.SetValue(Grid.RowProperty, 6);
            _txtBoxMailServerAdress = new TextBox();
            _txtBoxMailServerAdress.SetValue(Grid.ColumnProperty, 2);
            _txtBoxMailServerAdress.SetValue(Grid.RowProperty, 6);

            var saveSettingsButton = new Button {Content = "Save settings"};
            saveSettingsButton.SetValue(Grid.ColumnProperty, 2);
            saveSettingsButton.SetValue(Grid.RowProperty, 8);
            saveSettingsButton.HorizontalAlignment = HorizontalAlignment.Right;
            saveSettingsButton.MinWidth = 100;
            saveSettingsButton.Click += SaveSettingsButton_Click;

            _mainGrid.Children.Add(txtApiModeLabel);
            _mainGrid.Children.Add(_apiselectComboBox);
            _mainGrid.Children.Add(txtApiKeyLabel);
            _mainGrid.Children.Add(_txtBoxApiKey);
            _mainGrid.Children.Add(txtApiAdressLabel);
            _mainGrid.Children.Add(_txtBoxApiAdress);
            _mainGrid.Children.Add(txtMailAdressLabel);
            _mainGrid.Children.Add(_txtBoxMailadress);
            _mainGrid.Children.Add(txtMailPasswordLabel);
            _mainGrid.Children.Add(_txtBoxMailPassword);
            _mainGrid.Children.Add(txtMailServerLabel);
            _mainGrid.Children.Add(_txtBoxMailServerAdress);
            _mainGrid.Children.Add(saveSettingsButton);

            PopulateDataFields();

            return _mainGrid;
        }

        private void PopulateDataFields()
        {
            try
            {
                if (Settings == null)
                    return;

                _apiselectComboBox.SelectedIndex = Convert.ToInt32(Settings.Type);

                _txtBoxApiAdress.Text = Settings.ApiAdress;
                _txtBoxApiKey.Text = Settings.ApiKey;
                _txtBoxMailadress.Text = AESGCM.SimpleDecrypt(Settings.Email, _key);
                _txtBoxMailPassword.Text = AESGCM.SimpleDecrypt(Settings.Password, _key);
                _txtBoxMailServerAdress.Text = Settings.EmailServerAdress;
            }
            catch (Exception)
            {
                
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        public void SaveToFile()
        {
            try
            {

                Settings.ApiAdress = _txtBoxApiAdress.Text;
                Settings.ApiKey = _txtBoxApiKey.Text;
                Settings.Email = AESGCM.SimpleEncrypt(_txtBoxMailadress.Text, _key);
                Settings.Password = AESGCM.SimpleEncrypt(_txtBoxMailPassword.Text, _key);
                Settings.EmailServerAdress = _txtBoxMailServerAdress.Text;
                Settings.Type = (Settings.ApiTypes) Enum.Parse(typeof(Settings.ApiTypes),
                    Enum.GetName(typeof(Settings.ApiTypes), _apiselectComboBox.SelectedIndex));

                var json = JsonConvert.SerializeObject(Settings);

                File.WriteAllText("settings.json", json);
            }
            catch (Exception)
            {
                
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists("settings.json"))
                return;

            var textFile = File.ReadAllText("settings.json");
            var json = JsonConvert.DeserializeObject<Settings>(textFile);

            Settings = json;
        }
    }
}
