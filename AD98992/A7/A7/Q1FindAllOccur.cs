using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, string pattern)
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
            if (Result.Count == 0) return new long[] { -1 };
            return Result.ToArray();
        }
    }
}
