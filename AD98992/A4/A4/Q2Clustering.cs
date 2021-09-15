using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q2Clustering : Processor
    {
        public Q2Clustering(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, double>)Solve);

        public double Solve(long pointCount, long[][] points, long clusterCount)
        {
            Graph1 graph = new Graph1(pointCount);

            for (long i = 0; i < pointCount; i++)
            {
                graph.Parents[i] = i;
                graph.Rank[i] = 0;
                for (long j = 0; j < i; j++)
                {
                        graph.AddEdge(i, j, RadSumSq(points[i][0] - points[j][0], points[i][1] - points[j][1]));
                }
            }
            double Result = graph.Kruskal(clusterCount);

            return Math.Round(Result, 6);
        }

        public static double RadSumSq(long a, long b)
        {
            double Result = 0;
            Result += Math.Sqrt(a * a + b * b);
            return Result;
        }
    }

    internal class Graph1
    {
        public long NodeCount;
        public List<Edge1> Edges;
        public long[] Parents;
        public long[] Rank;

        public Graph1(long NodeCount)
        {
            this.NodeCount = NodeCount;
            Edges = new List<Edge1>();
            Parents = new long[NodeCount];
            Rank = new long[NodeCount];
        }

        public void AddEdge(long V, long E, double W)
        {
            Edges.Add(new Edge1(V, E, W));
        }

        public void MakeSet(long i)
        {
            Parents[i] = i;
            Rank[i] = 0;
        }

        public long Find(long i)
        {
            if (i != Parents[i])
            {
                Parents[i] = Find(Parents[i]);
            }
            return Parents[i];
        }

        public void Union(long i, long j)
        {
            long IDi = Find(i);
            long IDj = Find(j);

            if (IDi == IDj)
            {
                return;
            }

            if (Rank[IDi] > Rank[IDj])
            {
                Parents[IDj] = IDi;
            }

            else
            {
                Parents[IDi] = IDj;
                if (Rank[IDi] == Rank[IDj])
                {
                    Rank[IDj]++;
                }
            }
        }

        public double Kruskal(long ClusterCount)
        {
            Edges.Sort(delegate (Edge1 edge1, Edge1 edge2)
            { return (edge1.Weight.CompareTo(edge2.Weight)); });

            long Counter = 0;
            double Result = 0;

            foreach (Edge1 edge in Edges)
            {
                if (Find(edge.V) != Find(edge.E))
                {
                    Counter++;
                    Union(edge.V, edge.E);
                    if (Counter == NodeCount - ClusterCount + 1)
                    {
                        Result = edge.Weight;
                        break;
                    }
                }
            }
            return Result;
        }
    }

    class Edge1
    {
        public long V;
        public long E;
        public double Weight;

        public Edge1(long V, long E, double Weight)
        {
            this.V = V;
            this.E = E;
            this.Weight = Weight;
        }
    }
}
