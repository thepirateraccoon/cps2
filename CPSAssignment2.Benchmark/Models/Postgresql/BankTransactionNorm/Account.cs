using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionNorm
{
    class Account
    {
        public long ID { get; set; }
        public User User { get; set; }
        public long Money { get; set; }
        public long Saldo { get; set; }
    }
}
