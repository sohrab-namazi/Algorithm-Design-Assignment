using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AdvancedAlgorithms.Week4
{
    public class CD
    {
        //public static void Main(string[] args)
        //{
        //    long[] R = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long n = R[0];
        //    long m = R[1];
        //    long[][] cnf = new long[m][];

        //    for (long i = 0; i < m; i++)
        //    {
        //        cnf[i] = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    }

        //    Tuple<bool, long[]> Result = Solve(n, m, cnf);

        //    if (Result.Item1 == false)
        //    {
        //        Console.WriteLine("UNSATISFIABLE");
        //    }
        //    else
        //    {
        //        Console.WriteLine("SATISFIABLE");
        //        foreach (long res in Result.Item2) Console.Write(res + " ");
        //    }


        //}

        public static Tuple<bool, long[]> Solve(long v, long c, long[][] cnf)
        {
            long[] Answer = new long[v];
            bool IsSat = false;
            Tuple<bool, long[]> Result = new Tuple<bool, long[]>(IsSat, Answer);

            Graph graph = new Graph(2 * v);
            for (long i = 0; i < c; i++)
            {
                long[] Clause = cnf[i];
                if (Clause.Length == 1)
                {
                    graph.AddEdge(-Clause[0], Clause[0]);
                }
                else
                {
                    graph.AddEdge(-Clause[0], Clause[1]);
                    graph.AddEdge(-Clause[1], Clause[0]);
                }
            }
            graph.DFS(0);
            List<List<long>> SCCs = graph.ComputeSCCs();

            long[] Assigned = new long[graph.nodeCount];

            foreach (List<long> SCC in SCCs)
            {
                foreach (long s in SCC)
                {
                    if (Assigned[s] == 0)
                    {
                        Assigned[s] = 1;
                        Assigned[graph.Negate(s)] = -1;
                    }
                    if (SCC.Contains(graph.Negate(s))) return Result;
                }
            }

            IsSat = true;

            for (long i = 0; i < v; i++)
            {
                if (Assigned[i] == 1)
                {
                    Answer[i] = i + 1;
                }
                else if (Assigned[i] == -1)
                {
                    Answer[i] = -(i + 1);
                }
            }

            return new Tuple<bool, long[]>(IsSat, Answer);

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
            public List<long> ExploreNewMarked;

            public Graph(long nodeCount)
            {
                this.nodeCount = nodeCount;
                Nodes = new List<List<long>>();
                NodesReversed = new List<List<long>>();
                Marked = new List<bool>();
                preOrder = new List<long>();
                postOrder = new List<long>();
                ExploreNewMarked = new List<long>();

                for (long i = 0; i < nodeCount; i++)
                {
                    Marked.Add(false);
                    Nodes.Add(new List<long>());
                    preOrder.Add(-1);
                    postOrder.Add(-1);
                    NodesReversed.Add(new List<long>());
                }
            }

            public long Negate(long i)
            {
                if (i < nodeCount / 2)
                {
                    return (nodeCount / 2) + i;
                }
                return i - (nodeCount / 2);
            }

            public void AddEdge(long a, long b)
            {
                if (a > 0)
                {
                    a -= 1;
                }
                else
                {
                    a = (nodeCount / 2) - 1 - a;
                }
                if (b > 0)
                {
                    b -= 1;
                }
                else
                {
                    b = (nodeCount / 2) - 1 - b;
                }
                Nodes[(int)a].Add(b);
                NodesReversed[(int)b].Add(a);
            }

            public void Explore(long V)
            {
                Marked[(int)V] = true;
                ExploreNewMarked.Add(V);
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

            public List<List<long>> ComputeSCCs()
            {
                List<List<long>> Result = new List<List<long>>();

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
                        Result.Add(ExploreNewMarked);
                        ExploreNewMarked = new List<long>();
                        SCCNumbers++;
                    }
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

    }
}
