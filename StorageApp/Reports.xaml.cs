using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void BtnIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportItems());
        }

        private void BtnWo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportWorker());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
