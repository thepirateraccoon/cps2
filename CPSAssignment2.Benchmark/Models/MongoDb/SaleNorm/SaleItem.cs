using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class SaleItem
    {
        public ObjectId SaleId { get; set; }
        public ObjectId ItemId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
