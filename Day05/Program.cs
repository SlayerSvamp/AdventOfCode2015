using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var strings = File.ReadAllLines("input.txt").ToList();

            var vowels = "aeiou";
            var forbidden = new string[] { "ab", "cd", "pq", "xy" };

            var nice = new List<Func<string, bool>>
            {
                x => x.Count(y => vowels.Contains(y)) > 2,
                x => Enumerable.Range(0, x.Length -1).Any(i => x[i] == x[i+1]),
                x => !forbidden.Any(y => x.Contains(y))
            };

            Console.WriteLine($"\n Part 1:\r\n {strings.Count(x => nice.All(y => y(x)))}");

            nice = new List<Func<string, bool>>
            {
                x => Enumerable.Range(0, x.Length - 3).Any(i => x.Substring(i + 2).Contains(x.Substring(i, 2))),
                x => Enumerable.Range(0, x.Length - 2).Any(i => x[i] == x[i + 2])
            };

            Console.WriteLine($"\n Part 2:\r\n {strings.Count(x => nice.All(y => y(x)))}");

            Console.ReadLine();
        }
    }
}
