using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleNorm
{
    class SaleItem
    {
        public long SaleId { get; set; }
        public long ItemId { get; set; }
        public long Quantity { get; set; }
    }
}
