using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
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
        public IMongoCollection<Sale> Sales { get => this.GetDatabase("DeNormSale").GetCollection<Sale>("Sale"); }
        public IMongoCollection<Item> Items { get => this.GetDatabase("DeNormSale").GetCollection<Item>("Item"); }
        public IMongoCollection<Customer> Customers { get => this.GetDatabase("DeNormSale").GetCollection<Customer>("Customer"); }
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
            Customers.InsertMany(dbCustomers);

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
            Items.InsertMany(dbItems);
            

            
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
                long tmpcid = (long)customerSelecter.Next(1, customers.Count);
                customerdictionary.TryGetValue(tmpcid, out Customer customer);
                customer.SatisfactoryNumber = satisfactionSelecotr.Next(0, 10);
                //Define items to buy
                List<Item> itemList = new List<Item>();
                int itemsToBuy = itemTypeSelector.Next(1, 5);
                List<long> idUsed = new List<long>();
                for (int j = 0; j < itemsToBuy; j++)
                {
                    long tmpiid = (long)itemSelecter.Next(1, items.Count);
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
                    StoreLocation = StoreLocation.Locations[StoreSelecotr.Next(0, 9)]
                };
                sales.Add(sale);
            }
            Sales.InsertMany(sales);
        }
        public void Dispose()
        {
            this.DropDatabase("DeNormSale");
        }
        // TESTING MEASUREMENT METHODS
        public void Create(ref MeasurementTool tool, List<MasterItem> items = null, List<MasterCustomer> customers = null)
        {
            
            int workloadSize = 10_000;
            MasterCustomer[] cusArray = MasterCustomer.GenerateCustomers(500, "createDeNorm", 0).ToArray();
            MasterItem[] itemArray = items.ToArray();

            Random new_or_oldSelector = new Random(4573);
            Random customerGenSelector = new Random(867542);
            Random satisfySelector = new Random(2678);
            Random storeSelector = new Random(236786);
            Random diffItemSelector = new Random(83457);
            Random quantityItemSelector = new Random(6526);
            Random itemSelector = new Random(572);
            Random dateSelector = new Random(6762);
            DateTime startDate = new DateTime(2020, 1, 2);
            DateTime endDate = new DateTime(2020, 5, 28);
            int range = (endDate - startDate).Days;

            for (int i = 0; i < workloadSize; i++)
            {
                int new_old = new_or_oldSelector.Next(0, 2);
                string title = new_old == 0 ? "Mongo,Sale,DeNorm,Create,New user" : "Mongo,Sale,DeNorm,Create,Existing user";
                Customer customer = null;
                //New
                if (new_old == 0)
                {
                    MasterCustomer tmpcustomer = cusArray[customerGenSelector.Next(0, cusArray.Length)];
                    customer = new Customer
                    {
                        ID = ObjectId.GenerateNewId(),
                        Age = tmpcustomer.Age,
                        Email = tmpcustomer.Email,
                        Gender = tmpcustomer.Gender,
                    };
                    tool.Stopwatch.Start();
                    Customers.InsertOne(customer);
                    tool.Stopwatch.Stop();
                }
                //Exisiting
                else
                {
                    customer = Customers.AsQueryable().Sample(1).FirstOrDefault();
                }
                customer.SatisfactoryNumber = satisfySelector.Next(0,10);
                //Item selection and Saleitem creation
                List<string> usedItems = new List<string>();
                List<Item> itemList = new List<Item>();
                int diffItems = diffItemSelector.Next(1, 5);
                for (int j = 0; j < diffItems; j++)
                {
                    int itemIndex = itemSelector.Next(0, itemArray.Length);
                    if (!usedItems.Contains(itemArray[itemIndex].Name))
                    {
                        // Find item
                        usedItems.Add(itemArray[itemIndex].Name);
                        int itemQuant = quantityItemSelector.Next(1, 5);
                        Item foundItem = Items.Find(item => item.Name.Equals(itemArray[itemIndex].Name)).FirstOrDefault();
                        // Create SaleItem
                        itemList.Add(new Item 
                        {
                            ID = foundItem.ID,
                            Name = foundItem.Name,
                            Price = foundItem.Price,
                            Tags = foundItem.Tags,
                            Quantity = itemQuant
                        });
                    }
                    else
                        break;
                }
                //Sale creation
                Sale sale = new Sale
                {
                    Customer = customer,
                    SaleDate = startDate.AddDays(dateSelector.Next(range)),
                    CouponUsed = false,
                    PurchasedMethod = "Card",
                    StoreLocation = StoreLocation.Locations[storeSelector.Next(0, 9)],
                    Items = itemList
                };
                tool.Stopwatch.Start();
                Sales.InsertOne(sale);
                tool.Stopwatch.Stop();
                tool.SaveAndReset(title);
            }
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
