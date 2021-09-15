using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourseraAssignments.DS
{
    public class SimplePhoneBook
    {
        public class Contact
        {
            public string Name;
            public int Number;

            public Contact(string name, int number)
            {
                Name = name;
                Number = number;
            }
        }

        protected Dictionary<long, string> PhoneBook;

        //public static void Main(string[] args)
        //{
        //    long CommandCounts = long.Parse(Console.ReadLine());
        //    string[] commands = new string[CommandCounts];
        //    for (long i = 0; i < CommandCounts; i++)
        //    {
        //        commands[i] = Console.ReadLine();
        //    }
        //    string[] Result = new SimplePhoneBook().Solve(commands);

        //    foreach (string s in Result) Console.WriteLine(s);
        //}

        public string[] Solve(string[] commands)
        {
            PhoneBook = new Dictionary<long, string>();

            List<string> result = new List<string>();
            foreach (var cmd in commands)
            {
                var toks = cmd.Split();
                var cmdType = toks[0];
                var args = toks.Skip(1).ToArray();
                int number = int.Parse(args[0]);
                switch (cmdType)
                {
                    case "add":
                        Add(args[1], number);
                        break;
                    case "del":
                        Delete(number);
                        break;
                    case "find":
                        result.Add(Find(number));
                        break;
                }
            }
            return result.ToArray();
        }


        public void Add(string name, int number)
        {
            PhoneBook[number] = name;
        }

        public string Find(int number)
        {
            if (PhoneBook.ContainsKey(number))
            {
                return PhoneBook[number];
            }
            return "not found";
        }

        public void Delete(int number)
        {
            if (PhoneBook.ContainsKey(number))
            {
                PhoneBook.Remove(number);
            }
        }
    }
}
