using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt")
                .Select(x =>
                {
                    switch (x)
                    {
                        case '^': return -65536;
                        case 'v': return 65536;
                        case '<': return -1;
                        case '>': return 1;
                    }
                    return 0;
                })
                .ToList();

            if (input.Any(x => x == 0))
                Console.WriteLine("Error in input");

            var houses = new Dictionary<int, int>();
            var houses2 = new Dictionary<int, int>();
            var current = 0;

            var santa = new int[] { 0, 0 };
            var who = 0;

            houses[0] = 1;
            houses2[0] = 2;

            foreach (var direction in input)
            {
                current += direction;
                houses[current] = 1;

                santa[who] += direction;
                houses2[santa[who]] = 1;
                who ^= 1;
            }
            
            Console.WriteLine($"\n Part 1:\r\n {houses.Keys.Count}");
            Console.WriteLine($"\n Part 2:\r\n {houses2.Keys.Count}");
            
            Console.ReadLine();
        }
    }
}
