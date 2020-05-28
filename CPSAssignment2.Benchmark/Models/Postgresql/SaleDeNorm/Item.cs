using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm
{
    public class Item
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int price { get; set; }
        public List<TagItems> TagItems { get; set; }
    }
}
