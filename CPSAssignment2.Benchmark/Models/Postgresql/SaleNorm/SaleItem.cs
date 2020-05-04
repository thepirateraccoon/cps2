using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    class SaleItem
    {
        public Sale Sale { get; set; }
        public Item Item { get; set; }
        public long Quantity { get; set; }
    }
}
