using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q4Vaccine : Processor
    {

        public Q4Vaccine(string testDataName) : base(testDataName)
        {
            ExcludeTestCaseRangeInclusive(9, 9);
            ExcludeTestCaseRangeInclusive(17, 19);
            ExcludeTestCaseRangeInclusive(20, 106);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<string, string, string>)Solve);

        public string Solve(string dna, string pattern)
        {
            long[] res = ApproximateMatching(dna, pattern);
            string Result = "";
            if (res.Length == 0) return "No Match!";
            foreach (long s in res)
            {
                Result += s.ToString() + " ";
            }
            return Result;
        }

        protected static long[] ApproximateMatching(string text, string pattern)
        {
            List<long> Matches = new List<long>();
            bool a = false;

            for (long i = 0; i < text.Length; i++)
            {
                long m = 0;
                long j = 1;
                while (m <= 1 && j <= pattern.Length)
                {
                    try
                    {
                        long l = LCP(text.Substring((int)i + (int)j - 1), pattern.Substring((int)j - 1));
                        j += l;
                        if (l <= pattern.Length)
                        {
                            m++;
                            j++;
                        }
                    }
                    catch (Exception e)
                    {
                        a = true;
                        break;
                    }
                }

                if (a) break;

                j = 1;

                if (i == text.Length - 1) continue;

                if (m <= 1 && !Matches.Contains(i))
                {
                    Matches.Add(i);
                }

            }

            return Matches.ToArray();
        }

        private static long LCP(string v1, string v2)
        {
            var res = 0;
            long minsize = Math.Min(v1.Length, v2.Length);
            for (long i = 0; i < minsize; i++)
            {
                if (v1[(int)i].Equals(v2[(int)i])) res++;
            }
            return res;
        }

        public static Tuple<long, long> BWMatching(CharNode[] lastC, string pattern)
        {
            long TextSize = lastC.Length;
            bool[] MisMatches = new bool[TextSize];
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

                        //Array.Fill(MisMatches, false);
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
                        else if (!MisMatches[i])
                        {
                            MisMatches[lastC[i].Index] = true;
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
            // $ a b
            long[] Count = new long[3];

            for (int i = 0; i < Size; i++)
            {
                char c = S[i];
                if (c == '$') Count[0]++;
                else if (c.Equals('a')) Count[1]++;
                else if (c.Equals('b')) Count[2]++;
            }

            for (int j = 1; j < 3; j++)
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
                else if (c.Equals('a'))
                {
                    Count[1] -= 1;
                    Order[Count[1]] = k;
                }
                else if (c.Equals('b'))
                {
                    Count[2] -= 1;
                    Order[Count[2]] = k;
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
