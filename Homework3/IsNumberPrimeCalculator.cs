using System;
using System.Collections.Generic;

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
                if (numberToCheck > -1)
                {
                    if (IsNumberPrime(numberToCheck))
                    {
                        _primeNumbers.Add(numberToCheck);
                    }
                    _numbersToCheck.Consumed();
                }
            }
        }

        private bool IsNumberPrime(long numberWeAreChecking) {
            if (numberWeAreChecking % 2 == 0) {
                return false;
            }
            var lastNumberToCheck = Math.Sqrt(numberWeAreChecking);
            for (var currentDivisor = 3; currentDivisor < lastNumberToCheck; currentDivisor += 2) {
                if (numberWeAreChecking % currentDivisor == 0) {
                    return false;
                }
            }
            return true;
        }
    }
}