using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TestCommon;
using System.Drawing;
using E1;

namespace Exam1
{
    public class Q3SeamCarving1 : Processor // Calculate Energy
    {
        public Q3SeamCarving1(string testDataName) : base(testDataName) { }

        public static long LineNumbers;
        public static long ColumnNumbers;

        public int RowNumber { get; private set; }

        public override string Process(string inStr)
        {
            int i = 0;
            int j = 0;
            string[] RawData = inStr.Split("\n");
            //Color[,] data = new Color[RawData.Length, RawData[0].Length];
            List<List<Color>> dataa = new List<List<Color>>();
            //Color[,] pixels = new Color[RawData.Length, ];
            foreach (string line in RawData)
            {
                string[] elements = line.Split("|");
                LineNumbers = elements.Length;
                dataa.Add(new List<Color>());
                foreach (string s in elements)
                {
                    string[] colors = s.Split(",");
                    int R = int.Parse(colors[0]);
                    int G = int.Parse(colors[1]);
                    int B = int.Parse(colors[2]);
                    Color color = Color.FromArgb(R, G, B);
                    
                    dataa[i].Add(color);
                    j++;
                }
                i++;
            }
            ColumnNumbers = dataa.Count;
             

            Color[,] FinalColors = Utilities.ConvertToArray(dataa);
            var solved = Solve(FinalColors);
            string Result = "";
            for (long z = 0; z < ColumnNumbers; z++)
            {
                string line = "";
                for (long k = 0; k < LineNumbers; k++)
                {
                    if (k == LineNumbers - 1)
                    {
                        line += solved[z, k];
                    }
                    else
                    {
                        line += solved[z, k] + ",";
                    }
                }
                line += "\n";
                Result += line;
            }
            return Result;
        }
            

        //Just part 1 has been done
        public double[,] Solve(Color[,] data)
        {
            double[,] Energies = new double[ColumnNumbers, LineNumbers];
            for (long i = 0; i < ColumnNumbers; i++)
            {
                Energies[i, 0] = 1000;
            }
            for (long i = 0; i < LineNumbers; i++)
            {
                Energies[0, i] = 1000;
            }
            for (long i = 0; i < LineNumbers; i++)
            {
                Energies[ColumnNumbers - 1, i] = 1000;
            }
            for (long i = 0; i < ColumnNumbers; i++)
            {
                Energies[i, LineNumbers - 1] = 1000;
            }
            for (long i = 1; i < ColumnNumbers - 1; i++)
            {
                for (long j = 1; j < LineNumbers - 1; j++)
                {
                    double Rx = data[i + 1, j].R - data[i - 1, j].R;
                    double Ry = data[i, j + 1].R - data[i, j - 1].R;
                    double Gx = data[i + 1, j].G - data[i - 1, j].G;
                    double Gy = data[i, j + 1].G - data[i, j - 1].G; 
                    double Bx = data[i + 1, j].B - data[i - 1, j].B;
                    double By = data[i, j + 1].B - data[i, j - 1].B;
                    double DeltaX2 = Math.Pow(Rx, 2) + Math.Pow(Gx, 2) + Math.Pow(Bx, 2);
                    double DeltaY2 = Math.Pow(Ry, 2) + Math.Pow(Gy, 2) + Math.Pow(By, 2);
                    Energies[i, j] = Math.Round(Math.Sqrt(DeltaX2 + DeltaY2), 3);
                }
            }
            return Energies;
        }
    }

    public class Q3SeamCarving2 : Processor // Find Seam
    {
        public Q3SeamCarving2(string testDataName) : base(testDataName) { }

        public static int LineCounts;
        public static int ColumnCounts;


        public override string Process(string inStr)
        {
            string[] lines = inStr.Split("\n");
            LineCounts = lines.Length;
            ColumnCounts = lines[0].Split(",").Length;
            double[,] Data = new double[lines.Length, lines[0].Split(",").Length];
            int i = 0;
            foreach (string line in lines)
            {
                string[] Energies = line.Split(",");
                for (long j = 0; j < Energies.Length; j++)
                {
                    Data[i, j] = double.Parse(Energies[j]);
                }
                i++;
            }

            var solved = Solve(Data);
            // convert solved into output string
            return string.Empty;
        }


        public int[] Solve(double[,] data)
        {
            int[] Result = new int[data.Length];

            for (long i = 1; i < LineCounts - 1; i++)
            {
                double[] nums = new double[ColumnCounts];
                for (long j = 0; j < nums.Length; j++)
                {
                    nums[(int)j] = data[i, j];
                }
                Result[i] = MinIndex(nums);
            }
            Result[0] = Result[1];
            Result[LineCounts - 1] = Result[LineCounts - 2];

            for (long i = 1; i < ColumnCounts - 1; i++)
            {
                double[] nums = new double[LineCounts];
                for (long j = 0; j < nums.Length; j++)
                {
                    nums[(int)j] = data[i, j];
                }
                Result[i + LineCounts] = MinIndex(nums);
            }
            Result[LineCounts] = Result[LineCounts + 1];
            Result[LineCounts + ColumnCounts - 1] = Result[ColumnCounts + LineCounts - 2];

            return Result;
        }

        public int MinIndex(double[] nums)
        {
            double min = long.MaxValue;
            int MinIndex = 0;

            for (long i = 0; i < nums.Length; i++)
            {
                if (nums[i] < min)
                {
                    min = nums[i];
                    MinIndex = (int) i;
                }
            }
            return MinIndex;
        }
    }

    public class Q3SeamCarving3 : Processor // Remove Seam
    {
        public Q3SeamCarving3(string testDataName) : base(testDataName) { }

        public override string Process(string inStr)
        {
            // Parse input file
            double[,] data = new double[0, 0];
            var solved = Solve(data);
            // convert solved into output string
            return string.Empty;
        }


        public double[,] Solve(double[,] data)
        {
            throw new NotImplementedException();
        }
    }
}
