using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class User
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Name { get; set; }
    }
}
