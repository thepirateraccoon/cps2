using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    //Implements:
    //  MongoClient     : Mongodb connector
    //  IDisposable     : tears down the scheme and data and closes connection
    //  DbCommonMethods : Interface for common methods accessible by the Program.cs
    class MonSaleDeNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public static Type GetTypeName() { return new MonSaleDeNormDbContext(true).GetType(); }
        private MonSaleDeNormDbContext(bool b) { }
        public MonSaleDeNormDbContext() : base("mongodb://localhost:27017"){}
        public void Initiate()
        {
            this.DropDatabase("DeNormSale");
            this.GetDatabase("DeNormSale").CreateCollection("Sale");
            this.GetDatabase("DeNormSale").CreateCollection("Item");
            this.GetDatabase("DeNormSale").CreateCollection("Customer");
        }
        public void Seed(int dbSize, List<MasterItem> items, List<MasterCustomer> customers, List<string> tags = null)
        {
            //Structure for later use
            Dictionary<long, Customer> customerdictionary = new Dictionary<long, Customer>();
            Dictionary<long, Item> itemdictionary = new Dictionary<long, Item>();
            
            //Insert customers into db
            Console.WriteLine("Adding customers");
            List<Customer> dbCustomers = new List<Customer>();
            foreach (MasterCustomer customer in customers)
            {
                Customer tmpCus = new Customer { Age = customer.Age, Email = customer.Email, Gender = customer.Gender };
                dbCustomers.Add(tmpCus);
                customerdictionary.Add(customer.Id, tmpCus);
            }
            var customerDoc = this.GetDatabase("DeNormSale").GetCollection<Customer>("Customer");
            customerDoc.InsertMany(dbCustomers);

            //Insert items into db
            Console.WriteLine("Adding items + tags");
            List<Item> dbItems = new List<Item>();
            foreach (MasterItem item in items)
            {
                List<string> list = new List<string>() { item.Tag1 };
                if (item.Tag2 != null) list.Add(item.Tag2);
                if (item.Tag3 != null) list.Add(item.Tag3);
                Item tmpItem = new Item { Name = item.Name, Price = item.Price, Tags = list };
                dbItems.Add(tmpItem);
                itemdictionary.Add(item.Id, tmpItem);
            }
            var itemDoc = this.GetDatabase("DeNormSale").GetCollection<Item>("Item");
            itemDoc.InsertMany(dbItems);
            

            
            //Then generate seeded sales
            Console.WriteLine("Gen seed");
            Random customerSelecter = new Random(123456);
            Random itemAmountSelector = new Random(12341);
            Random itemTypeSelector = new Random(634784);
            Random itemSelecter = new Random(6463526);
            Random dateSelecotr = new Random(3435468);
            Random satisfactionSelecotr = new Random(39843);
            Random StoreSelecotr = new Random(74626);
            DateTime startDate = new DateTime(2015, 1, 1);
            DateTime endDate = new DateTime(2020, 1, 1);
            int range = (endDate - startDate).Days;
            List<Sale> sales = new List<Sale>();
            for (int i = 1; i <= dbSize; i++)
            {
                //Get user
                long tmpcid = (long)customerSelecter.Next(1, customers.Count - 1);
                customerdictionary.TryGetValue(tmpcid, out Customer customer);
                customer.SatisfactoryNumber = satisfactionSelecotr.Next(0, 10);
                //Define items to buy
                List<Item> itemList = new List<Item>();
                int itemsToBuy = itemTypeSelector.Next(1, 5);
                List<long> idUsed = new List<long>();
                for (int j = 0; j < itemsToBuy; j++)
                {
                    long tmpiid = (long)itemSelecter.Next(1, items.Count - 1);
                    if (!idUsed.Contains(tmpiid))
                    {
                        itemdictionary.TryGetValue(tmpcid, out Item item);
                        Item tmpItem = new Item
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Price = item.Price,
                            Tags = item.Tags,
                            Quantity = itemAmountSelector.Next(1, 5)
                        };
                        itemList.Add(tmpItem);
                    }
                    else
                        break;
                }
                //Define sale
                Sale sale = new Sale
                {
                    Customer = customer,
                    Items = itemList,
                    PurchasedMethod = "Card",
                    SaleDate = startDate.AddDays(dateSelecotr.Next(range)),
                    CouponUsed = false,
                    StoreLocation = StoreLocation.Locations[StoreSelecotr.Next(0, 8)]
                };
                sales.Add(sale);
            }
            var saleDoc = this.GetDatabase("DeNormSale").GetCollection<Sale>("Sale");
            saleDoc.InsertMany(sales);
        }
        public void Dispose()
        {
            this.DropDatabase("DeNormSale");
        }
        // TESTING MEASUREMENT METHODS
        public void Create(ref MeasurementTool tool)
        {
            throw new NotImplementedException();
        }

        public void Read(ref MeasurementTool tool)
        {
            throw new NotImplementedException();
        }

        public void Updater(ref MeasurementTool tool)
        {
            throw new NotImplementedException();
        }




    }
}
