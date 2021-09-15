using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AdvancedAlgorithms.Week2
{
    class Gauss
    {
        //public static void Main(string[] args)
        //{
        //    long Size = long.Parse(Console.ReadLine());
        //    double[,] matrix = new double[Size, Size + 1];
        //    for (long i = 0; i < Size; i++)
        //    {
        //        long[] Row = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //        int j = 0;
        //        foreach (long l in Row)
        //        {
        //            matrix[i, j] = l;
        //            j++;
        //        }
        //    }

        //    double[] Result =  Solve(Size, matrix);
        //    foreach (double res in Result)
        //    {
        //        if (res != (int) res)
        //        Console.Write("{0:N3}", ((double)res) + " ");
        //        else
        //        {
        //            Console.Write(res + ".000 ");
        //        }
        //    }
        //}

        public static double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            double[] Result = GaussianElimination(MATRIX_SIZE, matrix);
            return Result;
        }

        public static double[] GaussianElimination(long MatrixSize, double[,] Matrix)
        {
            double[] Result = new double[MatrixSize];
            long LastNonPivotRow = 0;
            long[] ResultIndex = new long[MatrixSize];

            while (true)
            {
                long FormerLastNnonPivotRow = LastNonPivotRow;
                long Pivot = GetPivot(ref LastNonPivotRow, Matrix, MatrixSize);
                if (Pivot == -1) break;
                if (FormerLastNnonPivotRow != LastNonPivotRow)
                    Swap(FormerLastNnonPivotRow + 1, LastNonPivotRow, MatrixSize, ref Matrix);
                ReScale(LastNonPivotRow, Pivot, ref Matrix, MatrixSize);
                SetZeroInColumn(LastNonPivotRow, Pivot, ref Matrix, MatrixSize);
                ResultIndex[LastNonPivotRow] = Pivot;
                LastNonPivotRow++;
            }

            for (long i = 0; i < MatrixSize; i++) Result[ResultIndex[i]] = (Matrix[i, MatrixSize]);
            return Result;
        }

        private static void SetZeroInColumn(long lastNonPivotRow, long pivot, ref double[,] matrix, long matrixSize)
        {
            for (long i = 0; i < matrixSize; i++)
            {
                if (i != lastNonPivotRow)
                {
                    double SameColumnWithPivot = matrix[i, pivot];
                    if (SameColumnWithPivot == 0) continue;

                    for (long j = 0; j <= matrixSize; j++)
                    {
                        matrix[i, j] -= matrix[lastNonPivotRow, j] * SameColumnWithPivot;
                    }
                }
            }
        }

        private static void ReScale(long lastNonPivotRow, long pivot, ref double[,] matrix, long MatrixSize)
        {
            double PivotValue = matrix[lastNonPivotRow, pivot];
            for (long i = 0; i <= MatrixSize; i++)
            {
                matrix[lastNonPivotRow, i] /= PivotValue;
            }
        }

        public static long GetPivot(ref long lastNonPivotRow, double[,] matrix, long MatrixSize)
        {
            while (lastNonPivotRow < MatrixSize)
            {
                for (long i = 0; i < MatrixSize; i++)
                {
                    if (matrix[lastNonPivotRow, i] != 0)
                    {
                        return i;
                    }
                }
                lastNonPivotRow++;
            }

            return -1;
        }

        public static double RoundDouble(double d)
        {
            long a = (long)d;
            double res = d - a;
            if (res >= 0)
            {
                if (res < 0.25)
                {
                    return a;
                }
                else if (res >= 0.75)
                {
                    return a + 1;
                }
                return a + 0.5;
            }
            else
            {
                if (res > -0.25)
                {
                    return a;
                }
                else if (res <= -0.75)
                {
                    return a - 1;
                }
                return a - 0.5;
            }
        }

        public static void Swap(long i, long j, long MatrixSize, ref double[,] matrix)
        {
            double[] temp = new double[MatrixSize + 1];

            for (long k = 0; k <= MatrixSize; k++)
            {
                temp[k] = matrix[i, k];
            }
            for (long k = 0; k <= MatrixSize; k++)
            {
                matrix[i, k] = matrix[j, k];
                matrix[j, k] = temp[k];
            }
        }


    }
}
