using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    class Sale
    {
        public long ID { get; set; }
        public DateTime SaleDate { get; set; }
        public Item[] Items { get; set; }
        public string StoreLocation { get; set; }
        public Customer Customer { get; set; }
        public bool CouponUsed { get; set; }
        public string PurchasedMethod { get; set; }
    }
}
