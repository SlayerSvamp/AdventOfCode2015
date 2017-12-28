using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    class Program
    {
        static T self<T>(T x) => x;
        static void Main(string[] args)
        {
            var sizes = File.ReadAllLines("input.txt")
                .Select(x => x.Split('x'))
                .Select(x => x.Select(y => int.Parse(y)).OrderBy(self).ToArray())
                .ToList();

            var paper = sizes.Select(x => x[0] * x[1] * 3 + x[1] * x[2] * 2 + x[2] * x[0] * 2).Sum();

            Console.WriteLine($"\n Part 1:\r\n {paper}");

            var ribbon = sizes.Select(x => x[0] * x[1] * x[2] + (x[0] + x[1]) * 2).Sum();

            Console.WriteLine($"\n Part 2:\r\n {ribbon}");

            Console.ReadLine();
        }
    }
}
