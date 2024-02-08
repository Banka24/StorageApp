using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для AddItem.xaml
/// </summary>
public partial class AddItem
{
    private const string SuccessMessage = "Товар добавлен";
    private const string FailMessage = "Произошла ошибка, проверьте логи.";

    public AddItem()
    {
        InitializeComponent();
    }

    private async Task<int> GetCategoryIdAsync()
    {
        using var context = new MyDbContext();

        return await context.Categories.Where(i => i.Name == Combo.Text).Select(i => i.Id).FirstOrDefaultAsync();
    }

    private async Task<bool> CheckCategory(int id)
    {
        if (id > 0) return true;

        await FileLogs.WriteLogAsync(new CategoryNotFoundException($"{Combo.Text}"));

        return false;
    }

    private async Task<Item> MakeItemAsync(int numberItem, int numberParty, int row, int shelf)
    {
        var categoryId = await GetCategoryIdAsync();

        if (await CheckCategory(categoryId))
            return new Item
            {
                InventoryNumber = $"{numberItem}{numberParty}{DateTime.Now:ddMMyyyy}", CategoryId = categoryId,
                StatusId = 1, Row = row, Shelf = shelf
            };

        return null;
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
            MessageBox.Show(FailMessage);
            await FileLogs.WriteLogAsync(new NullDataException());
            return;
        }

        using var context = new MyDbContext();

        context.Items.Add(item);

        var message = await context.PushAsync() ? SuccessMessage : FailMessage;

        MessageBox.Show(message);
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Editor());
    }

    private static async Task<string[]> GetCategoriesAsync()
    {
        using var context = new MyDbContext();
        return await context.Categories.Select(i => i.Name).ToArrayAsync();
    }

    private void LoadCategories(in string[] items)
    {
        foreach (var item in items) Combo.Items.Add(item);
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        var items = await GetCategoriesAsync();

        if (items is null)
        {
            MessageBox.Show(FailMessage);
            await FileLogs.WriteLogAsync(new NullDataException());
            return;
        }

        LoadCategories(items);
    }
}