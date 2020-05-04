using System;
using System.Collections.Generic;
using System.Text;

namespace CPSAssignment2.Benchmark.Models
{
    interface ICRUD<T>
    {
        public void Create(T obj);
        public void Create(List<T> objs);
        public void Read(T obj);
        public void Update(T current, T updater);
        public void Delete(T obj);
    }
}
