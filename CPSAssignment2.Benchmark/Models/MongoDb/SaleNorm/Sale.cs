using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class Sale
    {
        public long ID { get; set; }
        public DateTime SaleDate { get; set; }
        public int SatisfactoryNumber { get; set; }
        public long CustomerID { get; set; }
    }
}
