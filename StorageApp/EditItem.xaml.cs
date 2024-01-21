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

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для EditItem.xaml
    /// </summary>
    public partial class EditItem : Page
    {
        public EditItem()
        {
            InitializeComponent();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Data.Text) || string.IsNullOrWhiteSpace(Combo.Text) || string.IsNullOrWhiteSpace(InventoryNumberTextBox.Text))
            {
                MessageBox.Show("введите все требуемые данные данные");
            }

            using(var context = new MyDbContext())
            {
                var item = context.Items?.SingleOrDefault(i => i.InventoryNumber == InventoryNumberTextBox.Text);
                context.Items?.Remove(item);

            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Editor());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                string[] columns = { "Id", "CategoryId", "StatusId" };
                string[] categories = typeof(Item).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
                foreach (var category in categories)
                {
                    Combo.Items.Add(category);
                }
            }
        }
    }
}

