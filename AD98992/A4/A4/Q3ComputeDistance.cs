using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using GeoCoordinatePortable;
using Priority_Queue;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public static readonly char[] IgnoreChars = new char[] { '\n', '\r', ' ' };
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        private static double[][] ReadTree(IEnumerable<string> lines)
        {
            return lines.Select(line =>
                line.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(n => double.Parse(n)).ToArray()
                            ).ToArray();
        }
        public override string Process(string inStr)
        {
            return Process(inStr, (Func<long, long, double[][], double[][], long,
                                    long[][], double[]>)Solve);
        }
        public static string Process(string inStr, Func<long, long, double[][]
                                  , double[][], long, long[][], double[]> processor)
        {
            var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            long[] count = lines.First().Split(IgnoreChars,
                                               StringSplitOptions.RemoveEmptyEntries)
                                         .Select(n => long.Parse(n))
                                         .ToArray();
            double[][] points = ReadTree(lines.Skip(1).Take((int)count[0]));
            double[][] edges = ReadTree(lines.Skip(1 + (int)count[0]).Take((int)count[1]));
            long queryCount = long.Parse(lines.Skip(1 + (int)count[0] + (int)count[1])
                                         .Take(1).FirstOrDefault());
            long[][] queries = ReadTree(lines.Skip(2 + (int)count[0] + (int)count[1]))
                                        .Select(x => x.Select(z => (long)z).ToArray())
                                        .ToArray();

            return string.Join("\n", processor(count[0], count[1], points, edges,
                                queryCount, queries));
        }
        public double[] Solve(long nodeCount,
                            long edgeCount,
                            double[][] points,
                            double[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            Graph graph = new Graph(nodeCount);

            double[] Result = new double[queriesCount];

            for (long i = 0; i < nodeCount; i++)
            {
                graph.Nodes[(int)i].X = points[i][0];
                graph.Nodes[(int)i].Y = points[i][1];
            }

            for (long i = 0; i < edgeCount; i++)
            {
                graph.AddEdge((long)edges[i][0] - 1, (long)edges[i][1] - 1, edges[i][2]);
            }

            for (long i = 0; i < queriesCount; i++)
            {
                Result[i] = graph.AStar(queries[i][0] - 1, queries[i][1] - 1);
            }

            return Result;

        }


    }

    public class Graph
    {
        public long NodeCount;
        public List<Node> Nodes;
        public List<long> Parents;
        public List<bool> Marked;
        public List<double> HValue;
        public List<double> DistValue;
        public List<double> TotalDists;
        SimplePriorityQueue<long> queue;

        public Graph(long NodeCount)
        {
            this.NodeCount = NodeCount;
            Nodes = new List<Node>();
            Parents = new List<long>();
            Marked = new List<bool>();
            DistValue = new List<double>();
            TotalDists = new List<double>();
            queue = new SimplePriorityQueue<long>();
            HValue = new List<double>();

            for (long i = 0; i < NodeCount; i++)
            {
                Nodes.Add(new Node(i));
                Parents.Add(-1);
                Marked.Add(false);
                DistValue.Add(long.MaxValue);
                TotalDists.Add(long.MaxValue);
                HValue.Add(double.MaxValue);
            }

        }

        public void AddEdge(long u, long v, double w)
        {
            Nodes[(int)u].Edges.Add(new Edge(v, w));
        }

        public double AStar(long S, long T)
        {
            for (long i = 0; i < NodeCount; i++)
            {
                Marked[(int)i] = false;
                Parents[(int)i] = -1;
                DistValue[(int)i] = long.MaxValue;
                TotalDists[(int)i] = long.MaxValue;
                HValue[(int)i] = ComputeDistane(i, T);
            }
            queue.Clear();
            double Result = 0;
            DistValue[(int)S] = 0;
            UpdateTotalDist(S);

            for (long i = 0; i < NodeCount; i++)
            {
                queue.Enqueue(i, (float)TotalDists[(int)i]);
            }

            while (!Marked[(int)T] == true && queue.Count > 0)
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
                            Parents[(int)edge.E] = node;
                            UpdateTotalDist(edge.E);
                            queue.TryUpdatePriority(edge.E, (float)TotalDists[(int)edge.E]);
                        }
                    }
                }
            }

            if (DistValue[(int)T] != long.MaxValue)
            {
                Result = DistValue[(int)T];
            }

            else
            {
                Result = -1;
            }

            return Math.Round(Result, 6);
        }

        public double ComputeDistane(long i, long t)
        {
            return Math.Sqrt(Math.Pow(Nodes[(int)i].X - Nodes[(int)t].X, 2) + Math.Pow(Nodes[(int)i].Y - Nodes[(int)t].Y, 2));
        }

        public void UpdateTotalDist(long i)
        {
            TotalDists[(int)i] = DistValue[(int)i] + HValue[(int)i];
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

        public Node(long Index)
        {
            this.Index = Index;
            Edges = new List<Edge>();
        }
    }


}