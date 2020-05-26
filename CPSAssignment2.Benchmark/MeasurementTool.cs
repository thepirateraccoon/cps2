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
        private readonly int round;
        private readonly int threadcount;
        private readonly int dbsize;
        private readonly string DbName;
        private readonly Dictionary<string, long> querytimings;
        public Stopwatch Stopwatch { get => stopwatch; }
        public MeasurementTool(int round, int threadcount, int dbsize, string dbname)
        {
            stopwatch = new Stopwatch();
            querytimings = new Dictionary<string, long>();
            this.round = round;
            this.threadcount = threadcount;
            this.dbsize = dbsize;
        }
    }
}
