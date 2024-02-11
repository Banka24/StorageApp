using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для Registration.xaml
/// </summary>
public partial class Registration
{
    private const string SuccessMessage = "Пользователь добавлен";
    private const string FailMessage = "Произошла ошибка, проверьте логи.";

    public Registration()
    {
        InitializeComponent();
    }

    private static string[] CheckData(in string[] strings)
    {
        if (!strings.Any(string.IsNullOrWhiteSpace)) return strings;
        MessageBox.Show("Введено некорректное значение");
        return null;
    }

    private static Worker MakeWorker(in string[] data, in int nameId)
    {
        var rankId = GetRankId(data[4]);
        return rankId > 0 ? new Worker { Login = data[0], Password = data[1], NameId = nameId, RankId = rankId, OnWork = "NO" } : null;
    }

    private static int GetRankId(string rank)
    {
        return rank switch
        {
            "Administrator" => 1,
            "Supervisor" => 2,
            "Worker" => 3,
            _ => -1
        };
    }

    private static async Task<int> GetNameIdAsync(string lastName, string firstName)
    {
        using var context = new MyDbContext();
        var nameWorker = new Name
        {
            LastName = lastName,
            FirstName = firstName
        };

        context.Names.Add(nameWorker);

        if (await context.PushAsync() is false) MessageBox.Show(FailMessage);

        return nameWorker.Id;
    }

    private static async Task PushWorkerAsync(Worker worker)
    {
        using var context = new MyDbContext();

        context.Workers.Add(worker);

        var message = await context.PushAsync() ? SuccessMessage : FailMessage;

        MessageBox.Show(message);
    }

    private async void AcButton_Click(object sender, RoutedEventArgs e)
    {
        string[] getElements = [LogBox.Text, PassBox.Password, SurBox.Text, NameBox.Text, RankBox.Text];

        if (CheckData(getElements) is null)
        {
            MessageBox.Show(FailMessage);
            await FileLogs.WriteLogAsync(
                new ArgumentException("Произошла ошибка полученных данных. Были введены пустые значения"));
            return;
        }

        var nameId = await GetNameIdAsync(getElements[2], getElements[3]);

        var worker = MakeWorker(getElements, nameId);

        if (worker is not null) await PushWorkerAsync(worker);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        Window.GetWindow(this)?.Close();
    }

    private void SetRankValues()
    {
        using var context = new MyDbContext();

        var items = context.Ranks.Select(i => i.Title).ToArray();

        foreach (var item in items) RankBox.Items.Add(item);
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        SetRankValues();
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new AddRole());
    }
}