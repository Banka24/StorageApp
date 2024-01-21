using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для DeleteItem.xaml
    /// </summary>
    public partial class DeleteItem : Page
    {
        public DeleteItem()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Editor());
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(Deletebox.Text))
            {
                MessageBox.Show("введите все требуемые данные данные");
            }

            using var context = new MyDbContext();
            var item = context.Items?.Where(i => i.InventoryNumber == Deletebox.Text)?.First();
            context.Items.Remove(item);
            context.SaveChanges();
            MessageBox.Show("Товар удалён");
        }
    }
}
