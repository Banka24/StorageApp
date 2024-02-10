using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для InfoItem.xaml
/// </summary>
public partial class InfoItem
{
    public InfoItem()
    {
        InitializeComponent();
    }

    private static async Task<Item> GetInfo(string number)
    {
        var context = new MyDbContext();
        return await context.Items.Include(i => i.Status).Include(i => i.Category).FirstOrDefaultAsync(i => i.InventoryNumber == number);
    }

    private async void OKButton_Click(object sender, RoutedEventArgs e)
    {
        var item = await GetInfo(InventoryBox.Text);

        if (item is null)
        {
            MessageBox.Show("Такого товара нет.\nПерепроверьте инвентарный номер");
            return;
        }

        SetInfoItem(item);
    }

    private void SetInfoItem(Item item)
    {
        string[] textBoxes = [CategoryBox.Text, StatusBox.Text, RowBox.Text, ShelfBox.Text];
        string[] itemValues = [item.Category.Name, item.Status.Name, $"{item.Row}", $"{item.Shelf}"];

        for (var i = 0; i < itemValues.Length; i++) textBoxes[i] = itemValues[i];
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        Window.GetWindow(this)?.Close();
    }
}