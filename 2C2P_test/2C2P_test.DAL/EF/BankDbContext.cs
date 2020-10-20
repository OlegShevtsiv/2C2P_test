using _2C2P_test.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P_test.DAL.EF
{
    public class BankDbContext : DbContext
    {
        const string SQLiteDbPath = "Filename=../BankStorage.db";
        public DbSet<Transaction> Transactions { get; set; }

        public BankDbContext() 
            : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(SQLiteDbPath);
        }
    }
}
