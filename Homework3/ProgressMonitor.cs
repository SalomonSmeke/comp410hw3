using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3 {
    internal class ProgressMonitor {
        private readonly lockedList _results;
        public long TotalCount = 0;
        public long count = 0;
        public SemaphoreSlim STAHP = new SemaphoreSlim(1);

        public ProgressMonitor(lockedList results) { _results = results; }

        public void Run() {
            while (true) {
                Thread.Sleep(100); // wait for 1/10th of a second
                STAHP.Wait();
                count = _results.CountClear();
                TotalCount += count; // clear out the current primes to save some memory
                STAHP.Release();
                if (count != 0) Console.WriteLine("{0} primes found so far", TotalCount);
            }
        }
        
        public bool Clean()
        {
            bool output;
            STAHP.Wait();
            output = _results.Count() == 0;
            STAHP.Release();
            return output;
        }
    }
}