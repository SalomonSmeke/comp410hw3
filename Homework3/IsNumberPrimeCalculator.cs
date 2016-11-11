using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3 {
    internal class IsNumberPrimeCalculator {
        private readonly lockedList _primeNumbers;
        private readonly lockedQueue _numbersToCheck;

        public IsNumberPrimeCalculator(lockedList primeNumbers, lockedQueue numbersToCheck) {
            _primeNumbers = primeNumbers;
            _numbersToCheck = numbersToCheck;
        }

        public void CheckIfNumbersArePrime() {
            while (true) {
                var numberToCheck = _numbersToCheck.Dequeue();
                if (numberToCheck > -1) //If the queue was empty, just dont do anything.
                {
                    if (IsNumberPrime(numberToCheck)) _primeNumbers.Add(numberToCheck);
                    _numbersToCheck.Consumed();
                } else return;
                //We are done. Stop hogging the cpu let other threads finish. very minor optimization. 
                //But since we have 2 threads per core. It might matter.
                //Actually, you are running this on an i7 which has hyperthreading. So Im not sure.
            }
        }

        private bool IsNumberPrime(long numberWeAreChecking) {
            if (numberWeAreChecking % 2 == 0) return false;
            
            var lastNumberToCheck = Math.Sqrt(numberWeAreChecking);
            for (var currentDivisor = 3; currentDivisor < lastNumberToCheck; currentDivisor += 2) {
                if (numberWeAreChecking % currentDivisor == 0) return false;
            }
            return true;
        }
    }
}