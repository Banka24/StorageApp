using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private Item GetItemInfo(string number)
        {
            using(var context = new MyDbContext())
            {
                var item = context.Items.Include(i => i.Status).FirstOrDefault(i => i.InventoryNumber == number);
                return item;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var item = GetItemInfo(InventoryBox.Text);
            if(item is null)
            {
                MessageBox.Show("Такого товара нет.\nПерепроверьте инвентарный номер");
                return;
            }
            CategoryBox.Text = item.Category;
            StatusBox.Text = item.Status.Name;
            RowBox.Text = item.Row.ToString();
            ShelfBox.Text = item.Shelf.ToString();
        }
    }
}
