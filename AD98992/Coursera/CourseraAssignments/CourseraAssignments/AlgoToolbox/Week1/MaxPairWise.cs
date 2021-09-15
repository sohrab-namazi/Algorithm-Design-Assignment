using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.AlgoToolbox.Week1
{
    public class MaxPairWise
    {
        //public static void Main(string[] args)
        //{
        //    string n = Console.ReadLine();
        //    long[] Numbers = Console.ReadLine().Split().Select(long.Parse).ToArray();
        //    Console.WriteLine(Solve(Numbers));

        //}

        public static long Solve(long[] numbers)
        {
            long first = 0;
            long second = 0;
            int size = numbers.Length;
            List<long> nums = numbers.ToList();
            for (int i = 0; i < size; i++)
            {
                if (numbers[i] > first)
                {
                    first = numbers[i];

                }
            }

            nums.Remove(first);

            for (int j = 0; j < size - 1; j++)
            {
                if (nums[j] > second)
                {
                    second = nums[j];
                }
            }


            return first * second;
        }
    }
}
