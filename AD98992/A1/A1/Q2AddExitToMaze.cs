using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                long nodeA = edges[i][0];
                long nodeB = edges[i][1];
                graph.AddEdge(nodeA - 1, nodeB - 1);
            }

            long res = graph.CCNumbers();

            return res;
        }

        public class Graph
        {
            public long nodeCount;
            public List<List<long>> nodes;
            public List<bool> marked;
            public long CCCnumbers;

            public Graph(long NodeCount)
            {
                this.nodeCount = NodeCount;
                nodes = new List<List<long>>();
                marked = new List<bool>();
                for (long i = 0; i < nodeCount; i++)
                {
                    nodes.Add(new List<long>());
                    marked.Add(false);
                }
            }

            public void AddEdge(long a, long b)
            {
                nodes[(int)a].Add(b);
                nodes[(int)b].Add(a);
            }

            public void Explore(long V)
            {
                marked[(int)V] = true;
                foreach (long adj in nodes[(int)V])
                {
                    if (!marked[(int)adj])
                    {
                        Explore((int)adj);
                    }
                }
            }

            public void RecursiveDFS(long V)
            {
                for (long i = 0; i < nodeCount; i++)
                {
                    if (!marked[(int)i])
                    {
                        Explore(i);
                        CCCnumbers++;
                    }
                }
            }

            public void IterativeDFS(long V)
            {
                Stack<long> stack = new Stack<long>();
                stack.Push(V);

                while (stack.Count > 0)
                {
                    long node = stack.Pop();
                    marked[(int)node] = true;
                    foreach (long adj in nodes[(int)node])
                    {
                        if (!marked[(int)adj])
                        {
                            stack.Push(adj);
                        }
                    }
                }
            }

            public long CCNumbers()
            {
                for (long i = 0; i < nodeCount; i++)
                {
                    if (!marked[(int)i])
                    {
                        IterativeDFS(i);
                        CCCnumbers++;
                    }
                }

                return CCCnumbers;
            }
        }
    }
}
