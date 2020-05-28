﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CPSAssignment2.Benchmark.Models.Postgresql.SaleDeNorm
{
    public class Customer
    {
        public long ID { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}
