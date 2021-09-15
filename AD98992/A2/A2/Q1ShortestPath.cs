using System;
using System.Collections.Generic;
using TestCommon;

namespace A2
{
    public class Q1ShortestPath : Processor
    {
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long[][], long, long, long>)Solve);
        
        public long Solve(long NodeCount, long[][] edges, 
                          long StartNode,  long EndNode)
        {
            Graph graph = new Graph(NodeCount);

            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1);
            }

            graph.BFS(StartNode - 1);

            if (graph.Dists[(int)EndNode - 1].Equals(long.MaxValue)) return -1;

            return graph.Dists[(int)EndNode - 1];
        }

        public class Graph
        {
            public long NodeCounts;
            public List<List<long>> Nodes;
            public List<bool> Marked;
            public List<long> Dists;

            public Graph(long NodeCounts)
            {
                this.NodeCounts = NodeCounts;
                Nodes = new List<List<long>>();
                Marked = new List<bool>();
                Dists = new List<long>();

                for (long i = 0; i < NodeCounts; i++)
                {
                    Nodes.Add(new List<long>());
                    Marked.Add(false);
                    Dists.Add(long.MaxValue);
                }
            }

            public void AddEdge(long a, long b)
            {
                Nodes[(int)a].Add(b);
                Nodes[(int)b].Add(a);
            }

            public void BFS(long Source)
            {
                Dists[(int)Source] = 0;
                Marked[(int)Source] = true;
                Queue<long> queue = new Queue<long>();
                queue.Enqueue(Source);

                while (queue.Count > 0)
                {
                    long Node = queue.Dequeue();

                    foreach (long adj in Nodes[(int)Node])
                    {
                        if (!Marked[(int)adj])
                        {
                            queue.Enqueue(adj);
                            Marked[(int) adj] = true;
                            Dists[(int)adj] = Dists[(int)Node] + 1;
                        }
                    }
                }
            }
        }
    }


}
