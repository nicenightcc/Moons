using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Microservices.Adapters.EF.MySql
{
    public class MySqlContext : DbContext
    {
        protected string connectionString;
        protected Type[] tables;
        public MySqlContext(string connectionString, Type[] tables)
        {
            this.connectionString = connectionString;
            this.tables = tables;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);

            //Debug模式显示执行的sql语句
#if DEBUG
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
#endif

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var table in this.tables)
                if (modelBuilder.Model.FindEntityType(table) == null)
                    modelBuilder.Model.AddEntityType(table);

            base.OnModelCreating(modelBuilder);
        }
    }
}
