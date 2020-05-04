using System;
using System.Collections.Generic;
using System.Text;
using Npgsql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CPSAssignment2.Benchmark.Models.Postgresql.PostgresTest
{
    class PostgresDbTest : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=UserTest;Username=postgres;Password=supersafe");

        public DbSet<User> Users { get; set; }

    }
}
