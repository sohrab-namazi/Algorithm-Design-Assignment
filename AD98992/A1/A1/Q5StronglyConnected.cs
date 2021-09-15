using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q5StronglyConnected: Processor
    {
        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1);
            }

            graph.DFS(0);

            long res = graph.ComputeSCCs();

            return res;
        }

        public class Graph
        {
            public long nodeCount;
            public List<List<long>> Nodes;
            public List<List<long>> NodesReversed;
            List<bool> Marked;
            public List<long> preOrder;
            public List<long> postOrder;
            public long SCCNumbers;
            public long clk = 1;

            public Graph(long nodeCount)
            {
                this.nodeCount = nodeCount;
                Nodes = new List<List<long>>();
                NodesReversed = new List<List<long>>();
                Marked = new List<bool>();
                preOrder = new List<long>();
                postOrder = new List<long>();

                for (long i = 0; i < nodeCount; i++)
                {
                    Marked.Add(false);
                    Nodes.Add(new List<long>());
                    preOrder.Add(-1);
                    postOrder.Add(-1);
                    NodesReversed.Add(new List<long>());
                }
            }

            public void AddEdge(long a, long b)
            {
                Nodes[(int)a].Add(b);
                NodesReversed[(int)b].Add(a);
            }

            public void Explore(long V)
            {
                Marked[(int)V] = true;
                previsit(V);
                foreach (long adj in Nodes[(int)V])
                {
                    if (!Marked[(int)adj])
                    {
                        Explore(adj);
                    }
                }
                postvisit(V);
            }

            public void ExploreReversed(long V)
            {
                Marked[(int)V] = true;
                previsit(V);
                foreach (long adj in NodesReversed[(int)V])
                {
                    if (!Marked[(int)adj])
                    {
                        ExploreReversed(adj);
                    }
                }
                postvisit(V);
            }

            private void postvisit(long v)
            {
                postOrder[(int)v] = clk;
                clk++;
            }

            private void previsit(long v)
            {
                preOrder[(int)v] = clk;
                clk++;
            }

            public void DFS(long V)
            {
                for (long i = 0; i < nodeCount; i++)
                {
                    if (!Marked[(int)i])
                    {
                        ExploreReversed(i);
                    }
                }
            }


            public long ComputeSCCs()
            {
                List<Node> nodes = new List<Node>();

                for (long i = 0; i < nodeCount; i++)
                {
                    nodes.Add(new Node(i, postOrder[(int)i]));
                }

                nodes.Sort((x, y) => x.value.CompareTo(y.value));
                nodes.Reverse();

                for (long i = 0; i < nodeCount; i++)
                {
                    Marked[(int)i] = false;
                }

                for (long i = 0; i < nodeCount; i++)
                {
                    if (!Marked[(int)nodes[(int)i].index])
                    {
                        Explore(nodes[(int)i].index);
                        SCCNumbers++;
                    }
                }

                return SCCNumbers;
            }
        }

        public class Node
        {
            public long index;
            public long value;

            public Node(long index, long value)
            {
                this.index = index;
                this.value = value;
            }
        }
    }
}
