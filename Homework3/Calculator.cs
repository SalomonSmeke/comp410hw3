using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Homework3 {
    internal class Calculator {

        public void Run(NumberReader reader) {
            var results = new lockedList(new List<long>());
            var entries = new Queue<long>();

            foreach (var value in reader.ReadIntegers()) { entries.Enqueue(value); }

            var numbersToCheck = new lockedQueue(entries, entries.Count);

            StartComputationThreads(results, numbersToCheck);

            var progressMonitor = new ProgressMonitor(results);

            new Thread(progressMonitor.Run) { IsBackground = true }.Start();

            while (!numbersToCheck.Done() || !progressMonitor.Clean()) {
                Thread.Sleep(100); // wait for the computation to complete.
            }
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