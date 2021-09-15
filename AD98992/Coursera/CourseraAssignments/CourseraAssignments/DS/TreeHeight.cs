using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AD.String
{
    public class TreeHeight
    {
        //public static void Main(string[] args)
        //{
        //    long NodeCount = long.Parse(Console.ReadLine());
        //    string[] Input = Console.ReadLine().Split();
        //    long[] Tree = Input.Select(long.Parse).ToArray();
        //    long Result = Solve(NodeCount, Tree);
        //    Console.WriteLine(Result);
        //}

        public static long Solve(long nodeCount, long[] tree)
        {
            Tree TreeBFS = new Tree((int)nodeCount, tree);

            long Result = TreeBFS.BFS((int)TreeBFS.Root);

            return Result;
        }
        public class Tree
        {
            public List<List<long>> Nodes = new List<List<long>>();
            public long Root;
            public int NodeCount;
            public List<bool> Marked = new List<bool>();
            public List<long> Heights = new List<long>();

            public Tree(int NodeCount, long[] Nodes)
            {
                this.NodeCount = NodeCount;
                for (int i = 0; i < NodeCount; i++)
                {
                    this.Nodes.Add(new List<long>());
                    this.Marked.Add(false);
                    this.Heights.Add(0);
                }
                for (int i = 0; i < NodeCount; i++)
                {
                    if (Nodes[i] >= 0)
                    {
                        this.Nodes[(int)Nodes[i]].Add(i);
                    }
                    else
                    {
                        this.Root = i;
                    }
                }
            }

            public long BFS(int source)
            {
                Queue<int> queue = new Queue<int>();
                queue.Enqueue(source);

                while (queue.Count != 0)
                {
                    int FirstNode = queue.Dequeue();
                    Marked[FirstNode] = true;
                    foreach (int node in Nodes[FirstNode])
                    {
                        if (!Marked[node])
                        {
                            Heights[node] = Heights[FirstNode] + 1;
                            queue.Enqueue(node);
                        }
                    }
                }

                long max = 0;

                foreach (long height in Heights)
                {
                    if (height > max) max = height;
                }

                return max + 1;
            }

            public void DFS(int source)
            {
                Marked[source] = true;

                foreach (int child in Nodes[source])
                {
                    if (!Marked[child])
                    {
                        Heights[child] = Heights[source] + 1;
                        DFS(child);
                    }
                }
            }

        }

    }
}
