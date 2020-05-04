using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionNorm
{
    class Transaction
    {
        public long ID { get; set; }
        public long FromAccountID { get; set; }
        public long ToAccountID { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
