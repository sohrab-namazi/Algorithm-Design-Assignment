using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q4OrderOfCourse: Processor
    {
        public Q4OrderOfCourse(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);

        public long[] Solve(long nodeCount, long[][] edges)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1);
            }
            return graph.TopologicalSort();
        }

        public class Graph
        {
            public long nodeCount;
            public List<List<long>> nodes;
            public List<bool> marked;
            public long[] preVisit;
            public long[] postVisit;
            public long clk = 1;

            public Graph(long nodeCount)
            {
                this.nodeCount = nodeCount;
                nodes = new List<List<long>>();
                marked = new List<bool>();
                preVisit = new long[nodeCount];
                postVisit = new long[nodeCount];
                for (long i = 0; i < nodeCount; i++)
                {
                    nodes.Add(new List<long>());
                    marked.Add(false);
                }
            }

            public void AddEdge(long a, long b)
            {
                nodes[(int)a].Add(b);
            }

            public void Explore(long V)
            {
                marked[(int)V] = true;
                PreVisit(V);
                foreach (long adj in nodes[(int)V])
                {
                    if (!marked[(int)adj])
                    {
                        Explore(adj);
                    }
                }
                PostVisit(V);
            }

            public void DFS()
            {
                for (long i = 0; i < nodeCount; i++)
                {
                    if (!marked[(int)i])
                    {
                        Explore(i);
                    }
                }
            }

            private void PostVisit(long i)
            {
                postVisit[i] = clk;
                clk++;
            }

            private void PreVisit(long i)
            {
                preVisit[i] = clk;
                clk++;
            }

            public long[] TopologicalSort()
            {
                DFS();
                List<Node> nodes = new List<Node>();
                for (long i = 0; i < nodeCount; i++)
                {
                    Node node = new Node(i, postVisit[i]);
                    nodes.Add(node);
                }
                nodes.Sort((x, y) => x.value.CompareTo(y.value));
                nodes.Reverse();

                long[] Result = new long[nodeCount];
                for (long i = 0; i < nodeCount; i++)
                {
                    Result[i] = nodes[(int) i].index + 1;
                }
                return Result;
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


        public override Action<string, string> Verifier { get; set; } = TopSortVerifier;

        public static void TopSortVerifier(string inFileName, string strResult)
        {
            long[] topOrder = strResult.Split(TestTools.IgnoreChars)
                .Select(x => long.Parse(x)).ToArray();

            long count;
            long[][] edges;
            TestTools.ParseGraph(File.ReadAllText(inFileName), out count, out edges);

            // Build an array for looking up the position of each node in topological order
            // for example if topological order is 2 3 4 1, topOrderPositions[2] = 0, 
            // because 2 is first in topological order.
            long[] topOrderPositions = new long[count];
            for (int i = 0; i < topOrder.Length; i++)
                topOrderPositions[topOrder[i] - 1] = i;
            // Top Order nodes is 1 based (not zero based).

            // Make sure all direct depedencies (edges) of the graph are met:
            //   For all directed edges u -> v, u appears before v in the list
            foreach (var edge in edges)
                if (topOrderPositions[edge[0] - 1] >= topOrderPositions[edge[1] - 1])
                    throw new InvalidDataException(
                        $"{Path.GetFileName(inFileName)}: " +
                        $"Edge dependency violoation: {edge[0]}->{edge[1]}");

        }



        
    }

        


    }

