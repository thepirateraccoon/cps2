using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    class PsqlSaleNormDbContext : DbContext, IDisposable, DbCommonMethods
    {
        public PsqlSaleNormDbContext() : base()
        {
        }
        public void Initiate()
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
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=NormSale;Username=postgres;Password=supersafe");

        public void seed(List<MasterItem> items, List<MasterCustomer> customers, System.Diagnostics.Stopwatch sw)
        {
            throw new NotImplementedException();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Tag> Tags { get; set; }
     
    }
}
