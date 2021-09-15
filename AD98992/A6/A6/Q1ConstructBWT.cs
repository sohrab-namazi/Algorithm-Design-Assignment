using System;
using TestCommon;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Construct the Burrows–Wheeler transform of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> BWT(Text) </returns>
        public string Solve(string text)
        {
            BWT bWT = new BWT(text);
            bWT.MakeMatrix();
            string Result = bWT.GetBWForm();
            return Result;
        }

        public class BWT
        {
            string Text;
            int TextSize;
            string[] Matrix;

            public BWT(string Text)
            {
                this.Text = Text;
                this.TextSize = Text.Length;
                Matrix = new string[TextSize];
            }

            public void MakeMatrix()
            {
                Matrix[0] = "$";

                Matrix[0] += Text.Substring(0, TextSize - 1);

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
