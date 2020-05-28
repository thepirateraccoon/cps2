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
            List<string> tags = TagList(Items);
           
            List<MasterCustomer> Customers = MasterCustomer.GenerateCustomers(30);
            DbRunnerTest(PsqlSaleDeNormDbContext.GetTypeName().FullName, Customers, Items, tags);
            int cmd = 0;
            if (args.Length > 0)
                int.TryParse(args[0], out cmd);
            switch (cmd)
            {
                case 0: // run all as default, arguments can be used when using multiple docker containers
                    Console.WriteLine("Running all dbs");
                    //DbRunner(MonBankDeNormDbContext.GetTypeName().FullName, Customers, Items);
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
                    DbRunner(MonBankDeNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 2:
                    Console.WriteLine("Running MonBankNormDbContext");
                    DbRunner(MonBankNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 3:
                    Console.WriteLine("Running MonSaleDeNormDbContext");
                    DbRunner(MonSaleDeNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 4:
                    Console.WriteLine("Running MonSaleNormDbContext");
                    DbRunner(MonSaleNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 5:
                    Console.WriteLine("Running PsqlBankDeNormDbContext");
                    DbRunner(PsqlBankDeNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 6:
                    Console.WriteLine("Running PsqlBankNormDbContext");
                    DbRunner(PsqlBankNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 7:
                    Console.WriteLine("Running PsqlSaleDeNormDbContext");
                    DbRunner(PsqlSaleDeNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
                case 8:
                    Console.WriteLine("Running PsqlSaleNormDbContext");
                    DbRunner(PsqlSaleNormDbContext.GetTypeName().FullName, Customers, Items, tags);
                    break;
            }
            
        }

       

        private static void DbRunner(string db, List<MasterCustomer> customers, List<MasterItem> items, List<string> tags)
        {
            Console.WriteLine("Running database type: " + db);
            for (int round = 0; round <= 10; round++)
            {
                for (int threadcount = 1; threadcount <= 8; threadcount *= 2)
                {
                    for (int dbSizeFactor = 100; dbSizeFactor <= 100000; dbSizeFactor *= 10)
                    {
                        //Write progress report
                        Console.WriteLine(
                            "\tRunning params: Round=" + round + "/10  Thread= " 
                            + threadcount + "/8  Througput=" + dbSizeFactor + "/100000");

                        Thread[] ts = new Thread[threadcount];
                        for (int i = 0; i < ts.Length; i++)
                        {
                            ts[i] = new Thread(() =>
                            {
                                DbCommonMethods obj = (DbCommonMethods)Activator.CreateInstance(Type.GetType(db, true));
                                MeasurementTool tool = new MeasurementTool(round, threadcount, dbSizeFactor, db);
                                obj.Create(ref tool);
                                obj.Read(ref tool);
                                obj.Updater(ref tool);
                                queue.Enqueue(tool);
                            });
                        }
                        using (DbCommonMethods obj = (DbCommonMethods)Activator.CreateInstance(Type.GetType(db, true)))
                        {
                            //Initialize DB schemes
                            obj.Initiate();
                            obj.Seed(dbSizeFactor, items, customers, tags);
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
        private static void DbRunnerTest(string db, List<MasterCustomer> customers, List<MasterItem> items, List<string> tags)
        {
            Console.WriteLine("Running database Test type: " + db);
            using (DbCommonMethods obj = (DbCommonMethods)Activator.CreateInstance(Type.GetType(db, true)))
            {
                //Initialize DB schemes
                MeasurementTool tool = new MeasurementTool(1, 1, 100000, db);
                
                obj.Initiate();
                obj.Seed(1000, items, customers, tags);
                obj.Create(ref tool, items, null);
                Console.Read();
           
            }
                  
        }
        private static List<MasterItem> ParseItem()
        {
            string[] itemsfile = System.IO.File.ReadAllText(@"Items.csv", System.Text.Encoding.UTF8).Split("\n");
            List<MasterItem> Items = new List<MasterItem>();
            for (int i = 1; i < itemsfile.Length - 1; i++)
            {
                var k = (itemsfile[i]).Split(",");
                Items.Add(new MasterItem
                {
                    Id = int.Parse(k[0]),
                    Name = k[1],
                    Price = int.Parse(k[2]),
                    Tag1 = (k[3].Trim().Equals("") ? null :  k[3]),
                    Tag2 = (k[4].Equals(k[3]) || k[4].Trim().Equals("") ? null : k[4] ),
                    Tag3 = (k[5].Equals(k[4]) || k[5].Trim().Equals("") ? null : k[5])
                });
            }
            return Items;
        }
        private static List<string> TagList(List<MasterItem> items)
        {
            List<string> tags = new List<string>();
            Dictionary<string, string> taggs = new Dictionary<string, string>();
            foreach (MasterItem item in items)
            {

                if (!taggs.ContainsKey(item.Tag1)) taggs.Add(item.Tag1, null);
                if (item.Tag2 != null)
                    if (!taggs.ContainsKey(item.Tag2))
                        taggs.Add(item.Tag2, null);
                if (item.Tag3 != null)
                    if (!taggs.ContainsKey(item.Tag3))
                        taggs.Add(item.Tag3, null);
            }
            foreach (string key in taggs.Keys)
            {
                if (!key.Trim().Equals(""))
                    tags.Add(key);
            }
            return tags;
        }
    }
}
