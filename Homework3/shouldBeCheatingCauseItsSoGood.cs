using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Homework3
{
    class shouldBeCheatingCauseItsSoGood
    {
        private Queue<long> numbersToCheck;
        private long count;
        private SemaphoreSlim STAHPQueue = new SemaphoreSlim(1);
        private SemaphoreSlim STAHP = new SemaphoreSlim(1);
        public shouldBeCheatingCauseItsSoGood(Queue<long> prebuiltQueue, long count)
        {
            numbersToCheck = prebuiltQueue;
            this.count = count;
        }

        public long Dequeue()
        {
            long output = -1;
            STAHP.Wait();
            if (count > 0)
            {
                output = numbersToCheck.Dequeue();
                count--;    
            }
            STAHP.Release();
            return output;
        }

        public long Count()
        {
            long output;
            STAHP.Wait();
            output = count;
            STAHP.Release();
            return output;
        }
    }
}
