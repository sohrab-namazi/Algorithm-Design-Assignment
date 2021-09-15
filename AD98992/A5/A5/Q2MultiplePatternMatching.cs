using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q2MultiplePatternMatching : Processor
    {
        public Q2MultiplePatternMatching(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, string[] patterns)
        {
            List<long> Result = new List<long>();

            Trie trie = new Trie();

            trie.ConstructTrie(patterns);

            long TextSize = text.Length;

            for (int i = 0; i < TextSize; i++)
            {
                string Suffix = text.Substring(i);
                if (trie.TryMatch(Suffix + "$"))
                {
                    Result.Add(i);
                }
            }

            if (Result.Count == 0) return new long[] { -1 };

            return Result.ToArray();
        }

        public class Trie
        {
            public Node Root;
            //List<string> Edges;
            public long NodeCount = 1;
            //public List<Node> Nodes;

            public Trie()
            {
                Root = new Node(' ');
                //Edges = new List<string>();
            }

            public void ConstructTrie(string[] patterns)
            {
                Node CurrentNode;
                char CurrentSymbol;

                foreach (string Pattern in patterns)
                {
                    CurrentNode = Root;
                    for (int i = 0; i < Pattern.Length; i++)
                    {
                        CurrentSymbol = Pattern[i];
                        Node NextNode = CheckEdge(CurrentNode, CurrentSymbol);
                        if (NextNode != null)
                        {
                            CurrentNode = NextNode;
                        }
                        else
                        {
                            Node NewNode = new Node(CurrentSymbol);
                            CurrentNode.Childs.Add(NewNode);
                            //Edges.Add(CurrentNode.Number + "->" + NewNode.Number + ":" + CurrentSymbol);
                            //NodeCount++;
                            CurrentNode = NewNode;
                        }
                    }
                    CurrentNode.Childs.Add(new Node('$'));
                    NodeCount++;
                }
            }

            public Node CheckEdge(Node currentNode, char currentSymbol)
            {
                foreach (Node node in currentNode.Childs)
                {
                    if (node.Label.Equals(currentSymbol)) return node;
                    if (node.Label.Equals('$')) return new Node('$');
                }

                return null;
            }

            public bool TryMatch(string suffix)
            {
                Node CurrentNode = Root;
                int i = 0;
                char CurrentSymbol = suffix[i];

                while (true)
                {
                    if (CheckDollar(CurrentNode)) return true;
                    Node node = CheckEdge(CurrentNode, CurrentSymbol);
                    if (node != null)
                    {
                        i++;
                        CurrentSymbol = suffix[i];
                        CurrentNode = node;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public bool CheckDollar(Node currentNode)
            {
                foreach (Node node in currentNode.Childs)
                {
                    if (node.Label.Equals('$')) return true;
                }

                return false;
            }
        }

        public class Node
        {
            //public long Number;
            public char Label;
            public List<Node> Childs;

            public Node(char Label)
            {
                //this.Number = Number;
                this.Label = Label;
                Childs = new List<Node>();
            }

        }
    }
}


