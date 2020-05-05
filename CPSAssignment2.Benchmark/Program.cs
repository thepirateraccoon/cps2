using CPSAssignment2.Benchmark.Models;
using CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm;
using CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionDeNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm;
using CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CPSAssignment2.Benchmark
{
    class Program
    {
        private static ConcurrentQueue<MeasurementTool> queue = new ConcurrentQueue<MeasurementTool>();
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising");
            Console.WriteLine("Reading files");
            List<MasterItem> Items = ParseItem();
            List<MasterCustomer> Customers = ParseCustomer();

            DbRunner(new MonBankDeNormDbContext(), Customers,  Items);
            /*DbRunner(new MonBankNormDbContext(),   Customers,  Items);

            DbRunner(new MonSaleDeNormDbContext(), Customers,  Items);
            DbRunner(new MonSaleNormDbContext(),   Customers,  Items);

            DbRunner(new PsqlBankDeNormDbContext(),Customers,  Items);
            DbRunner(new PsqlBankNormDbContext(),  Customers,  Items);

            DbRunner(new PsqlSaleDeNormDbContext(),Customers,  Items);
            DbRunner(new PsqlSaleNormDbContext(),  Customers,  Items);*/
        }

        private static void DbRunner(DbCommonMethods db, List<MasterCustomer> customers, List<MasterItem> items) 
        {
            db.Initiate();
            using (db)
            {
                //New thread
                for (int round = 0; round <= 10; round++)
                {
                    for (int threadcount = 1; threadcount <= 8; threadcount *= 2)
                    {
                        for (int througput = 100; througput <= 100000; througput *= 10)
                        {
                            Thread[] ts = new Thread[threadcount];
                            for (int i = 0; i < ts.Length; i++)
                            {
                                ts[i] = new Thread(() => 
                                {
                                    DbCommonMethods obj = (DbCommonMethods)Activator.CreateInstance(Type.GetType(db.GetType().FullName, true));
                                    MeasurementTool tool = new MeasurementTool();
                                    obj.seed(items, customers, tool.Stopwatch);
                                    // REST OF METHODS TO TEST
                                    // PUBLISH Tool results somwhere global.
                                    queue.Enqueue(tool);
                                });
                            }
                            Barrier b = new Barrier(threadcount);
                            foreach (var t in ts)
                                t.Start();
                            foreach (var t in ts)
                                t.Join();
                        }
                    }
                }
            }
        }
        private static List<MasterItem> ParseItem()
        {
            string[] itemsfile = System.IO.File.ReadAllText(@"RandomItems.csv").Split("\n");
            List<MasterItem> Items = new List<MasterItem>();
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
        private static List<MasterCustomer> ParseCustomer()
        {
            List<MasterCustomer> Customers = new List<MasterCustomer>();
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
