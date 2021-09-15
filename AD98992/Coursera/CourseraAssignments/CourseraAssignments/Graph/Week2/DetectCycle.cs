using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments
{
    public class DetectCycle
    {
        //static void Main(string[] args)
        //{
        //    string firstLine = Console.ReadLine();
        //    long nodeCount = long.Parse(firstLine.Split()[0]);
        //    long edgesCount = long.Parse(firstLine.Split()[1]);

        //    long[][] edges = new long[edgesCount][];
        //    string line;

        //    for (long i = 0; i < edgesCount; i++)
        //    {
        //        edges[i] = new long[2];
        //        line = Console.ReadLine();
        //        edges[i][0] = long.Parse(line.Split()[0]);
        //        edges[i][1] = long.Parse(line.Split()[1]);

        //    }

        //    Console.WriteLine(Solve(nodeCount, edges));
        //}

        public static long Solve(long nodeCount, long[][] edges)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1);
            }

            if (graph.IsCyclic())
            {
                return 1;
            }
            return 0;
        }

        public class Graph
        {
            public long nodeCount;
            public List<List<long>> nodes;
            public List<bool> marked;
            public List<bool> InRecursion;

            public Graph(long nodeCount)
            {
                this.nodeCount = nodeCount;
                nodes = new List<List<long>>();
                marked = new List<bool>();
                InRecursion = new List<bool>();
                for (long i = 0; i < nodeCount; i++)
                {
                    nodes.Add(new List<long>());
                    marked.Add(false);
                    InRecursion.Add(false);
                }
            }

            public void AddEdge(long a, long b)
            {
                nodes[(int)a].Add(b);
            }

            public bool IsCyclic()
            {
                for (long i = 0; i < nodeCount; i++)
                {
                    if (CycleDetector(i)) return true;
                }
                return false;
            }

            public bool CycleDetector(long V)
            {
                marked[(int)V] = true;
                InRecursion[(int)V] = true;

                foreach (long adj in nodes[(int)V])
                {
                    if (!marked[(int)adj] && CycleDetector(adj))
                    {
                        return true;
                    }
                    else if (InRecursion[(int)adj])
                    {
                        return true;
                    }
                }
                InRecursion[(int)V] = false;
                return false;
            }
        }
    }
}
