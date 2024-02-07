using System.Linq;
using System.Windows;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для AddRole.xaml
    /// </summary>
    public partial class AddRole
    {
        private readonly MyDbContext _context;
        public AddRole()
        {
            InitializeComponent();
            _context = new MyDbContext();
        }

        private static string[] CheckData(in string[] strings)
        {
            if (!strings.Any(string.IsNullOrWhiteSpace)) return strings;

            MessageBox.Show("Введено некорректное значение");

            return null;
        }

        private Rank GetRank()
        {
            var getElements =  CheckData([BoxName.Text, RootBox.Text]);

            if(getElements is null) return null;

            var rank = new Rank
            {
                Title = getElements[0],
                Root = getElements[1],
            };

            return rank;
        }

        private async void AddRoleBtn_Click(object sender, RoutedEventArgs e)
        {
            var rank = GetRank();

            await _context.PushAsync(_context.Ranks, rank, "Должность была добавлена" );
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Registration());
        }

        private void AddRole_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _context.Dispose();
        }
    }
}
