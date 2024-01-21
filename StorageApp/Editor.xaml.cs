using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : Page
    {
        public Editor()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddItem());
        }

        private void BtnEd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditItem());
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DeleteItem());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
