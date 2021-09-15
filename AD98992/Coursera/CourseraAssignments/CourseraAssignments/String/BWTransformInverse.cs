using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AD.String
{
    public class BWTransformInverse
    {
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(Solve(Console.ReadLine()));
        //}

        public static string Solve(string bwt)
        {
            int TextSize = bwt.Length;
            CharNode[] FirstC = new CharNode[TextSize];
            int i = 0;
            foreach (char c in bwt)
            {
                FirstC[i] = new CharNode(c);
                i++;
            }
            CharNode[] LastC = (CharNode[])FirstC.Clone();
            FirstC = FirstC.OrderBy(x => x.Letter).ToArray();
            InitializeIndices(FirstC);
            string Result = BWTInverse(LastC);
            return new string(Result);
        }

        public static void InitializeIndices(CharNode[] firstC)
        {
            int TextSize = firstC.Length;

            for (int i = 0; i < TextSize; i++)
            {
                firstC[i].Index = i;
            }
        }

        public static string BWTInverse(CharNode[] lastC)
        {
            CharNode CurrentNode = lastC[0];
            char[] ResultArray = new char[lastC.Length - 1];
            int i = lastC.Length - 1;

            while (!CurrentNode.Letter.Equals('$'))
            {
                i--;
                ResultArray[i] = CurrentNode.Letter;
                CurrentNode = lastC[CurrentNode.Index];
            }

            return new string(ResultArray) + "$";
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
