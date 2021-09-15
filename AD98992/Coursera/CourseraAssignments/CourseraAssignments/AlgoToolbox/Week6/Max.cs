using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week6
{
    public class Max
    {
        //public static void Main(string[] args)
        //{
        //    string input = Console.ReadLine();
        //    long res = Solve(input);
        //    Console.WriteLine(res);
        //}

        public static long Solve(string expression)
        {
            long Size = expression.Length;
            long OpSize = (Size - 1) / 2;
            long NumSize = Size - ((Size - 1) / 2);
            //- : - 1
            //+ : 0
            //* : + 1
            long[] Operators = new long[OpSize];
            long[] Numbers = new long[NumSize];

            long[,] Maxs = new long[NumSize, NumSize];
            long[,] Mins = new long[NumSize, NumSize];

            for (int i = 0; i < Size; i++)
            {
                if (i % 2 == 0)
                {
                    Numbers[(i / 2)] = long.Parse(expression[i].ToString());
                }
                else
                {
                    if (expression[i] == '+')
                    {
                        Operators[(i - 1) / 2] = 0;
                    }
                    else if (expression[i] == '*')
                    {
                        Operators[(i - 1) / 2] = 1;
                    }
                    else
                    {
                        Operators[(i - 1) / 2] = -1;
                    }
                }
            }

            for (int i = 0; i < NumSize; i++)
            {
                Mins[i, i] = Numbers[i];
                Maxs[i, i] = Numbers[i];
            }

            for (int d = 1; d < NumSize; d++)
            {
                for (int i = 0; i < NumSize - d; i++)
                {
                    int j = i + d;
                    long[] MaxValues = new long[] { };
                    long[] MinValues = new long[] { };
                    MinAndMax(ref Maxs, ref Mins, i, j, Operators);
                }
            }

            return Maxs[0, NumSize - 1];
        }

        public static void MinAndMax(ref long[,] Maxs, ref long[,] Mins, long i, long j, long[] Operators)
        {
            long Min = long.MaxValue;
            long Max = long.MinValue;
            long a;
            long b;
            long c;
            long d;

            for (long k = i; k < j; k++)
            {
                long Operator = Operators[k];
                if (Operator > 0)
                {
                    a = Maxs[i, k] * Maxs[k + 1, j];
                    b = Maxs[i, k] * Mins[k + 1, j];
                    c = Mins[i, k] * Maxs[k + 1, j];
                    d = Mins[i, k] * Mins[k + 1, j];

                }
                else if (Operator == 0)
                {
                    a = Maxs[i, k] + Maxs[k + 1, j];
                    b = Maxs[i, k] + Mins[k + 1, j];
                    c = Mins[i, k] + Maxs[k + 1, j];
                    d = Mins[i, k] + Mins[k + 1, j];

                }
                else
                {
                    a = Maxs[i, k] - Maxs[k + 1, j];
                    b = Maxs[i, k] - Mins[k + 1, j];
                    c = Mins[i, k] - Maxs[k + 1, j];
                    d = Mins[i, k] - Mins[k + 1, j];
                }


                Min = Math.Min(Min, Math.Min(a, Math.Min(b, Math.Min(c, d))));
                Max = Math.Max(Max, Math.Max(a, Math.Max(b, Math.Max(c, d))));
            }

            Maxs[i, j] = Max;
            Mins[i, j] = Min;

        }
    }
}
