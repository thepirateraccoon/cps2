using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm
{
    class PsqlSaleDeNormDbContext : DbContext, IDisposable, DbCommonMethods
    {
        public static Type GetTypeName() { return new PsqlSaleDeNormDbContext(true).GetType(); }
        private PsqlSaleDeNormDbContext(bool b) { }
        public PsqlSaleDeNormDbContext() : base(){}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=DeNormSale;Username=postgres;Password=supersafe");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Sale>().Property(t => t.ID).HasIdentityOptions(1, 1);
            modelBuilder.Entity<Sale>().HasKey(t => new { t.ID, t.ItemId });

            modelBuilder.Entity<Sale>().Property(t => t.ID);
            modelBuilder.Entity<Customer>().HasKey(t => new { t.ID });
            modelBuilder.Entity<TagItems>()
            .HasKey(t => new { t.ItemId, t.TagId });

            modelBuilder.Entity<TagItems>()
                .HasOne(pt => pt.Item)
                .WithMany(p => p.TagItems)
                .HasForeignKey(pt => pt.ItemId);

            modelBuilder.Entity<TagItems>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.TagItems)
                .HasForeignKey(pt => pt.TagId);

        }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<TagItems> ItemTags { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Customer> Customers { get; set; }
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
        public void Seed(int dbSize, List<MasterItem> items, List<MasterCustomer> customers, List<string> tags = null)
        {
            Dictionary<string, long> tagger = new Dictionary<string, long>();
            Dictionary<long, Customer> customerdictionary = new Dictionary<long, Customer>();
            Dictionary<long, Item> itemdictionary = new Dictionary<long, Item>();

            Console.WriteLine("Adding customers");
            //Then insert customers into the db
            foreach (MasterCustomer customer in customers)
            {
                Customer tmpCus = new Customer { ID = customer.Id, Age = customer.Age, Email = customer.Email, Gender = customer.Gender };
                Customers.Add(tmpCus);
                SaveChanges();
                customerdictionary.Add(tmpCus.ID, tmpCus);
            }
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
                itemdictionary.Add(item.Id, dbItem);
                Items.Add(dbItem);
                long tmp = -1;
                tagger.TryGetValue(item.Tag1, out tmp);
                ItemTags.Add(new TagItems { ItemId = item.Id, TagId = tmp });
                if (item.Tag2 != null)
                {
                    tagger.TryGetValue(item.Tag2, out tmp);
                    ItemTags.Add(new TagItems { ItemId = item.Id, TagId = tmp });
                }
                if (item.Tag3 != null)
                {
                    tagger.TryGetValue(item.Tag3, out tmp);
                    ItemTags.Add(new TagItems { ItemId = item.Id, TagId = tmp });
                }
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
                List<Sale> sameSale = new List<Sale>();
                //Define Once
                long tmpcid = (long)customerSelecter.Next(1, customers.Count);
                Customer customer = null;
                customerdictionary.TryGetValue(tmpcid, out customer);

                //Define items to buy
                int itemsToBuy = itemTypeSelector.Next(1, 5);
                long? tmpSaleId = null;
                List<long> idUsed = new List<long>();
                for (int j = 0; j < itemsToBuy; j++)
                {
                    long tmpiid = (long)itemSelecter.Next(1, items.Count);
                    if (!idUsed.Contains(tmpiid))
                    {
                        idUsed.Add(tmpiid);
                        itemdictionary.TryGetValue(tmpiid, out Item itemtobuy);
                        var tmpSale = new Sale
                        {
                            //ID = tmpSaleId,
                            Date = startDate.AddDays(dateSelecotr.Next(range)),
                            ItemId = itemtobuy.ID,
                            ItemName = itemtobuy.Name,
                            ItemPrice = itemtobuy.Price,
                            ItemQuantity = itemAmountSelector.Next(1, 5),
                            CustomerId = customer.ID,
                            CustomerAge = customer.Age,
                            CustomerEmail = customer.Email,
                            CustomerGender = customer.Gender,
                            CustomerSatisfactoryNumber = satisfactionSelecotr.Next(0, 10),
                            CouponUsed = false,
                            PurchaseMethod = "Card",
                            StoreLocation = StoreLocation.Locations[StoreSelecotr.Next(0, 9)]
                        };
                        Sales.Add(tmpSale);
                        
                       
                        Console.WriteLine(tmpSale.ID);
                        tmpSaleId = tmpSale.ID;
                    }
                    else
                        break;
                }
            }
            //Then insert seeded sales into the db
            SaveChanges();
        }
        public override void Dispose()
        {
            this.Database.EnsureDeleted();
            this.Database.CloseConnection();
            base.Dispose();
        }

        

        public void Create(ref MeasurementTool tool, List<MasterItem> items = null, List<MasterCustomer> customers = null)
        {
            int workloadSize = 10_00;
            long maxVal = Customers.OrderByDescending(cus => cus.ID).First().ID;
            MasterCustomer[] cusArray = MasterCustomer.GenerateCustomers(500, "createDeNorm", maxVal).ToArray();
            MasterItem[] itemArray = items.ToArray();

            Random new_or_oldSelector = new Random(4573);
            Random customerGenSelector = new Random(867542);
            Random customerSelector = new Random(456542);
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
                Console.WriteLine($"{i} ud af {workloadSize}");
                int new_old = new_or_oldSelector.Next(0, 2);
                string title = new_old == 0 ? "PostgreSQL,Sale,DeNorm,Create,New user" : "PostgreSQL,Sale,DeNorm,Create,Existing user";
                Customer customer = null;
                //New
                if (new_old == 0)
                {
                    MasterCustomer tmpcustomer = cusArray[customerGenSelector.Next(0, cusArray.Length)];
                    customer = new Customer
                    {
                        Age = tmpcustomer.Age,
                        Email = tmpcustomer.Email,
                        Gender = tmpcustomer.Gender,
                    };
                    tool.Stopwatch.Start();
                    Customers.Add(customer);
                    Console.WriteLine(customer.ID);
                    SaveChanges();
                    tool.Stopwatch.Stop();
                }
                //Exisiting
                else
                {
                    int max = (int)Customers.OrderByDescending(cus => cus.ID).First().ID;
                    long ranpos = (long)  customerSelector.Next(1, max);
                    customer = Customers.Find(ranpos);
                }
                int satisfactorynumber = satisfySelector.Next(0, 10);
                //Item selection and Saleitem creation
                List<string> usedItems = new List<string>();
                int diffItems = diffItemSelector.Next(1, 5);
                long? tmpSaleId = null;
                for (int j = 0; j < diffItems; j++)
                {
                    int itemIndex = itemSelector.Next(0, itemArray.Length);
                    if (!usedItems.Contains(itemArray[itemIndex].Name))
                    {
                        // Find item
                        usedItems.Add(itemArray[itemIndex].Name);
                        int itemQuant = quantityItemSelector.Next(1, 5);
                        Item foundItem = Items.Where(it => it.Name.Equals(itemArray[itemIndex].Name)).First();
                        //Sale creation
                        Sale sale = new Sale
                        {
                            ID = tmpSaleId,
                            ItemId = foundItem.ID,
                            ItemName = foundItem.Name,
                            ItemPrice = foundItem.Price,
                            ItemQuantity = itemQuant,
                            CustomerId = customer.ID,
                            CustomerAge = customer.Age,
                            CustomerEmail = customer.Email,
                            CustomerGender = customer.Gender,
                            Date = startDate.AddDays(dateSelector.Next(range)),
                            CouponUsed = false,
                            PurchaseMethod = "Card",
                            StoreLocation = StoreLocation.Locations[storeSelector.Next(0, 9)],
                            CustomerSatisfactoryNumber = satisfactorynumber
                        };
                        tool.Stopwatch.Start();
                        Sales.Add(sale);
                        SaveChanges();
                        tool.Stopwatch.Stop();
                        tmpSaleId = sale.ID;
                    }
                    else
                        break;
                }
                //Saves measurements every sale update
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
