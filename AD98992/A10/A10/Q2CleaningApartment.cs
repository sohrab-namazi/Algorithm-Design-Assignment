using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public String[] Solve(int V, int E, long[,] matrix)
        {
            String Result = "";
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

            List<String> FinalResult = new List<String>(Result.Split("\n"));
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
