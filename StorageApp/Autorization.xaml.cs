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
using System.Windows.Shapes;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
      
        public Autorization()
        {
            InitializeComponent();
        }
        private Worker CheckUser(string login, string password)
        {
            using (var context = new MyDbContext())
            {
                var user = context.Workers.SingleOrDefault(i => i.Login == login && i.Password == password);
                return user;
            }

        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = MyPassword.Password;
            var worker = CheckUser(login, password);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
