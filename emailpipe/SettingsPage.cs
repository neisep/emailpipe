using emailpipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace emailpipe
{
    public class SettingsPage : IPage
    {
        private Grid _mainGrid;
        private Settings _settings;

        private ComboBox _apiselectComboBox;
        private TextBox _txtBoxApiKey;
        private TextBox _txtBoxApiAdress;
        private TextBox _txtBoxMailadress;
        private TextBox _txtBoxMailPassword;
        private TextBox _txtBoxMailServerAdress;

        public SettingsPage()
        {
            _mainGrid = new Grid();
            _mainGrid.SetValue(Grid.ColumnProperty, 1);
            _mainGrid.SetValue(Grid.RowProperty, 2);
            _settings = new Settings();
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
            _apiselectComboBox.Items.Add("OsTicket");

            var txtApiKeyLabel = new Label { Content = "Api key:" };
            txtApiKeyLabel.SetValue(Grid.ColumnProperty, 1);
            txtApiKeyLabel.SetValue(Grid.RowProperty, 1);
            _txtBoxApiKey = new TextBox();
            _txtBoxApiKey.SetValue(Grid.ColumnProperty, 2);
            _txtBoxApiKey.SetValue(Grid.RowProperty, 1);

            var txtApiAdressLabel = new Label { Content = "Adress:" };
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

            return _mainGrid;
        }

        public void SaveToFile()
        {
            if (string.IsNullOrEmpty(_settings.ApiAdress) || string.IsNullOrEmpty(_settings.ApiKey))
            {
                _settings.ApiAdress = _txtBoxApiAdress.Text;
                _settings.ApiKey = _txtBoxApiKey.Text;
                _settings.Email = _txtBoxMailadress.Text;
                _settings.Password = _txtBoxMailPassword.Text;
                _settings.EmailServerAdress = _txtBoxMailServerAdress.Text;
            }

            var json = JsonConvert.SerializeObject(_settings);
        }

        public void LoadFromFile()
        {
            
        }
    }
}
