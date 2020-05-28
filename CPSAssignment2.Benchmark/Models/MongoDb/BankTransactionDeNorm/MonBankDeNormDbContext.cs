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
        //Seed the database but should also be the C (Create) in the CRUD.
        //The two lists are from random data from included CSV files
        public void Seed(int dbSize, List<MasterItem> items, List<MasterCustomer> customers, List<string> tags = null)
        {
            List<User> dbUsers = new List<User>();
            foreach (MasterCustomer x in customers)
            {
                List<Account> accounts = new List<Account>();
                //ObjectId.GenerateNewId() generate a new obj id since the idObject in Account wont be recocgnized as primary key.
                if (x.Accounts > 0) { accounts.Add(new Account { Money = x.Account1, Saldo = x.Account1, ID = ObjectId.GenerateNewId() }); }
                if (x.Accounts > 1) { accounts.Add(new Account { Money = x.Account2, Saldo = x.Account2, ID = ObjectId.GenerateNewId() }); }
                if (x.Accounts > 2) { accounts.Add(new Account { Money = x.Account3, Saldo = x.Account3, ID = ObjectId.GenerateNewId() }); }
                var user = new User { Name = x.Name, Accounts = accounts };
                dbUsers.Add(user);
            }
            var document = this.GetDatabase("DeNormBank").GetCollection<User>("User");
  
            document.InsertMany(dbUsers);
           
        }
        //Tear down
        public void Dispose()
        {
            this.DropDatabase("DeNormBank");
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
