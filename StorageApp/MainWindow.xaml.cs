using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Role
        {
            Administrator = 1,
            Supervision,
            Worker,
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnInfIt_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InfoItem());
        }

        private void BtnInfIt_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Editor());
        }

        private void BtnGoAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Reports());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var window = new Autorization();
            window.Show();
            Close();
        }

        private async Task<Worker> GetWorker(MyDbContext context)
        {
            return await context.Workers?.Where(i => i.Name.FirstName == SharedContext.Name)?.SingleOrDefaultAsync();
        }

        private async Task<Worker> StartWorkShift(MyDbContext context)
        {
            var worker = await GetWorker(context);

            if (worker == null)
            {
                return null;
            }

            if (worker.OnWork == "YES")
            {
                MessageBox.Show("Вы закончили смену");
                worker.OnWork = "NO";
            }
            else
            {
                MessageBox.Show("Вы начали смену");
                worker.OnWork = "YES";
            }
            return worker;
        }

        private async void BtnGoWork_Click(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            var worker = await StartWorkShift(context);
            try
            {
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textName.Text = $"Добрый день, {SharedContext.Name}";
            switch (SharedContext.Role)
            {
                case (int)Role.Administrator:
                    BtnInfAdmin.Visibility = Visibility.Visible;
                    BtnGoAdmin.Visibility = Visibility.Visible;
                    break;
                case (int)Role.Worker:
                    BtnInfIt.Visibility = Visibility.Visible;
                    BtnGoWork.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
