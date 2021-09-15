using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week3
{
    public class MaxSalary
    {
        //public static void Main(string[] args)
        //{
        //    long n = long.Parse(Console.ReadLine());
        //    long[] numbers = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    Console.WriteLine(Solve(n, numbers));

        //}

        public static string Solve(long n, long[] numbers)
        {
            List<long> NumbersList = numbers.ToList();
            string[] answers = new string[n];
            int i = 0;
            while (NumbersList.Count != 0)
            {
                string Max = "0";
                for (int j = 0; j < NumbersList.Count; j++)
                {
                    if (Comparing(Convert.ToString(NumbersList[j]), Max) > 0)
                    {
                        Max = Convert.ToString(NumbersList[j]);
                    }
                }
                NumbersList.Remove(Convert.ToInt64(Max));
                answers[i] = Max;
                i++;
            }
            string result = "";
            foreach (string answer in answers)
            {
                result += answer;
            }
            return result;
        }

        public static int Comparing(string a, string b)
        {
            string FirstCase = a + b;
            string SecondCase = b + a;

            int NumberA = Convert.ToInt32(FirstCase);
            int NumberB = Convert.ToInt32(SecondCase);



            if (NumberA > NumberB) return 1;

            return -1;
        }
    }
}
