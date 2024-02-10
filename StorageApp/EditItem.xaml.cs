using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для EditItem.xaml
/// </summary>
public partial class EditItem
{
    private const string SuccessMessage = "Замена проведена успешно.";
    private const string FailMessage = "Произошла ошибка, проверьте логи.";

    public EditItem()
    {
        InitializeComponent();
    }

    private async Task<Item> GetItemAsync()
    {
        using var context = new MyDbContext();
        return await context.Items.Include(i => i.Status).Include(i => i.Category).FirstOrDefaultAsync(i => i.InventoryNumber == InventoryNumberTextBox.Text);
    }

    private async Task PushItemAsync()
    {
        using var context = new MyDbContext();

        await EditInfoAsync();
        var message = await context.PushAsync() ? SuccessMessage : FailMessage;

        MessageBox.Show(message);
    }

    private async Task EditInfoAsync()
    {
        using var context = new MyDbContext();
        var item = await GetItemAsync();

        if (item is null)
        {
            MessageBox.Show(FailMessage);
            await FileLogs.WriteLogAsync(new DataException($"{InventoryNumberTextBox}"));
            return;
        }

        switch (Combo.Text)
        {
            case "Row":
                if (!int.TryParse(Data.Text, out var row)) MessageBox.Show("Введите число");
                item.Row = row;
                break;
            case "Shelf":
                if (!int.TryParse(Data.Text, out var shelf)) MessageBox.Show("Введите число");
                item.Shelf = shelf;
                break;
            case "Category":
                var categoryId = context.Categories.FirstOrDefault(i => i.Name == ComboCategory.Text)?.Id;
                if (categoryId is not null) item.CategoryId = (int)categoryId!;
                break;
            case "Status":
                var statusId = context.Status?.FirstOrDefault(i => i.Name == ComboCategory.Text)?.Id;

                if (statusId is not null) item.StatusId = (byte)statusId;
                break;
        }
    }

    private async void Redact_Click(object sender, RoutedEventArgs e)
    {
        if ((string.IsNullOrWhiteSpace(Data.Text) && Data.IsEnabled) || string.IsNullOrWhiteSpace(Combo.Text) || string.IsNullOrWhiteSpace(InventoryNumberTextBox.Text))
        {
            MessageBox.Show("Введите все требуемые данные данные");
            return;
        }

        await PushItemAsync();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Editor());
    }

    private static string[] GetCategories(string[] columns)
    {
        return typeof(Item).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        var categories = GetCategories(["Id", "InventoryNumber", "StatusId", "CategoryId"]);

        foreach (var category in categories) Combo.Items.Add(category);
    }

    private void Combo_LostMouseCapture(object sender, MouseEventArgs e)
    {
        if (Combo.Text is "Status" or "Category")
        {
            ComboCategory.IsEnabled = true;
            Data.IsEnabled = false;
            return;
        }

        ComboCategory.IsEnabled = false;
        Data.IsEnabled = true;
    }

    private void AddItems(in string[] categories)
    {
        foreach (var category in categories) ComboCategory.Items.Add(category);
    }

    private void ComboCategory_GotMouseCapture(object sender, MouseEventArgs e)
    {
        ComboCategory.Items.Clear();

        using var context = new MyDbContext();

        switch (Combo.Text)
        {
            case "Status":
                var statuses = context.Status?.Select(i => i.Name).ToArray();
                AddItems(statuses);
                break;

            case "Category":
                var categories = context.Categories?.Select(i => i.Name).ToArray();
                AddItems(categories);
                break;
        }
    }
}