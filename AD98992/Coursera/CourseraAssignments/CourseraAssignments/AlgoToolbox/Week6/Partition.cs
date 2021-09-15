using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week6
{
    public class Partition
    {

        //public static void Main(string[] args)
        //{
        //    long n = long.Parse(Console.ReadLine());
        //    long[] Nums = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long res = Solve(n, Nums);
        //    Console.WriteLine(res);
        //}

        public static long Solve(long souvenirsCount, long[] souvenirs)
        {
            if (souvenirsCount < 3) return 0;
            long sum = 0;
            foreach (long a in souvenirs) sum += a;
            if (sum % 3 != 0) return 0;
            if (subsetSum(souvenirs, souvenirs.Length - 1, sum / 3, sum / 3, sum / 3)) return 1;
            return 0;
        }


        public static bool subsetSum(long[] souvenirs, long size, long a, long b, long c)
        {
            if (a == 0 && b == 0 && c == 0) return true;

            bool One = false;
            if (a - souvenirs[size] >= 0)
            {
                One = subsetSum(souvenirs, size - 1, a - souvenirs[size], b, c);
            }

            bool Two = false;
            if (!One && (b - souvenirs[size] >= 0))
            {
                Two = subsetSum(souvenirs, size - 1, a, b - souvenirs[size], c);
            }

            bool Three = false;
            if ((!One && !Two) && (c - souvenirs[size] >= 0))
            {
                Three = subsetSum(souvenirs, size - 1, a, b, c - souvenirs[size]);
            }

            return One || Two || Three;
        }
    }
}
