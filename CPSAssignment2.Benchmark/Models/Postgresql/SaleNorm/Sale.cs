using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleNorm
{
    public class Sale
    {
        public long ID { get; set; }
        public DateTime SaleDate { get; set; }
        public Customer Customer { get; set; }
        public int SatisfactoryNumber { get; set; }
        public List<SaleItem> SaleItems { get; set; }
    }
}
