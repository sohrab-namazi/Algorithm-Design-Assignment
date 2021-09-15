using System;
using System.Collections.Generic;
using TestCommon;

namespace Exam
{
    public class Q2BoardGame : Processor
    {
        public Q2BoardGame(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, long[,], string[]>)Solve);

        public string[] Solve(int BoardSize, long[,] Board)
        {
            string Result = "";
            long VarCount = BoardSize * BoardSize * 3;
            Result += RowRule(BoardSize);
            Result += ColumnRule(BoardSize);
            Result += ColumnColorRule(BoardSize);
            Result += ConstRule(BoardSize, Board);
            Result += CellRule(BoardSize);

            List<String> FinalResult = new List<String>(Result.Split("\n"));
            FinalResult.RemoveAt(FinalResult.Count - 1);
            int ClauseCount = FinalResult.Count;
            FinalResult.Add(VarCount + " " + ClauseCount);
            FinalResult.Reverse();
            return FinalResult.ToArray();
        }
        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;


        private static string CellRule(long Dim)
        {
            string Result = "";
            for (long i = 0; i < Dim; i++)
            {
                List<long> Candidates = new List<long>();
                for (long j = 0; j < Dim; j++)
                {
                    Candidates.Clear();
                    long Var = GetVarNumber(Dim, i, j);
                    Candidates.Add(Var);
                    Candidates.Add(Var + 1);
                    Candidates.Add(Var + 2);
                    Result += ExactlyOneOf(Candidates);
                }
            }
            return Result;
        }

        private static string ConstRule(int boardSize, long[,] board)
        {
            string Result = "";
            for (long i = 0; i < boardSize; i++)
            {
                for (long j = 0; j < boardSize; j++)
                {
                    long Value = board[i, j];
                    long EmptyVar = GetVarNumber(boardSize, i, j);
                    long BlueVar = EmptyVar + 1;
                    long RedVar = EmptyVar + 2;

                    if (Value == 1)
                    {
                        Result += EmptyVar + " 0\n";
                    }
                    else if (Value == 2)
                    {
                        Result += BlueVar + " " + EmptyVar + " 0\n";
                    }
                    else
                    {
                        Result += RedVar + " " + EmptyVar + " 0\n";
                    }
                }
            }
            return Result;
        }

        private static string ColumnColorRule(int Dim)
        {
            string Result = "";
            

            for (long j = 0; j < Dim; j++)
            {
                List<long> RedVars = new List<long>();
                List<long> BlueVars = new List<long>();
                for (long i = 0; i < Dim; i++)
                {
                    long StartVar = GetVarNumber(Dim, i, j);
                    long BlueVar = StartVar + 1;
                    long RedVar = StartVar + 2;
                    RedVars.Add(RedVar);
                    BlueVars.Add(BlueVar);
                }
                foreach (long Red in RedVars)
                {
                    foreach (long Blue in BlueVars)
                    {
                        Result += (-Red) + " " + (-Blue) + " 0\n";
                    }
                }
            }

            return Result;
        }

        private static string ColumnRule(int Dim)
        {
            string Result = "";
            for (long j = 0; j < Dim; j++)
            {
                long StartVar = GetVarNumber(Dim, 0, j);
                List<long> Candidates = new List<long>();
                for (long i = 0; i < Dim; i++)
                {
                    long First = GetVarNumber(Dim, i, j);
                    Candidates.Add(First + 1);
                    Candidates.Add(First + 2);
                }
                Result += AtleastOneOf(Candidates);
            }
            return Result;
        }

        private static string RowRule(int Dim)
        {
            string Result = "";
            for (long i = 0; i < Dim; i++)
            {
                List<long> Candidates = new List<long>();
                long StartVar = GetVarNumber(Dim, i, 0);
                long EndVar = StartVar + Dim * 3 - 1;
                for (long k = StartVar; k <= EndVar; k++)
                {
                    if (k % 3 != 1)
                        Candidates.Add(k);
                }
                Result += AtleastOneOf(Candidates);
            }
            return Result;
        }

        public static string ExactlyOneOf(List<long> Candidates)
        {
            string Result = "";
            int n = Candidates.Count;

            foreach (long i in Candidates) Result += i + " ";
            Result += "0\n";

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    Result += (-Candidates[i]) + " " + (-Candidates[j]) + " 0\n";
                }
            }

            return Result;
        }

        public static long GetVarNumber(long Dim, long i, long j)
        {
            return Dim * 3 * i + 3 * j + 1;
        }

        public static string AtleastOneOf(List<long> Vars)
        {
            string Result = "";
            int n = Vars.Count;

            foreach (int i in Vars) Result += i + " ";
            Result += "0\n";

            return Result;
        }

        public static string AtMostOneOf(List<int> Candidates)
        {
            string Result = "";
            int n = Candidates.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    Result += (-Candidates[i]) + " " + (-Candidates[j]) + " 0\n";
                }
            }

            return Result;
        }
    }
}
