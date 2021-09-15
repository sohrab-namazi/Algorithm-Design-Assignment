using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week4
{
    public class MajElement
    {

        //public static void Main(string[] args)
        //{
        //    long n = long.Parse(Console.ReadLine());
        //    long[] Nums = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long res = Solve(n, Nums);
        //    Console.WriteLine(res);
        //}

        public static long Solve(long n, long[] a)
        {
            long answer = MajorityElement(n, a, 0, n - 1);
            if (answer > 0) return 1;
            return 0;
        }

        public static long MajorityElement(long n, long[] numbers, long l, long r)
        {
            if (r <= l)
            {
                return numbers[l];
            }

            long mid = (r + l) / 2;
            long FirstMaj = MajorityElement(mid - l + 1, numbers, l, mid);
            long SecondMaj = MajorityElement(r - mid, numbers, mid + 1, r);

            if (FirstMaj == SecondMaj) return FirstMaj;

            long max = Math.Max(FirstMaj, SecondMaj);

            if (MajCount(numbers, FirstMaj, l, r + 1) > (n / 2))
            {
                return FirstMaj;
            }

            if (MajCount(numbers, SecondMaj, l, r + 1) > (n / 2)) return SecondMaj;

            return -1;
        }

        public static long MajCount(long[] numbers, long number, long l, long r)
        {
            long answer = 0;

            for (long i = l; i < r; i++)
            {
                if (numbers[i] == number) answer++;
            }
            return answer;
        }
    }
}
