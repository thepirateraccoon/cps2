using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class MonSaleNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public MonSaleNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        public void Initiate()
        {
            this.DropDatabase("NormSale");
            this.GetDatabase("NormSale").CreateCollection("Item");
            this.GetDatabase("NormSale").CreateCollection("ItemTag");
            this.GetDatabase("NormSale").CreateCollection("Sale");
            this.GetDatabase("NormSale").CreateCollection("SaleItem");
            this.GetDatabase("NormSale").CreateCollection("Tag");
            this.GetDatabase("NormSale").CreateCollection("Customer");
        }
        public void Dispose()
        {
            this.DropDatabase("NormSale");
        }

        public void seed(List<MasterItem> items, List<MasterCustomer> customers, System.Diagnostics.Stopwatch sw)
        {
            throw new NotImplementedException();
        }
    }
}
