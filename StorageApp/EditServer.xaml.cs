using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для EditServer.xaml
/// </summary>
public partial class EditServer
{
    public EditServer()
    {
        InitializeComponent();
    }

    private static string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["StorageDB"].ConnectionString;
    }

    private string MakeConnectionString(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            DataSource = AddressBox.Text,
            InitialCatalog = DataBox.Text
        };

        return builder.ConnectionString;
    }

    private async Task EditServerSettingsAsync()
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        var connectionStringSettings = GetConnectionString();

        if (string.IsNullOrWhiteSpace(connectionStringSettings))
        {
            await FileLogs.WriteLogAsync(new Exception("Строка подключения - пуста"));
            return;
        }

        config.ConnectionStrings.ConnectionStrings["StorageDB"].ConnectionString = MakeConnectionString(connectionStringSettings);

        try
        {
            config.Save(ConfigurationSaveMode.Modified);
            MessageBox.Show("Настройки успешно применены\nПерезапустите приложение");
        }
        catch (ConfigurationErrorsException ex)
        {
            await FileLogs.WriteLogAsync(ex);
            MessageBox.Show("Произошла ошибка, проверьте логи");
        }
    }

    private async void AcButton_Click(object sender, RoutedEventArgs e)
    {
        if (AddressBox.Text == "NULL" && DataBox.Text == "NULL")
        {
            MessageBox.Show("Невозможно подключиться к несуществующей БД.\nЗаполните поля.");
            return;
        }

        await EditServerSettingsAsync();
    }

    private void CancelBtn_Click(object sender, RoutedEventArgs e)
    {
        var window = new Authorization();
        window.Show();
        Window.GetWindow(this)?.Close();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        var data = GetDataFromConnectionString(GetConnectionString());
        ServerData.Text = $"Data Source: {data[0]}\tInitial Catalog: {data[1]}";
    }

    private static string[] GetDataFromConnectionString(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        return [builder.DataSource, builder.InitialCatalog];
    }
}