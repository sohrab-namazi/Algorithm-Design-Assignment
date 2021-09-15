using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace A5
{
    class Program
    {
        static void Main(string[] args)
        {
            long i = 0;
            while (i < 500000)
            {
                i++;
                Console.WriteLine($"log(logn) : {F1(i * 1000000)}");
                Console.WriteLine($"logn : {F2(i * 1000000)}");
                if (Math.Abs(F3(i * 100000) - long.MaxValue) < 10000) throw new Exception();
            }
        }

        private static long F2(long v)
        {
            return (long) Math.Log10(v);
        }

        private static long F3(long v)
        {
            return (long)Math.Pow(2, v);
        }

        private static object F1(long v)
        {
            return F2(F2(v));
        }
    }
}

