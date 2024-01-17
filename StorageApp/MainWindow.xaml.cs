using System;
using System.Collections.Generic;
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

        public MainWindow(string name, int role) : this()
        {
            textName.Text = $"Добрый день, {name}";
            switch (role)
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
    }
}
