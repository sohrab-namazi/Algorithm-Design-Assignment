using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AD.Graph.Week6
{
    public class SuffixArray
    {
        //public static void Main(string[] args)
        //{
        //    string text = Console.ReadLine();
        //    long[] Result = Solve(text);
        //    foreach (long res in Result) Console.Write(res + " ");
        //}

        public static long[] Solve(string text)
        {
            long[] Result = BuildSuffixArray(text);
            return Result;
        }

        private static long[] BuildSuffixArray(string S)
        {
            long[] Order = SortCharacters(S);
            long[] Class = ComputeCharClasses(S, Order);
            long L = 1;
            long Size = S.Length;
            while (L < Size)
            {
                Order = SortDoubled(S, L, Order, Class);
                Class = UpdateClasses(Order, Class, L);
                L *= 2;
            }
            return Order;
        }

        private static long[] SortCharacters(string S)
        {
            int Size = S.Length;
            long[] Order = new long[Size];
            // $ A C G T
            long[] Count = new long[5];

            for (int i = 0; i < Size; i++)
            {
                char c = S[i];
                if (c == '$') Count[0]++;
                else if (c.Equals('A')) Count[1]++;
                else if (c.Equals('C')) Count[2]++;
                else if (c.Equals('G')) Count[3]++;
                else if (c.Equals('T')) Count[4]++;
            }

            for (int j = 1; j < 5; j++)
            {
                Count[j] = Count[j] + Count[j - 1];
            }

            for (int k = Size - 1; k >= 0; k--)
            {
                char c = S[k];

                if (c.Equals('$'))
                {
                    Count[0] -= 1;
                    Order[Count[0]] = k;
                }
                else if (c.Equals('A'))
                {
                    Count[1] -= 1;
                    Order[Count[1]] = k;
                }
                else if (c.Equals('C'))
                {
                    Count[2] -= 1;
                    Order[Count[2]] = k;
                }
                else if (c.Equals('G'))
                {
                    Count[3] -= 1;
                    Order[Count[3]] = k;
                }
                else if (c.Equals('T'))
                {
                    Count[4] -= 1;
                    Order[Count[4]] = k;
                }
            }
            return Order;
        }

        private static long[] ComputeCharClasses(string S, long[] Order)
        {
            long Size = S.Length;
            long[] Class = new long[Size];
            Class[Order[0]] = 0;

            for (int i = 1; i < Size; i++)
            {
                if (!S[(int)Order[i]].Equals(S[(int)Order[i - 1]]))
                {
                    Class[(int)Order[i]] = Class[(int)Order[i - 1]] + 1;
                }
                else
                {
                    Class[Order[i]] = Class[Order[i - 1]];
                }
            }
            return Class;
        }

        private static long[] SortDoubled(string S, long L, long[] Order, long[] Class)
        {
            int Size = S.Length;
            long[] Count = new long[Size];
            long[] NewOrder = new long[Size];

            for (int i = 0; i < Size; i++)
            {
                Count[Class[i]] = Count[Class[i]] + 1;
            }

            for (int j = 1; j < Size; j++)
            {
                Count[j] = Count[j] + Count[j - 1];
            }

            for (int k = Size - 1; k >= 0; k--)
            {
                long Start = (Order[k] - L + Size) % Size;
                long CL = Class[(int)Start];
                Count[(int)CL] = Count[(int)CL] - 1;
                NewOrder[(int)Count[(int)CL]] = Start;
            }
            return NewOrder;
        }

        private static long[] UpdateClasses(long[] NewOrder, long[] Class, long L)
        {
            long Size = NewOrder.Length;
            long[] NewClass = new long[Size];
            NewClass[NewOrder[0]] = 0;

            for (int i = 1; i < Size; i++)
            {
                long Cur = NewOrder[i];
                long Prev = NewOrder[i - 1];
                long Mid = (Cur + L) % Size;
                long MidPrev = (Prev + L) % Size;

                if (Class[Cur] != Class[Prev] || Class[Mid] != Class[MidPrev])
                {
                    NewClass[Cur] = NewClass[Prev] + 1;
                }
                else
                {
                    NewClass[Cur] = NewClass[Prev];
                }
            }
            return NewClass;
        }
    }
}
