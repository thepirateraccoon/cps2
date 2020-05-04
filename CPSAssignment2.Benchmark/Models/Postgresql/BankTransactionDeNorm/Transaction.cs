using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionDeNorm
{
    class Transaction
    {
        public long ID { get; set; }
        public long FromAccount { get; set; }
        public long ToAccount { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
