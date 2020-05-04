using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.BankTransactionDeNorm
{
    class Account
    {
        public long ID { get; set; }
        public long Money { get; set; }
        public long Saldo { get; set; }
    }
}
