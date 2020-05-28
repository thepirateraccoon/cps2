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
        public Dictionary<string, List<double>> Measures { get; }
        public Stopwatch Stopwatch { get => stopwatch; }
        public MeasurementTool(int round, int threadcount, int dbsize, string dbname)
        {
            stopwatch = new Stopwatch();
            querytimings = new Dictionary<string, long>();
            this.round = round;
            this.threadcount = threadcount;
            this.dbsize = dbsize;
            this.Measures = new Dictionary<string, List<double>>();
        }
        public string SaveAndReset(string title)
        {
            double mili = stopwatch.Elapsed.TotalMilliseconds;
            if (!Measures.TryGetValue(title, out List<double> current))
            {
                current = new List<double>() { mili };
                Measures.Add(title, current);
            }
            else
            {
                current.Add(mili);
            }
            stopwatch.Reset();
            return $"{title}: {mili}, size: {current.Count}";
        }
    }
}
