using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class MonBankNormDbContext : MongoClient, IDisposable
    {
        public MonBankNormDbContext() : base("mongodb://localhost:27017")
        {
            this.DropDatabase("NormBank");
            var db = this.GetDatabase("NormBank");
            
        }

        public void Dispose()
        {
            this.DropDatabase("NormBank");
        }
    }
}
