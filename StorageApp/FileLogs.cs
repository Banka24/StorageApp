using System;
using System.IO;
using System.Threading.Tasks;

namespace StorageApp
{
    internal static class FileLogs
    {
        private const string PATH = "StorageAppLogs.txt";
        public static async Task<Task> WriteLog(Exception ex)
        { 
            await Task.Run(() => { File.AppendAllText(PATH, $"{DateTime.Now} в объекте {ex.Source} произошла ошибка: {ex.Message}.\nТрассировка стека:{ex.StackTrace}.\n\n"); });
            return Task.CompletedTask;
        }
    }
}
