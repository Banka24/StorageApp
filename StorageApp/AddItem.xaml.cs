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

        private async Task<int> GetCategoryIdAsync()
        {
            return await _context.Categories.Where(i => i.Name == Combo.Text).Select(i => i.Id).FirstOrDefaultAsync();
        }

        private async Task<Item> MakeItemAsync(int numberItem, int numberParty, int row, int shelf)  
        {
            var categoryId = await GetCategoryIdAsync();
            if ( categoryId <= 0)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи");
                await FileLogs.WriteLogAsync(new Exception(message:"Была найдена несуществующая категория"));
                return null;
            }
            var item = new Item
            {
                InventoryNumber = $"{numberItem}{numberParty}{DateTime.Now:ddMMyyyy}",
                CategoryId = categoryId,
                StatusId = 1,
                Row = row,
                Shelf = shelf
            };

            return item;
        }

        private async Task PushItemAsync(Item item)
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
                await FileLogs.WriteLogAsync(ex);
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

            var item = await MakeItemAsync(numberItem, numberParty, row, shelf);

            if (item is null)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи");
                await FileLogs.WriteLogAsync(new Exception(message: "Был получен пустой объект"));
                return;
            }
            await PushItemAsync(item);

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
