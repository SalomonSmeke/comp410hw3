using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Homework3
{
    class lockedQueue
    {
        private Queue<long> numbersToCheck;
        private long count;
        private long init;
        private long complete;
        private SemaphoreSlim STAHP = new SemaphoreSlim(1);
        public lockedQueue(Queue<long> prebuiltQueue, long count)
        {
            numbersToCheck = prebuiltQueue;
            this.count = count;
            init = count;
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

        public bool Done(){ return complete==init; }

        public void Consumed() {
            STAHP.Wait();
            complete++;
            STAHP.Release();
        }
    }
}
