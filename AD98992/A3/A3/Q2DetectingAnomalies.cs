using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q2DetectingAnomalies:Processor
    {
        public Q2DetectingAnomalies(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long[] dist;

        public long Solve(long nodeCount, long[][] edges)
        {
            Graph graph = new Graph(nodeCount);
            for (long i = 0; i < edges.Length; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1, edges[i][2]);
            }
            if (graph.CheckNegativeCycle()) return 1;
            return 0;
        }

        public class Graph
        {
            public long NodeCount;
            public List<Edge> Edges;
            public List<long> Dists;
            public List<long> Parents;
            public List<bool> Marked;
            public List<List<long>> Nodes;

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

                Console.WriteLine(Condition);
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

