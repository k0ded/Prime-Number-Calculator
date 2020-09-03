using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCalculator
{
    internal class Program
    {
        private readonly List<int> _primeNumbers = new List<int>();
        private DateTime _timeExecuted;
        
        public static void Main()
        {
            var program = new Program();
            program._primeNumbers.Add(2);
            program.CalculatePrimes();
        }

        private void CalculatePrimes()
        {
            Console.WriteLine("What number are you looking to find primes until?");
            var number = GetNumber();
            Console.WriteLine("Calculating numbers from 0 -> " + number );
            _timeExecuted = DateTime.Now;
            
            for (var i = 3; i < number; i++) 
            { 
                if (!IsPrime(i)) continue; 
                _primeNumbers.Add(i);
            }

            DisplayPrimes();
            TimeToDisplay();
        }

        private void TimeToDisplay()
        {
            Console.WriteLine(DateTime.Now.Subtract(_timeExecuted));
        }

        private void DisplayPrimes()
        {
            var sb = new StringBuilder();

            foreach (var t in _primeNumbers)
            {
                sb.Append(t + ",");
            }

            Console.WriteLine(sb.ToString());
        }

        private bool IsPrime(int i)
        {
            var amountPassed = 0;
            var amountWentThrough = 0;
            if (_primeNumbers.Count > 250000)
            {
                var amountOfTasks = 1 + _primeNumbers.Count / 250000;
                var taskAgreement = new bool[amountOfTasks];
                
                for (var taskNumber = 0; taskNumber < amountOfTasks ; taskNumber++)
                {
                    var number = taskNumber;
                    Task.Run(() =>
                    {
                        var wentThrough = 0;
                        var passed = 0;
                        for (var j = number * 250000;
                            j < Math.Min(_primeNumbers.Count - (amountOfTasks - number) * 250000, 250000);
                            j++)
                        {
                            wentThrough++;
                            if ((double) i / _primeNumbers[j] % 1 != 0)
                            {
                                passed++;
                            }

                            if (j + 1 == _primeNumbers.Count) continue;
                            if (_primeNumbers[j + 1] > Math.Sqrt(i))
                            {
                                break;
                            }
                        }

                        if (passed == wentThrough)
                            taskAgreement[number] = true;
                    });
                }
                
                return taskAgreement.All(agreed => agreed);
            }
            
            for (var j = 0; j < _primeNumbers.Count; j++)
            {
                amountWentThrough++;
                if ((double) i / _primeNumbers[j] % 1 != 0)
                {
                    amountPassed++;
                }

                if (j + 1 == _primeNumbers.Count) continue;
                if (_primeNumbers[j + 1] > Math.Sqrt(i))
                {
                    break;
                }
            }

            return amountPassed == amountWentThrough;
        }

        private int GetNumber()
        {
            var answer = Console.ReadLine();
            int number;
            try
            {
                number = Convert.ToInt32(answer);
            }
            catch
            {
                Console.WriteLine("That wasn't a number, please try again!");
                return GetNumber();
            }

            return number;
        }
    }
}