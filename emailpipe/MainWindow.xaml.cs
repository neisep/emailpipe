//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Emailpipe.Common.Models;
using Emailpipe.Common;
using Emailpipe.Api.Interfaces;
using Emailpipe.Api;

namespace emailpipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private byte[] _key;
        private StatusPage _statusPage;
        private SettingsPage _settingsPage;
        private StringBuilder _statusText;
        private bool _listen;

        private UIElement _activeUiElement;

        private Iapi _apihelpdesk;
        private ObservableCollection<ListViewItem> _emailList = new ObservableCollection<ListViewItem>();
        public MainWindow()
        {
            InitializeComponent();

            CreateKeyFile();
            LoadKeyFile();
            _statusText = new StringBuilder();
            _statusText.Append("Status: Not connected");
        }

        private void CreateKeyFile()
        {
            if(!File.Exists("ashibashi.nei"))
                File.WriteAllBytes("ashibashi.nei", AESGCM.NewKey());
        }

        private void LoadKeyFile()
        {
            if (File.Exists("ashibashi.nei"))
                _key = File.ReadAllBytes("ashibashi.nei");
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });

            

            _statusPage = new StatusPage {EmailListView = {ItemsSource = _emailList}};

            _settingsPage = new SettingsPage(_key);

            if (_settingsPage.Settings != null)
                LoadApi();

            LoadStatusWindow();

            _emailList.CollectionChanged += _emailList_CollectionChanged;

            var borderCanvas = new Canvas {Background = Brushes.Orange};
            borderCanvas.Children.Add(new TextBlock { Text = "Simple email pipe for helpdesks", FontWeight = FontWeights.UltraBlack});
            
            borderCanvas.SetValue(Grid.ColumnSpanProperty, 2);

            MainGrid.Children.Add(borderCanvas);

            AddControllerButtons();

            var statusText = new TextBlock();
            statusText.SetValue(Grid.RowProperty, 3);
            statusText.VerticalAlignment = VerticalAlignment.Bottom;

            MainGrid.Children.Add(statusText);
        }

        private void LoadApi()
        {
            switch (_settingsPage.Settings.Type)
                {
                    case Settings.ApiTypes.osTicket:
                    {
                        _apihelpdesk = new OsTicket
                        {
                            ApiAdress = _settingsPage.Settings.ApiAdress,
                            ApiKey1 = _settingsPage.Settings.ApiKey
                        };
                    }
                        break;
                    case Settings.ApiTypes.katak:
                        throw new NotImplementedException("Implementation for Katak Support is missing!");
                        //break;
                }
        }

        private void _emailList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in e.NewItems)
                        {
                            if (item != null && !(item is ListViewItem))
                                return;

                            var listViewItem = (ListViewItem)item;

                            if (!(listViewItem?.Tag is MimeMessage))
                                return;

                            //var message = MailMessage((MimeMessage)listViewItem.Tag);
                            var message = new MailMessage((MimeMessage)listViewItem.Tag);


                            _apihelpdesk?.AddnewTicket(message);
                        }
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void AddControllerButtons()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });

            grid.SetValue(Grid.RowProperty, 2);

            TextBlock startText = new TextBlock();
            startText.FontFamily = FindResource("FontAwesome") as FontFamily;
            startText.Text = "\uf011 Start";

            var startButton = new Button {Content = startText };
            startButton.SetValue(Grid.RowProperty, 0);
            startButton.SetValue(Grid.ColumnProperty, 1);
            startButton.Click += StartButton_Click;

            TextBlock statusText = new TextBlock();
            statusText.FontFamily = FindResource("FontAwesome") as FontFamily;
            statusText.Text = "\uf0ae Status";

            var statusButton = new Button {Content = statusText};
            statusButton.SetValue(Grid.RowProperty, 2);
            statusButton.SetValue(Grid.ColumnProperty, 1);
            statusButton.Click += StatusButton_Click;

            //< TextBlock x: Name = "tbFontAwesome" Text = "&#xf011;" FontFamily = "{StaticResource FontAwesome}" Foreground = "Gray" FontSize = "32" Margin = "10" VerticalAlignment = "Center" />
            //              f085
            TextBlock cog = new TextBlock();
            cog.FontFamily = FindResource("FontAwesome") as FontFamily;
            cog.Text = "\uf085 Settings";

            var settingsButton = new Button {Content = cog };
            settingsButton.SetValue(Grid.RowProperty, 8);
            settingsButton.SetValue(Grid.ColumnProperty, 1);
            settingsButton.Click += SettingsButton_Click;

            grid.Children.Add(startButton);
            grid.Children.Add(statusButton);
            grid.Children.Add(settingsButton);

            MainGrid.Children.Add(grid);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartListen();
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            LoadStatusWindow();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSettingsWindow();
        }

        private void LoadStatusWindow()
        {
            RemoveActiveElement();

            _activeUiElement = _statusPage.GenerateContent();
            MainGrid.Children.Add(_activeUiElement);
        }

        private void LoadSettingsWindow()
        {
            RemoveActiveElement();

            _activeUiElement = _settingsPage.GenerateContent();
            MainGrid.Children.Add(_activeUiElement);
        }

        private void RemoveActiveElement()
        {
            if (_activeUiElement != null)
                MainGrid.Children.Remove(_activeUiElement);
        }

        private void StartListen()
        {
            if (_settingsPage.Settings != null && !_listen)
            {
                var imap = new Imap(_settingsPage.Settings.EmailServerAdress, 143, AESGCM.SimpleDecrypt(_settingsPage.Settings.Email, _key), AESGCM.SimpleDecrypt(_settingsPage.Settings.Password, _key), _emailList);
                imap.StartMailManager(imap, _apihelpdesk);
                imap.Listen(_statusText);
                _listen = true;
            }
        }
    }
}
