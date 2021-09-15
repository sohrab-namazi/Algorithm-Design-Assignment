using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A11
{
    public class Q3SchoolBus : Processor
    {
        public Q3SchoolBus(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long[][], Tuple<long, long[]>>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.TSPVerifier;

        public virtual Tuple<long, long[]> Solve(long nodeCount, long[][] edges)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1, edges[i][2]);
            }

            List<long> Answer = graph.TSP();

            if (Answer.Count == 0) return new Tuple<long, long[]>(-1, new long[] { });
            else
            {
                long Shortest = Answer[Answer.Count - 1];
                Answer.RemoveAt(Answer.Count - 1);
                return new Tuple<long, long[]>(Shortest, Answer.ToArray());
            }
        }

        public class Graph
        {
            public long BestResult = long.MaxValue;
            public long NodeCount;
            public long[,] Nodes;
            public bool[] Marked;
            public long?[,] Parents;

            public Graph(long NodeCount)
            {
                this.NodeCount = NodeCount;
                Nodes = new long[NodeCount, NodeCount];
                Marked = new bool[NodeCount];
                Parents = new long?[(int)Math.Pow(2, NodeCount), NodeCount];
            }

            public void AddEdge(long a, long b, long w)
            {
                Nodes[a, b] = w;
                Nodes[b, a] = w;
            }

            public List<long> TSP()
            {
                List<long> Result = new List<long>();
                int TwoToN = (int)Math.Pow(2, NodeCount);
                int[][] BinaryNumbers = new int[TwoToN][];
                List<int> OnesCount = new List<int>();
                MakeBinary(ref BinaryNumbers, ref OnesCount, TwoToN);
                long?[,] C = new long?[TwoToN, NodeCount];
                C[1, 0] = 0;
                int[] Pows = new int[TwoToN];
                Pows[0] = 1;
                for (int i = 1; i < TwoToN; i++)
                {
                    Pows[i] = Pows[i - 1] * 2;
                }

                for (int s = 2; s < NodeCount + 1; s++)
                {
                    List<int> SubSets = GetSubSetS(s, BinaryNumbers, TwoToN, ref OnesCount);

                    foreach (int S in SubSets)
                    {
                        C[S, 0] = 1_000_000_000;
                        List<int> Is = GetIs(S, BinaryNumbers);

                        foreach (int i in Is)
                        {
                            if (i != 0)
                            {
                                foreach (int j in Is)
                                {
                                    if (j != i)
                                    {
                                        long? temp = C[Subtract(S, i, BinaryNumbers, TwoToN, Pows), j];
                                        if (temp != null && Nodes[j, i] != 0)
                                        {
                                            if (C[S, i] != null)
                                            {
                                                if (C[S, i] > temp + Nodes[j, i])
                                                {
                                                    Parents[S, i] = j;
                                                    C[S, i] = (long)temp + Nodes[j, i];
                                                }
                                            }
                                            else
                                            {
                                                Parents[S, i] = j;
                                                C[S, i] = (long)(temp + Nodes[j, i]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                long? Min = long.MaxValue;

                long BestI = -1;

                for (long i = 0; i < NodeCount; i++)
                {
                    long? temp = C[(int)TwoToN - 1, i];
                    if (temp != null && temp < 1_000_000_000 && Nodes[i, 0] != 0)
                    {
                        long temp2 = (long)temp + Nodes[i, 0];
                        if (Min > temp2)
                        {
                            Min = temp2;
                            BestI = i;
                        }

                    }
                }

                if (Min == long.MaxValue)
                {
                    Result.Add(-1);
                    return Result;
                }

                int l = TwoToN - 1;

                while (true)
                {
                    Result.Add(BestI + 1);
                    long a = BestI;
                    if (Parents[l, BestI] == null) break;
                    BestI = (long)Parents[l, BestI];
                    l = Subtract(l, (int)a, BinaryNumbers, TwoToN, Pows);
                }

               //Result.Reverse();

                Result.Add((long)Min);

                return Result;
            }

            private List<int> GetIs(int s, int[][] binaryNumbers)
            {
                List<int> Result = new List<int>();

                for (int i = 0; i < NodeCount; i++)
                {
                    if (binaryNumbers[s][i] == 1) Result.Add((int)NodeCount - i - 1);
                }
                return Result;
            }

            public void MakeBinary(ref int[][] BinaryNumbers, ref List<int> OnesCount, int TwoToN)
            {
                OnesCount.Add(0);
                BinaryNumbers[0] = new int[NodeCount];
                for (int i = 1; i < TwoToN; i++)
                {
                    BinaryNumbers[i] = (int[])BinaryNumbers[i - 1].Clone();

                    for (int k = 0; k < NodeCount; k++)
                    {
                        if (BinaryNumbers[i - 1][NodeCount - k - 1] == 0)
                        {
                            BinaryNumbers[i][NodeCount - k - 1] = 1;
                            break;
                        }
                        else
                        {
                            BinaryNumbers[i][NodeCount - k - 1] = 0;
                        }
                    }

                    int Count = 0;
                    for (int j = 0; j < NodeCount; j++)
                    {
                        if (BinaryNumbers[i][j] == 1) Count++;
                    }
                    OnesCount.Add(Count);
                }


            }

            public int ToDeciaml(int[] Binary, int[] Pows, int k)
            {
                int Result = 0;

                for (int i = 0; i < NodeCount; i++)
                {
                    if (Binary[i] == 1) Result += Pows[NodeCount - i - 1];
                }

                Result -= Binary[k] * Pows[NodeCount - k - 1]; 

                return Result;
            }

            private int Subtract(int s, int i, int[][] BinaryNumbers, int TwoToN, int[] Pows)
            {
                int k = (int) NodeCount - i - 1;
                int Result = ToDeciaml(BinaryNumbers[s], Pows, k);
                return Result;
            }

            public List<int> GetSubSetS(int x, int[][] BinaryNumbers, int TwoToN, ref List<int> OnesCount)
            {
                List<int> Result = new List<int>();
                for (int i = 0; i < TwoToN; i++)
                {
                    if (OnesCount[i] == x && BinaryNumbers[i][(int)NodeCount - 1] == 1) Result.Add(i);
                }
                return Result;
            }

        }

        public class Edge
        {
            public long Weight;
            public long P;
            public long Q;

            public Edge(long P, long Q, long W)
            {
                this.P = P;
                this.Q = Q;
                this.Weight = W;
            }
        }


    }
}
