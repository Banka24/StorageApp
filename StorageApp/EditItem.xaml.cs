using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для EditItem.xaml
    /// </summary>
    public partial class EditItem
    {
        public EditItem()
        {
            InitializeComponent();
        }

        private async Task<Item> GetItem(MyDbContext context)
        {
            var item = await context.Items.Include(i => i.Status).Include(i => i.Category).FirstOrDefaultAsync(i => i.InventoryNumber == InventoryNumberTextBox.Text);
            if (item == null)
            {
                MessageBox.Show("Такого товара нет.\nПроверьте инвентарный номер.");
            }
            return item;
        }

        private async Task PushItem()
        {
            using var context = new MyDbContext();
            await EditInfo(context);
            try
            {
                await context.SaveChangesAsync();
                MessageBox.Show("Замена проведена успешно");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка, проверьте логи.");
                await FileLogs.WriteLogAsync(ex);
            }
        }

        private async Task EditInfo(MyDbContext context)
        {
            var item = await GetItem(context);

            switch (Combo.Text)
            {
                case "Row":
                    if (!int.TryParse(Data.Text, out var row))
                    {
                        MessageBox.Show("Введите число");
                    }
                    item.Row = row;
                    break;
                case "Shelf":
                    if (!int.TryParse(Data.Text, out var shelf))
                    {
                        MessageBox.Show("Введите число");
                    }
                    item.Shelf = shelf;
                    break;
                case "Category":
                    var categoryId = context.Categories.FirstOrDefault(i => i.Name == ComboCategory.Text)?.Id;
                    if (categoryId is not null)
                    {
                        item.CategoryId = (int)categoryId!;
                    }
                    break;
                case "Status":
                    var statusId = context.Status?.FirstOrDefault(i => i.Name == ComboCategory.Text)?.Id;

                    if (statusId is not null)
                    {
                        item.StatusId = (byte)statusId;
                    }
                    break;
            }
        }

        private async void Redact_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Data.Text) && Data.IsEnabled || string.IsNullOrWhiteSpace(Combo.Text) || string.IsNullOrWhiteSpace(InventoryNumberTextBox.Text))
            {
                MessageBox.Show("введите все требуемые данные данные");
                return;
            }

            await PushItem();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Editor());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = new MyDbContext();
            string[] columns = ["Id", "InventoryNumber", "StatusId", "CategoryId"];
            var categories = typeof(Item).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
            foreach (var category in categories)
            {
                Combo.Items.Add(category);
            }
        }

        private void Combo_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if(Combo.Text is "Status" or "Category")
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
            foreach (var category in categories)
            {
                ComboCategory.Items.Add(category);
            }
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
}

