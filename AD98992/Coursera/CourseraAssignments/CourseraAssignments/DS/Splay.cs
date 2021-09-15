using System;
using System.Collections.Generic;
using System.Text;
//has Some problem with older c# compilers
namespace CourseraAssignments.DS
{
    public class Splay
    {

        //public static void Main(string[] args)
        //{
        //    long OperationNumbers = long.Parse(Console.ReadLine());

        //    string[] Operations = new string[OperationNumbers];
            
        //    for (long i = 0; i < OperationNumbers; i++)
        //    {
        //        Operations[i] = Console.ReadLine();
        //    }

        //    string[] Result = Solve(Operations);

        //    foreach (string s in Result) Console.WriteLine(s);
        //}

        public static string[] Solve(string[] lines)
        {
            X = 0;
            Tree = new SplayTree();
            List<string> result = new List<string>();
            foreach (var line in lines)
            {
                char cmd = line[0];
                string args = line.Substring(1).Trim();
                var output = CommandDict[cmd](args);
                if (null != output)
                    result.Add(output);
            }
            return result.ToArray();
        }

        public static readonly Dictionary<char, Func<string, string>> CommandDict = new Dictionary<char, Func<string, string>>()
        {
            ['+'] = Add,
            ['-'] = Del,
            ['?'] = Find,
            ['s'] = Sum
        };

        public static long M = 1_000_000_001;

        public static long X = 0;

        public static SplayTree Tree;

        private static long Convert(long i)
            => i = (i + X) % M;

        private static string Add(string arg)
        {
            long i = Convert(long.Parse(arg));
            Tree.STAdd(i);
            return null;
        }

        private static string Del(string arg)
        {
            long i = Convert(long.Parse(arg));
            Tree.STDelete(i);
            return null;
        }

        private static string Find(string arg)
        {
            long i = Convert(int.Parse(arg));
            Node node2 = Tree.STFind(Tree.Root, i);
            if (node2 == null || node2.Key != i) return "Not found";
            return "Found";
        }

        private static string Sum(string arg)
        {
            var toks = arg.Split();
            long l = Convert(long.Parse(toks[0]));
            long r = Convert(long.Parse(toks[1]));
            long sum = Tree.sum(l, r);
            X = sum;
            return sum.ToString();
        }

        public class SplayTree
        {
            public Node Root;

            public Node Find(Node Root, long i)
            {
                if (Root == null) return null;
                Node curr = Root;
                while (curr.Key != i)
                {
                    if (curr.Key > i)
                    {
                        if (curr.LeftChild == null) break;
                        curr = curr.LeftChild;
                    }
                    else if (curr.Key < i)
                    {
                        if (curr.RightChild == null) break;
                        curr = curr.RightChild;
                    }
                }
                return curr;
            }

            public void Add(long Key)
            {
                if (this.Root == null)
                {
                    Node root = new Node(Key);
                    this.Root = root;
                    return;
                }
                if (this.Find(this.Root, Key).Key == Key) return;
                Node node = new Node(Key);
                Node Parent = Find(this.Root, Key);
                if (Parent.Key > Key) Parent.LeftChild = node;
                else
                {
                    Parent.RightChild = node;
                }
                node.Parent = Parent;
            }

            public Node STFind(Node Root, long i)
            {
                Node Found = Find(Root, i);
                Splay(i);
                return Found;
            }

            public void STAdd(long Key)
            {
                Add(Key);
                Splay(Key);
            }

            public void Splay(long Key)
            {
                Node node = Find(this.Root, Key);

                if (node == null) return;

                while (node.Parent != null)
                {
                    if (Key == Root.Key) return;
                    if (node == null) return;

                    if (node.Parent.Parent == null)
                    {
                        Zig(node);
                    }
                    else if ((node.Parent.LeftChild == node && node.Parent.Parent.LeftChild == node.Parent) || (node.Parent.RightChild == node && node.Parent.Parent.RightChild == node.Parent))
                    {
                        ZigZig(node);
                    }
                    else
                    {
                        ZigZag(node);
                    }
                }

                Root = node;

            }

            public void Zig(Node node)
            {
                Node parent = node.Parent;
                //LeftZig
                if (parent.RightChild == node)
                {
                    parent.RightChild = node.LeftChild;
                    if (node.LeftChild != null) node.LeftChild.Parent = parent;
                    node.LeftChild = parent;
                    parent.Parent = node;
                    node.Parent = null;
                    this.Root = node;
                }
                //RightZig
                if (parent.LeftChild == node)
                {
                    parent.LeftChild = node.RightChild;
                    if (node.RightChild != null) node.RightChild.Parent = parent;
                    node.RightChild = parent;
                    parent.Parent = node;
                    node.Parent = null;
                    this.Root = node;
                }
            }

