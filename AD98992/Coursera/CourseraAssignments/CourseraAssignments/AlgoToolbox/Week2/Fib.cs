using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week2
{
    public class Fib
    {
        //public static void Main(string[] args)
        //{
        //    long n = long.Parse(Console.ReadLine());
        //    Console.WriteLine(Solve(n));
        //}

        public static long Solve(long n)
        {
            long[] Fibs = new long[n + 1];

            if (n == 0) return 0;
            if (n == 1) return 1;

            Fibs[0] = 0;
            Fibs[1] = 1;

            for (int i = 2; i <= n; i++)
            {
                Fibs[i] = Fibs[i - 1] + Fibs[i - 2];
            }
            return Fibs[n];
        }
    }
}
