using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm
{
    class MonBankDeNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public MonBankDeNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        public void Initiate()
        {
            this.DropDatabase("DeNormBank");
            this.GetDatabase("DeNormBank").CreateCollection("User");
            this.GetDatabase("DeNormBank").CreateCollection("Transaction");
        }
        public void Dispose()
        {
            this.DropDatabase("DeNormBank");
        }

        public void seed(List<MasterItem> items, List<MasterCustomer> customers, System.Diagnostics.Stopwatch sw)
        {
            List<User> dbUsers = new List<User>();
            foreach (MasterCustomer x in customers)
            {
                List <Account> accounts = new List<Account>();
                if (x.Accounts > 0) { accounts.Add(new Account { Money = x.Account1, Saldo = x.Account1, ID = ObjectId.GenerateNewId() }); }
                if (x.Accounts > 1) { accounts.Add(new Account { Money = x.Account2, Saldo = x.Account2, ID = ObjectId.GenerateNewId() }); }
                if (x.Accounts > 2) { accounts.Add(new Account { Money = x.Account3, Saldo = x.Account3, ID = ObjectId.GenerateNewId() }); }
                var user = new User { Name = x.Name, Accounts = accounts };
                dbUsers.Add(user);
            }
            var document = this.GetDatabase("DeNormBank").GetCollection<User>("User");
            sw.Start();
            document.InsertMany(dbUsers);
            sw.Stop();
        }
    }
}
