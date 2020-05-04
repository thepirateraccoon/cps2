using CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.MongoTest;
using CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionDeNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.PostgresTest;
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
                if (Console.ReadLine().Contains("kill"))
                {

                }
            }
        }
    }
}
