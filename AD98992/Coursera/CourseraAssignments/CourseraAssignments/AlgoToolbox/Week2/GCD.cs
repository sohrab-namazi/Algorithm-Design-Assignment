using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week2
{
    public class GCD
    {
        //public static void Main(string[] args)
        //{
        //    long[] n = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    Console.WriteLine(Solve(n[0], n[1]));
        //}

        public static long Solve(long a, long b)
        {
            if (b == 0) return a;

            return Solve(b, a % b);
        }
    }
}
