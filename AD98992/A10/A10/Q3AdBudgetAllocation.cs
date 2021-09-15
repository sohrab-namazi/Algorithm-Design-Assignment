using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            string Result = "";

            long[] Matrix = new long[varCount];

            for (long i = 0; i < varCount; i++)
            {
                Matrix[i] = GetVariableIndex(i);
            }

            for (long i = 0; i < eqCount; i++)
            {
                long B = b[i];
                List<long> NonZeroVariables = new List<long>();
                List<long> Coefficients = new List<long>();

                for (long j = 0; j < varCount; j++)
                {
                    if (A[i][j] != 0)
                    {
                        NonZeroVariables.Add(Matrix[j]);
                        Coefficients.Add(A[i][j]);
                    }
                }
                string SubAnswer = CheckInequality(NonZeroVariables, Coefficients, B);
                if (SubAnswer.Equals("No Need")) continue;
                Result += SubAnswer;
            }

            List<String> FinalResult = new List<String>(Result.Split("\n"));
            FinalResult.RemoveAt(FinalResult.Count - 1);
            int ClauseCount = FinalResult.Count;
            FinalResult.Add(varCount + " " + ClauseCount + 2);
            FinalResult.Reverse();
            return FinalResult.ToArray();
        }

        public static string CheckInequality(List<long> nonZeroVariables, List<long> coefficients, long b)
        {
            int Count = nonZeroVariables.Count;
            if (Count == 1) return CheckInequality1(nonZeroVariables, coefficients, b);
            else if (Count == 2) return CheckInequality2(nonZeroVariables, coefficients, b);
            else if(Count == 3) return CheckInequality3(nonZeroVariables, coefficients, b);
            return "No Need";
        }

        private static string CheckInequality3(List<long> nonZeroVariables, List<long> coefficients, long b)
        {
            string Result = "";

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (long k = 0; k < 2; k++)
                    {
                        if (i * coefficients[0] + j * coefficients[1] + k * coefficients[2] > b)
                        {
                            Result += CaseMaker3(i, j, k, nonZeroVariables[0], nonZeroVariables[1], nonZeroVariables[2]);
                        }
                    }
                }
            }

            return Result;
        }

        private static string CaseMaker3(int i, int j, long k, long var1, long var2, long var3)
        {
            string Result = "";

            if (i == 0 && j == 0 && k == 0)
            {
                Result += var1 + " " + var2 + " " + var3 + " 0\n";
            }
            else if (i == 0 && j == 1 && k == 0)
            {
                Result += var1 + " " + -var2 + " " + var3 + " 0\n";
            }
            else if (i == 1 && j == 0 && k == 0)
            {
                Result += -var1 + " " + var2 + " " + var3 + " 0\n";
            }
            else if (i == 1 && j == 1 && k == 0)
            {
                Result += -var1 + " " + -var2 + " " + var3 + " 0\n";
            }
            if (i == 0 && j == 0 && k == 1)
            {
                Result += var1 + " " + var2 + " " + -var3 + " 0\n";
            }
            else if (i == 0 && j == 1 && k == 1)
            {
                Result += var1 + " " + -var2 + " " + -var3 + " 0\n";
            }
            else if (i == 1 && j == 0 && k == 1)
            {
                Result += -var1 + " " + var2 + " " + -var3 + " 0\n";
            }
            else if (i == 1 && j == 1 && k == 1)
            {
                Result += -var1 + " " + -var2 + " " + -var3 + " 0\n";
            }

            return Result;
        }

        private static string CheckInequality2(List<long> nonZeroVariables, List<long> coefficients, long b)
        {
            string Result = "";

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (i * coefficients[0] + j * coefficients[1] > b)
                    {
                        Result += CaseMaker2(i, j, nonZeroVariables[0], nonZeroVariables[1]);
                    }
                }
            }

            return Result;
        }

        public static string CaseMaker2(int i, int j, long var1, long var2)
        {
            string Result = "";

            if (i == 0 && j == 0)
            {
                Result += var1 + " " + var2 + " 0\n";
            }
            else if (i == 0 && j == 1)
            {
                Result += var1 + " " + -var2 + " 0\n";
            }
            else if (i == 1 && j == 0)
            {
                Result += -var1 + " " + var2 + " 0\n";
            }
            else
            {
                Result += -var1 + " " + -var2 + " 0\n";
            }
            return Result;
        }

        public static string CaseMaker1(int i, long variable)
        {
            if (i == 0) return variable + " 0\n";
            else return -variable + " 0\n";
        }

        private static string CheckInequality1(List<long> nonZeroVariables, List<long> coefficients, long b)
        {
            string Result = "";
            long Variable = nonZeroVariables[0];
            long Coefficient = coefficients[0];

            for (int i = 0; i < 2; i++)
            {
                if (i * Coefficient > b)
                {
                    Result += CaseMaker1(i, Variable);
                }
            }

            return Result;
        }

        public static long GetVariableIndex(long i)
        {
            return i + 1;
        }
    }
}
