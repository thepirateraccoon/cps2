using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionNorm
{
    class Transaction
    {
        public long ID { get; set; }
        public Account FromAccountID { get; set; }
        public Account ToAccountID { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
