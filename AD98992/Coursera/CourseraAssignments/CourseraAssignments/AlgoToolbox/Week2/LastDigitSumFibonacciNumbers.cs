using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week2
{
    public class LastDigitSumFibonacciNumbers
    {

        //public static void Main(string[] args)
        //{
        //    long n = long.Parse(Console.ReadLine());
        //    Console.WriteLine(Solve(n));
        //}

        public static long Solve(long n)
        {
            List<long> Fibs = new List<long>();

            Fibs.Add(0);
            Fibs.Add(1);
            int i = 2;
            while (true)
            {
                Fibs.Add((Fibs[i - 1] + Fibs[i - 2]) % 10);
                if (Fibs[i] == 1 && Fibs[i - 1] == 0)
                {
                    break;
                }
                i++;
            }

            long TLength = Fibs.Count - 2;

            long sum = 0;

            long r = n % TLength;
            long p = n / TLength;

            for (int o = 0; o <= r; o++) sum += Fibs[o];

            foreach (int num in Fibs) sum += num % 10;

            return (sum - 1) % 10;
        }
    }
}
