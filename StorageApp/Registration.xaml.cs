using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private string[] CheckData(in string[] strings)
        {
            foreach (string s in strings)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    MessageBox.Show("Введено некорректное значение");
                    return null;
                }
            }
            return strings;
        }

        private Worker MakeWorker(in string[] data, in int nameId)
        {
            var worker = new Worker()
            {
                Login = data[0],
                Password = data[1],
                NameId = nameId,
                RankId = GetRankId(data[4]),
                OnWork = "NO",
            };

            return worker;
        }

        private int GetRankId(string rank)
        {
            switch (rank)
            {
                case "Administrator":
                    return 1;
                case "Supervisior":
                    return 2;
                case "Worker":
                    return 3;
            }

            return -1;
        }

        private async Task<int> GetNameId(string lastName, string firstName)
        {
            using var context = new MyDbContext();
            var nameWorker = new Name
            {
                LastName = lastName,
                FirstName = firstName
            };

            context.Names.Add(nameWorker);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
                await FileLogs.WriteLog(ex);
            }

            return nameWorker.Id;
        }

        private async Task PushWorker(Worker worker)
        {
            using var context = new MyDbContext();
            context.Workers.Add(worker);
            try
            {
                await context.SaveChangesAsync();
                MessageBox.Show("Пользователь добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
                await FileLogs.WriteLog(ex);
            }
        }

        private async void AcButton_Click(object sender, RoutedEventArgs e)
        {
            string[] getElements = [LogBox.Text, PassBox.Password, SurBox.Text, NameBox.Text, RankBox.Text];
            if (CheckData(getElements) is null)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
                await FileLogs.WriteLog(new ArgumentException($"Произошла ошибка полученных данных. Были введены пустые значения"));
                return;
            };
            int nameId = await GetNameId(getElements[2], getElements[3]);
            Worker worker = MakeWorker(getElements, nameId);
            await PushWorker(worker);
        }

        private void CanellButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Window.GetWindow(this)?.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            string[] items = context.Ranks.Select(i => i.Title).ToArray();
            foreach (var item in items)
            {
                RankBox.Items.Add(item);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRole());
        }
    }
}
