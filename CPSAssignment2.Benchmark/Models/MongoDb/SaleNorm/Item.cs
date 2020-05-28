﻿using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class Item
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
