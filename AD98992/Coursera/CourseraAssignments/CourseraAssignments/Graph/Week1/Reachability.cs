using System;
using System.Collections.Generic;

namespace A1
{
    class Program
    {
        public class Reachability
        {
            public static long Solve(long nodeCount, long[][] edges, long StartNode, long EndNode)
            {
                Graph graph = new Graph(nodeCount);

                foreach (long[] edge in edges)
                {
                    graph.addEdge(edge[0] - 1, edge[1] - 1);
                }

                graph.Explore(StartNode - 1);

                if (graph.Marked[(int)EndNode - 1])
                {
                    return 1;
                }
                return 0;
            }

            //public static void Main(string[] args)
            //{
            //    string FirstLine = Console.ReadLine();
            //    long NodeCount = long.Parse(FirstLine.Split()[0]);
            //    long EdgeCount = long.Parse(FirstLine.Split()[1]);
            //    long[][] Edges = new long[EdgeCount][];
            //    for (long i = 0; i < EdgeCount; i++)
            //    {
            //        string MiddleLine = Console.ReadLine();
            //        long a = long.Parse(MiddleLine.Split()[0]);
            //        long b = long.Parse(MiddleLine.Split()[1]);
            //        Edges[i] = new long[] { a, b };
            //    }
            //    string LastLine = Console.ReadLine();
            //    long StartNode = long.Parse(LastLine.Split()[0]);
            //    long EndNode = long.Parse(LastLine.Split()[1]);
            //    long res = Solve(NodeCount, Edges, StartNode, EndNode);
            //    Console.WriteLine(res);
            //}
        }
    }
    public class Graph
    {
        public long NodeCount;
        public List<List<long>> Nodes;
        public List<bool> Marked;

        public Graph(long NodeCount)
        {
            this.NodeCount = NodeCount;
            Nodes = new List<List<long>>();
            Marked = new List<bool>();
            for (int i = 0; i < NodeCount; i++)
            {
                Nodes.Add(new List<long>());
                Marked.Add(false);
            }
        }

        public void addEdge(long a, long b)
        {
            this.Nodes[(int)a].Add(b);
            this.Nodes[(int)b].Add(a);
        }

        public void DFS(long startNode)
        {
            for (long i = 0; i < NodeCount; i++)
            {
                if (!Marked[(int)i])
                {
                    Explore(i);
                }
            }
        }

        public void Explore(long startNode)
        {
            Marked[(int)startNode] = true;
            foreach (long node in Nodes[(int)startNode])
            {
                if (!Marked[(int)node]) Explore(node);
            }
        }
    }
}

