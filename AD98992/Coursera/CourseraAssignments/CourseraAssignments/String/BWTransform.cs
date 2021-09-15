using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AD.String
{
    public class BWTransform
    {
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(Solve(Console.ReadLine()));
        //}

        public static string Solve(string text)
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
