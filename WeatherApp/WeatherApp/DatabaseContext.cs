using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherApp.Models;

namespace WeatherApp
{
    public class DatabaseContext :DbContext
    {
        public DbSet<NamedCity> Cities { get; set; }
        private string databasePath;

        public DatabaseContext(string databasePath)
        {
            this.databasePath = databasePath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
