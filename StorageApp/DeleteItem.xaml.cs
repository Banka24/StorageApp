using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для DeleteItem.xaml
/// </summary>
public partial class DeleteItem
{
    private const string SuccessMessage = "Товар удалён";
    private const string FailMessage = "Произошла ошибка, проверьте логи";

    public DeleteItem()
    {
        InitializeComponent();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Editor());
    }

    private async Task<Item> GetItemAsync()
    {
        using var context = new MyDbContext();
        return await context.Items.Where(i => i.InventoryNumber == Delete.Text).FirstAsync();
    }

    private async Task RemoveItemAsync()
    {
        var item = await GetItemAsync();

        if (item is null)
        {
            await FileLogs.WriteLogAsync(new NullDataException());
            await FileLogs.WriteLogAsync(new RemoveNullDataException());
            return;
        }

        using var context = new MyDbContext();
        context.Items.Remove(item);
        var message = await context.PushAsync() ? SuccessMessage : FailMessage;

        MessageBox.Show(message);
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Delete.Text))
        {
            MessageBox.Show("Введите все требуемые данные");
            return;
        }

        await RemoveItemAsync();
    }
}