using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm
{
    class PsqlSaleDeNormDbContext : DbContext, IDisposable
    {
        public PsqlSaleDeNormDbContext() : base()
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
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=DeNormSale;Username=postgres;Password=supersafe");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasKey(c => new { c.ID, c.ItemName });
            modelBuilder.Entity<ItemTag>()
                .HasKey(c => new { c.Item, c.Tag });
        }
        public DbSet<Sale> Sales{ get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
    }
}
