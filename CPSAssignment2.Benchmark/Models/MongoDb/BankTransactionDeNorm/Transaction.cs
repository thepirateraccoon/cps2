using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm
{
    class Transaction
    {
        public long ID { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public long FromAccountId { get; set; }
        public long ToAccountId { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
