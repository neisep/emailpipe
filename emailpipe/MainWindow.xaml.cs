using emailpipe.ApiRepo;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;

namespace emailpipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private StatusPage _statusPage;
        private SettingsPage _settingsPage;

        private readonly ApiRepoBase _apihelpdesk;
        private ObservableCollection<ListViewItem> _emailList = new ObservableCollection<ListViewItem>();
        public MainWindow()
        {
            InitializeComponent();

            _apihelpdesk = new OsTicket()
            {
                ApiKey1 = "4A29909545187D793821BA05A99E2F99",
                ApiPath = "http://your.tld/ticket/api/http.php/tickets.json",
            };
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Star) });

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });

            StartListen();

            _statusPage = new StatusPage();
            _statusPage.EmailListView.ItemsSource = _emailList;

            MainFrame.SetValue(Grid.ColumnProperty, 1);
            MainFrame.SetValue(Grid.RowProperty, 2);
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            _settingsPage = new SettingsPage();

            LoadStatusWindow();

            _emailList.CollectionChanged += _emailList_CollectionChanged;

            var borderCanvas = new Canvas {Background = Brushes.Orange};
            borderCanvas.Children.Add(new TextBlock { Text = "Simple email pipe for helpdesks", FontWeight = FontWeights.UltraBlack});
            
            borderCanvas.SetValue(Grid.ColumnSpanProperty, 2);

            MainGrid.Children.Add(borderCanvas);

            AddControllerButtons();
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

            var resendToIntegrationButton = new Button
            {
                Content = new TextBlock {Text = "Upload to integration", TextWrapping = TextWrapping.Wrap}
            };
            resendToIntegrationButton.SetValue(Grid.RowSpanProperty, 2);
            resendToIntegrationButton.SetValue(Grid.RowProperty, 2);
            resendToIntegrationButton.SetValue(Grid.ColumnProperty, 1);

            var clearButton = new Button {Content = "Clear list"};
            clearButton.SetValue(Grid.RowProperty, 9);
            clearButton.SetValue(Grid.ColumnProperty, 1);

            var settingsButton = new Button {Content = "Settings"};
            settingsButton.SetValue(Grid.RowProperty, 8);
            settingsButton.SetValue(Grid.ColumnProperty, 1);
            settingsButton.Click += SettingsButton_Click;

            grid.Children.Add(resendToIntegrationButton);
            grid.Children.Add(statusButton);
            //grid.Children.Add(clearButton);
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
            MainFrame.Navigate(_statusPage?.GenerateContent());
        }

        private void LoadSettingsWindow()
        {
            MainFrame.Navigate(_settingsPage?.GenerateContent());
        }

        private void StartListen()
        {
            //var imap = new Imap("localhost", 143, "test@your.tld", "blablabla", _emailList);
            //imap.StartMailManager(imap, _apihelpdesk);
            //imap.Listen();

        }
    }
}
