﻿using System;
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
using System.Runtime.Remoting.Contexts;
using System.Net.NetworkInformation;

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

        private Item GetItem(MyDbContext context)
        {
            var item = context.Items.Include(i => i.Status).Include(i => i.Category).FirstOrDefault(i => i.InventoryNumber == InventoryNumberTextBox.Text);
            if (item == null)
            {
                MessageBox.Show("Такого товара нет.\nПроверьте инвентарный номер.");
            }
            return item;
        }

        private void EditInfo()
        {
            using var context = new MyDbContext();
            Item item = GetItem(context);
            if (item is not null)
            {
                switch (Combo.Text)
                {
                    case "Row":
                        if (!int.TryParse(Data.Text, out int row))
                        {
                            MessageBox.Show("Введите число");
                            return;
                        }
                        item.Row = row;
                        break;
                    case "Shelf":
                        if (!int.TryParse(Data.Text, out int shelf))
                        {
                            MessageBox.Show("Введите число");
                            return;
                        }
                        item.Shelf = shelf;
                        break;
                    //case "CategoryId":
                    //    string[] columns = { "Id", "InventoryNumber", "Status", "Category" };
                    //    string[] categories = typeof(Item).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
                    //    foreach (var category in categories)
                    //    {
                    //        Combo.Items.Add(category);
                    //    }
                    //    break;
                    //case "StatusId":
                    //    break;
                    default:
                        MessageBox.Show("Произошла ошибка.\nЯ не знаю такой операции.\nВозможно это добавиться в обновлениях.");
                        return;
                }
                context.SaveChanges();
                MessageBox.Show("Замена проведена успешно");
            }
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Data.Text) || string.IsNullOrWhiteSpace(Combo.Text) || string.IsNullOrWhiteSpace(InventoryNumberTextBox.Text))
            {
                MessageBox.Show("введите все требуемые данные данные");
                return;
            }
            EditInfo();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Editor());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                string[] columns = { "Id", "InventoryNumber", "StatusId", "CategoryId" };
                string[] categories = typeof(Item).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
                foreach (var category in categories)
                {
                    Combo.Items.Add(category);
                }
            }
        }

        private void Combo_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if(Combo.Text == "Status" || Combo.Text == "Category")
            {
                ComboCategory.IsEnabled = true;
                return;
            }
            ComboCategory.IsEnabled = false;
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
            string[] columns = { "Id", "Items" };
            using var context = new MyDbContext();
            switch (Combo.Text)
            {
                case "Status":
                    string[] categories = typeof(Status).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
                    AddItems(categories);
                    break;
                case "Category":
                    categories = typeof(Category).GetProperties().Where(x => !columns.Contains(x.Name)).Select(x => x.Name).ToArray();
                    AddItems(categories);
                    break;
            }
        }
    }
}

