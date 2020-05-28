using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
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
            modelBuilder.Entity<Sale>().HasKey(t => new { t.ID, t.ItemId });
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
                    price = item.Price
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
                long tmpcid = (long)customerSelecter.Next(1, customers.Count - 1);
                Customer customer = null;
                customerdictionary.TryGetValue(tmpcid, out customer);

                //Define items to buy
                int itemsToBuy = itemTypeSelector.Next(1, 5);
                List<long> idUsed = new List<long>();
                for (int j = 0; j < itemsToBuy; j++)
                {
                    long tmpiid = (long)itemSelecter.Next(1, items.Count - 1);
                    if (!idUsed.Contains(tmpiid))
                    {
                        idUsed.Add(tmpiid);
                        itemdictionary.TryGetValue(tmpiid, out Item itemtobuy);
                        var tmpSale = new Sale
                        {
                            ID = i,
                            Date = startDate.AddDays(dateSelecotr.Next(range)),
                            ItemId = itemtobuy.ID,
                            ItemName = itemtobuy.Name,
                            ItemPrice = itemtobuy.price,
                            ItemQuantity = itemAmountSelector.Next(1, 5),
                            CustomerId = customer.ID,
                            CustomerAge = customer.Age,
                            CustomerEmail = customer.Email,
                            CustomerGender = customer.Gender,
                            CustomerSatisfactoryNumber = satisfactionSelecotr.Next(0, 10),
                            CouponUsed = false,
                            PurchaseMethod = "Card",
                            StoreLocation = StoreLocation.Locations[StoreSelecotr.Next(0, 8)]
                        };
                        Sales.Add(tmpSale);
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
