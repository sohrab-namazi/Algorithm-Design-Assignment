using System;
using System.Collections.Generic;

namespace A8
{
    class Program
    {
        static void Main(string[] args)
        {
            long stockCount = 3;
            long pointCount = 4;
            long[][] matrix = new long[stockCount][];
            matrix[0] = new long[] {1, 2, 3, 4 };
            matrix[1] = new long[] {2, 3, 4, 6 };
            matrix[2] = new long[] {6, 5, 4, 3 };
            long res = Solve(stockCount, pointCount, matrix);
            Console.WriteLine(res);
        }

        public static long Solve(long stockCount, long pointCount, long[][] matrix)
        {
            Network network = new Network(stockCount * 2 + 2);
            network.InitializeBMatching(pointCount, matrix);
            network.GetMaxFlow();
            return network.Source.adjs.Count;
        }

        public class Network
        {
            public long NodeCount;
            public bool[] Marked;
            public List<Node> Nodes;
            public Edge[] Parents;
            public Edge[] MaxMatchingParents;
            public Node Source;
            public Node Target;
            public Queue<Node> Queue;
            public long StockCount;

            public Network(long NodeCount)
            {
                this.NodeCount = NodeCount;
                Marked = new bool[NodeCount];
                Nodes = new List<Node>();
                Parents = new Edge[NodeCount];
                MaxMatchingParents = new Edge[NodeCount];
                Queue = new Queue<Node>();
                for (long i = 0; i < NodeCount; i++) Nodes.Add(new Node(i));
                Source = Nodes[(int)NodeCount - 2];
                Target = Nodes[(int)NodeCount - 1];
                StockCount = (NodeCount - 2) / 2;
            }

            public bool CheckEdge(long PointCount, long[] a, long[] b)
            {
                for (long i = 0; i < PointCount; i++)
                {
                    if (a[i] >= b[i]) return false;
                }
                return true;
            }

            public void InitializeBMatching(long pointCount, long[][] matrix)
            {
                for (long i = 0; i < StockCount; i++)
                {
                    for (long j = 0; j < StockCount; j++)
                    {
                        if (CheckEdge(pointCount, matrix[i], matrix[j]))
                        {
                            AddEdge((int)i, (int)(j + StockCount), 1);
                        }
                    }
                }

                for (long i = 0; i < StockCount; i++)
                {
                    AddEdge((int)Source.Index, (int)i, 1);
                }

                for (long i = StockCount; i < 2 * StockCount; i++)
                {
                    AddEdge((int)i, (int)Target.Index, 1);
                }
            }

            public void AddEdge(int u, int v, int c)
            {
                Edge edge = new Edge(Nodes[u], Nodes[v], c);
                edge.Flow = c;
                Nodes[u].adjs.Add(edge);
            }

            public long[] GetMaxFlow()
            {
                long[] MaxMatching = new long[StockCount];
                Array.Fill(MaxMatching, -1);
                long MaxFlow = 0;

                while (true)
                {
                    var tuple = BFS();
                    if (tuple.Item2 == long.MaxValue) break;
                    AddFlow(tuple.Item1, tuple.Item2);
                    MaxFlow += tuple.Item2;
                    for (long i = 1; i < tuple.Item1.Count - 1; i++)
                    {
                        MaxMatching[tuple.Item1[(int)i].U.Index] = tuple.Item1[(int)i].V.Index + 1 - StockCount;
                    }
                }

                return MaxMatching;
            }

            private void AddFlow(List<Edge> EdgesInPath, long MinEdge)
            {
                foreach (Edge edge in EdgesInPath)
                {
                    edge.Flow -= MinEdge;
                    if (edge.Flow == 0)
                    {
                        Nodes[(int)edge.U.Index].adjs.Remove(edge);
                    }
                    bool HasReveresed = false;
                    foreach (Edge backEdge in Nodes[(int)edge.V.Index].adjs)
                    {
                        if (backEdge.V == edge.U)
                        {
                            HasReveresed = true;
                            backEdge.Flow += MinEdge;
                        }
                    }

                    if (HasReveresed) continue;

                    Edge BackEdge = new Edge(edge.V, edge.U, edge.Capacity);
                    BackEdge.Flow = edge.Capacity - edge.Flow;
                    Nodes[(int)edge.V.Index].adjs.Add(BackEdge);
                }
            }

            public Tuple<List<Edge>, long> BFS()
            {
                Edge[] MCopy = (Edge[])MaxMatchingParents.Clone();
                Array.Fill(Parents, null);
                Array.Fill(Marked, false);
                Queue.Clear();
                Queue.Enqueue(Source);

                while (Queue.Count > 0)
                {
                    Node node = Queue.Dequeue();
                    Marked[node.Index] = true;
                    if (node.Index == NodeCount - 1) break;
                    foreach (Edge edge in node.adjs)
                    {
                        if (edge.Flow != 0 && !Marked[edge.V.Index])
                        {
                            Queue.Enqueue(edge.V);
                            MaxMatchingParents[edge.V.Index] = edge;
                            Marked[edge.V.Index] = true;
                            Parents[edge.V.Index] = edge;
                        }
                    }
                }
                if (!Marked[Target.Index])
                {
                    MaxMatchingParents = MCopy;
                    return new Tuple<List<Edge>, long>(new List<Edge>(), long.MaxValue);
                }
                return ReconstructPath();
            }

            public Tuple<List<Edge>, long> ReconstructPath()
            {
                long MinEdge = long.MaxValue;
                List<Edge> EdgesInPath = new List<Edge>();
                Node CurNode = Target;

                while (CurNode != Source)
                {
                    Edge edge = Parents[CurNode.Index];
                    EdgesInPath.Add(edge);
                    if (edge.Flow < MinEdge) MinEdge = edge.Flow;
                    CurNode = edge.U;
                }

                return new Tuple<List<Edge>, long>(EdgesInPath, MinEdge);
            }
        }

        public class Node
        {
            public long Index;
            public List<Edge> adjs;

            public Node(long Index)
            {
                this.Index = Index;
                adjs = new List<Edge>();
            }
        }

        public class Edge
        {
            public Node U;
            public Node V;
            public long Flow;
            public long Capacity;

            public Edge(Node U, Node V, long Capacity)
            {
                this.U = U;
                this.V = V;
                this.Capacity = Capacity;
            }
        }
    }
}
