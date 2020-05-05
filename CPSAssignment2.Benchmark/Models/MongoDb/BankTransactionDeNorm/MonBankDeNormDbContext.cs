using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm
{
    //Implements:
    //  MongoClient     : Mongodb connector
    //  IDisposable     : tears down the scheme and data and closes connection
    //  DbCommonMethods : Interface for common methods accessible by the Program.cs
    class MonBankDeNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        //Used to create type on runtime, so we could reduce redundancy
        public static Type GetTypeName() { return new MonBankDeNormDbContext(true).GetType();  }
        private MonBankDeNormDbContext(bool b) { }
        //Calls the superclass with the connectionstring
        public MonBankDeNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        //Initialises db scheme, called at each parameter level shift
        public void Initiate()
        {
            this.DropDatabase("DeNormBank");
            this.GetDatabase("DeNormBank").CreateCollection("User");
            this.GetDatabase("DeNormBank").CreateCollection("Transaction");
        }
        //Tear down
        public void Dispose()
        {
            this.DropDatabase("DeNormBank");
        }
        //Seed the database but should also be the C (Create) in the CRUD.
        //The two lists are from random data from included CSV files
        //Tool is the one we are going to use to keep track of measrues
        public void seed(List<MasterItem> items, List<MasterCustomer> customers, MeasurementTool tool)
        {
            List<User> dbUsers = new List<User>();
            foreach (MasterCustomer x in customers)
            {
                List <Account> accounts = new List<Account>();
                //ObjectId.GenerateNewId() generate a new obj id since the idObject in Account wont be recocgnized as primary key.
                if (x.Accounts > 0) { accounts.Add(new Account { Money = x.Account1, Saldo = x.Account1, ID = ObjectId.GenerateNewId() }); }
                if (x.Accounts > 1) { accounts.Add(new Account { Money = x.Account2, Saldo = x.Account2, ID = ObjectId.GenerateNewId() }); }
                if (x.Accounts > 2) { accounts.Add(new Account { Money = x.Account3, Saldo = x.Account3, ID = ObjectId.GenerateNewId() }); }
                var user = new User { Name = x.Name, Accounts = accounts };
                dbUsers.Add(user);
            }
            var document = this.GetDatabase("DeNormBank").GetCollection<User>("User");
            tool.Stopwatch.Start();
            document.InsertMany(dbUsers);
            tool.Stopwatch.Stop();
        }
    }
}
