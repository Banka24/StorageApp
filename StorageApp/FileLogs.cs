using System;
using System.IO;
using System.Threading.Tasks;

namespace StorageApp
{
    internal static class FileLogs
    {
        private static readonly string path = @"D:\learn\C#\MyPracticWork\StorageApp\StorageApp\bin\Debug\StorageAppLogs.txt";

        public static async Task WriteLog(Exception ex)
        { 
            await Task.Run(() => { File.AppendAllText(path, $"{DateTime.Now} в объекте {ex.Source} произошла ошибка: {ex.Message}.\nТрассировка стека:{ex.StackTrace}.\n\n"); });
        }
           
    }
}
