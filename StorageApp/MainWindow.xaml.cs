using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private enum Role
    {
        Administrator = 1,
        Supervisor,
        Worker
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnInfIt_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new InfoItem());
    }

    private void BtnInfIt_Click_1(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Editor());
    }

    private void BtnGoAdmin_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Reports());
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        var window = new Authorization();
        window.Show();
        Close();
    }

    private static async Task<Worker> GetWorkerAsync()
    {
        using var context = new MyDbContext();
        return await context.Workers?.Where(i => i.Name.FirstName == SharedContext.Name).SingleOrDefaultAsync()!;
    }

    private static async Task StartWorkShiftAsync()
    {
        using var context = new MyDbContext();

        var worker = await GetWorkerAsync();

        if (worker == null) return;

        if (worker.OnWork == "YES")
        {
            MessageBox.Show("Вы закончили смену");
            worker.OnWork = "NO";
        }
        else
        {
            MessageBox.Show("Вы начали смену");
            worker.OnWork = "YES";
        }
    }

    private async void BtnGoWork_Click(object sender, RoutedEventArgs e)
    {
        using var context = new MyDbContext();
        await StartWorkShiftAsync();
        await context.PushAsync();
    }

    private static void MakeButtonVisible(Button[] buttons)
    {
        foreach (var button in buttons) button.Visibility = Visibility.Visible;
    }

    private void MakeMainMenu(string name, int role)
    {
        TextName.Text = $"Добрый день, {name}";
        switch (role)
        {
            case (int)Role.Administrator:
                MakeButtonVisible([BtnInfAdmin, BtnGoAdmin, RegistrationBtn]);
                break;

            case (int)Role.Supervisor:
            case (int)Role.Worker:
                MakeButtonVisible([BtnInfIt, BtnGoWork]);
                break;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MakeMainMenu(SharedContext.Name, SharedContext.Role);
    }

    private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Registration());
    }
}