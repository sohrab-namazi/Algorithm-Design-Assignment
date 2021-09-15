using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion:Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long,long[][], long[]>)Solve);

        

        public static long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            Graph graph = new Graph(NodeCount);
            for (long i = 0; i < EdgeCount; i++)
            {
                graph.AddEdge(edges[i][0] - 1, edges[i][1] - 1, edges[i][2]);
            }
            long[] Result = new long[QueriesCount];

            for (long i = 0; i < QueriesCount; i++)
            {
                long res = graph.Dijkstra(Queries[i][0] - 1, Queries[i][1] - 1, NodeCount);
                Result[i] = res;
            }
            return Result;
        }

        public class Graph
        {
            public long NodeCount;
            public List<List<Edge>> Nodes;
            public List<List<Edge>> NodesR;
            //public List<long> Dists;
            //public List<long> DistsR;
            //public List<bool> Proc;
            //public List<bool> ProcR;
            public long[] Dists;
            public long[] DistsR;
            public bool[] Proc;
            public bool[] ProcR;
            SimplePriorityQueue<long> queue;
            SimplePriorityQueue<long> queueR;

            public Graph(long Nodecount)
            {
                NodeCount = Nodecount;
                Nodes = new List<List<Edge>>();
                NodesR = new List<List<Edge>>();
                Proc = new bool[Nodecount]; 
                ProcR = new bool[Nodecount]; 
                Dists = new long[Nodecount]; 
                DistsR = new long[Nodecount];
                queue = new SimplePriorityQueue<long>();
                queueR = new SimplePriorityQueue<long>();

                for (long i = 0; i < Nodecount; i++)
                {
                    Nodes.Add(new List<Edge>());
                    NodesR.Add(new List<Edge>());
                }
            }

            public void AddEdge(long a, long b, long w)
            {
                Nodes[(int)a].Add(new Edge(b, w));
                NodesR[(int)b].Add(new Edge(a, w));
            }

            public long Dijkstra(long S, long T, long NodeCount)
            {
                Array.Fill(Dists, long.MaxValue);
                Array.Fill(DistsR, long.MaxValue);
                Proc = new bool[NodeCount];
                ProcR = new bool[NodeCount];
                Dists[(int)S] = 0;
                DistsR[(int)T] = 0;
                queue.Clear();
                queueR.Clear();

                for (long i = 0; i < NodeCount; i++)
                {
                    queue.Enqueue(i, Dists[(int)i]);
                    queueR.Enqueue(i, DistsR[(int)i]);
                }

                while (true)
                {
                    long V = queue.Dequeue();
                    if (Dists[(int)V] != long.MaxValue)
                    {
                        Process((int)V, queue);
                    }
                    if (ProcR[(int)V])
                    {
                        return ShortestPath();
                    }

                    long VR = queueR.Dequeue();
                    if (DistsR[(int)VR] != long.MaxValue)
                    {
                        ProcessR((int)VR, queueR);
                    }
                    if (Proc[(int)VR])
                    {
                        return ShortestPath();
                    }
                }
            }

            public void Process(long U, SimplePriorityQueue<long> queue)
            {
                Proc[(int)U] = true;
                foreach (Edge edge in Nodes[(int)U])
                {
                    if (!Proc[(int) edge.E])
                    {
                        Relax(U, edge, queue);
                    }
                }
            }

            public void ProcessR(long U, SimplePriorityQueue<long> queueR)
            {
                ProcR[(int)U] = true;
                foreach (Edge edge in NodesR[(int)U])
                {
                    if (!ProcR[(int) edge.E])
                    {
                        RelaxR(U, edge, queueR);
                    }
                }
            }

            public void Relax(long U, Edge edge, SimplePriorityQueue<long> queue)
            {
                if (Dists[(int)edge.E] > Dists[(int)U] + edge.Weight)
                {
                    Dists[(int)edge.E] = Dists[(int)U] + edge.Weight;
                    queue.TryUpdatePriority(edge.E, Dists[(int)edge.E]);
                }
            }

            public void RelaxR(long U, Edge edge, SimplePriorityQueue<long> queueR)
            {
                if (DistsR[(int)edge.E] > DistsR[(int)U] + edge.Weight)
                {
                    DistsR[(int)edge.E] = DistsR[(int)U] + edge.Weight;
                    queueR.TryUpdatePriority(edge.E, DistsR[(int)edge.E]);
                }
            }

            public long ShortestPath()
            {
                long Distane = long.MaxValue;

                for (long i = 0; i < NodeCount; i++)
                {
                    if ((Dists[(int)i] != long.MaxValue) && (DistsR[(int)i] != long.MaxValue) && (Dists[(int)i] + DistsR[(int)i] < Distane) && (Proc[(int)i] || ProcR[(int)i]))
                    {
                        Distane = Dists[(int)i] + DistsR[(int)i];
                    }
                }
                if (Distane == long.MaxValue) return -1;

                return Distane;
            }
        }

        public class Edge
        {
            public long E;
            public long Weight;

            public Edge(long E, long Weight)
            {
                this.E = E;
                this.Weight = Weight;
            }
        }
    }
}
