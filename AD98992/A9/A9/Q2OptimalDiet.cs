using System;
using System.Collections.Generic;
using TestCommon;

namespace A9
{
    public class Q2OptimalDiet : Processor
    {
        public Q2OptimalDiet(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int N, int M, double[,] matrix1)
        {
            //N : Inequality Count
            //M : variable counts
            double[] ObjectiveFunction = new double[M];
            for (long i = 0; i < M; i++)
            {
                ObjectiveFunction[i] = matrix1[N, i];
            }
            List<List<long>> Permutations = GetPermutations(N + M, M, new List<List<long>>());
            double[] BestAnswer = new double[M + 1];
            bool TotalHasAnswer = false;
               
            foreach (List<long> Inequalities in Permutations)
            {
                double[,] Matrix = new double[M, M + 1];
                int j = 0;
                foreach (long InequalityIndex in Inequalities)
                {
                    if (InequalityIndex < N)
                    {
                        for (long k = 0; k < M + 1; k++)
                        {
                            Matrix[j, k] = matrix1[InequalityIndex, k];
                        }
                    }
                    else
                    {
                        long Index = InequalityIndex - N;
                        Matrix[j, Index] = -1;
                    }
                    j++;
                }
                double[] Vertex = GaussianElimination(M, Matrix);
                bool HasAnswer = CheckAllConstraints(N, M, Vertex, matrix1);

                if (HasAnswer)
                {
                    TotalHasAnswer = true;
                    double Answer = getAnswer(M, Vertex, ObjectiveFunction);
                    if (Answer >= BestAnswer[M])
                    {
                        BestAnswer[M] = Answer;
                        UpdateAnswer(M, Vertex, ref BestAnswer);
                    }
                }
            }

            if (!TotalHasAnswer) return "No Solution";

            if (CheckInfinity(M, BestAnswer))
            {
                return "Infinity";
            }

            string Result = "Bounded Solution" + "\n";

            for (long i = 0; i < M; i++)
            {
                Result += RoundDouble(BestAnswer[i]) + " ";
            }

            return Result;
        }

        private static bool CheckInfinity(long M, double[] bestAnswer)
        {
            double Sum = 0;

            for (long i = 0; i < M; i++)
            {
                Sum += bestAnswer[i];
            }

            if (Sum > 1000_000_000) return true;

            return false;
        }

        private static void UpdateAnswer(int M, double[] vertex, ref double[] bestAnswer)
        {
            for (long i = 0; i < M; i++)
            {
                bestAnswer[i] = vertex[i];
            }
        }

        private static double getAnswer(long M, double[] Vertex, double[] ObjF)
        {
            double Answer = 0;
            for (long i = 0; i < M; i++)
            {
                Answer += Vertex[i] * ObjF[i];
            }

            return Answer;
        }

        private static bool CheckAllConstraints(long N, long M, double[] vertex, double[,] matrix1)
        {
            for (long i = 0; i < N; i++)
            {
                double LeftSide = 0;
                for (long j = 0; j < M; j++)
                {
                    LeftSide += vertex[j] * matrix1[i, j];
                }
                if (RoundDouble(LeftSide) > matrix1[i, M]) return false;
            }

            for (long i = 0; i < M; i++)
            {
                if (vertex[i] < 0) return false;
            }

            return true;
        }

        public static List<List<long>> GetPermutations(long n, long m, List<List<long>> Numbers)
        {
            if (m == 1)
            {
                for (long i = 0; i < n; i++)
                {
                    List<long> subset = new List<long>();
                    subset.Add(i);
                    Numbers.Add(subset);
                }
                return Numbers;
            }

            Numbers = GetPermutations(n, m - 1, Numbers);
            List<List<long>> NewNumbers = new List<List<long>>();
            foreach (List<long> subset in Numbers)
            {
                long last = subset[subset.Count - 1];
                for (long i = 0; i < n; i++)
                {
                    if (i > last)
                    {
                        subset.Add(i);
                        NewNumbers.Add(new List<long>(subset));
                        subset.RemoveAt(subset.Count - 1);
                    }
                }
            }

            return NewNumbers;
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
