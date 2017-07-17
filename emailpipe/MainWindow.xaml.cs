using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace emailpipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ListViewItem> _emailList = new ObservableCollection<ListViewItem>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Star) });

            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });

            MainGrid.Background = Brushes.Pink;

            GridView gridView = new GridView();
            gridView.Columns.Add(new GridViewColumn { Header = "Subject", Width = 250, DisplayMemberBinding = new Binding("Subject") });
            gridView.Columns.Add(new GridViewColumn { Header = "Date", Width = 80, DisplayMemberBinding = new Binding("Date") });

            EmailListView.SetValue(Grid.ColumnProperty, 1);
            EmailListView.SetValue(Grid.RowProperty, 2);
            EmailListView.View = gridView;
            EmailListView.ItemsSource = _emailList;
            StartListen();
            EmailListView.Opacity = 0.7;

            Canvas test = new Canvas();
            test.Background = Brushes.Orange;
            test.Children.Add(new TextBlock { Text = "Simple email pipe for helpdesks", FontWeight = FontWeights.UltraBlack});
            
            test.SetValue(Grid.ColumnSpanProperty, 2);

            MainGrid.Children.Add(test);

            AddControllerButtons();
        }

        private void AddControllerButtons()
        {
            Grid grid = new Grid();
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

            Button resendToIntegrationButton = new Button();
            resendToIntegrationButton.Content = new TextBlock { Text = "Upload to integration", TextWrapping = TextWrapping.Wrap };
            resendToIntegrationButton.SetValue(Grid.RowSpanProperty, 2);
            resendToIntegrationButton.SetValue(Grid.RowProperty, 2);

            Button clearButton = new Button();
            clearButton.Content = "Clear list";
            clearButton.SetValue(Grid.RowProperty, 9);

            Button settingsButton = new Button();
            settingsButton.Content = "Settings";
            settingsButton.SetValue(Grid.RowProperty, 8);

            grid.Children.Add(resendToIntegrationButton);
            grid.Children.Add(refreshButton);
            grid.Children.Add(clearButton);
            grid.Children.Add(settingsButton);

            MainGrid.Children.Add(grid);
        }

        private void StartListen()
        {
            imap imap = new imap("localhost", 143, "test@localhost.com", "password", _emailList);
            imap.Listen();
        }

        
         
    }
}
