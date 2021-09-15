using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.DS
{
    public class MergingTables
    {
        long[] parent;
        long[] tableSizes;
        long[] rank;

        //public static void Main(string[] args)
        //{
        //    long[] Input1 = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    long TableNumbers = Input1[0];
        //    long QueryNumbers = Input1[1];
        //    long[] TargetTables = new long[QueryNumbers];
        //    long[] SourceTables = new long[QueryNumbers];
        //    long[] TableSizes = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    for (long i = 0; i < QueryNumbers; i++)
        //    {
        //        long[] Input2 = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //        TargetTables[i] = Input2[0];
        //        SourceTables[i] = Input2[1];
        //    }

        //    long[] Result = Solve(TableSizes, TargetTables, SourceTables);

        //    foreach (long res in Result) Console.WriteLine(res);

        //}

        public static long[] Solve(long[] tableSizes, long[] targetTables, long[] sourceTables)
        {
            long RankMax = 0;
            //long Size = targetTables.Length;
            long[] Rank = new long[tableSizes.Length];
            long[] Parents = new long[tableSizes.Length];
            long[] Maxs = new long[targetTables.Length];

            for (long i = 0; i < tableSizes.Length; i++)
            {
                Rank[i] = tableSizes[i];
                Parents[i] = i;
            }

            RankMax = Rank.Max();

            for (long i = 0; i < targetTables.Length; i++)
            {
                Union(targetTables[i] - 1, sourceTables[i] - 1, ref Parents, ref Rank, ref RankMax);
                Maxs[i] = RankMax;
            }

            return Maxs;
        }

        public static long Find(long i, long[] Parents)
        {
            if (i != Parents[i])
            {
                Parents[i] = Find(Parents[i], Parents);
            }

            return Parents[i];
        }

        public static void Union(long i, long j, ref long[] Parents, ref long[] Rank, ref long MaxRank)
        {
            long First = Find(i, Parents);
            long Second = Find(j, Parents);
            if (First == Second) return;

            long ID = Math.Min(First, Second);
            Rank[ID] = Rank[First] + Rank[Second];

            if (Rank[ID] > MaxRank)
            {
                MaxRank = Rank[ID];
            }

            if (ID == First)
            {
                Parents[Second] = First;
                //Rank[Second]= Rank[First] + Rank[Second];

            }
            else
            {
                Parents[First] = Second;
                //Rank[First] = Rank[First] + Rank[Second];
            }
        }
    }
}
