using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AD.String
{
    public class BWTMatching
    {
        //public static void Main(string[] args)
        //{
        //    string Text = Console.ReadLine();
        //    long n = long.Parse(Console.ReadLine());
        //    string[] patterns = Console.ReadLine().Split();
        //    long[] Result = Solve(Text, n, patterns);
        //    foreach (long res in Result) Console.Write(res + " ");
        //}

        public static long[] Solve(string text, long n, string[] patterns)
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
