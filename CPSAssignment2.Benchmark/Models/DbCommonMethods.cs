using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models
{
    interface DbCommonMethods : IDisposable
    {
        public void Initiate();
        public void Seed(int dbSize, List<MasterItem> items = null, List<MasterCustomer> customers = null, List<string> tags = null);
        public void Create(ref MeasurementTool tool, List<MasterItem> items = null, List<MasterCustomer> customers = null);
        public void Read(ref MeasurementTool tool);
        public void Updater(ref MeasurementTool tool);

    }
}
