using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    class MonSaleDeNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public MonSaleDeNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        public void Initiate()
        {
            this.DropDatabase("DeNormSale");
            this.GetDatabase("DeNormSale").CreateCollection("Sale");
        }
        public void Dispose()
        {
            this.DropDatabase("DeNormSale");
        }

        public void seed(List<MasterItem> items, List<MasterCustomer> customers, System.Diagnostics.Stopwatch sw)
        {
            
        }
    }
}