            public void ZigZig(Node node)
            {
                Node parent = node.Parent;
                Node ancestor = parent.Parent;
                Node godFather = ancestor.Parent;
                int lr = 0;
                if (godFather != null)
                {
                    if (godFather.LeftChild == ancestor)
                    {
                        lr = -1;
                    }
                    else
                    {
                        lr = 1;
                    }
                }


                //RightZigZig
                if (parent.LeftChild == node)
                {
                    ancestor.LeftChild = parent.RightChild;
                    if (parent.RightChild != null) parent.RightChild.Parent = ancestor;
                    ancestor.Parent = parent;

                    parent.RightChild = ancestor;
                    parent.LeftChild = node.RightChild;
                    if (node.RightChild != null) node.RightChild.Parent = parent;

                    parent.Parent = node;

                    node.RightChild = parent;
                    node.Parent = godFather;
                    if (lr == -1) godFather.LeftChild = node;
                    if (lr == 1) godFather.RightChild = node;
                }

                //LeftZigZig
                else
                {
                    ancestor.RightChild = parent.LeftChild;
                    if (parent.LeftChild != null) parent.LeftChild.Parent = ancestor;
                    ancestor.Parent = parent;

                    parent.LeftChild = ancestor;
                    parent.RightChild = node.LeftChild;
                    if (node.LeftChild != null) node.LeftChild.Parent = parent;
                    parent.Parent = node;

                    node.LeftChild = parent;
                    node.Parent = godFather;
                    if (lr == -1) godFather.LeftChild = node;
                    if (lr == 1) godFather.RightChild = node;
                }
            }

            public void ZigZag(Node node)
            {
                Node parent = node.Parent;
                Node ancestor = parent.Parent;
                Node godFather = ancestor.Parent;
                int lr = 0;
                if (godFather != null)
                {
                    if (godFather.LeftChild == ancestor)
                    {
                        lr = -1;
                    }
                    else
                    {
                        lr = 1;
                    }
                }

                //left to right
                if (parent.RightChild == node)
                {
                    ancestor.LeftChild = node.RightChild;
                    if (node.RightChild != null) node.RightChild.Parent = ancestor;
                    ancestor.Parent = node;

                    parent.RightChild = node.LeftChild;
                    if (node.LeftChild != null) node.LeftChild.Parent = parent;
                    parent.Parent = node;

                    node.LeftChild = parent;
                    node.RightChild = ancestor;
                    node.Parent = godFather;
                    if (lr == -1) godFather.LeftChild = node;
                    if (lr == 1) godFather.RightChild = node;
                }

                //right to left
                else
                {
                    ancestor.RightChild = node.LeftChild;
                    if (node.LeftChild != null) node.LeftChild.Parent = ancestor;
                    ancestor.Parent = node;

                    parent.LeftChild = node.RightChild;
                    if (node.RightChild != null) node.RightChild.Parent = parent;
                    parent.Parent = node;

                    node.RightChild = parent;
                    node.LeftChild = ancestor;
                    node.Parent = godFather;
                    if (lr == -1) godFather.LeftChild = node;
                    if (lr == 1) godFather.RightChild = node;
                }
            }

            public void STDelete(long i)
            {
                Node node = STFind(Root, i);
                if (node == null) return;
                if (node.Key != i) return;
                Node next = Next(node.Key);
                if (next != null)
                {
                    Splay(next.Key);
                    Splay(node.Key);
                    Node l = node.LeftChild;
                    Node r = node.RightChild;
                    if (r != null) r.LeftChild = l;
                    if (l != null) l.Parent = r;
                    this.Root = r;
                    r.Parent = null;
                    return;
                }
                else
                {
                    Splay(node.Key);
                    Node l = node.LeftChild;
                    if (l != null)
                    {
                        l.Parent = null;
                        Root = l;
                    }
                    else
                    {
                        Root = null;
                    }
                }

            }

            public Node LeftDescendant(Node node)
            {
                if (node.LeftChild == null) return node;
                else
                {
                    return (LeftDescendant(node.LeftChild));
                }
            }

            public Node Next(long i)
            {
                Node node = Find(Root, i);

                if (node.RightChild != null)
                {
                    return LeftDescendant(node.RightChild);
                }
                else
                {
                    Node n = RightAncestor(node);
                    if (n != null) return n;
                    else return null;
                }
            }

            public Node RightAncestor(Node node)
            {
                if (node.Parent == null) return null;
                if (node.Key < node.Parent.Key)
                {
                    return node.Parent;
                }
                else
                {
                    return RightAncestor(node.Parent);
                }
            }

            public long sum(long l, long r)
            {
                long result = 0;
                Node node = STFind(Root, l);
                if (node == null) return 0;
                if (node.Key < l)
                {
                    Node nodeNext = Next(node.Key);
                    if (nodeNext == null) return 0;
                    node = nodeNext;
                }
                while (node != null && node.Key <= r)
                {
                    result += node.Key;
                    node = Next(node.Key);
                }
                return result;
            }


        }

        public class Node
        {
            public long Key;
            public Node LeftChild;
            public Node RightChild;
            public Node Parent;
            public Node(long Key)
            {
                this.Key = Key;
            }
        }

    }
}
