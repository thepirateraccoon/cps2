using MongoDB.Bson;
using MongoDB.Driver;
using System;
using MongoDB.Driver.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    //Implements:
    //  MongoClient     : Mongodb connector
    //  IDisposable     : tears down the scheme and data and closes connection
    //  DbCommonMethods : Interface for common methods accessible by the Program.cs
    class MonSaleNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public static Type GetTypeName() { return new MonSaleNormDbContext(true).GetType(); }
        private MonSaleNormDbContext(bool b) { }
        public MonSaleNormDbContext() : base("mongodb://localhost:27017"){}
        public void Initiate()
        {
            this.DropDatabase("NormSale");
            this.GetDatabase("NormSale").CreateCollection("Item");
            this.GetDatabase("NormSale").CreateCollection("TagItem");
            this.GetDatabase("NormSale").CreateCollection("Sale");
            this.GetDatabase("NormSale").CreateCollection("SaleItem");
            this.GetDatabase("NormSale").CreateCollection("Tag");
            this.GetDatabase("NormSale").CreateCollection("Customer");
        }
        public IMongoCollection<Item> Items { get => this.GetDatabase("NormSale").GetCollection<Item>("Item"); }
        public IMongoCollection<TagItem> TagItems { get => this.GetDatabase("NormSale").GetCollection<TagItem>("TagItem"); }
        public IMongoCollection<Sale> Sales { get => this.GetDatabase("NormSale").GetCollection<Sale>("Sale"); }
        public IMongoCollection<SaleItem> SaleItems { get => this.GetDatabase("NormSale").GetCollection<SaleItem>("SaleItem"); }
        public IMongoCollection<Tag> Tags { get => this.GetDatabase("NormSale").GetCollection<Tag>("Tag"); }
        public IMongoCollection<Customer> Customers { get => this.GetDatabase("NormSale").GetCollection<Customer>("Customer"); }
        public void Seed(int dbSize, List<MasterItem> items, List<MasterCustomer> customers, List<string> tags = null)
        {
            //Structure for later use
            Dictionary<long, Customer> customerdictionary = new Dictionary<long, Customer>();
            Dictionary<long, Item> itemdictionary = new Dictionary<long, Item>();
            Dictionary<string, ObjectId > tagdictionary = new Dictionary<string, ObjectId>();

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
            Console.WriteLine("Adding tag");
            foreach (string tag in tags)
            {
                Tag tmpTag = new Tag { Name = tag };
                Tags.InsertOne(tmpTag);
                tagdictionary.Add(tag, tmpTag.ID);
            }

            //Insert items into db
            Console.WriteLine("Adding items");
            List<TagItem> dbTagItems = new List<TagItem>();
            foreach (MasterItem item in items)
            {
                Item tmpItem = new Item { Name = item.Name, Price = item.Price };
                Items.InsertOne(tmpItem);
                itemdictionary.Add(item.Id, tmpItem);

                tagdictionary.TryGetValue(item.Tag1, out ObjectId tagId);
                dbTagItems.Add(new TagItem { ItemID = tmpItem.ID, TagID = tagId });
                if (item.Tag2 != null)
                    if (tagdictionary.TryGetValue(item.Tag2, out ObjectId tagId2))
                        dbTagItems.Add(new TagItem { ItemID = tmpItem.ID, TagID = tagId2 });
                if (item.Tag3 != null)
                    if (tagdictionary.TryGetValue(item.Tag3, out ObjectId tagId3))
                        dbTagItems.Add(new TagItem { ItemID = tmpItem.ID, TagID = tagId3 });
            }
            TagItems.InsertMany(dbTagItems);



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
            List<SaleItem> saleitems = new List<SaleItem>();
            for (int i = 1; i <= dbSize; i++)
            {
                //Get user
                long tmpcid = (long)customerSelecter.Next(1, customers.Count);
                customerdictionary.TryGetValue(tmpcid, out Customer customer);

                Sale sale = new Sale
                {
                    ID = ObjectId.GenerateNewId(),
                    CustomerID = customer.ID,
                    SaleDate = startDate.AddDays(dateSelecotr.Next(range)),
                    PurchaseMethod = "Card",
                    SatisfactoryNumber = satisfactionSelecotr.Next(0, 10),
                    StoreLocation = StoreLocation.Locations[StoreSelecotr.Next(0, 9)],
                    CouponUsed = false
                };
                sales.Add(sale);
                //Define items to buy
                int itemsToBuy = itemTypeSelector.Next(1, 5);
                List<long> idUsed = new List<long>();
                for (int j = 0; j < itemsToBuy; j++)
                {
                    long tmpiid = (long)itemSelecter.Next(1, items.Count);
                    if (!idUsed.Contains(tmpiid))
                    {
                        itemdictionary.TryGetValue(tmpcid, out Item item);

                        //Define saleitem
                        SaleItem saleitem = new SaleItem
                        {
                            ItemId = item.ID,
                            SaleId = sale.ID,
                            Quantity = itemAmountSelector.Next(1,5),
                            Price = item.Price
                        };
                        saleitems.Add(saleitem);
                    }
                    else
                        break;
                }
            }
            Sales.InsertMany(sales);
            SaleItems.InsertMany(saleitems);
        }
        public void Dispose()
        {
            this.DropDatabase("NormSale");
        }

        public void Create(ref MeasurementTool tool, List<MasterItem> items = null, List<MasterCustomer> customers = null)
        {
            int workloadSize = 10_000;
            MasterCustomer[] cusArray = MasterCustomer.GenerateCustomers(500, "createNorm", 0).ToArray();
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
                string title = new_old == 0 ? "Mongo,Sale,Norm,Create,New user" : "Mongo,Sale,Norm,Create,Existing user";
                Customer customer = null;
                //New
                if (new_old == 0)
                {
                    MasterCustomer tmpcustomer = cusArray[customerGenSelector.Next(0, cusArray.Length)];
                    customer = new Customer
                    {
                        ID = ObjectId.GenerateNewId(), Age = tmpcustomer.Age, Email = tmpcustomer.Email, Gender = tmpcustomer.Gender 
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
                //Sale creation
                Sale sale = new Sale
                {
                    CustomerID = customer.ID,
                    SaleDate = startDate.AddDays(dateSelector.Next(range)),
                    CouponUsed = false,
                    PurchaseMethod = "Card",
                    SatisfactoryNumber = satisfySelector.Next(0, 10),
                    StoreLocation = StoreLocation.Locations[storeSelector.Next(0, 8)]
                };
                tool.Stopwatch.Start();
                Sales.InsertOne(sale);
                tool.Stopwatch.Stop();
                //Item selection and Saleitem creation
                List<string> usedItems = new List<string>();
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
                        SaleItem saleitem = new SaleItem
                        {
                            ItemId = foundItem.ID,
                            SaleId = sale.ID,
                            Price = foundItem.Price,
                            Quantity = itemQuant
                        };
                        tool.Stopwatch.Start();
                        SaleItems.InsertOne(saleitem);
                        tool.Stopwatch.Stop();
                    }
                    else
                        break;
                }
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
