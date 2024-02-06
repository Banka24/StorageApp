using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для AddItem.xaml
    /// </summary>

    public partial class AddItem
    {
        private readonly MyDbContext _context;
        public AddItem()
        {
            InitializeComponent();
            _context = new MyDbContext();
        }

        private async Task<Item> MakeItem(int numberItem, int numberParty, int row, int shelf)  
        {
            var item = new Item
            {
                InventoryNumber = $"{numberItem}{numberParty}{DateTime.Now:ddMMyyyy}",
                CategoryId = await _context.Categories?.Where(i => i.Name == Combo.Text).Select(i => i.Id).FirstOrDefaultAsync()!,
                StatusId = 1,
                Row = Convert.ToInt32(row),
                Shelf = Convert.ToInt32(shelf)
            };

            return item;
        }

        private async Task PushItem(Item item)
        {
            _context.Items.Add(item);
            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Товар добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
                await FileLogs.WriteLog(ex);
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Combo.Text))
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (!int.TryParse(NumberItem.Text, out var numberItem) || !int.TryParse(NumberParty.Text, out var numberParty))
            {
                MessageBox.Show("Введите числа в номер партии и порядковый номер");
                return;
            }

            if (!int.TryParse(RowTextBox.Text, out var row) || !int.TryParse(ShelfTextBox.Text, out var shelf))
            {
                MessageBox.Show("Введите ряд и полку");
                return;
            }

            var item = await MakeItem(numberItem, numberParty, row, shelf);

            await PushItem(item);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Editor());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var items = await _context.Categories.Select(i => i.Name).ToArrayAsync();
            foreach (var item in items)
            {
                Combo.Items.Add(item);
            }
        }
        
        private void AddItem_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _context.Dispose();
        }
    }
}
