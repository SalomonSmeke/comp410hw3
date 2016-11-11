using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Homework3 {
    internal class Calculator {

        public void Run(NumberReader reader) {
            var results = new lockedList(new List<long>()); //Create a list wrapped in an object thats a little more threadsafe.
            var entries = new Queue<long>();

            foreach (var value in reader.ReadIntegers()) entries.Enqueue(value); //Enqueue before to avoid lock contention

            var numbersToCheck = new lockedQueue(entries, entries.Count); //Wrap the queue in a threadsafe structure.

            StartComputationThreads(results, numbersToCheck);

            var progressMonitor = new ProgressMonitor(results);

            new Thread(progressMonitor.Run) { IsBackground = true }.Start();

            while (!numbersToCheck.Done() || !progressMonitor.Clean()) Thread.Sleep(100); // wait for the computation to complete. Signaled by a clean progmon and empty completed queue.
            //I thought about changing sleep to depend on how many numbers are left in the queue... but nah too much work.
            //The order of these is deliberate, since the first check will most usually be the concerning one.

            Console.WriteLine("{0} of the numbers were prime", progressMonitor.TotalCount);
        }

        private static void StartComputationThreads(lockedList results, lockedQueue numbersToCheck) {
            var threads = CreateThreads(results, numbersToCheck);
            threads.ForEach(thread => thread.Start());
        }
        
        private static List<Thread> CreateThreads(lockedList results, lockedQueue numbersToCheck) {
            var threadCount = Environment.ProcessorCount*2;

            Console.WriteLine("Using {0} compute threads and 1 I/O thread", threadCount);

            var threads =
                (from threadNumber in Sequence.Create(0, threadCount)
                    let calculator = new IsNumberPrimeCalculator(results, numbersToCheck)
                    let newThread =
                        new Thread(calculator.CheckIfNumbersArePrime) {
                            IsBackground = true,
                            Priority = ThreadPriority.BelowNormal
                        }
                    select newThread).ToList();
            return threads;
        }
    }
}