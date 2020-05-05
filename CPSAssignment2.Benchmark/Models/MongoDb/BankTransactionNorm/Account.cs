using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ID { get; set; }
        public ObjectId UserId { get; set; }
        public long Money { get; set; }
        public long Saldo { get; set; }
    }
}
