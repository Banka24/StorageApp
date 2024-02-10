using System;
using System.IO;
using System.Threading.Tasks;
using StorageApp.Properties;

namespace StorageApp;

internal static class FileLogs
{
    private const string Path = "StorageAppLogs.txt";

    public static async Task<Task> WriteLogAsync(Exception ex)
    {
        await Task.Run(() =>
        {
            File.AppendAllText(Path, string.Format(Resources.FileLogs_WriteLog_, DateTime.Now, ex.Source, ex.Message, ex.StackTrace, ex.InnerException));
        });
        return Task.CompletedTask;
    }
}