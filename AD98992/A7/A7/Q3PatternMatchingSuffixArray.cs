using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
//using Q1FindAllOccur;

namespace A7
{
    public class Q3PatternMatchingSuffixArray : Processor
    {
        public Q3PatternMatchingSuffixArray(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, string[], long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, long n, string[] patterns)
        {
            text += "$";
            long[] Order = BuildSuffixArray(text);
            int TextSize = text.Length;
            CharNode[] FirstC = new CharNode[TextSize];
            CharNode[] LastC = new CharNode[TextSize];
            for (int k = 0; k < TextSize; k++)
            {
                LastC[k] = new CharNode(text[((int)(Order[k] + TextSize - 1)) % TextSize], k);
            }

            FirstC = LastC.OrderBy(x => x.Letter).ToArray();

            for (long h = 0; h < TextSize; h++)
            {
                LastC[FirstC[h].Index].LastToFirst = (int)h;
            }

            List<long> Result = new List<long>();

            bool[] SuffixArrayVisited = new bool[TextSize];

            for (long k = 0; k < n; k++)
            {
                var tuple = BWMatching(LastC, patterns[k]);
                if (tuple.Item1 == -1 && tuple.Item2 == -1) continue;
                for (long j = tuple.Item2; j <= tuple.Item1; j++)
                {
                    if (!SuffixArrayVisited[j])
                    {
                        SuffixArrayVisited[j] = true;
                        Result.Add(Order[j]);
                    }
                }
            }

            if (Result.Count == 0) return new long[] { -1 };
            return Result.ToArray();
        }

        public static Tuple<long, long> BWMatching(CharNode[] lastC, string pattern)
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
                        Top = lastC[FirstMatchIndex].LastToFirst;
                        Bottom = lastC[LastMathIndex].LastToFirst;
                    }
                    else
                    {
                        return new Tuple<long, long>(-1, -1);
                    }
                }
                else
                {
                    return new Tuple<long, long>(Bottom, Top);
                }
            }
            return new Tuple<long, long>(-1, -1);
        }

        public static void InitializeIndices(CharNode[] firstC)
        {
            int TextSize = firstC.Length;

            for (int i = 0; i < TextSize; i++)
            {
                firstC[i].Index = i;
            }
        }

        private static long[] BuildSuffixArray(string S)
        {
            long[] Order = SortCharacters(S);
            long[] Class = ComputeCharClasses(S, Order);
            long L = 1;
            long Size = S.Length;
            while (L < Size)
            {
                Order = SortDoubled(S, L, Order, Class);
                Class = UpdateClasses(Order, Class, L);
                L *= 2;
            }
            return Order;
        }

        private static long[] SortCharacters(string S)
        {
            int Size = S.Length;
            long[] Order = new long[Size];
            // $ A C G T
            long[] Count = new long[5];

            for (int i = 0; i < Size; i++)
            {
                char c = S[i];
                if (c == '$') Count[0]++;
                else if (c.Equals('A')) Count[1]++;
                else if (c.Equals('C')) Count[2]++;
                else if (c.Equals('G')) Count[3]++;
                else if (c.Equals('T')) Count[4]++;
            }

            for (int j = 1; j < 5; j++)
            {
                Count[j] = Count[j] + Count[j - 1];
            }

            for (int k = Size - 1; k >= 0; k--)
            {
                char c = S[k];

                if (c.Equals('$'))
                {
                    Count[0] -= 1;
                    Order[Count[0]] = k;
                }
                else if (c.Equals('A'))
                {
                    Count[1] -= 1;
                    Order[Count[1]] = k;
                }
                else if (c.Equals('C'))
                {
                    Count[2] -= 1;
                    Order[Count[2]] = k;
                }
                else if (c.Equals('G'))
                {
                    Count[3] -= 1;
                    Order[Count[3]] = k;
                }
                else if (c.Equals('T'))
                {
                    Count[4] -= 1;
                    Order[Count[4]] = k;
                }
            }
            return Order;
        }

        private static long[] ComputeCharClasses(string S, long[] Order)
        {
            long Size = S.Length;
            long[] Class = new long[Size];
            Class[Order[0]] = 0;

            for (int i = 1; i < Size; i++)
            {
                if (!S[(int)Order[i]].Equals(S[(int)Order[i - 1]]))
                {
                    Class[(int)Order[i]] = Class[(int)Order[i - 1]] + 1;
                }
                else
                {
                    Class[Order[i]] = Class[Order[i - 1]];
                }
            }
            return Class;
        }

        private static long[] SortDoubled(string S, long L, long[] Order, long[] Class)
        {
            int Size = S.Length;
            long[] Count = new long[Size];
            long[] NewOrder = new long[Size];

            for (int i = 0; i < Size; i++)
            {
                Count[Class[i]] = Count[Class[i]] + 1;
            }

            for (int j = 1; j < Size; j++)
            {
                Count[j] = Count[j] + Count[j - 1];
            }

            for (int k = Size - 1; k >= 0; k--)
            {
                long Start = (Order[k] - L + Size) % Size;
                long CL = Class[(int)Start];
                Count[(int)CL] = Count[(int)CL] - 1;
                NewOrder[(int)Count[(int)CL]] = Start;
            }
            return NewOrder;
        }

        private static long[] UpdateClasses(long[] NewOrder, long[] Class, long L)
        {
            long Size = NewOrder.Length;
            long[] NewClass = new long[Size];
            NewClass[NewOrder[0]] = 0;

            for (int i = 1; i < Size; i++)
            {
                long Cur = NewOrder[i];
                long Prev = NewOrder[i - 1];
                long Mid = (Cur + L) % Size;
                long MidPrev = (Prev + L) % Size;

                if (Class[Cur] != Class[Prev] || Class[Mid] != Class[MidPrev])
                {
                    NewClass[Cur] = NewClass[Prev] + 1;
                }
                else
                {
                    NewClass[Cur] = NewClass[Prev];
                }
            }
            return NewClass;
        }

        public class CharNode
        {
            public int Index { get; set; }
            public char Letter { get; set; }
            public int LastToFirst;

            public CharNode(char l)
            {
                this.Letter = l;
            }

            public CharNode(char l, int index)
            {
                this.Letter = l;
                this.Index = index;
            }
        }
    }
}
