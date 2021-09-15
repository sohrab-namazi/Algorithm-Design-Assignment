using System;
using System.Collections.Generic;
using TestCommon;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName)
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        /// <summary>
        /// Construct the suffix array of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> SuffixArray(Text), that is, the list of starting positions
        /// (0-based) of sorted suffixes separated by spaces </returns>
        public long[] Solve(string text)
        {
            long TextSize = text.Length;
            Suffix[] Suffixes = new Suffix[TextSize];
            for (int i = 0; i < TextSize; i++)
            {
                Suffixes[i] = new Suffix(text.Substring(i), i);
            }

            Array.Sort(Suffixes, delegate (Suffix x, Suffix y) { return x.Text.CompareTo(y.Text); });

            long[] Result = new long[TextSize];

            for (int i = 0; i < TextSize; i++)
            {
                Result[i] = Suffixes[i].Index;
            }

            return Result;
        }
        public class Suffix
        {
            public int Index { get; set; }
            public string Text { get; set; }

            public Suffix(string Text, int Index)
            {
                this.Text = Text;
                this.Index = Index;
            }
        }
    }
}
