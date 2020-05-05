using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class MonBankNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public MonBankNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        public void Initiate()
        {
            this.DropDatabase("NormBank");
            this.GetDatabase("NormBank").CreateCollection("User");
            this.GetDatabase("NormBank").CreateCollection("Transaction");
            this.GetDatabase("NormBank").CreateCollection("Account");
        }
        public void Dispose()
        {
            this.DropDatabase("NormBank");
        }

        public void seed(List<MasterItem> items, List<MasterCustomer> customers, System.Diagnostics.Stopwatch sw)
        {
            List<Account> dbAccount = new List<Account>();
            var documentusr = this.GetDatabase("NormBank").GetCollection<User>("User");
            var documentacc = this.GetDatabase("NormBank").GetCollection<Account>("Account");
            foreach (MasterCustomer x in customers)
            {
                var user = new User { Name = x.Name };
                sw.Start();
                documentusr.InsertOne(user);
                sw.Stop();
                Account[] accounts = new Account[x.Accounts];
                if (x.Accounts > 0) 
                {
                    sw.Start();
                    documentacc.InsertOne(new Account { Saldo = x.Account1, Money = x.Account1, UserId = user.ID });
                    sw.Stop();
                }
                if (x.Accounts > 1) 
                {
                    sw.Start();
                    documentacc.InsertOne(new Account { Saldo = x.Account2, Money = x.Account2, UserId = user.ID });
                    sw.Stop();
                }
                if (x.Accounts > 2)
                {
                    sw.Start();
                    documentacc.InsertOne(new Account { Saldo = x.Account3, Money = x.Account3, UserId = user.ID });
                    sw.Stop();
                }
            } 
        }
    }
}
