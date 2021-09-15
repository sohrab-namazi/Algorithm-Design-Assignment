using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q1BuildingRoads : Processor
    {
        public Q1BuildingRoads(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], double>)Solve);

        public double Solve(long pointCount, long[][] points)
        {
            double Result = 0;
            Graph2 graph = new Graph2(pointCount);

            for (long i = 0; i < pointCount; i++)
            {
                for (long j = 0; j < i; j++)
                {
                    if (i != j)
                    {
                        graph.AddEdge(i, j, RadSumSq(points[i][0] - points[j][0], points[i][1] - points[j][1]));
                    }
                }
            }

            List<Edge2> Results = graph.Kruskal();

            foreach (Edge2 edge in Results)
            {
                Result += edge.Weight;
            }

            return Math.Round(Result, 6);
        }

        public static double RadSumSq(long a, long b)
        {
            double Result = 0;
            Result += Math.Sqrt(a * a + b * b);
            return Result;
        }
    }

    internal class Graph2
    {
        public long NodeCount;
        public List<Edge2> Edges;
        public long[] Parents;
        public long[] Rank;

        public Graph2(long NodeCount)
        {
            this.NodeCount = NodeCount;
            Edges = new List<Edge2>();
            Parents = new long[NodeCount];
            Rank = new long[NodeCount];
        }

        public void AddEdge(long V, long E, double W)
        {
            Edges.Add(new Edge2(V, E, W));
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


        public List<Edge2> Kruskal()
        {
            List<Edge2> Result = new List<Edge2>();
            Edges.Sort(delegate (Edge2 edge1, Edge2 edge2)
            { return (edge1.Weight.CompareTo(edge2.Weight)); });

            for (long i = 0; i < NodeCount; i++)
            {
                MakeSet(i);
            }

            foreach (Edge2 edge in Edges)
            {
                if (Find(edge.V) != Find(edge.E))
                {
                    Result.Add(edge);
                    Union(edge.V, edge.E);
                }
            }
            return Result;
        }
    }

    class Edge2
    {
        public long V;
        public long E;
        public double Weight;

        public Edge2(long V, long E, double Weight)
        {
            this.V = V;
            this.E = E;
            this.Weight = Weight;
        }
    }
}
