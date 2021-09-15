using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week5
{
    public class EditDistance
    {
        //public static void Main(string[] args)
        //{
            
        //    Console.WriteLine(Solve(Console.ReadLine(), Console.ReadLine()));
        //}

        public static long Solve(string str1, string str2)
        {
            string[] str1Array = str1.Split();
            string[] str2Array = str2.Split();
            long str1Size = str1.Length;
            long str2Size = str2.Length;
            long[,] Table = new long[str1Size + 1, str2Size + 1];

            for (int i = 0; i < str1Size + 1; i++)
            {
                Table[i, 0] = i;
            }
            for (int i = 0; i < str2Size + 1; i++)
            {
                Table[0, i] = i;
            }
            for (int i = 1; i < str1Size + 1; i++)
            {
                for (int j = 1; j < str2Size + 1; j++)
                {
                    Table[i, j] = long.MaxValue;
                    long up = Table[i - 1, j] + 1;
                    long left = Table[i, j - 1] + 1;
                    long UpLeft = Table[i - 1, j - 1];
                    if (!(str1[i - 1].Equals(str2[j - 1]))) UpLeft++;
                    Table[i, j] = Math.Min(up, Math.Min(left, UpLeft));
                }
            }

            return Table[str1Size, str2Size];
        }
    }
}
