using emailpipe.ApiRepo;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using emailpipe.Models;

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

        private UIElement _activeUiElement;

        private ApiRepoBase _apihelpdesk;
        private ObservableCollection<ListViewItem> _emailList = new ObservableCollection<ListViewItem>();
        public MainWindow()
        {
            InitializeComponent();

            CreateKeyFile();
            LoadKeyFile();
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

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });

            

            _statusPage = new StatusPage {EmailListView = {ItemsSource = _emailList}};

            _settingsPage = new SettingsPage(_key);

            if (_settingsPage.Settings != null)
                LoadApi();

            StartListen();

            LoadStatusWindow();

            _emailList.CollectionChanged += _emailList_CollectionChanged;

            var borderCanvas = new Canvas {Background = Brushes.Orange};
            borderCanvas.Children.Add(new TextBlock { Text = "Simple email pipe for helpdesks", FontWeight = FontWeights.UltraBlack});
            
            borderCanvas.SetValue(Grid.ColumnSpanProperty, 2);

            MainGrid.Children.Add(borderCanvas);

            AddControllerButtons();
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

                            _apihelpdesk?.AddnewTicket((MimeMessage)listViewItem.Tag);
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
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });

            grid.SetValue(Grid.RowProperty, 2);

            var statusButton = new Button {Content = "Status"};
            statusButton.SetValue(Grid.RowProperty, 1);
            statusButton.SetValue(Grid.ColumnProperty, 1);
            statusButton.Click += StatusButton_Click;

            var settingsButton = new Button {Content = "Settings"};
            settingsButton.SetValue(Grid.RowProperty, 8);
            settingsButton.SetValue(Grid.ColumnProperty, 1);
            settingsButton.Click += SettingsButton_Click;

            grid.Children.Add(statusButton);
            grid.Children.Add(settingsButton);

            MainGrid.Children.Add(grid);
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
            if (_settingsPage.Settings != null)
            {
                var imap = new Imap(_settingsPage.Settings.EmailServerAdress, 143, AESGCM.SimpleDecrypt(_settingsPage.Settings.Email, _key), AESGCM.SimpleDecrypt(_settingsPage.Settings.Password, _key), _emailList);
                imap.StartMailManager(imap, _apihelpdesk);
                imap.Listen();
            }
        }
    }
}
