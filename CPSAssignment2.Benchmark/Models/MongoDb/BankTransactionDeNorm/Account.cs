using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm
{
    class Account
    {
        //ObjectId is a way of telling mongodb that this is a ID attribute
        public ObjectId ID { get; set; }
        public long Money { get; set; }
        public long Saldo { get; set; }
    }
}
