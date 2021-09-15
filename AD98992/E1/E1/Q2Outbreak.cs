using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q2Outbreak : Processor
    {
        public Q2Outbreak(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string[], string>)Solve);

        public static Tuple<int, int, int[,], int[,]> ProcessQ2(string[] data)
        {
            var temp = data[0].Split();
            int N = int.Parse(temp[0]);
            int M = int.Parse(temp[1]);
            int[,] carriers = new int[N, 2];
            int[,] safe = new int[M, 2];
            for (int i = 0; i < N; i++)
            {
                carriers[i, 0] = int.Parse(data[i + 1].Split()[0]);
                carriers[i, 1] = int.Parse(data[i + 1].Split()[1]);
            }

            for (int i = 0; i < M; i++)
            {
                safe[i, 0] = int.Parse(data[i + N + 1].Split()[0]);
                safe[i, 1] = int.Parse(data[i + N + 1].Split()[1]);
            }
            return Tuple.Create(N, M, carriers, safe);
        }
        public string Solve(string[] input)
        {
            var data = ProcessQ2(input);
            return Solve(data.Item1,data.Item2,data.Item3,data.Item4).ToString();
        }
        public double Solve(int N, int M, int[,] carrier, int[,] safe)
        {
            // M = safe
            double res = 0;

            Graph graph = new Graph(M + N);
            graph.MCount = M;

            for (long i = 0; i < M; i++)
            {
                graph.Nodes[(int)i].X = safe[i, 0];
                graph.Nodes[(int)i].Y = safe[i, 1];
                graph.Nodes[(int)i].Type = 0;
            }

            for (long i = M; i < M + N; i++)
            {
                graph.Nodes[(int)i].X = carrier[i - M, 0];
                graph.Nodes[(int)i].Y = carrier[i - M, 1];
                graph.Nodes[(int)i].Type = 1;
            }

            for (long i = M; i < M + N; i++)
            {
                for (long j = 0; j < M; j++)
                {
                    graph.AddEdge(i, j, ComputeDistance(graph.Nodes[(int)i], graph.Nodes[(int)j]));
                }
            }

            for (long i = M; i < M + N; i++)
            {
                graph.Dijkstra(i);
            }



            return res;
        }
        public static double ComputeDistance(Node i, Node j)
        {
            double ix = i.X;
            double iy = i.Y;
            double jx = j.X;
            double jy = j.Y;

            return Math.Sqrt(Math.Pow(ix - jx, 2) + Math.Pow(iy - jy, 2));

        }
    }
    public class Graph
    {
        public long NodeCount;
        public List<Node> Nodes;
        public List<long> Parents;
        public List<bool> Marked;
        public List<double> DistValue;
        SimplePriorityQueue<long> queue;
        public List<double> GeneralDists;
        public long MCount;

        public Graph(long NodeCount)
        {
            this.NodeCount = NodeCount;
            Nodes = new List<Node>();
            Parents = new List<long>();
            Marked = new List<bool>();
            DistValue = new List<double>();
            queue = new SimplePriorityQueue<long>();
            GeneralDists = new List<double>();

            for (long i = 0; i < NodeCount; i++)
            {
                Nodes.Add(new Node(i));
                Parents.Add(-1);
                Marked.Add(false);
                DistValue.Add(long.MaxValue);
            }
            for (long i = 0; i < MCount; i++)
            {
                GeneralDists.Add(long.MaxValue);
            }

        }

        public void AddEdge(long u, long v, double w)
        {
            Nodes[(int)u].Edges.Add(new Edge(v, w));
        }

        public void Dijkstra(long S)
        {
            for (long i = 0; i < NodeCount; i++)
            {
                Marked[(int)i] = false;
                Parents[(int)i] = -1;
                DistValue[(int)i] = long.MaxValue;
            }

            queue.Clear();
            DistValue[(int)S] = 0;

            for (long i = 0; i < NodeCount; i++)
            {
                queue.Enqueue(i, (float)DistValue[(int)i]);
            }

            while (queue.Count > 0)
            {
                long node = queue.Dequeue();
                Marked[(int)node] = true;

                foreach (Edge edge in Nodes[(int)node].Edges)
                {
                    if (!Marked[(int)edge.E])
                    {
                        if (DistValue[(int)edge.E] > DistValue[(int)node] + edge.W)
                        {
                            DistValue[(int)edge.E] = DistValue[(int)node] + edge.W;
                            if (GeneralDists[(int) edge.E - GeneralDists.Count] > DistValue[(int)edge.E])
                            {
                                GeneralDists[(int)edge.E - GeneralDists.Count] = DistValue[(int)edge.E];
                            }
                            Parents[(int)edge.E] = node;
                            queue.TryUpdatePriority(edge.E, (float)DistValue[(int)edge.E]);
                        }
                    }
                }
            }
        }

        public double ComputeDistane(long i, long t)
        {
            return Math.Sqrt(Math.Pow(Nodes[(int)i].X - Nodes[(int)t].X, 2) + Math.Pow(Nodes[(int)i].Y - Nodes[(int)t].Y, 2));
        }

    }

    public class Edge
    {
        public long E;
        public double W;

        public Edge(long E, double W)
        {
            this.E = E;
            this.W = W;
        }
    }

    public class Node
    {
        public long Index;
        public List<Edge> Edges;
        public double X;
        public double Y;
        //0 = safe
        public long Type;

        public Node(long Index)
        {
            this.Index = Index;
            Edges = new List<Edge>();
        }
    }

}
