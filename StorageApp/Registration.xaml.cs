using System;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration
    {
        public Registration()
        {
            InitializeComponent();
        }

        private static string[] CheckData(in string[] strings)
        {
            if (!strings.Any(string.IsNullOrWhiteSpace)) return strings;
            MessageBox.Show("Введено некорректное значение");
            return null;

        }

        private static Worker MakeWorker(in string[] data, in int nameId)
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

        private static int GetRankId(string rank)
        {
            return rank switch
            {
                "Administrator" => 1,
                "Supervisor" => 2,
                "Worker" => 3,
                _ => -1
            };
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

        private static async Task PushWorker(Worker worker)
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
                await FileLogs.WriteLog(new ArgumentException("Произошла ошибка полученных данных. Были введены пустые значения"));
                return;
            }
            var nameId = await GetNameId(getElements[2], getElements[3]);
            var worker = MakeWorker(getElements, nameId);
            await PushWorker(worker);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Window.GetWindow(this)?.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            var items = context.Ranks.Select(i => i.Title).ToArray();
            foreach (var item in items)
            {
                RankBox.Items.Add(item);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddRole());
        }
    }
}
