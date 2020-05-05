using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CPSAssignment2.Benchmark
{
    //Class to measure, and store work
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
