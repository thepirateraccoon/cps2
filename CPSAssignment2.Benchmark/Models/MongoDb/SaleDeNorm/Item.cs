using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    public class Item
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public int Price { get; set; }
        [BsonIgnoreIfNull]
        public int? Quantity { get; set; }
    }
}