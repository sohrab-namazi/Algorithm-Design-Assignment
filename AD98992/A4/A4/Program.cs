using Priority_Queue;
using System;
using System.Collections.Generic;

namespace A4
{
    class Program
    {
        static void Main(string[] args)
        {
            long NodeCount = 2;
            long EdgeCount = 1;
            double[] point1 = new double[] {0, 0};
            double[] point2 = new double[] {0, 1};
            double[] point3 = new double[] {2, 1};
            double[] point4 = new double[] {2, 0};
            double[][] points = new double[][] {point1, point2};
            double[] edge1 = new double[] {1, 2, 1};
            double[] edge2 = new double[] {4, 1, 2};
            double[] edge3 = new double[] {2, 3, 2};
            double[] edge4 = new double[] {1, 3, 6};
            double[][] Edges = new double[][] {edge1};
            long queriesCount = 4;
            long[] query1 = new long[] {1, 1};
            long[] query2 = new long[] {2, 2};
            long[] query3 = new long[] {1, 2};
            long[] query4 = new long[] {2, 1};
            long[][] queries = new long[][] {query1, query2, query3, query4};
            //double[] Result = Solve(NodeCount, EdgeCount, points, Edges, queriesCount, queries);
            //foreach (double res in Result)
            //{
            //    Console.WriteLine(res);
            //}
            //long[] edge5 = new long[] { 9, 9 };
            //long[] edge6 = new long[] { 8, 9 };
            //long[] edge7 = new long[] { 3, 11 };
            //long[] edge8 = new long[] { 4, 12 };
            //long[] edge9 = new long[] { 2, 8 };
            //long[] edge10 = new long[] { 4, 4 };
            //long[] edge11 = new long[] { 6, 7 };
            //long[] edge12 = new long[] { 2, 6 };

            //double res = Solve(NodeCount, Edges, clusterCount);
            //Console.WriteLine(res);
        }

        
    }

    
}
