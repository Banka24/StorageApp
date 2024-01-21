using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;

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

        private void BtnGoWork_Click(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            var worker = context.Workers.Where(i => i.Name.FirstName == SharedContext.Name).SingleOrDefault();
            if(worker == null)
            {
                return;
            }
            if(worker.OnWork == "YES") 
            {
                MessageBox.Show("Вы закончили смену");
                worker.OnWork = "NO";
                return;
            }

            MessageBox.Show("Вы начали смену");
            worker.OnWork = "YES";

            context.SaveChanges();
        }
    }
}
