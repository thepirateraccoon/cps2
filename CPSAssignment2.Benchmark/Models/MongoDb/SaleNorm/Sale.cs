using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class Sale
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public DateTime SaleDate { get; set; }
        public int SatisfactoryNumber { get; set; }
        public ObjectId CustomerID { get; set; }
        public string PurchaseMethod { get; set; }
        public string StoreLocation { get; set; }
        public bool CouponUsed { get; set; }
    }
}
