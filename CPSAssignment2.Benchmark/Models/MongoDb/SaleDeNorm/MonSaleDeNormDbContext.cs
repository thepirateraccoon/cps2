using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    class MonSaleDeNormDbContext : MongoClient, IDisposable
    {
        public MonSaleDeNormDbContext() : base("mongodb://localhost:27017")
        {
            this.DropDatabase("DeNormSale");
            this.GetDatabase("DeNormSale").CreateCollection("Sale");
        }

        public void Dispose()
        {
            this.DropDatabase("DeNormSale");
        }
    }
}
