using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q1MinCost : Processor
    {
        public Q1MinCost(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1, edges[i][2]);
            }

            graph.Dijkstra(startNode - 1, endNode - 1);
            if (graph.Dists[(int)endNode - 1].Equals(long.MaxValue)) return -1;
            return graph.Dists[(int) endNode - 1];
        }

        public class Graph
        {
            public long NodeCount;
            public List<List<Edge>> Nodes;
            public List<long> Dists;
            public List<bool> Marked;
            public List<long> Path;

            public Graph(long Nodecount)
            {
                NodeCount = Nodecount;
                Nodes = new List<List<Edge>>();
                Dists = new List<long>();
                Marked = new List<bool>();
                Path = new List<long>();

                for (long i = 0; i < Nodecount; i++)
                {
                    Nodes.Add(new List<Edge>());
                    Marked.Add(false);
                    Dists.Add(long.MaxValue);
                    Path.Add(-1);
                }
            }

            public void AddEdge(long a, long b, long w)
            {
                Nodes[(int)a].Add(new Edge(b, w));
            }

            public void Dijkstra(long S, long EndNode)
            {
                Dists[(int)S] = 0;
                SimplePriorityQueue<long> queue = new SimplePriorityQueue<long>();
                queue.Enqueue(S, 0);

                while (queue.Count > 0)
                {
                    long Node = queue.Dequeue();
                    Marked[(int)Node] = true;
                    if (Node == EndNode) return;
                    foreach (Edge edge in Nodes[(int)Node])
                    {
                        if (!Marked[(int)edge.E])
                        {
                            if (Dists[(int)edge.E] > Dists[(int)Node] + edge.Weight)
                            {
                                Path[(int)edge.E] = Node;
                                Dists[(int)edge.E] = Dists[(int)Node] + edge.Weight;
                                queue.TryUpdatePriority(edge.E, Dists[(int) edge.E]);
                            }
                            if (!queue.Contains(edge.E))
                            {
                                queue.Enqueue(edge.E, Dists[(int)edge.E]);
                            }
                        }
                    }
                }
            }
        }

        public class Edge
        {
            public long E;
            public long Weight;

            public Edge(long E, long Weight)
            {
                this.E = E;
                this.Weight = Weight;
            }
        }
    }
}
