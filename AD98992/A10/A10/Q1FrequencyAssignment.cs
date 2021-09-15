using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q1FrequencyAssignment : Processor
    {
        public Q1FrequencyAssignment(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);


        public String[] Solve(int V, int E, long[,] matrix)
        {
            List<String> FinalResult = new List<String>();
            int VariableCounts = V * 3;
            int[,] Matrix = new int[V, 3];

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Matrix[i, j] = GetVariableIndex(i, j);
                }
            }
            for (int i = 0; i < V; i++)
            {
                List<int> Candidates = new List<int>();
                for (int j = 0; j < 3; j++)
                {
                    Candidates.Add(Matrix[i, j]);
                }
                FinalResult.Add(ExactlyOneOf(Candidates));
            }

            for (int i = 0; i < E; i++)
            {
                int P = (int)matrix[i, 0] - 1;
                int Q = (int)matrix[i, 1] - 1;
                for (int j = 0; j < 3; j++)
                {
                    List<int> Candidates = new List<int>();
                    Candidates.Add(Matrix[P, j]);
                    Candidates.Add(Matrix[Q, j]);
                    FinalResult.Add(AtMostOneOf(Candidates));
                }
            }

            int ClauseCount = FinalResult.Count;
            FinalResult.Add(VariableCounts + " " + ClauseCount);
            FinalResult.Reverse();
            return FinalResult.ToArray();
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public static int GetVariableIndex(int i, int j)
        {
            return (3 * i) + (j + 1);
        }

        public static string ExactlyOneOf(List<int> Candidates)
        {
            string Result = "";
            int n = Candidates.Count;

            foreach (int i in Candidates) Result += i + " ";
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
