using System.Windows;
using System.Windows.Controls;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для EditServer.xaml
    /// </summary>
    public partial class EditServer : Page
    {
        public EditServer()
        {
            InitializeComponent();
        }

        private void AcButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CanellBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
