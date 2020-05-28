using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    public class SaleItem
    {
        public Sale Sale { get; set; }
        public long SaleId { get; set; }
        public long ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
