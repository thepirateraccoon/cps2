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
        //Seed the database but should also be the C (Create) in the CRUD.
        //The two lists are from random data from included CSV files
        //Tool is the one we are going to use to keep track of measrues
        public void Seed(int dbSize, List<MasterItem> items, List<MasterCustomer> customers, List<string> tags = null)
        {
            List<Account> dbAccount = new List<Account>();
            var documentusr = this.GetDatabase("NormBank").GetCollection<User>("User");
            var documentacc = this.GetDatabase("NormBank").GetCollection<Account>("Account");
            foreach (MasterCustomer x in customers)
            {
                var user = new User { Name = x.Name };

                //Becuase of the [BsonId] in user, the InserOne alters the user object and inserts the generated id
                documentusr.InsertOne(user);

                Account[] accounts = new Account[x.Accounts];
                if (x.Accounts > 0)
                {

                    documentacc.InsertOne(new Account { Saldo = x.Account1, Money = x.Account1, UserId = user.ID });

                }
                if (x.Accounts > 1)
                {

                    documentacc.InsertOne(new Account { Saldo = x.Account2, Money = x.Account2, UserId = user.ID });

                }
                if (x.Accounts > 2)
                {

                    documentacc.InsertOne(new Account { Saldo = x.Account3, Money = x.Account3, UserId = user.ID });

                }
            }
        }
        //Tear down
        public void Dispose()
        {
            this.DropDatabase("NormBank");
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
