using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A11
{
    public class Q2FunParty : Processor
    {
        public Q2FunParty(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long[], long[][], long>)Solve);

        public virtual long Solve(long n, long[] funFactors, long[][] hierarchy)
        {
            Tree tree = new Tree(n, funFactors);
            foreach (long[] edge in hierarchy)
            {
                tree.AddEdgeTwo(edge[0] - 1, edge[1] - 1);
            }

            tree.DFS(tree.Nodes[0]);

            long Result = tree.FunParty(tree.Nodes[0]);

            return Result;
        }

        public class Tree
        {
            public long NodeCount;
            public List<Node> Nodes;
            public List<Node> NodesTwo;
            public bool[] Marked;

            public Tree(long NodeCount, long[] FunFactors)
            {
                this.NodeCount = NodeCount;
                Nodes = new List<Node>();
                NodesTwo = new List<Node>();
                Marked = new bool[NodeCount];
                for (long i = 0; i < NodeCount; i++)
                {
                    Nodes.Add(new Node(i, FunFactors[(int)i]));
                    NodesTwo.Add(new Node(i, FunFactors[(int)i]));
                }
            }

            public Node FindRoot()
            {
                foreach (Node node in Nodes)
                {
                    if (node.Parent == null)
                    {
                        return node;
                    }
                }
                return null;
            }

            public void AddEdgeTwo(long a, long b)
            {
                NodesTwo[(int)b].Childs.Add(NodesTwo[(int)a]);
                NodesTwo[(int)a].Parent = NodesTwo[(int)b];
                NodesTwo[(int)a].Childs.Add(NodesTwo[(int)b]);
                NodesTwo[(int)b].Parent = NodesTwo[(int)a];
            }

            public void AddEdge(long a, long b)
            {
                Nodes[(int)a].Childs.Add(Nodes[(int)b]);
                Nodes[(int)b].Parent = Nodes[(int)a];
            }

            public long FunParty(Node node)
            {
                if (node.DValue == long.MaxValue)
                {
                    if (node.Childs.Count == 0) return node.FunFactor;
                    else
                    {
                        long m0 = 0;
                        long m1 = 0;
                        m1 = node.FunFactor;

                        foreach (Node Child in node.Childs)
                        {
                            m0 += FunParty(Child);
                            foreach (Node ChildChild in Child.Childs)
                            {
                                m1 += FunParty(ChildChild);
                            }
                        }
                        node.DValue = Math.Max(m0, m1);
                    }
                    return node.DValue;
                }
                else
                {
                    return node.DValue;
                }
            }

            public void DFS(Node node)
            {
                Marked[node.Index] = true;
                foreach (Node Child in NodesTwo[(int)node.Index].Childs)
                {
                    if (!Marked[(int)Child.Index])
                    {
                        AddEdge(node.Index, Child.Index);
                        DFS(Child);
                    }
                }
            }
        }

        public class Node
        {
            public long FunFactor;
            public List<Node> Childs;
            public long Index;
            public Node Parent;
            public long DValue;

            public Node(long Index, long FunFactor)
            {
                DValue = long.MaxValue;
                this.Index = Index;
                this.FunFactor = FunFactor;
                Childs = new List<Node>();
            }
        }
    }
}
