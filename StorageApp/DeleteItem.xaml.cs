using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для DeleteItem.xaml
    /// </summary>
    public partial class DeleteItem
    {
        public DeleteItem()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Editor());
        }

        private async Task RemoveItem()
        {
            using var context = new MyDbContext();
            var item = await context.Items?.Where(i => i.InventoryNumber == Delete.Text).FirstAsync()!;
            context.Items?.Remove(item);

            try
            {
                await context.SaveChangesAsync();
                MessageBox.Show("Товар удалён");
            }
            catch(Exception ex)
            {
                await FileLogs.WriteLogAsync(ex);
            }

        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(Delete.Text))
            {
                MessageBox.Show("введите все требуемые данные данные");
            }

            await RemoveItem();
        }
    }
}
