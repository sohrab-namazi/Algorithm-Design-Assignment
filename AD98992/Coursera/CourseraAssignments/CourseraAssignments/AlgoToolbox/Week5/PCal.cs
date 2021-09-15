using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week5
{
    public class PCal
    {
        public static long a = 0;

        //public static void Main(string[] args)
        //{
        //    long[] res = Solve(long.Parse(Console.ReadLine()));
        //    Console.WriteLine(a - 1);
        //    foreach (long r in res) Console.Write(r + " ");
        //}

        public static long[] Solve(long n)
        {

            long[] results = new long[n + 1];

            for (long i = 2; i < n + 1; i++)
            {
                long Minimum = long.MaxValue;

                if (i % 2 == 0)
                {
                    long c = 1 + results[i / 2];
                    if (c < Minimum) Minimum = c;
                }
                if (i % 3 == 0)
                {
                    long b = 1 + results[i / 3];
                    if (b < Minimum) Minimum = b;
                }

                long d = 1 + results[i - 1];

                if (d < Minimum) Minimum = d;
                results[i] = Minimum;
            }

            long[] resultPaths = new long[results[n] + 1];
            resultPaths[0] = n;
            long number = n;
            long j = 1;

            while (number != 1)
            {

                if ((number % 3 == 0) && results[number] - 1 == results[number / 3])
                {
                    resultPaths[j] = number / 3;
                    number /= 3;
                }
                else if ((number % 2 == 0) && results[number] - 1 == results[number / 2])
                {
                    resultPaths[j] = number / 2;
                    number /= 2;
                }
                else
                {
                    resultPaths[j] = number - 1;
                    number--;
                }

                j++;
            }

            a = j;

            Array.Reverse(resultPaths);

            return resultPaths;
        }
    }
}
