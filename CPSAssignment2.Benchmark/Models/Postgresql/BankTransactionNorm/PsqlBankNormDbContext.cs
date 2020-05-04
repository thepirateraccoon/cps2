using System;
using System.Collections.Generic;
using System.Text;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionNorm
{
    class PsqlBankNormDbContext : DbContext, IDisposable
    {
        public PsqlBankNormDbContext() : base()
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
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=NormBank;Username=postgres;Password=supersafe");
    }
}
