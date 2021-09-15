using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AD.Graph.Week6
{
    public class KMPAlgorithm
    {
        //public static void Main(string[] args)
        //{
        //    string pattern = Console.ReadLine();
        //    string text = Console.ReadLine();
        //    long[] Result = Solve(text, pattern);
        //    foreach (long res in Result) Console.Write(res + " ");
        //}

        public static long[] Solve(string text, string pattern)
        {
            long[] Result = KMP(text, pattern);
            return Result;
        }

        public static int[] ComputePrefixFunction(string Pattern)
        {
            long Size = Pattern.Length;
            int[] Result = new int[Size];

            Result[0] = 0;
            int Border = 0;

            for (int i = 1; i < Size; i++)
            {
                while (Border > 0 && !(Pattern[i].Equals(Pattern[Border])))
                {
                    Border = Result[Border - 1];
                }
                if (Pattern[i].Equals(Pattern[Border]))
                {
                    Border++;
                }
                else
                {
                    Border = 0;
                }
                Result[i] = Border;
            }
            return Result;
        }

        public static long[] KMP(string Text, string Pattern)
        {
            List<long> Result = new List<long>();
            int PatternSize = Pattern.Length;
            string S = Pattern + "$" + Text;
            int SSize = S.Length;
            int[] PreFixes = ComputePrefixFunction(S);

            for (int i = PatternSize + 1; i < SSize; i++)
            {
                if (PreFixes[i] == PatternSize)
                {
                    Result.Add(i - 2 * PatternSize);
                }
            }
            return Result.ToArray();
        }
    }
}
