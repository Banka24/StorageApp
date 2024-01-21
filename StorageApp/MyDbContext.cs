﻿using System.Data.Entity;

namespace StorageApp
{
    class MyDbContext : DbContext
    {
        public MyDbContext() : base("StorageDB")
        {

        }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Name> Names { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Category> Categorys { get; set; }
    }
}
