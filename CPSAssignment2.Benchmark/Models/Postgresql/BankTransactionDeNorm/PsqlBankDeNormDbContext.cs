using System;
using System.Collections.Generic;
using System.Text;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionDeNorm
{
    class PsqlBankDeNormDbContext : DbContext, IDisposable
    {
        public PsqlBankDeNormDbContext() : base()
        {
            if (this.Database.CanConnect())
            {
                this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
            else
            {
                this.Database.EnsureCreated();
            }
        }

        public override void Dispose()
        {
            this.Database.EnsureDeleted();
            this.Database.CloseConnection();
            base.Dispose();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=DeNormBank;Username=postgres;Password=supersafe");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(c => new { c.ID, c.AccountId});
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
