using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AdvancedAlgorithms.Week1
{
    class Airline
    {
        //public static void Main(string[] args)
        //{
        //    long[] Count = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long flightCount = Count[0];
        //    long crewCount = Count[1];
        //    long[][] info = new long[flightCount][];
        //    for (long i = 0; i < flightCount; i++)
        //    {
        //        long[] input = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //        info[i] = input;
        //    }

        //    long[] Res = Solve(flightCount, crewCount, info);
        //    foreach (long res in Res) Console.Write(res + " ");
        //}

        public static long[] Solve(long flightCount, long crewCount, long[][] info)
        {
            Network network = new Network(flightCount + crewCount + 2);
            network.InitializeBMatching(flightCount, crewCount, info);
            long[] Result = network.GetMaxFlow();
            return Result;
        }

        public class Network
        {
            public long NodeCount;
            public bool[] Marked;
            public List<Node> Nodes;
            public Edge[] Parents;
            public Node Source;
            public Node Target;
            public Queue<Node> Queue;
            public long m;
            public long n;

            public Network(long NodeCount)
            {
                this.NodeCount = NodeCount;
                Marked = new bool[NodeCount];
                Nodes = new List<Node>();
                Parents = new Edge[NodeCount];
                Queue = new Queue<Node>();
                for (long i = 0; i < NodeCount; i++) Nodes.Add(new Node(i));
                Source = Nodes[(int)NodeCount - 2];
                Target = Nodes[(int)NodeCount - 1];
            }

            public void InitializeBMatching(long m, long n, long[][] edges)
            {
                this.m = m;
                this.n = n;

                for (long i = 0; i < m; i++)
                {
                    for (long j = 0; j < n; j++)
                    {
                        if (edges[i][j] == 1)
                        {
                            AddEdge((int)i, (int)(m + j), 1);
                        }
                    }
                }

                for (long i = 0; i < m; i++)
                {
                    AddEdge((int)(m + n), (int)i, 1);
                }

                for (long i = 0; i < n; i++)
                {
                    AddEdge((int)(i + m), (int)(m + n + 1), 1);
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
                long MaxFlow = 0;
                long[] MaxMatching = new long[m];
                for (long i = 0; i < m; i++)
                {
                    MaxMatching[i] = -1;
                }

                while (true)
                {
                    var tuple = BFS();
                    if (tuple.Item2 == long.MaxValue) break;
                    AddFlow(tuple.Item1, tuple.Item2);
                    MaxFlow += tuple.Item2;
                    for (long i = 1; i < tuple.Item1.Count - 1; i++)
                    {
                        if (tuple.Item1[(int)i].U.Index < m)
                        {
                            MaxMatching[tuple.Item1[(int)i].U.Index] = tuple.Item1[(int)i].V.Index + 1 - m;
                        }
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
                for (long i = 0; i < NodeCount; i++)
                {
                    Parents[i] = null;
                    Marked[i] = false;
                }
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
                            Marked[edge.V.Index] = true;
                            Parents[edge.V.Index] = edge;
                        }
                    }
                }
                if (!Marked[Target.Index])
                {
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
