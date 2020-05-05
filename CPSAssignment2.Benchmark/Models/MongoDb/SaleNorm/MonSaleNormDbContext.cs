using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    //Implements:
    //  MongoClient     : Mongodb connector
    //  IDisposable     : tears down the scheme and data and closes connection
    //  DbCommonMethods : Interface for common methods accessible by the Program.cs
    class MonSaleNormDbContext : MongoClient, IDisposable, DbCommonMethods
    {
        public static Type GetTypeName() { return new MonSaleNormDbContext(true).GetType(); }
        private MonSaleNormDbContext(bool b) { }
        public MonSaleNormDbContext() : base("mongodb://localhost:27017")
        {
            
        }
        public void Initiate()
        {
            this.DropDatabase("NormSale");
            this.GetDatabase("NormSale").CreateCollection("Item");
            this.GetDatabase("NormSale").CreateCollection("ItemTag");
            this.GetDatabase("NormSale").CreateCollection("Sale");
            this.GetDatabase("NormSale").CreateCollection("SaleItem");
            this.GetDatabase("NormSale").CreateCollection("Tag");
            this.GetDatabase("NormSale").CreateCollection("Customer");
        }
        public void Dispose()
        {
            this.DropDatabase("NormSale");
        }

        public void seed(List<MasterItem> items, List<MasterCustomer> customers, MeasurementTool tool)
        {
            throw new NotImplementedException();
        }
    }
}
