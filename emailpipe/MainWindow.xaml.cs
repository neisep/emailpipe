using emailpipe.ApiRepo;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace emailpipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
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

            //MainGrid.Background = Brushes.Black;

            GridView gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "Subject", Width = 250, DisplayMemberBinding = new Binding("Subject") });
            gridView.Columns.Add(new GridViewColumn { Header = "Date", Width = 80, DisplayMemberBinding = new Binding("Date") });

            EmailListView.SetValue(Grid.ColumnProperty, 1);
            EmailListView.SetValue(Grid.RowProperty, 2);
            EmailListView.View = gridView;
            EmailListView.ItemsSource = _emailList;
            StartListen();
            EmailListView.Opacity = 0.7;

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


            var refreshButton = new Button {Content = "Refresh list"};
            refreshButton.SetValue(Grid.RowProperty, 1);
            refreshButton.SetValue(Grid.ColumnProperty, 1);

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

            grid.Children.Add(resendToIntegrationButton);
            grid.Children.Add(refreshButton);
            grid.Children.Add(clearButton);
            grid.Children.Add(settingsButton);

            MainGrid.Children.Add(grid);
        }

        private void StartListen()
        {
            var imap = new Imap("localhost", 143, "test@your.tld", "blablabla", _emailList);
            imap.StartMailManager(imap, _apihelpdesk);
            imap.Listen();

        }
    }
}
