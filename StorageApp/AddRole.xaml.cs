using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp;

/// <summary>
///     Логика взаимодействия для AddRole.xaml
/// </summary>
public partial class AddRole
{
    private const string SuccessMessage = "Должность была добавлен";
    private const string FailMessage = "Возникла ошибка добавления должности";
    private const string ErrorRankMessage = "Такая должность уже существует";

    public AddRole()
    {
        InitializeComponent();
    }

    private static string[] CheckData(in string[] strings)
    {
        return strings.Any(string.IsNullOrWhiteSpace) ? null : strings;
    }

    private static async Task<bool> CheckRankInBaseAsync(string rankTitle)
    {
        using var context = new MyDbContext();

        return await context.Ranks.FirstOrDefaultAsync(i => i.Title == rankTitle) is null;
    }

    private async Task<Rank> GetRank()
    {
        var getElements = CheckData([BoxName.Text, RootBox.Text]) ?? throw new DataException(BoxName.Text, RootBox.Text);

        if (await CheckRankInBaseAsync(BoxName.Text)) throw new Exception(ErrorRankMessage);

        return new Rank { Title = getElements[0], Root = getElements[1] };
    }

    private async void AddRoleBtn_Click(object sender, RoutedEventArgs e)
    {
        Rank rank;

        try
        {
            rank = await GetRank();
        }
        catch (Exception ex)
        {
            MessageBox.Show(FailMessage);
            await FileLogs.WriteLogAsync(ex);
            return;
        }

        using var context = new MyDbContext();

        context.Ranks.Add(rank);

        var message = await context.PushAsync() ? SuccessMessage : FailMessage;

        MessageBox.Show(message);
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Registration());
    }
}