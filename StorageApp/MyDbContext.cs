using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace StorageApp
{
    internal class MyDbContext() : DbContext("StorageDB")
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Name> Names { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Category> Categories { get; set; }

        public async Task PushAsync<T>(DbSet dbSet, T value, string message)
        {
            if (value is not null)
            {
                dbSet.Add(value);
                try
                {
                    await SaveChangesAsync();
                    MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка, проверьте логи.");
                    await FileLogs.WriteLogAsync(ex);
                }
            }
        }
    }
}
