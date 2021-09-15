using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week4
{
    //differ with coursera type
    public class BS
    {
        //public static void Main(string[] args)
        //{
        //    long[] Nums1 = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long[] Num2 = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long[] res = Solve(Num2, Nums1);
        //    foreach (long r in res) Console.Write(r + " ");

        //}

        public static long[] Solve(long[] a, long[] b)
        {
            long size = b.Length;
            long[] answers = new long[size];
            for (int i = 0; i < size; i++)
            {
                answers[i] = BSearch(a, b[i], 0, a.Length - 1);

            }
            return answers;
        }

        public static long BSearch(long[] numbers, long n, long l, long r)
        {
            if (l >= r)
            {
                if (numbers[l] == n) return l;
                else return -1;
            }
            long MidIndex = (l + r) / 2;
            if (numbers[MidIndex] == n)
            {
                return MidIndex;
            }
            else if (numbers[MidIndex] > n)
            {
                return (BSearch(numbers, n, l, MidIndex));
            }
            else
            {
                return (BSearch(numbers, n, MidIndex + 1, r));
            }
        }
    }
}
