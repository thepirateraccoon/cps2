using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm
{
    class Sale
    {
        public long ID { get; set; }
        public DateTime Date { get; set; }
        public string ItemName { get; set; }
        public long ItemPrice { get; set; }
        public long ItemQuantity { get; set; }
        public long CustomerId { get; set; }
        public string CustomerGender { get; set; }
        public int CustomerAge { get; set; }
        public string CustomerEmail { get; set; }
        public int CustomerSatisfactoryNumber { get; set; }
        public string StoreLocation { get; set; }
        public bool CouponUsed { get; set; }
        public string PurchaseMethod { get; set; }

    }
}
