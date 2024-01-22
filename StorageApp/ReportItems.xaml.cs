using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для ReportItems.xaml
    /// </summary>
    public partial class ReportItems : Page
    {
        private enum Status : byte
        {
            Pull = 1,
            Chill,
            Ready,
            Go
        }
        public ReportItems()
        {
            InitializeComponent();
        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Reports());
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = chartpoint.ChartView as PieChart;

            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }

            var selectedSeries = chartpoint.SeriesView as PieSeries;
            selectedSeries.PushOut = 8;
        }

        private async Task SetPieValues()
        {
            double[] values = await GetPieValues();
            PieSeries[] series = { Series2,  Series3, Series4, Series5 };
            for (int i = 0; i < values.Length && i < series.Length; i++)
            {
                series[i].Values.Add(values[i]);
            }
        }

        private async Task<double[]> GetPieValues()
        {
            using var context = new MyDbContext();
            double pullItem = await context.Items.Where(i => i.StatusId == (byte)Status.Pull)?.CountAsync();
            double chilItem = await context.Items.Where(i => i.StatusId == (byte)Status.Chill)?.CountAsync();
            double readyItem = await context.Items.Where(i => i.StatusId == (byte)Status.Ready)?.CountAsync();
            double goItem = await context.Items.Where(i => i.StatusId == (byte)Status.Go)?.CountAsync();
            return [pullItem, chilItem, readyItem, goItem];
        }
        
        private void PrintPie()
        {
            PointLabel = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})";

            DataContext = this;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await SetPieValues();
            PrintPie();
        }
    }
}
