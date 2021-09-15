using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.DS
{
    public class BinaryTreeTraversal
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

        //    long[][] Result = Solve(Nodes);

        //    foreach (long[] res in Result)
        //    {
        //        foreach (long r in res) Console.Write(r + " ");
        //        Console.WriteLine();
        //    }
        //}

        public static long[][] Solve(long[][] nodes)
        {
            long[][] Result = new long[3][];
            long Size = nodes.Length;
            Node[] Tree = new Node[Size];
            long[] PreOrder = new long[Size];
            long[] InOrder = new long[Size];
            long[] PostOrder = new long[Size];
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


            Stack<Node> stack = new Stack<Node>();
            Node Current = Tree[0];
            long j = 0;

            while (Current != null || stack.Count > 0)
            {
                while (Current != null)
                {
                    stack.Push(Current);
                    Current = Current.LeftChild;
                }

                Node Popped = stack.Pop();
                Current = Popped;
                InOrder[j] = Popped.Key;
                j++;

                Current = Current.RightChild;


            }

            j = 0;
            Current = Tree[0];
            stack.Clear();

            while (Current != null || stack.Count > 0)
            {
                while (Current != null)
                {
                    PreOrder[j] = Current.Key;
                    stack.Push(Current);
                    j++;
                    Current = Current.LeftChild;
                }

                Current = stack.Pop();
                Current = Current.RightChild;
            }

            j = 0;
            Current = Tree[0];
            stack.Clear();

            while (Current != null || stack.Count > 0)
            {
                while (Current != null)
                {
                    PostOrder[j] = Current.Key;
                    stack.Push(Current);
                    j++;
                    Current = Current.RightChild;
                }

                Current = stack.Pop();
                Current = Current.LeftChild;
            }

            Array.Reverse(PostOrder);

            Result[0] = InOrder;
            Result[1] = PreOrder;
            Result[2] = PostOrder;
            return Result;
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
