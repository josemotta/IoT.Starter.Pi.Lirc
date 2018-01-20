using System;
using System.Diagnostics;
using System.IO;

namespace Contest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string example = "";

            while (!example.ToLower().Equals("x"))
            {
                example = Console.ReadLine();
                Console.WriteLine(example);
                Console.WriteLine(example.Bash());
            }

            //Console.Read();
        }
    }
}
