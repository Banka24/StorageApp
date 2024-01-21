using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для InfoItem.xaml
    /// </summary>
    public partial class InfoItem : Page
    {
        public InfoItem()
        {
            InitializeComponent();
        }

        private Item GetInfo(string number)
        {
            using(var context = new MyDbContext())
            {
                var item = context.Items.Include(i => i.Status).Include(i => i.Category).FirstOrDefault(i => i.InventoryNumber == number);
                return item;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var item = GetInfo(InventoryBox.Text);
            if(item is null)
            {
                MessageBox.Show("Такого товара нет.\nПерепроверьте инвентарный номер");
                return;
            }
            CategoryBox.Text = item.Category.Name;
            StatusBox.Text = item.Status.Name;
            RowBox.Text = item.Row.ToString();
            ShelfBox.Text = item.Shelf.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
