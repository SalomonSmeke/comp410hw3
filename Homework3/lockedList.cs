using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework3
{
    class lockedList
    {
        private List<long> list;
        private SemaphoreSlim STAHP = new SemaphoreSlim(1);

        public lockedList(List<long> list) { this.list = list; }

        public void Add(long item)
        {
            STAHP.Wait();
            list.Add(item);
            STAHP.Release();
        }

        public long CountClear()
        {
            long output = 0;
            STAHP.Wait();
            output = list.Count;
            list.Clear();
            STAHP.Release();
            return output;
        }

        public long Count()
        {
            return list.Count();
        }
    }
}
