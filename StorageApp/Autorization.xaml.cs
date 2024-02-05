using System;
using System.Windows;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization
    {
      
        public Authorization()
        {
            InitializeComponent();
        }

        private static string GetFullPath()
        {
            const string jsonFilePath = "zeroUser.json";
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName ?? string.Empty;
            var fullPath = Path.Combine(projectDirectory, jsonFilePath);
            return fullPath;
        }

        private static Worker CheckZeroUser(string login, string password)
        {
            var fullFilePath = GetFullPath();
            var jsonFile = File.ReadAllText(fullFilePath);
            var jsonObject = JObject.Parse(jsonFile);

            if (!jsonObject.TryGetValue("Login", out var value) || value.ToString() != login || !jsonObject.TryGetValue("Password", out var value1) || value1.ToString() != password) return null;

            var worker = new Worker
            {
                Login = login,
                Password = password,
            };
            return worker;

        }

        private static async Task CheckUser(string login, string password)
        {
            var context = new MyDbContext();
            Worker user = null;
            try
            {
                user = await context.Workers.Include(i => i.Name).SingleOrDefaultAsync(i => i.Login == login && i.Password == password);
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Произошла ошибка, проверьте настройки подключения к сети и проверьте логи");
                await FileLogs.WriteLog(ex);
            }

            if (user is not null)
            {
                SharedContext.Name = user.Name.FirstName;
                SharedContext.Role = user.RankId;
            }
            else
            {
                user = CheckZeroUser(login, password);

                if (user is not null)
                {
                    SharedContext.Role = 1;
                    SharedContext.Name = "Админ";
                }
                else
                {
                    MessageBox.Show("Такого пользователя нет. Проверьте логин и пароль.");
                }
            }
        }

        private void ChangeWindow()
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text;
            var password = MyPassword.Password;
            await Task.Run(async () => await CheckUser(login, password));
            if (SharedContext.Role != 0)
            {
                ChangeWindow();
            }
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
