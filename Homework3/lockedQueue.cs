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
        public lockedQueue(Queue<long> prebuiltQueue, long count) //Initialize with the number of elements. No need to call .Count
        {
            numbersToCheck = prebuiltQueue;
            this.count = count;
            init = count; //Numbers this queue was initialized with.
        }

        public long Dequeue() //Locked wrapper for Dequeue.
        {
            long output = -1;
            STAHP.Wait();
            if (count > 0)
            {
                output = numbersToCheck.Dequeue();
                count--; //Avoid asking the queue for a count.    
            }
            STAHP.Release();
            return output;
        }

        public bool Done(){ return init==complete; }

        public void Consumed() {
            Interlocked.Increment(ref complete); 
            //Interocked is amazing. If I did some more restructuring after finding that out
            //I probably could have gotten another second faster here. A+ 
        }
    }
}
