using System;
using System.Collections.Generic;
using System.Text;

namespace CourseraAssignments.DS
{
    public class CheckBrackets
    {
        //public static void Main(string[] args)
        //{
        //    string str = Console.ReadLine();
        //    long Result = Solve(str);
        //    if (Result == -1) Console.WriteLine("Success");
        //    else Console.WriteLine(Result);
        //}

        public static long Solve(string str)
        {
            Stack stack = new Stack();
            int size = str.Length;
            for (int i = 0; i < size; i++)
            {
                if (str[i].Equals('{') || str[i].Equals('(') || str[i].Equals('['))
                {
                    stack.Push(str[i]);
                }
                else if (str[i].Equals('}') || str[i].Equals(')') || str[i].Equals(']'))
                {
                    if (stack.IsEmpty()) return i + 1;
                    char c = stack.Pop();
                    if ((c.Equals('{') && !(str[i].Equals('}'))) || (c.Equals('(') && !(str[i].Equals(')'))) || (c.Equals('[') && !(str[i].Equals(']'))))
                    {
                        return i + 1;
                    }
                }
            }
            if (stack.IsEmpty()) return -1;

            char Dual;
            char Self = stack.Elements[0];

            if (Self.Equals('('))
            {
                Dual = ')';
            }
            else if (Self.Equals('{'))
            {
                Dual = '}';
            }
            else
            {
                Dual = ']';
            }

            int counter = 0;

            for (int i = size - 1; i >= 0; i--)
            {
                if (str[i].Equals(Dual))
                {
                    counter++;
                }
                if (str[i].Equals(Self))
                {
                    if (counter == 0)
                    {
                        return i + 1;
                    }
                    else
                    {
                        counter--;
                    }
                }
            }
            return 0;
        }


        public class Stack
        {
            public List<char> Elements = new List<char>();
            public bool IsEmpty()
            {
                if (Elements.Count == 0) return true;
                return false;
            }
            public void Push(char c)
            {
                Elements.Add(c);
            }
            public char Pop()
            {
                int lastIndex = Elements.Count - 1;
                char Head = Elements[lastIndex];
                Elements.RemoveAt(lastIndex);
                return Head;
            }
        }
    }
}
