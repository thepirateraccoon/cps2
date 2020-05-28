using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class TagItem
    {
      
        public ObjectId ItemID { get; set; }
        
        public ObjectId TagID { get; set; }
    }
}
