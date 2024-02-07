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
        public AddItem()
        {
            InitializeComponent();
        }

        private async Task<int> GetCategoryIdAsync()
        {
            int categoryId;
            using (var context = new MyDbContext())
            {
                categoryId = await context.Categories.Where(i => i.Name == Combo.Text).Select(i => i.Id).FirstOrDefaultAsync();
            }

            if (categoryId <= 0)
            {
                throw new CategoryNotFoundException($"{Combo.Text}");
            }

            return categoryId;
        }

        private async Task<Item> MakeItemAsync(int numberItem, int numberParty, int row, int shelf)
        {
            int categoryId;

            try
            {
                categoryId = await GetCategoryIdAsync();
            }
            catch (Exception ex)
            {
                await FileLogs.WriteLogAsync(ex);
                throw;
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
                await FileLogs.WriteLogAsync(new Exception(message:"Был передан пустой объект"));
                return;
            }

            using var context = new MyDbContext();

            if (await context.PushAsync(context.Items, item))
            {
                MessageBox.Show("Товар добавлен");
            }
            else
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Editor());
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string[] items;

            using (var context = new MyDbContext())
            {
                items = await context.Categories.Select(i => i.Name).ToArrayAsync();
            }

            foreach (var item in items)
            {
                Combo.Items.Add(item);
            }
        }
    }
}
