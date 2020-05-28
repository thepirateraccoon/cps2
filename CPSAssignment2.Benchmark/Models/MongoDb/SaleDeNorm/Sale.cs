using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    class Sale
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public DateTime SaleDate { get; set; }
        public List<Item> Items { get; set; }
        public string StoreLocation { get; set; }
        public Customer Customer { get; set; }
        public bool CouponUsed { get; set; }
        public string PurchasedMethod { get; set; }
        
    }
}
