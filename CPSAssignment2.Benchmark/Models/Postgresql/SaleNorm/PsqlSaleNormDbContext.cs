using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    class PsqlSaleNormDbContext : DbContext, IDisposable, DbCommonMethods
    {
        public static Type GetTypeName() { return new PsqlSaleNormDbContext(true).GetType(); }

        private PsqlSaleNormDbContext(bool b) { }
        public PsqlSaleNormDbContext() : base(){}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=NormSale;Username=postgres;Password=supersafe");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagItem>()
            .HasKey(t => new { t.ItemId, t.TagId });

            modelBuilder.Entity<TagItem>()
                .HasOne(pt => pt.Item)
                .WithMany(p => p.TagItems)
                .HasForeignKey(pt => pt.ItemId);

            modelBuilder.Entity<TagItem>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.TagItems)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<SaleItem>()
            .HasKey(t => new { t.ItemId, t.SaleId });

            modelBuilder.Entity<SaleItem>()
                .HasOne(pt => pt.Item)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(pt => pt.ItemId);

            modelBuilder.Entity<SaleItem>()
                .HasOne(pt => pt.Sale)
                .WithMany(t => t.SaleItems)
                .HasForeignKey(pt => pt.SaleId);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagItem> TagItems { get; set; }

        public void Initiate()
        {
            if (this.Database.CanConnect())
            {
                this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
            else
            {
                this.Database.EnsureCreated();
            }
        }
        public void Seed(int dbSize, List<MasterItem> items = null, List<MasterCustomer> customers = null, List<string> tags = null)
        {
            Dictionary<string, long> tagger = new Dictionary<string, long>();
            Dictionary<long, Customer> customerdictionary = new Dictionary<long, Customer>();
            Dictionary<long, Item> itemdictionary = new Dictionary<long, Item>();
            //Dbsize should correspond to the amount of sales already exisitng in the db, which it should be seeded with.
            //Therefore we need to generate customers, and items.
            Console.WriteLine("Adding tags");
            foreach (var tag in tags)
            {
                Tag newtag = new Tag { Name = tag };
                Tags.Add(newtag);
                SaveChanges();
                tagger.Add(newtag.Name, newtag.ID);
            }
            Console.WriteLine("Adding items");
            //First insert items into the db
            foreach (MasterItem item in items)
            {
               
                Item dbItem = new Item
                {
                    ID = item.Id,
                    Name = item.Name,
                    Price = item.Price
                };
                Items.Add(dbItem);
                itemdictionary.Add(item.Id, dbItem);
                tagger.TryGetValue(item.Tag1, out long tmp);
                TagItems.Add(new TagItem { ItemId = item.Id, TagId = tmp});
                if (item.Tag2 != null)
                {
                    tagger.TryGetValue(item.Tag2, out tmp);
                    TagItems.Add(new TagItem { ItemId = item.Id, TagId = tmp });
                }
                if (item.Tag3 != null)
                {
                    tagger.TryGetValue(item.Tag3, out tmp);
                    TagItems.Add(new TagItem { ItemId = item.Id, TagId = tmp});
                    
                } 
            }
            Console.WriteLine("Adding customers");
            //Then insert customers into the db
            foreach (MasterCustomer customer in customers)
            {
                Customer tmpCus = new Customer { ID = customer.Id, Age = customer.Age, Email = customer.Email, Gender = customer.Gender };
                Customers.Add(tmpCus);
                SaveChanges();
                customerdictionary.Add(tmpCus.ID, tmpCus);
            }
            Console.WriteLine("Gen seed");
            //Then generate seeded sales
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
            MeasurementTool tool = new MeasurementTool(0, 0, 0, "");
            for (int i = 1; i <= dbSize; i++)
            {
                long tmpcid = (long) customerSelecter.Next(1, customers.Count);
                Customer customer = null;
                customerdictionary.TryGetValue(tmpcid, out customer);
                var tmpSale = new Sale { 
                    ID = i, Customer = customer, SaleDate = startDate.AddDays(dateSelecotr.Next(range)), 
                    SatisfactoryNumber = satisfactionSelecotr.Next(0, 10),
                    CouponUsed = false, PurchaseMethod = "Card", StoreLocation = StoreLocation.Locations[StoreSelecotr.Next(0, 9)]
                };
                Sales.Add(tmpSale);
                int itemsToBuy = itemTypeSelector.Next(1, 5);
                List<long> idUsed = new List<long>();
                for (int j = 0; j < itemsToBuy; j++)
                {
                    long tmpiid =  (long) itemSelecter.Next(1, items.Count);
                    if (!idUsed.Contains(tmpiid))
                    {
                        idUsed.Add(tmpiid);
                        itemdictionary.TryGetValue(tmpiid, out Item item);
                        SaleItems.Add(new SaleItem { SaleId = tmpSale.ID, ItemId = tmpiid, Quantity = itemAmountSelector.Next(1, 5), ItemPrice = item.Price});
                    }
                    else
                        break;
                }
        
            }
            //Then insert seeded sales into the db
            SaveChanges();
        }



        public void Create(ref MeasurementTool tool, List<MasterItem> items = null, List<MasterCustomer> customers = null)
        {
            /*
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
                string title = new_old == 0 ? "PostgreSQL,Sale,Norm,Create,New user" : "PostgreSQL,Sale,Norm,Create,Existing user";
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
                        Gender = tmpcustomer.Gender
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
            }*/
        }

        public void Read(ref MeasurementTool tool)
        {
            throw new NotImplementedException();
        }

        public void Updater(ref MeasurementTool tool)
        {
            throw new NotImplementedException();
        }
        public override void Dispose()
        {
            this.Database.EnsureDeleted();
            this.Database.CloseConnection();
            base.Dispose();
        }

        
    }
}
