using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.BankTransactionDeNorm
{
    class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long AccountId { get; set; }
        public long Money { get; set; }
        public long Saldo { get; set; }
    }
}
