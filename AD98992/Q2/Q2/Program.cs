using System;
using System.Collections.Generic;

namespace Q2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public long[] Solve(string text, string pattern)
        {
            List<long> Result = new List<long>();
            Dictionary<char, long> Dict = ComputeDict(pattern);

            return Result.ToArray();
        }

        public Dictionary<char, long> ComputeDict(string pattern)
        {
            Dictionary<char, long> Dict = new Dictionary<char, long>();

            long Size = pattern.Length;


            for (long i = 0; i < Size; i++)
            {
                char c = pattern[(int) i];
                if (Dict.ContainsKey(c))
                {

                }
                else
                {
                    Dict.Add(c, 0);
                }

                Dict[c] = Math.Max(Size - i - 1, 1);
            }

            Dict.Add('*', Size);

            return Dict;
        }

    }
}
