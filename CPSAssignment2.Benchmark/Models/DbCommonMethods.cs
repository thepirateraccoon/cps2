using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models
{
    interface DbCommonMethods : IDisposable
    {
        public void Initiate();
        public void seed(List<MasterItem> items, List<MasterCustomer> customers, MeasurementTool tool);

    }
}
