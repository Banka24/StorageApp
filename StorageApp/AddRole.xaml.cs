using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для AddRole.xaml
    /// </summary>
    public partial class AddRole : Page
    {
        public AddRole()
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

        private async Task<Rank> GetRank()
        {
            string[] getElements = [BoxName.Text, RootBox.Text];
            if (CheckData(getElements) is null)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
                await FileLogs.WriteLog(new ArgumentException($"Произошла ошибка полученных данных. Были введены пустые значения"));
                return null;
            };

            var rank = new Rank
            {
                Title = getElements[0],
                Root = getElements[1],
            };

            return rank;
        }

        private async Task PushRank()
        {
            using var context = new MyDbContext();
            var rank = await GetRank();
            context.Ranks.Add(rank);
            await context.SaveChangesAsync();
        }

        private void AddRoleBtn_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(PushRank);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }
    }
}
