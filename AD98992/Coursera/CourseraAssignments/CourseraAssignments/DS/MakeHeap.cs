using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.DS
{
    public class MakeHeap
    {
        static long k;

        //public static void Main(string[] args)
        //{
        //    long n = long.Parse(Console.ReadLine());
        //    string[] Input = Console.ReadLine().Split();
        //    long[] array = Input.Select(long.Parse).ToArray();
        //    Tuple<long, long>[] Result = Solve(array);
        //    int i = 0;
        //    foreach (Tuple<long, long> tuple in Result)
        //    {
        //        i++;
        //    }
        //    Console.WriteLine(i);
        //    foreach (Tuple<long, long> tuple in Result)
        //    {
        //        Console.WriteLine(tuple.Item1 + " " + tuple.Item2);
        //    }

        //}

        public static Tuple<long, long>[] Solve(long[] array)
        {
            List<Tuple<long, long>> Result = new List<Tuple<long, long>>();
            long k = 0;
            long Size = array.Length;

            for (long i = Size / 2; i >= 0; i--)
            {
                //Console.WriteLine(i);
                SiftDown(ref array, i, ref Result);
            }
            return Result.ToArray();

        }

        public static long SiftDown(ref long[] array, long i, ref List<Tuple<long, long>> Result)

        {
            long Size = array.Length;
            long MaxIndex = i;
            long l = LeftChild(i);
            long r = RightChild(i);

            if (l < Size && array[MaxIndex] > array[l])
            {
                MaxIndex = l;
            }

            if (r < Size && array[MaxIndex] > array[r])
            {
                MaxIndex = r;
            }

            if (i != MaxIndex)
            {
                Swap(ref array, i, MaxIndex, k, ref Result);
                k++;
                SiftDown(ref array, MaxIndex, ref Result);
            }
            //tuple = new Tuple<long, long>(Pointer, array[i]);
            return MaxIndex;
        }

        public static long LeftChild(long i)
        {
            return 2 * i + 1;
        }

        public static long RightChild(long i)
        {
            return 2 * i + 2;
        }

        public static void Swap(ref long[] array, long i, long j, long k, ref List<Tuple<long, long>> Result)
        {
            Result.Add(new Tuple<long, long>(i, j));
            long a = array[i];
            long b = array[j];
            array[i] = b;
            array[j] = a;

        }
    }
}
