﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class Transaction
    {
        //ObjectId is a way of telling mongodb that this is the ID attribute
        //BsonId is the attribut that defines the outmost id of a document (row record ~= collection document)
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId FromAccountID { get; set; }
        public ObjectId ToAccountID { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
