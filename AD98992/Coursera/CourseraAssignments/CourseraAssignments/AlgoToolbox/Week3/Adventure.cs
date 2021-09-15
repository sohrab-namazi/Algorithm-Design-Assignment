using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week3
{
    public class Adventure
    {
        //public static void Main(string[] args)
        //{
        //    long slotCount = long.Parse(Console.ReadLine());
        //    long[] adRevenue = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long[] averageDaily = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    Console.WriteLine(Solve(slotCount, adRevenue, averageDaily));

        //}
        public static long Solve(long slotCount, long[] adRevenue, long[] averageDailyClick)
        {
            long TotalSum = 0;
            Array.Sort(adRevenue);
            Array.Sort(averageDailyClick);
            for (int i = 0; i < Math.Min(adRevenue.Length, slotCount); i++)
            {
                TotalSum += adRevenue[i] * averageDailyClick[i];
            }
            return TotalSum;
        }
    }
}
