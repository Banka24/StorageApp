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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace StorageApp
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        SqlDataBase sqlDataBase;
        public Autorization()
        {
            InitializeComponent();
        }

        private void CheckUser(string queryString)
        {
            using (var command = new SqlCommand(queryString, sqlDataBase.GetConnection()))
            {
                byte result = Convert.ToByte(command.ExecuteScalar());
                if (result == 1)
                {
                    ChangeWindow();
                }
                else
                {
                    MessageBox.Show("Такого пользователя нет");
                }
            }
        }

        private string CheckString(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                return str;
            }
            return "NULL";
        }

        private void ChangeWindow()
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = CheckString(LoginTextBox.Text);
            string password = CheckString(MyPassword.Password);

            string queryString = $"SELECT COUNT(*) FROM Worker WHERE Login = '{login}' AND Password = '{password}'";
            CheckUser(queryString);

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sqlDataBase = new SqlDataBase();
            sqlDataBase.ConnectionOpen();
        }
    }
}
