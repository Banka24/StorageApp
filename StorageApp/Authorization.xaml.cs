﻿using System;
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
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName ?? string.Empty;
            return Path.Combine(projectDirectory, "zeroUser.json");
        }

        private static Worker GetZeroUser(string login, string password)
        {
            var jsonFile = File.ReadAllText(GetFullPath());
            var jsonObject = JObject.Parse(jsonFile);

            if (!jsonObject.TryGetValue("Login", out var value) || value.ToString() != login || !jsonObject.TryGetValue("Password", out var value1) || value1.ToString() != password) return null;

            var worker = new Worker
            {
                Login = login,
                Password = password,
            };

            return worker;
        }

        private static async Task<Worker> GetUserAsync(string login, string password)
        {
            using var context = new MyDbContext();

            try
            {
                var user = await context.Workers.Include(i => i.Name).SingleOrDefaultAsync(i => i.Login == login && i.Password == password);
                return user;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка, проверьте настройки подключения к сети и проверьте логи");
                await FileLogs.WriteLogAsync(ex);
            }

            return null;
        }

        private async Task CheckUserAsync()
        {
            var user = await GetUserAsync(LoginTextBox.Text, MyPassword.Password);

            if (user is not null)
            {
                SharedContext.Name = user.Name.FirstName;
                SharedContext.Role = user.RankId;
            }
            else if(GetZeroUser(LoginTextBox.Text, MyPassword.Password) is not null)
            {
                SharedContext.Role = 1;
                SharedContext.Name = "Админ";
            }
            else
            {
                MessageBox.Show("Такого пользователя нет. Проверьте логин и пароль.");
                return;
            }
            ChangeWindow();
        }

        private void ChangeWindow()
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(CheckUserAsync);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void SettingServerBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame?.Navigate(new EditServer());
        }
    }
}