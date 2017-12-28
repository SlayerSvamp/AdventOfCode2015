using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var input = File.ReadAllLines("input.txt").Select(x => long.Parse(x)).ToList();
            var correctSum = input.Sum() / 4; //3 for "part 1", 4 for "part 2"
            var bestList = Enumerable.Repeat(0L, input.Count).ToList();
            while (!Console.KeyAvailable)
            {
                var combination = new List<long>();
                foreach (var package in input)
                    if (random.Next(4) == 0)
                        combination.Add(package);

                if (combination.Sum() != correctSum)
                    continue;

                if (combination.Count > bestList.Count)
                    continue;

                if (combination.Count == bestList.Count && combination.Aggregate((a, b) => a * b) >= bestList.Aggregate((a, b) => a * b))
                    continue;

                bestList = combination;

                Console.Clear();
                Console.WriteLine($"best combination has {bestList.Count} packages and has a QE of {bestList.Aggregate((a, b) => a * b)}");
            }
        }
    }
}