using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.Week4
{
    public class ExchangingMoney
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
        //        edges[i] = new long[3];
        //        line = Console.ReadLine();
        //        edges[i][0] = long.Parse(line.Split()[0]);
        //        edges[i][1] = long.Parse(line.Split()[1]);
        //        edges[i][2] = long.Parse(line.Split()[2]);

        //    }

        //    long StartNode = long.Parse(Console.ReadLine());

        //    string[] Result = Solve(nodeCount, edges, StartNode);

        //    foreach (string res in Result) Console.WriteLine(res);

        //}

        public static string[] Solve(long nodeCount, long[][] edges, long startNode)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1, edges[i][2]);
            }

            string[] Result = graph.BellmanFord(startNode - 1);

            return Result;
        }

        public class Graph
        {
            public long NodeCount;
            public List<Edge> Edges;
            public List<long> Dists;
            public List<long> Parents;
            public List<bool> Marked;
            public List<List<long>> Nodes;
            public long XNegativeCycleStarter = -1;

            public Graph(long NodeCount)
            {
                this.NodeCount = NodeCount;
                Edges = new List<Edge>();
                Dists = new List<long>();
                Parents = new List<long>();
                Marked = new List<bool>();
                Nodes = new List<List<long>>();

                for (long i = 0; i < NodeCount; i++)
                {
                    Dists.Add(long.MaxValue);
                    Parents.Add(-1);
                    Marked.Add(false);
                    Nodes.Add(new List<long>());
                }
            }

            public void AddEdge(long a, long b, long w)
            {
                Edges.Add(new Edge(a, b, w));
                Nodes[(int)a].Add(b);
            }

            public bool Relax(Edge edge)
            {
                bool BeRelaxed = false;
                if (Dists[(int)edge.E] > Dists[(int)edge.V] + edge.Weight && !Dists[(int)edge.V].Equals(long.MaxValue))
                {
                    Dists[(int)edge.E] = Dists[(int)edge.V] + edge.Weight;
                    Parents[(int)edge.E] = edge.V;
                    BeRelaxed = true;
                }
                return BeRelaxed;
            }

            public bool CheckNegativeCycle(long S)
            {
                Dists[(int)S] = 0;
                bool Condition;

                for (long i = 0; i < NodeCount - 1; i++)
                {
                    Condition = false;
                    foreach (Edge edge in Edges)
                    {
                        bool Resume = Relax(edge);
                        if (Resume) Condition = true;
                    }
                    if (!Condition) break;
                }

                Condition = false;

                foreach (Edge edge in Edges)
                {
                    bool Resume = Relax(edge);
                    if (Resume) Condition = true;
                }

                return Condition;
            }

            public bool CheckNegativeCycle()
            {
                bool Flag = false;

                for (long i = 0; i < NodeCount; i++)
                {
                    Flag = CheckNegativeCycle(i);
                    if (Flag) return true;
                }

                return false;
            }

            public string[] BellmanFord(long S)
            {
                string[] Result = new string[NodeCount];
                List<long> Reachable = DetectInfiniteArbitrage(S);

                foreach (long reach in Reachable)
                {
                    Result[reach] = "-";
                }

                for (long i = 0; i < NodeCount; i++)
                {
                    if (Result[i] == null || !Result[i].Equals("-"))
                    {
                        if (Dists[(int)i].Equals(long.MaxValue)) Result[i] = "*";
                        else Result[i] = Dists[(int)i].ToString();
                    }
                }

                //Dists[(int)S] = 0;
                //bool Flag = true;
                //bool Condition;

                //while (Flag)
                //{
                //    Condition = false;
                //    foreach (Edge edge in Edges)
                //    {
                //        bool Resume = Relax(edge);
                //        if (Resume) Condition = true;
                //    }
                //    if (!Condition) break;
                //}

                return Result;
            }

            public List<long> DetectInfiniteArbitrage(long S)
            {
                Queue<long> queue = new Queue<long>();
                Dists[(int)S] = 0;
                bool Condition = false;
                List<long> Result = new List<long>();

                for (long i = 0; i < NodeCount - 1; i++)
                {
                    Condition = false;
                    foreach (Edge edge in Edges)
                    {
                        bool Resume = Relax(edge);
                        if (Resume) Condition = true;
                    }
                    if (!Condition) break;
                }

                if (!Condition)
                {
                    return new List<long>();
                }

                long counter = 0;

                foreach (Edge edge in Edges)
                {
                    bool Resume = Relax(edge);
                    if (Resume)
                    {
                        queue.Enqueue(edge.E);
                        if (counter == 0)
                        {
                            XNegativeCycleStarter = edge.V;
                            counter++;
                        }
                    }
                }

                while (queue.Count > 0)
                {
                    long Node = queue.Dequeue();
                    Result.Add(Node);
                    Marked[(int)Node] = true;
                    foreach (long adj in Nodes[(int)Node])
                    {
                        if (!Marked[(int)adj] && !queue.Contains(adj))
                        {
                            queue.Enqueue(adj);
                            Parents[(int)adj] = Node;
                        }
                    }
                }

                return Result;
            }

            public bool IsThereArbitragePath(long S, long U)
            {
                List<long> Reachable = DetectInfiniteArbitrage(S);
                if (!Reachable.Contains(U))
                {
                    return false;
                }
                return true;
            }
        }
        public class Edge
        {
            public long V;
            public long E;
            public long Weight;

            public Edge(long V, long E, long Weight)
            {
                this.V = V;
                this.E = E;
                this.Weight = Weight;
            }
        }
    }
}
