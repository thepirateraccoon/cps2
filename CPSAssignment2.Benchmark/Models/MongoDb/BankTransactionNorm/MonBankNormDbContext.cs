using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    //Implements:
    //  MongoClient     : Mongodb connector
    //  IDisposable     : tears down the scheme and data and closes connection
    //  DbCommonMethods : Interface for common methods accessible by the Program.cs
    class MonBankNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        //Used to create type on runtime, so we could reduce redundancy
        public static Type GetTypeName() { return new MonBankNormDbContext(true).GetType(); }
        private MonBankNormDbContext(bool b) { }
        //Calls the superclass with the connectionstring
        public MonBankNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        //Initialises db scheme, called at each parameter level shift
        public void Initiate()
        {
            this.DropDatabase("NormBank");
            this.GetDatabase("NormBank").CreateCollection("User");
            this.GetDatabase("NormBank").CreateCollection("Transaction");
            this.GetDatabase("NormBank").CreateCollection("Account");
        }
        //Tear down
        public void Dispose()
        {
            this.DropDatabase("NormBank");
        }
        //Seed the database but should also be the C (Create) in the CRUD.
        //The two lists are from random data from included CSV files
        //Tool is the one we are going to use to keep track of measrues
        public void seed(List<MasterItem> items, List<MasterCustomer> customers, ref MeasurementTool tool)
        {
            List<Account> dbAccount = new List<Account>();
            var documentusr = this.GetDatabase("NormBank").GetCollection<User>("User");
            var documentacc = this.GetDatabase("NormBank").GetCollection<Account>("Account");
            foreach (MasterCustomer x in customers)
            {
                var user = new User { Name = x.Name };
                tool.Stopwatch.Start();
                //Becuase of the [BsonId] in user, the InserOne alters the user object and inserts the generated id
                documentusr.InsertOne(user);
                tool.Stopwatch.Stop();
                Account[] accounts = new Account[x.Accounts];
                if (x.Accounts > 0) 
                {
                    tool.Stopwatch.Start();
                    documentacc.InsertOne(new Account { Saldo = x.Account1, Money = x.Account1, UserId = user.ID });
                    tool.Stopwatch.Stop();
                }
                if (x.Accounts > 1) 
                {
                    tool.Stopwatch.Start();
                    documentacc.InsertOne(new Account { Saldo = x.Account2, Money = x.Account2, UserId = user.ID });
                    tool.Stopwatch.Stop();
                }
                if (x.Accounts > 2)
                {
                    tool.Stopwatch.Start();
                    documentacc.InsertOne(new Account { Saldo = x.Account3, Money = x.Account3, UserId = user.ID });
                    tool.Stopwatch.Stop();
                }
            } 
        }
    }
}
