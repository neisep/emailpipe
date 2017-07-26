using emailpipe.ApiRepo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace emailpipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private katak _katakApi;
        private OsTicket _osTicket;
        private ObservableCollection<ListViewItem> _emailList = new ObservableCollection<ListViewItem>();
        public MainWindow()
        {
            InitializeComponent();

            //_katakApi = new katak();
            //_katakApi.ApiPath = "http://your.tld/ticket/api/tickets.email";
            //_katakApi.ApiKey1 = "01E0900582B8215CFB6230AF05B27BDA";

            _osTicket = new OsTicket();
            _osTicket.ApiKey1 = "4A29909545187D793821BA05A99E2F99";
            _osTicket.ApiPath = "http://your.tld/ticket/api/http.php/tickets.json";

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

            Canvas borderCanvas = new Canvas();
            borderCanvas.Background = Brushes.Orange;
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

                            if (!(item is ListViewItem))
                                return;

                            ListViewItem listViewItem = (ListViewItem)item;

                            if (!(listViewItem.Tag is MailMessage))
                                return;

                            if (_katakApi != null)
                                _katakApi.AddnewTicket((MailMessage)listViewItem.Tag);
                            else if (_osTicket != null)
                                _osTicket.AddnewTicket((MailMessage)listViewItem.Tag);
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
                default:
                    break;
            }
        }

        private void AddControllerButtons()
        {
            Grid grid = new Grid();
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


            Button refreshButton = new Button();
            refreshButton.Content = "Refresh list";
            refreshButton.SetValue(Grid.RowProperty, 1);
            refreshButton.SetValue(Grid.ColumnProperty, 1);

            Button resendToIntegrationButton = new Button();
            resendToIntegrationButton.Content = new TextBlock { Text = "Upload to integration", TextWrapping = TextWrapping.Wrap };
            resendToIntegrationButton.SetValue(Grid.RowSpanProperty, 2);
            resendToIntegrationButton.SetValue(Grid.RowProperty, 2);
            resendToIntegrationButton.SetValue(Grid.ColumnProperty, 1);

            Button clearButton = new Button();
            clearButton.Content = "Clear list";
            clearButton.SetValue(Grid.RowProperty, 9);
            clearButton.SetValue(Grid.ColumnProperty, 1);

            Button settingsButton = new Button();
            settingsButton.Content = "Settings";
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
            imap imap = new imap("localhost", 143, "test@your.tld", "blablabla", _emailList);
            imap.Listen();

        }
    }
}
