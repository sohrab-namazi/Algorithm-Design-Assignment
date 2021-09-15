using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week3
{
    public class ChangeMoney
    {
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(Solve(long.Parse(Console.ReadLine())));
        //}

        public static long Solve(long money)
        {
            //int[] coins = new int[3] { 10, 5, 1 };
            long CoinCounts = 0;
            CoinCounts += money / 10;
            money = money % 10;
            if (money < 10 && money >= 5)
            {
                CoinCounts++;
                money -= 5;
            }
            CoinCounts += money;
            return CoinCounts;
        }
    }
}
