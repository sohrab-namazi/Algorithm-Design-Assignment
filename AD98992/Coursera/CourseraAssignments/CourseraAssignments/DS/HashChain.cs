using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.DS
{
    public class HashChain
    {
        public List<string>[] Chains;

        //public static void Main(string[] args)
        //{
        //    long BucketCount = long.Parse(Console.ReadLine());
        //    long QueryCount = long.Parse(Console.ReadLine());
        //    string[] commands = new string[QueryCount];
        //    for (long i = 0; i < QueryCount; i++)
        //    {
        //        commands[i] = Console.ReadLine();
        //    }
        //    string[] Result = new HashChain().Solve(BucketCount ,commands);

        //    foreach (string s in Result) Console.WriteLine(s);
        //}

        public string[] Solve(long bucketCount, string[] commands)
        {
            Chains = new List<string>[bucketCount]; 
            for(long i = 0; i < bucketCount; i++)
            {
                Chains[i] = new List<string>();
            }

            List<string> result = new List<string>();
            foreach (var cmd in commands)
            {
                var toks = cmd.Split();
                var cmdType = toks[0];
                var arg = toks[1];

                switch (cmdType)
                {
                    case "add":
                        Add(arg);
                        break;
                    case "del":
                        Delete(arg);
                        break;
                    case "find":
                        result.Add(Find(arg));
                        break;
                    case "check":
                        result.Add(Check(int.Parse(arg)));
                        break;
                }
            }
            return result.ToArray();
        }

        public const long BigPrimeNumber = 1000000007;
        public const long ChosenX = 263;

        public static long PolyHash(
            string str, int start, int count,
            long p = BigPrimeNumber, long x = ChosenX)
        {
            long hash = 0;
            //for (long i = 0; i < str.Length; i++)
            //{
            //    hash = (str[(int)i] * ((long) Math.Pow(x, i))) % BigPrimeNumber;
            //}
            for (int i = count - 1; i >= 0; i--)
            {
                hash = (hash * x + str[i + start]) % p;
            }
            return hash;
        }

        public void Add(string str)
        {
            long Hash = PolyHash(str, 0, str.Length) % Chains.Length;
            if (!Chains[Hash].Contains(str))
            {
                Chains[Hash].Add(str);
            }
        }

        public string Find(string str)
        {
            long Hash = PolyHash(str, 0, str.Length) % Chains.Length;

            if (Chains[Hash].Contains(str))
            {
                return "yes";
            }
            return "no";

        }

        public void Delete(string str)
        {
            long Hash = PolyHash(str, 0, str.Length) % Chains.Length;

            if (Chains[Hash].Contains(str))
            {
                Chains[Hash].Remove(str);
            }
        }

        public string Check(int i)
        {
            string Result = "";
            for (long j = 0; j < Chains[i].Count; j++)
            {
                if (j != 0)
                {
                    Result = Chains[i][(int)j] + " " + Result;

                }
                else
                {
                    Result = Chains[i][(int)j] + Result;

                }
            }
            if (Result == "")
            {
                return "";
            }
            return Result;
        }

    }
}
