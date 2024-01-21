using LiveCharts;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using System.Linq;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для ReportWorker.xaml
    /// </summary>
    public partial class ReportWorker : Page
    {
        public ReportWorker()
        {
            InitializeComponent();
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Reports());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            var workers = context.Workers.Include(i => i.Name).Include(i => i.Rank).ToList();
            ListWorkers.ItemsSource = workers;
        }

        private void UpdatePage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }
    }
}
