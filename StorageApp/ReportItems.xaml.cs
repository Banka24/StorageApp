using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Linq;
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

            PointLabel = chartPoint =>
            string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;

        }

        private void BackClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Reports());
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (PieChart)chartpoint.ChartView;

            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void SetValues(int pullItem, int chilItem, int readyItem, int goItem)
        {
            Series2.Values.Add(Convert.ToDouble(pullItem));
            Series3.Values.Add(Convert.ToDouble(chilItem));
            Series4.Values.Add(Convert.ToDouble(readyItem));
            Series5.Values.Add(Convert.ToDouble(goItem));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                var pullItem = context.Items.Where(i => i.StatusId == (byte)Status.Pull)?.Count() ?? 0;
                var chilItem = context.Items.Where(i => i.StatusId == (byte)Status.Chill)?.Count() ?? 0;
                var readyItem = context.Items.Where(i => i.StatusId == (byte)Status.Ready)?.Count() ?? 0;
                var goItem = context.Items.Where(i => i.StatusId == (byte)Status.Go)?.Count() ?? 0;
                SetValues(pullItem, chilItem, readyItem, goItem);
            }
        }
    }
}
