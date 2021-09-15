using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.DS
{
    public class IsBSTEasy
    {

        //public static void Main(string[] args)
        //{
        //    long NodeCounts = long.Parse(Console.ReadLine());


        //    long[][] Nodes = new long[NodeCounts][];

        //    for (long i = 0; i < NodeCounts; i++)
        //    {
        //        long[] Node = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //        Nodes[i] = Node;
        //    }

        //    if (Solve(Nodes)) Console.WriteLine("CORRECT");
        //    else Console.WriteLine("INCORRECT");
        //}


        public static bool Solve(long[][] nodes)
        {
            if (nodes.Length == 0) return true;
            long Size = nodes.Length;
            Node[] Tree = new Node[Size];
            long i = 0;
            foreach (long[] node in nodes)
            {
                long Key = node[0];
                long L = node[1];
                long R = node[2];
                Node Node = new Node(Key, L, R);
                Tree[i] = Node;
                i++;

            }

            foreach (Node Node in Tree)
            {
                if (Node.LeftChildIndex != -1)
                {
                    Node.LeftChild = Tree[Node.LeftChildIndex];
                }
                else
                {
                    Node.LeftChild = null;
                }
                if (Node.RightChildIndex != -1)
                {
                    Node.RightChild = Tree[Node.RightChildIndex];
                }
                else
                {
                    Node.RightChild = null;
                }
            }
            Node Root = Tree[0];

            return IsBST(Root, long.MinValue, long.MaxValue);
        }

        public static bool IsBST(Node node, long Min, long Max)
        {
            if (node.Key <= Min || node.Key >= Max) return false;

            if (node.LeftChild == null && node.RightChild == null) return true;
            else if (node.LeftChild == null)
            {
                return IsBST(node.RightChild, node.Key, Max);
            }
            else if (node.RightChild == null)
            {
                return IsBST(node.LeftChild, Min, node.Key);
            }
            else
            {
                return ((IsBST(node.RightChild, node.Key, Max))) && (IsBST(node.LeftChild, Min, node.Key));
            }


        }


        public class Node
        {
            public bool Marked;
            public Node Parent;
            public long LeftChildIndex;
            public long RightChildIndex;
            public long Key;
            public Node LeftChild;
            public Node RightChild;
            public Node(long Key, long LeftChild, long RightChild)
            {
                this.Key = Key;
                this.LeftChildIndex = LeftChild;
                this.RightChildIndex = RightChild;
            }

        }

    }
}
