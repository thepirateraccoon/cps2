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
using System.Threading.Tasks;

namespace CPSAssignment2.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising");
            using (PsqlBankNormDbContext db = new PsqlBankNormDbContext())
            {
                /*if (Console.ReadLine().Contains("kill"))
                {

                }*/
            }
            using (MonBankDeNormDbContext db = new MonBankDeNormDbContext()){}
            using (MonBankNormDbContext db = new MonBankNormDbContext()) {}

            using (MonSaleDeNormDbContext db = new MonSaleDeNormDbContext()) { }
            using (MonSaleNormDbContext db = new MonSaleNormDbContext()) { }

            using (PsqlBankDeNormDbContext db = new PsqlBankDeNormDbContext()) { }
            using (PsqlBankNormDbContext db = new PsqlBankNormDbContext()) { }

            using (PsqlSaleDeNormDbContext db = new PsqlSaleDeNormDbContext()) { }
            using (PsqlSaleNormDbContext db = new PsqlSaleNormDbContext()) { }


        }
    }
}
