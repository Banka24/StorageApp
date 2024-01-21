using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
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
    /// Логика взаимодействия для AddItem.xaml
    /// </summary>
    public partial class AddItem : Page
    {
        public AddItem()
        {
            InitializeComponent();
        }

        private bool CheckDigitInString(in string value)
        {
            bool flag = false;
            char[] chars = value.ToCharArray();
            foreach (char c in chars)
            {
                flag = char.IsDigit(c) ? true : false;
            }
            return flag;
        }

        private Item MakeItem() 
        {
            using var context = new MyDbContext();
            var item = new Item
            {
                InventoryNumber = $"{NumberItem.Text}{NumberParty.Text}{DateTime.Now.ToString("ddMMyyyy")}",
                CategoryId = context.Categorys.Where(i => i.Name == Combo.Text).Select(i => i.Id).FirstOrDefault(),
                StatusId = 1,
                Row = Convert.ToInt32(RowTextBox.Text),
                Shelf = Convert.ToInt32(ShelfTextBox.Text),
            };

            return item;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Combo.Text))
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (!CheckDigitInString(NumberItem.Text) || !CheckDigitInString(NumberParty.Text))
            {
                MessageBox.Show("Введите числа в номер партии и порядковый номер");
                return;
            }

            if(!CheckDigitInString(RowTextBox.Text) || !CheckDigitInString(ShelfTextBox.Text))
            {
                MessageBox.Show("Введите ряд и полку");
                return;
            }

            Item item = MakeItem();

            using var context = new MyDbContext();
            context.Items.Add(item);
            context.SaveChanges();
            MessageBox.Show("Товар добавлен");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Editor());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using(var context = new MyDbContext())
            {
                string[] items = context.Categorys.Select(i => i.Name).ToArray();
                foreach (var item in items)
                {
                    Combo.Items.Add(item);
                }
            }
        }
    }
}
