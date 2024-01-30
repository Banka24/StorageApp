﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Windows.Controls;

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
                await FileLogs.WriteLog(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textName.Text = $"Добрый день, {SharedContext.Name}";
            switch (SharedContext.Role)
            {
                case (int)Role.Administrator:
                    Button[] adminButtons = [BtnInfAdmin, BtnGoAdmin, RegistrationBtn, SettingServerBtn];
                    foreach (var button in adminButtons)
                    {
                        button.Visibility = Visibility.Visible;
                    }
                    break;

                case (int)Role.Worker:
                    Button[] workerButtons = [BtnInfIt, BtnGoWork];
                    foreach (var button in workerButtons)
                    {
                        button.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Registration());
        }

        private void SettingServerBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EditServer());
        }
    }
}
