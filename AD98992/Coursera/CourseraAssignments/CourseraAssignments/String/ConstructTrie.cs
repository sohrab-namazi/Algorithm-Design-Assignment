using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.String
{
    public class ConstructTrie
    {
        //public static void Main(string[] args)
        //{
        //    int PatternCount = int.Parse(Console.ReadLine());
        //    string[] Patterns = new string[PatternCount];
        //    for (int i = 0; i < PatternCount; i++)
        //    {
        //        Patterns[i] = Console.ReadLine();
        //    }

        //    string[] Result = Solve(PatternCount, Patterns);

        //    foreach (string s in Result) Console.WriteLine(s);

        //}

        public static string[] Solve(long n, string[] patterns)
        {
            Trie trie = new Trie();
            trie.ConstructTrie(patterns);
            return trie.Edges.ToArray();
        }

        public class Trie
        {
            public Node Root;
            public List<string> Edges;
            public long NodeCount = 1;
            //public List<Node> Nodes;

            public Trie()
            {
                Root = new Node(0, ' ');
                Edges = new List<string>();
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
                            Node NewNode = new Node(NodeCount, CurrentSymbol);

                            CurrentNode.Childs.Add(NewNode);
                            Edges.Add(CurrentNode.Number + "->" + NewNode.Number + ":" + CurrentSymbol);
                            NodeCount++;
                            CurrentNode = NewNode;
                        }
                    }
                    //CurrentNode.Childs.Add(new Node('$'));
                    //NodeCount++;
                }
            }

            public Node CheckEdge(Node currentNode, char currentSymbol)
            {
                foreach (Node node in currentNode.Childs)
                {
                    if (node.Label.Equals(currentSymbol)) return node;
                }

                return null;
            }
        }

        public class Node
        {
            public long Number;
            public char Label;
            public List<Node> Childs;

            public Node(long Number, char Label)
            {
                this.Number = Number;
                this.Label = Label;
                Childs = new List<Node>();
            }

        }
    }
}
