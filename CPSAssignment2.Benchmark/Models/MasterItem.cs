﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models
{
    //Class to hold all data from random data in CSV
    class MasterItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
    }
}
