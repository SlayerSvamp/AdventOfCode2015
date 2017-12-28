using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").ToList();

            var onlyCode = input.Select(x =>
                {
                    var remade = x.Trim('"');
                    int index;
                    while ((index = remade.IndexOf("\\\\")) != -1)
                        remade = $"{remade.Substring(0, index)}.{remade.Substring(index + 2)}";

                    while ((index = remade.IndexOf("\\\"")) != -1)
                        remade = $"{remade.Substring(0, index)}{remade.Substring(index + 1)}";

                    while ((index = remade.IndexOf("\\x")) != -1)
                        remade = $"{remade.Substring(0, index)}{remade.Substring(index + 3)}";

                    return x.Length - remade.Length;
                })
                .Sum();

            Console.WriteLine($"\n Part 1:\r\n {onlyCode}");

            var extraCode = input.Count * 2 + input
                .SelectMany(x => x.Where(y => y == '"' || y == '\\'))
                .Count();
            Console.WriteLine($"\n Part 2:\r\n {extraCode}");

            Console.ReadLine();
        }
    }
}
