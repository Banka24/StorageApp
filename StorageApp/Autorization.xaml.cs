using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;

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
                var user = context.Workers.Include(i => i.Name).SingleOrDefault(i => i.Login == login && i.Password == password);
                if (user is Worker)
                {
                    SharedContext.Name = user.Name.FirstName;
                    SharedContext.Role = user.RankId;
                    ChangeWindow();
                }
                else
                {
                    MessageBox.Show("Такого пользователя нет. Проверьте логин и пароль.");
                }
                return user;

            }
        }

        private void ChangeWindow()
        {
            var window = new MainWindow();
            window.Show();
            Close();
            
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = MyPassword.Password;
            CheckUser(login, password);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
