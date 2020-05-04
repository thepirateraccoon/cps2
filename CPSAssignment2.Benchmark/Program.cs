using CPSAssignment2.Benchmark.Models;
using CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.MongoTest;
using CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionDeNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.PostgresTest;
using CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace CPSAssignment2.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising");
            Console.WriteLine("Reading files");
            
            ArrayList Items = ParseItem();
            ArrayList Customers = ParseCustomer();



            /*
            using (MonBankDeNormDbContext db = new MonBankDeNormDbContext()){}
            using (MonBankNormDbContext db = new MonBankNormDbContext()) {}

            using (MonSaleDeNormDbContext db = new MonSaleDeNormDbContext()) { }
            using (MonSaleNormDbContext db = new MonSaleNormDbContext()) { }

            using (PsqlBankDeNormDbContext db = new PsqlBankDeNormDbContext()) { }
            using (PsqlBankNormDbContext db = new PsqlBankNormDbContext()) { }

            using (PsqlSaleDeNormDbContext db = new PsqlSaleDeNormDbContext()) { }
            using (PsqlSaleNormDbContext db = new PsqlSaleNormDbContext()) { }
            */

        }

        private static ArrayList ParseItem()
        {
            string[] itemsfile = System.IO.File.ReadAllText(@"RandomItems.csv").Split("\n");
            ArrayList Items = new ArrayList();
            for (int i = 1; i < itemsfile.Length - 1; i++)
            {
                var k = (itemsfile[i]).Split(";");
                Items.Add(new MasterItem
                {
                    Price = int.Parse(k[0]),
                    Name = k[1],
                    Tag1 = k[2],
                    Tag2 = k[3],
                    Tag3 = k[4]
                });
            }
            return Items;
        }
        private static ArrayList ParseCustomer()
        {
            ArrayList Customers = new ArrayList();
            string[] customersfile = System.IO.File.ReadAllText(@"RandomCustomer.csv").Split("\n");
            for (int i = 1; i < customersfile.Length - 1; i++)
            {
                var k = (customersfile[i]).Split(";");
                Customers.Add(new MasterCustomer
                {
                    Name = k[0],
                    Email = k[2],
                    Gender = k[3],
                    Accounts = int.Parse(k[4]),
                    Account1 = int.Parse(k[8]),
                    Account2 = int.Parse(k[9]),
                    Account3 = int.Parse(k[10]),
                    Age = int.Parse(k[11])
                });
            }
            return Customers;
        }
    }
}
