using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    public class TagItem
    {
        public long ItemId { get; set; }
        public Item Item { get; set; }

        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
