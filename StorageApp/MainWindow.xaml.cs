﻿using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Linq;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Role
        {
            Administrator = 1,
            Worker
        }
        public MainWindow()
        {
            InitializeComponent();
            textName.Text = $"Добрый день, {SharedContext.Name}";
            if (SharedContext.Role == (int)Role.Worker)
            {
                BtnInfIt.Visibility = Visibility.Visible;
                BtnGoWork.Visibility = Visibility.Visible;
            }
        }

        private void BtnInfIt_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InfoItem());
        }
    }
}
