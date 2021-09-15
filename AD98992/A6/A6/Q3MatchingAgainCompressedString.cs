using System;
using System.Linq;
using TestCommon;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        /// <summary>
        /// Implement BetterBWMatching algorithm
        /// </summary>
        /// <param name="text"> A string BWT(Text) </param>
        /// <param name="n"> Number of patterns </param>
        /// <param name="patterns"> Collection of n strings Patterns </param>
        /// <returns> A list of integers, where the i-th integer corresponds
        /// to the number of substring matches of the i-th member of Patterns
        /// in Text. </returns>
        public long[] Solve(string text, long n, String[] patterns)
        {
            int TextSize = text.Length;
            CharNode[] FirstC = new CharNode[TextSize];
            int i = 0;
            foreach (char c in text)
            {
                FirstC[i] = new CharNode(c);
                i++;
            }
            CharNode[] LastC = (CharNode[])FirstC.Clone();
            FirstC = FirstC.OrderBy(x => x.Letter).ToArray();
            InitializeIndices(FirstC);

            long[] Result = new long[n];

            for (long k = 0; k < n; k++)
            {
                Result[k] = BWMatching(FirstC, LastC, patterns[k]);
            }

            return Result;
        }

        public static long BWMatching(CharNode[] firstC, CharNode[] lastC, string pattern)
        {
            long TextSize = lastC.Length;
            long Top = 0;
            long Bottom = TextSize - 1;
            while (Top <= Bottom)
            {
                if (pattern.Length > 0)
                {
                    char Symbol = pattern[pattern.Length - 1];
                    pattern = pattern.Remove(pattern.Length - 1);
                    bool HasMatch = false;
                    int Count = 0;
                    long FirstMatchIndex = -1;
                    long LastMathIndex = -1;
                    for (long i = Top; i <= Bottom; i++)
                    {
                        if (lastC[i].Letter.Equals(Symbol))
                        {
                            Count++;
                            if (Count == 1)
                            {
                                FirstMatchIndex = i;
                                HasMatch = true;
                            }
                            LastMathIndex = i;
                        }
                    }
                    if (HasMatch)
                    {
                        Top = lastC[FirstMatchIndex].Index;
                        Bottom = lastC[LastMathIndex].Index;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return Bottom - Top + 1;
                }
            }
            return -1;
        }

        public static void InitializeIndices(CharNode[] firstC)
        {
            int TextSize = firstC.Length;

            for (int i = 0; i < TextSize; i++)
            {
                firstC[i].Index = i;
            }
        }

        public class CharNode
        {
            public int Index { get; set; }
            public char Letter { get; set; }

            public CharNode(char l)
            {
                this.Letter = l;
            }
        }
    }
}
