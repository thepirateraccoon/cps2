using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm
{
    public class TagItems
    {
        public long ItemId { get; set; }
        public Item Item { get; set; }

        public long TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
