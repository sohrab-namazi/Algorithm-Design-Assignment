using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.Sat;
using TestCommon;

namespace A11
{
    public class Q4RescheduleExam : Processor
    {
        public Q4RescheduleExam(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, char[], long[][], char[]>)Solve);

        public override Action<string, string> Verifier =>
            TestTools.GraphColorVerifier;


        public virtual char[] Solve(long nodeCount, char[] colors, long[][] edges)
        {
            string Result = "";
            Graph graph = new Graph(nodeCount, colors);
            foreach (long[] edge in edges)
            {
                long a = edge[0];
                long b = edge[1];
                graph.AddEdge((int)a - 1, (int)b - 1);
            }

            long VarNums = nodeCount * 3;
            IntVar[] Vars = new IntVar[VarNums];
            CpModel model = new CpModel();
            for (long i = 0; i < VarNums; i++)
            {
                IntVar var = model.NewBoolVar((i + 1).ToString());
                Vars[i] = var;
            }

            int j = 0;
            for (long i = 0; i < VarNums; i += 3)
            {
                IntVar R = Vars[i];
                IntVar G = Vars[i + 1];
                IntVar B = Vars[i + 2];
                char c = graph.Nodes[j].color;
                if (c.Equals('R'))
                {
                    model.AddBoolAnd(new ILiteral[] { R.Not(), R.Not() });
                }
                else if (c.Equals('G'))
                {
                    model.AddBoolAnd(new ILiteral[] { G.Not(), G.Not() });
                }
                else
                {
                    model.AddBoolAnd(new ILiteral[] { B.Not(), B.Not() });
                }
                model.AddBoolOr(new ILiteral[] { R, G, B });
                model.AddBoolOr(new ILiteral[] { R.Not(), G.Not() });
                model.AddBoolOr(new ILiteral[] { R.Not(), B.Not() });
                model.AddBoolOr(new ILiteral[] { B.Not(), G.Not() });
                j++;
            }

            foreach (long[] edge in edges)
            {
                long X = (edge[0] - 1) * 3;
                long Y = (edge[1] - 1) * 3;
                IntVar XR = Vars[X];
                IntVar XG = Vars[X + 1];
                IntVar XB = Vars[X + 2];
                IntVar YR = Vars[Y];
                IntVar YG = Vars[Y + 1];
                IntVar YB = Vars[Y + 2];

                model.AddBoolOr(new ILiteral[] { XR.Not(), YR.Not() });
                model.AddBoolOr(new ILiteral[] { XG.Not(), YG.Not() });
                model.AddBoolOr(new ILiteral[] { XB.Not(), YB.Not() });


            }

            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);
            if (status == CpSolverStatus.Feasible)
            {
                for (long i = 0; i < VarNums; i += 3)
                {
                    long a = solver.Value(Vars[i]);
                    long b = solver.Value(Vars[i + 1]);
                    long c = solver.Value(Vars[i + 2]);
                    if (a == 1)
                    {
                        Result += "R";
                    }
                    if (b == 1)
                    {   
                        Result += "G";
                    }
                    if (c == 1)
                    {
                        Result += "B";
                    }
                }
            }
            else
            {
                Result = "IMPOSSIBLE";
            }
            return Result.ToCharArray();
        }

        public class Graph
        {
            public long NodeCount;
            public List<Node> Nodes;

            public Graph(long NodeCount, char[] colors)
            {
                this.NodeCount = NodeCount;
                Nodes = new List<Node>();
                for (long i = 0; i < NodeCount; i++)
                {
                    Nodes.Add(new Node(i, colors[i]));
                }
            }

            public void AddEdge(int a, int b)
            {
                Nodes[a].adjs.Add(Nodes[b]);
                Nodes[b].adjs.Add(Nodes[a]);
            }
        }

        public class Node
        {
            public long Index;
            public char color;
            public List<Node> adjs;

            public Node(long Index, char color)
            {
                this.color = color;
                this.Index = Index;
                adjs = new List<Node>();
            }
        }
    }
}
