﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BanktTransactionDeNorm
{
    class Transaction
    {
        public long ID { get; set; }
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }
        public long FromAccountId { get; set; }
        public long ToAccountId { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
