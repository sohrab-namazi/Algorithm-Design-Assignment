using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week5
{
    public class MoneyChange
    {
        private static readonly int[] COINS = new int[] { 1, 3, 4 };

        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(Solve(long.Parse(Console.ReadLine())));
        //}

        public static long Solve(long n)
        {
            long[] results = new long[n + 1];
            results[0] = 0;
            int size = COINS.Length;

            for (long i = 1; i < n + 1; i++)
            {
                long MinCoin = long.MaxValue;
                for (int j = 0; j < size; j++)
                {
                    long ThisMinCoin = long.MaxValue;
                    if (i >= COINS[j])
                    {
                        ThisMinCoin = 1 + results[i - COINS[j]];
                    }

                    if (ThisMinCoin < MinCoin) MinCoin = ThisMinCoin;

                    results[i] = MinCoin;
                }
            }

            return results[n];
        }
    }
}
