using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class Transaction
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId FromAccountID { get; set; }
        public ObjectId ToAccountID { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
