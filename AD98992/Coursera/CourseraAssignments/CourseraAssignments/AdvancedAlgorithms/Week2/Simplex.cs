using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Simplex
{
    class Simplex
    {
        private double[] c;
        private double[,] A;
        private double[] b;
        private HashSet<int> N = new HashSet<int>();
        private HashSet<int> B = new HashSet<int>();
        private long w;
        private double v = 0;

        public Simplex(double[] c, double[,] A, double[] b)
        {
            int vars = c.Length, constraints = b.Length;
            w = vars;

            if (vars != A.GetLength(1))
            {
                throw new Exception("Number of variables in c doesn't match number in A.");
            }

            if (constraints != A.GetLength(0))
            {
                throw new Exception("Number of constraints in A doesn't match number in b.");
            }

            // Extend max fn coefficients vector with 0 padding
            this.c = new double[vars + constraints];
            Array.Copy(c, this.c, vars);

            // Extend coefficient matrix with 0 padding
            this.A = new double[vars + constraints, vars + constraints];
            for (int i = 0; i < constraints; i++)
            {
                for (int j = 0; j < vars; j++)
                {
                    this.A[i + vars, j] = A[i, j];
                }
            }

            // Extend constraint right-hand side vector with 0 padding
            this.b = new double[vars + constraints];
            Array.Copy(b, 0, this.b, vars, constraints);

            // Populate non-basic and basic sets
            for (int i = 0; i < vars; i++)
            {
                N.Add(i);
            }

            for (int i = 0; i < constraints; i++)
            {
                B.Add(vars + i);
            }
        }

        public Tuple<double, double[]> maximize()
        {
            while (true)
            {
                // Find highest coefficient for entering var
                int e = -1;
                double ce = 0;
                foreach (var _e in N)
                {
                    if (c[_e] > ce)
                    {
                        ce = c[_e];
                        e = _e;
                    }
                }

                // If no coefficient > 0, there's no more maximizing to do, and we're almost done
                if (e == -1) break;

                // Find lowest check ratio
                double minRatio = double.PositiveInfinity;
                int l = -1;
                foreach (var i in B)
                {
                    if (A[i, e] > 0)
                    {
                        double r = b[i] / A[i, e];
                        if (r < minRatio)
                        {
                            minRatio = r;
                            l = i;
                        }
                    }
                }

                // Unbounded
                if (double.IsInfinity(minRatio))
                {
                    return Tuple.Create<double, double[]>(double.PositiveInfinity, null);
                }

                pivot(e, l);
            }

            // Extract amounts and slack for optimal solution
            List<double> x = new List<double>();
            int n = b.Length;
            for (var i = 0; i < w; i++)
            {
                x.Add(b[i]);
            }

            // Return max and variables
            return Tuple.Create<double, double[]>(v, x.ToArray());
        }

        

        private void pivot(int e, int l)
        {
            N.Remove(e);
            B.Remove(l);

            b[e] = b[l] / A[l, e];

            foreach (var j in N)
            {
                A[e, j] = A[l, j] / A[l, e];
            }

            A[e, l] = 1 / A[l, e];

            foreach (var i in B)
            {
                b[i] = b[i] - A[i, e] * b[e];

                foreach (var j in N)
                {
                    A[i, j] = A[i, j] - A[i, e] * A[e, j];
                }

                A[i, l] = -1 * A[i, e] * A[e, l];
            }

            v = v + c[e] * b[e];

            foreach (var j in N)
            {
                c[j] = c[j] - c[e] * A[e, j];
            }

            c[l] = -1 * c[e] * A[e, l];

            N.Add(l);
            B.Add(e);
        }
    }

    class MainClass
    {
        private static bool CheckSolution(long Ine, long Vars, double[] LastLastRow, double[] Objective, double[,] Matrix)
        {
            for (long i = 0; i < Ine; i++)
            {
                double Left = 0;
                for (long j = 0; j < Vars; j++)
                {
                    Left += Objective[j] * Matrix[i, j];
                }
                if (Left > LastLastRow[i]) return false;
            }
            return true;
        }

        static void Main(string[] args)
        {
            long[] input = Console.ReadLine().Split().Select(long.Parse).ToArray();
            long N = input[0];
            long M = input[1];
            double[,] matrix = new double[N, M];
            for (long i = 0; i < N; i++)
            {
                long[] Row = Console.ReadLine().Split().Select(long.Parse).ToArray();
                int j = 0;
                foreach (long l in Row)
                {
                    matrix[i, j] = l;
                    j++;
                }
            }

            double[] LastRow = Console.ReadLine().Split().Select(double
                .Parse).ToArray();

            //for (long j = 0; j < N; j++)
            //{
            //    matrix[j, M] = LastRow[j];
            //}


            double[] LastLastRow = Console.ReadLine().Split().Select(double.Parse).ToArray();

            //for (long j = 0; j < M; j++)
            //{
            //    matrix[N, j] = LastLastRow[j];
            //}

            var s = new Simplex(
              LastLastRow,
              matrix,
              LastRow
            );

            var answer = s.maximize();
            string result = "";
            //Console.WriteLine(answer.Item1);
            if (answer.Item2 == null)
            {
                Console.WriteLine("Infinity");
            }
            else
            {
                bool HasSolution = CheckSolution(N, M, LastRow, answer.Item2, matrix);
                if (HasSolution)
                {
                    Console.WriteLine("Bounded solution");
                    foreach (double d in answer.Item2)
                    {
                        result += d.ToString("F3") + " ";
                    }
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("No solution");
                }
            }
        }
    }
}