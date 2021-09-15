using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q4SuffixTree : Processor
    {
        public Q4SuffixTree(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String[]>)Solve);

        public string[] Solve(string text)
        {
            Trie trie = new Trie(text);

            long TextSize = text.Length;

            for (int i = 0; i < TextSize; i++)
            {
                trie.AddSuffix(i);
            }

            trie.DFS(trie.Root);

            trie.Edges.RemoveAt(0);

            return trie.Edges.ToArray();
        }

        public class Trie
        {
            string Text;
            public Node Root;
            public List<string> Edges;

            public Trie(string Text)
            {
                this.Text = Text;
                Root = new Node("");
                Edges = new List<string>();
            }

            public void AddSuffix(int i)
            {
                string Suffix = Text.Substring(i);
                Node CurrentNode = Root;

                while (true)
                {
                    int match = TryMatch(Suffix, CurrentNode);

                    if (match == -1)
                    {
                        Suffix = Suffix.Substring(CurrentNode.Label.Length);
                        Node nextNode = CheckEdge(CurrentNode, Suffix[0]);
                        if (nextNode != null)
                        {
                            CurrentNode = nextNode;
                            continue;
                        }
                        else
                        {
                            CurrentNode.Childs.Add(new Node(Suffix));
                            break;
                        }
                    }

                    else if (match == 0)
                    {
                        Node nextNode = CheckEdge(CurrentNode, Suffix[0]);
                        if (nextNode != null)
                        {
                            CurrentNode = nextNode;
                            continue;
                        }
                        else
                        {
                            CurrentNode.Childs.Add(new Node(Suffix));
                            break;
                        }
                    }

                    else
                    {
                        Suffix = Suffix.Substring(match);
                        string FormerLabel = CurrentNode.Label;
                        CurrentNode.Label = FormerLabel.Substring(0, match);
                        Node NextNode = new Node(FormerLabel.Substring(match));
                        NextNode.Childs.AddRange(CurrentNode.Childs);
                        CurrentNode.Childs.Clear();
                        CurrentNode.Childs.Add(NextNode);
                        CurrentNode.Childs.Add(new Node(Suffix));
                        break;
                    }
                }
            }

            public int TryMatch(string suffix, Node currentNode)
            {
                int match = 0;
                string Label = currentNode.Label;
                for (int i = 0; i < Label.Length; i++)
                {
                    if (!Label[i].Equals(suffix[i])) break;
                    match++;
                }

                if (match == Label.Length) return -1;
                return match;
            }

            public Node CheckEdge(Node currentNode, char curretnSymbol)
            {
                foreach (Node node in currentNode.Childs)
                {
                    if (node.Label[0].Equals(curretnSymbol)) return node;
                }
                return null;
            }

            public void DFS(Node node)
            {
                Edges.Add(node.Label);
                foreach (Node adj in node.Childs) DFS(adj);
            }
        }

        public class Node
        {
            public List<Node> Childs;
            public string Label;

            public Node(string Label)
            {
                this.Label = Label;
                Childs = new List<Node>();
            }
        }
    }
}
