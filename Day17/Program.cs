using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void CountCombinations(List<int> containers, List<int> used, int from, List<List<int>> combinations)
        {
            var current = containers.Where((x, i) => used.Contains(i)).Sum();
            if (current == 150)
            {
                combinations.Add(used);
                return;
            }
            if (current > 150)
                return;

            if (from >= containers.Count)
                return;
            
            for (int i = from; i < containers.Count; i++)
            {
                var list = used.ToList();
                list.Add(i);
                CountCombinations(containers, list, i + 1, combinations);
            }
        }
        static void Main(string[] args)
        {
            var containers = File.ReadAllLines("input.txt").Select(x => int.Parse(x)).ToList();

            var combinations = new List<List<int>>();
            CountCombinations(containers, new List<int>(), 0, combinations);

            Console.WriteLine("Part 1:");
            Console.WriteLine($"Number of container combinations: {combinations.Count}");
            Console.CursorTop++;

            var min = combinations.Min(x => x.Count);
            var minCount = combinations.Count(x => x.Count == min);

            Console.WriteLine("Part 2:");
            Console.WriteLine($"Number of container combinations with least amount of containers: {minCount}");

            Console.ReadLine();
        }
    }
}
