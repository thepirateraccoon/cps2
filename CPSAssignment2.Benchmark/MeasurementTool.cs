using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CPSAssignment2.Benchmark
{
    class MeasurementTool
    {
        private readonly Stopwatch stopwatch;
        public Stopwatch Stopwatch { get => stopwatch; }
        public MeasurementTool()
        {
            stopwatch = new Stopwatch();
        }
       
    }
}
