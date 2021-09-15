using System;
using System.Collections.Generic;
using System.Text;
using TestCommon;

namespace Exam1
{
    public class Q1GeneticMutation : Processor
    {
        public Q1GeneticMutation(string testDataName) : base(testDataName) { }
        public override string Process(string inStr)
            => TestTools.Process(inStr, (Func<string, string, string>)Solve);


        static int no_of_chars = 256;

        public string Solve(string firstDNA, string secondDNA)
        {
            BWT bwt = new BWT(firstDNA);
            bwt.MakeMatrix();
            foreach (string s in bwt.Matrix)
            {
                if (s.Equals(secondDNA)) return "1";
            }
            return "-1";
        }

        public class BWT
        {
            string Text;
            int TextSize;
            public string[] Matrix;

            public BWT(string Text)
            {
                this.Text = Text;
                this.TextSize = Text.Length;
                Matrix = new string[TextSize];
            }

            public void MakeMatrix()
            {
                //Matrix[0] = "$";

                Matrix[0] += Text;

                for (int i = 1; i < TextSize; i++)
                {
                    Matrix[i] += Matrix[i - 1][TextSize - 1];
                    Matrix[i] += Matrix[i - 1].Substring(0, TextSize - 1);
                }
            }

            public string GetBWForm()
            {
                Array.Sort(Matrix);

                string BWT = "";

                for (long i = 0; i < TextSize; i++)
                {
                    BWT += Matrix[i][TextSize - 1];
                }

                return BWT;
            }
        }

    }
}
