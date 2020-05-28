using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    //Implements:
    //  MongoClient     : Mongodb connector
    //  IDisposable     : tears down the scheme and data and closes connection
    //  DbCommonMethods : Interface for common methods accessible by the Program.cs
    class MonSaleDeNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public static Type GetTypeName() { return new MonSaleDeNormDbContext(true).GetType(); }
        private MonSaleDeNormDbContext(bool b) { }
        public MonSaleDeNormDbContext() : base("mongodb://localhost:27017"){}
        public void Initiate()
        {
            this.DropDatabase("DeNormSale");
            this.GetDatabase("DeNormSale").CreateCollection("Sale");
            this.GetDatabase("DeNormSale").CreateCollection("Item");
            this.GetDatabase("DeNormSale").CreateCollection("Customer");
        }
        public void Seed(int dbSize, List<MasterItem> items, List<MasterCustomer> customers, List<string> tags = null)
        {
            
        }
        public void Dispose()
        {
            this.DropDatabase("DeNormSale");
        }
        // TESTING MEASUREMENT METHODS
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
