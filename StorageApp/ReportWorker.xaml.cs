using System.Windows;
using System.Data.Entity;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для ReportWorker.xaml
    /// </summary>
    public partial class ReportWorker
    {
        public ReportWorker()
        {
            InitializeComponent();
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Reports());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            var workers = await context.Workers?.Include(i => i.Name).Include(i => i.Rank).ToListAsync()!;
            ListWorkers.ItemsSource = workers;
        }

        private void UpdatePage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Refresh();
        }
    }
}
