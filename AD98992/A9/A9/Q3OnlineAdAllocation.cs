using System;
using System.Collections.Generic;
using TestCommon;

namespace A9
{
    public class Q3OnlineAdAllocation : Processor
    {

        public Q3OnlineAdAllocation(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int M, int N, double[,] matrix1)
        {
            //N : Variables Count
            //M : Inequalities Count
            long[] RowVariables = new long[M];
            double[,] Table = CreateTable(M, N, matrix1, ref RowVariables);
            // double[] ObjectiveFunctions = GetObjectiveFunction(M, N, matrix1);


            while (true)
            {
                int EntryVariable = FindEntryVariable(M, N, Table);
                if (EntryVariable < 0) break;
                long DepartingVariable = FindDepartingVariable(M, N, Table, EntryVariable);
                if (DepartingVariable < 0) return "Infinity";
                RowVariables[DepartingVariable] = EntryVariable;
                ReScale(DepartingVariable, EntryVariable, ref Table, M + N);
                SetZeroInColumn(DepartingVariable, EntryVariable, ref Table, M, N);
            }



            string Solution = "Bounded Solution" + "\n";
            double[] Answers = new double[N];
            for (long i = 0; i < M; i++)
            {
                if (RowVariables[i] < N)
                {
                    Answers[RowVariables[i]] = Table[i, N + M];
                }
            }

            bool HasAnswer = CheckAllConstraints(M, N, Answers, matrix1);

            if (!HasAnswer) return "No Solution";

            for (long i = 0; i < N; i++)
            {
                Solution += RoundDouble(Answers[i]) + " ";
            }

            return Solution;
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

        private static double[] GetObjectiveFunction(int M, int N, double[,] matrix1)
        {
            double[] ObjectiveFunction = new double[M];
            for (long i = 0; i < M; i++)
            {
                ObjectiveFunction[i] = matrix1[N, i];
            }

            return ObjectiveFunction;
        }

        private static int FindDepartingVariable(int M, int N, double[,] Table, int EntryVariable)
        {
            int DepartingIndex = -1;
            double DepartingValue = long.MaxValue;

            for (int i = 0; i < M; i++)
            {
                double candidate = Table[i, M + N] / Table[i, EntryVariable];
                if (candidate >= 0 && candidate < DepartingValue)
                {
                    DepartingValue = candidate;
                    DepartingIndex = i;
                }
            }

            return DepartingIndex;
        }

        private static int FindEntryVariable(int M, long N, double[,] Table)
        {
            int EntryIndex = -1;
            double EntryValue = long.MaxValue;
            for (int i = 0; i < N + M; i++)
            {
                if (Table[M, i] < 0 && Table[M, i] < EntryValue)
                {
                    EntryValue = Table[M, i];
                    EntryIndex = i;
                }
            }

            return EntryIndex;
        }

        private static double[,] CreateTable(int M, int N, double[,] matrix1, ref long[] RowVariables)
        {
            double[,] Table = new double[M + 1, N + M + 1];
            for (long i = 0; i < M + 1; i++)
            {
                if (i != M) RowVariables[i] = N + i;
                for (long j = 0; j < N; j++)
                {
                    Table[i, j] = matrix1[i, j];
                }
                Table[i, N + i] = 1;
                Table[i, N + M] = matrix1[i, N];
            }
            for (long i = 0; i < N; i++)
            {
                Table[M, i] *= -1;
            }
            return Table;
        }

        private static void ReScale(long lastNonPivotRow, long pivot, ref double[,] matrix, long MatrixSize)
        {
            double PivotValue = matrix[lastNonPivotRow, pivot];
            for (long i = 0; i <= MatrixSize; i++)
            {
                matrix[lastNonPivotRow, i] /= PivotValue;
            }
        }

        private static void SetZeroInColumn(long lastNonPivotRow, long pivot, ref double[,] matrix, long M, long N)
        {
            for (long i = 0; i < M + 1; i++)
            {
                if (i != lastNonPivotRow)
                {
                    double SameColumnWithPivot = matrix[i, pivot];
                    if (SameColumnWithPivot == 0) continue;

                    for (long j = 0; j <= M + N; j++)
                    {
                        matrix[i, j] -= matrix[lastNonPivotRow, j] * SameColumnWithPivot;
                    }
                }
            }
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


    }

}
