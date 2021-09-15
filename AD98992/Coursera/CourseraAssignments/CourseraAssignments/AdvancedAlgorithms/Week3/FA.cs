using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AdvancedAlgorithms.Week3
{
    public class FA
    {
        //public static void Main(string[] args)
        //{
        //    int[] R = Console.ReadLine().Split().Select(int.Parse).ToArray();
        //    int n = R[0];
        //    int m = R[1];
        //    long[,] matrix = new long[m, 2];
        //    for (int i = 0; i < m; i++)
        //    {
        //        long[] Row = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //        matrix[i, 0] = Row[0];
        //        matrix[i, 1] = Row[1];
        //    }

        //    string[] Result = Solve(n, m, matrix);
        //    Console.WriteLine(Result[0]);
        //    for (int i = 1; i < Result.Length; i++) Console.Write(Result[i]);
        //}

        public static string[] Solve(int V, int E, long[,] matrix)
        {
            List<string> FinalResult = new List<string>();
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
