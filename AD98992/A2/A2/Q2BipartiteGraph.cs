using System;
using System.Collections.Generic;
using TestCommon;

namespace A2
{
    public class Q2BipartiteGraph : Processor
    {
        public Q2BipartiteGraph(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long NodeCount, long[][] edges)
        {
            Graph graph = new Graph(NodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1);
            }

            long res = graph.IsBipartite();

            return res;
        }
    }

    public class Graph
    {
        public long NodeCount;
        public List<List<long>> Nodes;
        //-1 : White, 0 : Not visited, 1 : Black
        public List<long> Color;
        public List<bool> Marked;

        public Graph(long NodeCount)
        {
            this.NodeCount = NodeCount;
            Nodes = new List<List<long>>();
            Color = new List<long>();
            Marked = new List<bool>();
            for (long i = 0; i < NodeCount; i++)
            {
                Nodes.Add(new List<long>());
                Color.Add(0);
                Marked.Add(false);
            }
        }

        public void AddEdge(long a, long b)
        {
            Nodes[(int)a].Add(b);
            Nodes[(int)b].Add(a);
        }

        public long IsBipartite()
        {
            Queue<long> queue = new Queue<long>();
            queue.Enqueue(0);
            Color[0] = -1;

            while (queue.Count > 0)
            {
                long Node = queue.Dequeue();
               // Marked[(int) Node] = true;

                foreach (long adj in Nodes[(int)Node])
                {
                    if (Color[(int)adj] == 0)
                    {
                        if (Color[(int)Node] == -1)
                        {
                            Color[(int)adj] = 1;
                        }
                        else
                        {
                            Color[(int)adj] = -1;
                        }
                        queue.Enqueue(adj);
                    }
                    else
                    {
                        if (Color[(int)Node] == Color[(int)adj])
                        {
                            return 0;
                        }
                    }
                }
            }
            return 1;
        }
    }
}
