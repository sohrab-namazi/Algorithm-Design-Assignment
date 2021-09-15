using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AdvancedAlgorithms.Week3
{
    public class Apartment
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
        //    for (int i = 1; i < Result.Length; i++) Console.WriteLine(Result[i]);
        //}

        public static string[] Solve(int V, int E, long[,] matrix)
        {
            string Result = "";
            long[,] Matrix = new long[V, V];
            Graph graph = new Graph(V);

            for (int i = 0; i < E; i++)
            {
                graph.AddEdge(matrix[i, 0] - 1, matrix[i, 1] - 1);
            }

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    Matrix[i, j] = GetVariableIndex(V, i, j);
                }
            }

            for (int i = 0; i < V; i++)
            {
                List<int> RowConstraint = new List<int>();
                for (int j = 0; j < V; j++)
                {
                    RowConstraint.Add((int)Matrix[i, j]);
                }
                Result += ExactlyOneOf(RowConstraint);
            }

            for (int i = 0; i < V; i++)
            {
                List<int> RowConstraint = new List<int>();
                for (int j = 0; j < V; j++)
                {
                    RowConstraint.Add((int)Matrix[j, i]);
                }
                Result += ExactlyOneOf(RowConstraint);
            }

            for (long i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    if (i != j && graph.Nodes[i, j] == 0)
                    {
                        Result += NoEdgeBetween(V, Matrix, (int)i, (int)j);
                    }
                }
            }

            List<string> FinalResult = new List<string>(Result.Split("\n"));
            FinalResult.RemoveAt(FinalResult.Count - 1);
            int ClauseCount = FinalResult.Count;
            FinalResult.Add(V * V + " " + ClauseCount);
            FinalResult.Reverse();

            return FinalResult.ToArray();
        }

        public class Graph
        {
            public int NodeCount;
            public long[,] Nodes;


            public Graph(int NodeCount)
            {
                this.NodeCount = NodeCount;
                Nodes = new long[NodeCount, NodeCount];
            }

            public void AddEdge(long a, long b)
            {
                Nodes[a, b] = 1;
                Nodes[b, a] = 1;
            }
        }

        public static int GetVariableIndex(int V, int i, int j)
        {
            return (V * i) + (j + 1);
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

        public static string NoEdgeBetween(int V, long[,] Matrix, int i, int j)
        {
            string Result = "";

            for (int k = 0; k < V - 1; k++)
            {
                long P = Matrix[i, k];
                long Q = Matrix[j, k + 1];
                Result += -P + " " + -Q + " 0\n";
            }

            return Result;
        }
    }
}
