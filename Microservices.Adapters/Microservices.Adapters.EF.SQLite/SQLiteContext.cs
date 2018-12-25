using Microservices.Common;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microservices.Adapters.EF.SQLite
{
    public class SQLiteContext : DbContext
    {
        protected string connectionString;
        protected Type[] tables;
        public SQLiteContext(string connectionString, Type[] tables)
        {
            this.connectionString = connectionString;
            this.tables = tables;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var table in this.tables)
            {
                if (modelBuilder.Model.FindEntityType(table) == null)
                    modelBuilder.Model.AddEntityType(table);
                else
                    Log.Logger.Warn($"Table [{table.Name}] is exist!");
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
