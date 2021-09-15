using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            Network network = new Network(nodeCount);

            for (long i = 0; i < edgeCount; i++)
            {
                long[] edge = edges[i];
                network.AddEdge((int) edge[0] - 1, (int) edge[1] - 1, (int) edge[2]);
            }

            return network.EdmondsKarp();
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

            public Network(long NodeCount)
            {
                
                this.NodeCount = NodeCount;
                Marked = new bool[NodeCount];
                Nodes = new List<Node>();
                Parents = new Edge[NodeCount];
                MaxMatchingParents = new Edge[NodeCount];
                Queue = new Queue<Node>();
                for (long i = 0; i < NodeCount; i++) Nodes.Add(new Node(i));
                Source = Nodes[0];
                Target = Nodes[(int)NodeCount - 1];

            }

            public void AddEdge(int u, int v, int c)
            {
                Edge edge = new Edge(Nodes[u], Nodes[v], c);
                edge.Flow = c;
                Nodes[u].adjs.Add(edge);
            }

            public long EdmondsKarp()
            {
                long MaxFlow = 0;

                while (true)
                {
                    var tuple = BFS();
                    if (tuple.Item2 == long.MaxValue) break;
                    AddFlow(tuple.Item1, tuple.Item2);
                    MaxFlow += tuple.Item2;
                }

                return MaxFlow;
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
