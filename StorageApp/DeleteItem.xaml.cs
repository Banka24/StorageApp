using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading.Tasks;
using System.Data.Entity;

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

        private async Task RemoveItem()
        {
            using var context = new MyDbContext();
            var item = await context.Items?.Where(i => i.InventoryNumber == Deletebox.Text)?.FirstAsync();
            context.Items.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }

            MessageBox.Show("Товар удалён");
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(Deletebox.Text))
            {
                MessageBox.Show("введите все требуемые данные данные");
            }

            await RemoveItem();
        }
    }
}
