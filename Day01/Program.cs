using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");

            Console.WriteLine($"\n Part 1:\r\n {input.Select(x => x == '(' ? 1 : -1).Sum()}");

            int i;
            var floor = 0;
            for (i = 0; floor >= 0; i++)
                floor += input[i] == '(' ? 1 : -1;

            Console.WriteLine($"\n Part 2:\r\n {i}");

            Console.ReadLine();
        }
    }
}
