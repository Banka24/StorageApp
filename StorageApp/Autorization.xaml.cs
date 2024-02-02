using System;
using System.Windows;
using System.Data.Entity;
using System.Threading.Tasks;

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

        private async Task<Worker> CheckUser(string login, string password)
        {
            var context = new MyDbContext();
            Worker user = null;
            try
            {
                user = await context.Workers.Include(i => i.Name)?.SingleOrDefaultAsync(i => i.Login == login && i.Password == password);
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Произошла ошибка, проверьте логи");
                await FileLogs.WriteLog(ex);
            }
            if (user is not null)
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

        private void ChangeWindow()
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = MyPassword.Password;
            await CheckUser(login, password);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SettingServerBtn_Click(object sender, RoutedEventArgs e)
        {
            Content = new EditServer();
            Show();
        }
    }
}
