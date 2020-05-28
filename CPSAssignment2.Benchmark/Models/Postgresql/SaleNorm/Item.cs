using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    public class Item
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public List<TagItem> TagItems { get; set; }
        public List<SaleItem> SaleItems { get; set; }
    }
}
