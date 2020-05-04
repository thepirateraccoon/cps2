using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BanktTransactionDeNorm
{
    class MonBankDeNormDbContext : MongoClient, IDisposable
    {
        public MonBankDeNormDbContext() : base("mongodb://localhost:27017")
        {
            this.DropDatabase("DeNormBank");
            this.GetDatabase("DeNormBank");
        }

        public void Dispose()
        {
            this.DropDatabase("DeNormBank");
        }
    }
}
