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
            //Both CSV files has the properties: BuildAction=None, CopyToOutputDir=Always
            List<MasterItem> Items = ParseItem();
            List<MasterCustomer> Customers = ParseCustomer();
            int cmd = 0;
            if (args.Length > 0)
                int.TryParse(args[0], out cmd);
            switch (cmd)
            {
                case 0: // run all as default, arguments can be used when using multiple docker containers
                    Console.WriteLine("Running all dbs");
                    DbRunner(MonBankDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    /*
                    DbRunner(MonBankNormDbContext.GetTypeName().FullName, Customers, Items);
                    DbRunner(MonSaleDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    DbRunner(MonSaleNormDbContext.GetTypeName().FullName, Customers, Items);
                    DbRunner(PsqlBankDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    DbRunner(PsqlBankNormDbContext.GetTypeName().FullName, Customers, Items);
                    DbRunner(PsqlSaleDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    DbRunner(PsqlSaleNormDbContext.GetTypeName().FullName, Customers, Items);
                    */
                    break;
                case 1:
                    Console.WriteLine("Running MonBankDeNormDbContext");
                    DbRunner(MonBankDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 2:
                    Console.WriteLine("Running MonBankNormDbContext");
                    DbRunner(MonBankNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 3:
                    Console.WriteLine("Running MonSaleDeNormDbContext");
                    DbRunner(MonSaleDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 4:
                    Console.WriteLine("Running MonSaleNormDbContext");
                    DbRunner(MonSaleNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 5:
                    Console.WriteLine("Running PsqlBankDeNormDbContext");
                    DbRunner(PsqlBankDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 6:
                    Console.WriteLine("Running PsqlBankNormDbContext");
                    DbRunner(PsqlBankNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 7:
                    Console.WriteLine("Running PsqlSaleDeNormDbContext");
                    DbRunner(PsqlSaleDeNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
                case 8:
                    Console.WriteLine("Running PsqlSaleNormDbContext");
                    DbRunner(PsqlSaleNormDbContext.GetTypeName().FullName, Customers, Items);
                    break;
            }
            
        }

        private static void DbRunner(string db, List<MasterCustomer> customers, List<MasterItem> items)
        {
            Console.WriteLine("Running database type: " + db);
            for (int round = 0; round <= 10; round++)
            {
                for (int threadcount = 1; threadcount <= 8; threadcount *= 2)
                {
                    for (int througput = 100; througput <= 100000; througput *= 10)
                    {
                        //Write progress report
                        Console.WriteLine(
                            "\tRunning params: Round=" + round + "/10  Thread= " 
                            + threadcount + "/8  Througput=" + througput + "/100000");

                        Thread[] ts = new Thread[threadcount];
                        for (int i = 0; i < ts.Length; i++)
                        {
                            ts[i] = new Thread(() =>
                            {
                                DbCommonMethods obj = (DbCommonMethods)Activator.CreateInstance(Type.GetType(db, true));
                                MeasurementTool tool = new MeasurementTool(round, threadcount, througput, db);
                                obj.seed(items, customers, ref tool);
                                
                                queue.Enqueue(tool);
                            });
                        }
                        using (DbCommonMethods obj = (DbCommonMethods)Activator.CreateInstance(Type.GetType(db, true)))
                        {
                            //Initialize DB schemes
                            obj.Initiate();
                            foreach (var t in ts)
                                t.Start();
                            //Run workload
                            foreach (var t in ts)
                                t.Join();
                        } // TEARDOWN of DB Schemes / data
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
