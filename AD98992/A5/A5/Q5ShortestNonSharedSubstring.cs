using Priority_Queue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q5ShortestNonSharedSubstring : Processor
    {
        public Q5ShortestNonSharedSubstring(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, String>)Solve);

        private string Solve(string text1, string text2)
        {
            Trie trie = new Trie();
            trie.MakeTrie(text1 + "$");
            trie.MakeTrie(text2 + "#");
            string res = trie.BFS();
            return res;
        }

        public class Trie
        {
            public List<Node> Nodes;
            public Node Root;
            public long NodeCount = 1;

            public Trie()
            {
                Root = new Node(' ');
                Root.Height = 0;
                Nodes = new List<Node>();
                Nodes.Add(Root);
                Root.HasSharp = true;
                Root.HasDollar = true;
            }

            public void MakeTrie(string text)
            {
                string[] patterns = new string[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    patterns[i] = text.Substring(i);
                }
                if (text[text.Length - 1].Equals('$'))
                {
                    ConstructTrie(patterns, 1);
                }
                else
                {
                    ConstructTrie(patterns, 2);
                }
            }


            public void ConstructTrie(string[] patterns, int trieNumber)
            {
                Node CurrentNode;
                char CurrentSymbol;

                foreach (string Pattern in patterns)
                {
                    CurrentNode = Root;
                    for (int i = 0; i < Pattern.Length; i++)
                    {
                        CurrentSymbol = Pattern[i];
                        Node NextNode = CheckEdge(CurrentNode, CurrentSymbol, trieNumber, i);
                        if (NextNode != null)
                        {
                            CurrentNode = NextNode;
                            if (trieNumber == 1) CurrentNode.HasDollar = true;
                            else CurrentNode.HasSharp = true;
                        }
                        else
                        {
                            Node NewNode = new Node(CurrentSymbol);
                            if (trieNumber == 1) NewNode.HasDollar = true;
                            else NewNode.HasSharp = true;
                            if (NewNode.HasDollar) CurrentNode.HasDollar = true;
                            if (NewNode.HasSharp) CurrentNode.HasSharp = true;
                            NewNode.Parent = CurrentNode;
                            CurrentNode.Childs.Add(NewNode);
                            Nodes.Add(NewNode);
                            NewNode.Height = CurrentNode.Height + 1;
                            CurrentNode = NewNode;
                        }
                    }
                }
            }

            public Node CheckEdge(Node currentNode, char currentSymbol, int trieNumber, long index)
            {
                if (trieNumber == 1)
                {
                    foreach (Node node in currentNode.Childs)
                    {
                        if (node.Label.Equals(currentSymbol)) return node;
                        if (node.Label.Equals('$')) return new Node('$');
                    }
                    return null;
                }
                foreach (Node node in currentNode.Childs)
                {
                    if (node.Label.Equals(currentSymbol)) return node;
                    if (node.Label.Equals('#')) return new Node('#');
                }
                return null;
            }

            public string BFS()
            {
                Node CurrentNode = Root;
                long CurrentHeight = long.MaxValue;
                foreach (Node node in Nodes)
                {
                    if (node.HasDollar && !node.HasSharp && node.Height < CurrentHeight)
                    {
                        if (node.Label.Equals('$')) continue;
                        CurrentNode = node;
                        CurrentHeight = node.Height;
                    }
                }
                return GoRoot(CurrentNode);
            }

            public string GoRoot(Node node)
            {
                List<char> path = new List<char>();
                Node CurrentNode = node;
                while (CurrentNode != Root)
                {
                    path.Add(CurrentNode.Label);
                    CurrentNode = CurrentNode.Parent;
                }
                path.Reverse();
                return new string(path.ToArray());
            }
        }

        public class Node
        {
            public long Height;
            public char Label;
            public List<Node> Childs;
            public bool HasDollar;
            public bool HasSharp;
            public Node Parent;

            public Node(char Label)
            {
                this.Label = Label;
                Childs = new List<Node>();
            }
        }
    }
}
