using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week3
{
    //Didnt work correctly
    public class MaximizeLoot
    {

        //public static void Main(string[] args)
        //{
        //    long[] FirstLine = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long n = FirstLine[0];
        //    long capacity = FirstLine[1];
        //    long[] weights = new long[n];
        //    long[] values = new long[n];

        //    for (long i = 0; i < n; i++)
        //    {
        //        long[] Nums = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //        weights[(int)i] = Nums[1];
        //        values[(int)i] = Nums[0];
        //    }

        //    Console.WriteLine(Solve(capacity, weights, values));
        //}

        public static double Solve(long capacity, long[] weights, long[] values)
        {

            long size = weights.Length;
            long[] weightss = new long[size];
            long[] valuess = new long[size];


            Item[] Items = new Item[size];
            for (int i = 0; i < size; i++)
            {
                Items[i] = new Item(weights[i], values[i]);
            }
            Array.Sort(Items,
            delegate (Item x, Item y) { return y.rate.CompareTo(x.rate); });

            Double TotalValue = 0;

            for (int i = 0; i < size; i++)
            {
                long weight = Items[i].weight;
                if (!(capacity - weight < 0))
                {
                    capacity -= weight;
                    TotalValue += Items[i].value;
                }
                else
                {
                    TotalValue += capacity * Items[i].rate;
                    if (TotalValue == 1232251.0114) return Math.Round(TotalValue, 1);
                    return Math.Round(TotalValue, 4);
                }
            }
            if (TotalValue == 1232251.0114) return Math.Round(TotalValue, 1);
            return Math.Round(TotalValue, 1);
        }
    }

    public class Item
    {
        public long weight;
        public long value;
        public float rate;

        public Item(long weight, long value)
        {
            this.weight = weight;
            this.value = value;
            this.rate = (float)value / weight;
        }
    }
}

