using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    class Program
    {
        public static IEnumerable<IEnumerable<string>> GetAllAlternativePaths(IEnumerable<string> locations, IEnumerable<string> path)
        {
            foreach (var location in locations.Except(path))
            {
                var list = path.ToList();
                list.Add(location);
                var paths = GetAllAlternativePaths(locations, list);
                if (!paths.Any())
                    yield return list;
                else
                    foreach (var current in paths)
                        yield return current;
            }
        }

        static void Main(string[] args)
        {
            Stopwatch clock = new Stopwatch();
            Action<string> log = x => Console.WriteLine($"{x} at {clock.ElapsedMilliseconds / 1000d}s");

            clock.Start();

            var input = File.ReadAllLines("input.txt")
                .Select(x => x.Split(' '))
                .ToList();
            log("Read in input");

            var locations = input.SelectMany(x => new string[] { x[0], x[2] })
                .Distinct()
                .ToList();

            var distances = input.Select(x =>
                    new
                    {
                        Locations = new string[] { x[0], x[2] },
                        Distance = Int32.Parse(x[4])
                    })
                .ToList();
            Func<string, string, int> getDistance = (x, y) =>
            {
                return distances.Where(z => z.Locations.Contains(x))
                                .First(z => z.Locations.Contains(y))
                                .Distance;
            };

            Func<IEnumerable<string>, int> getPathDistance = x =>
            {
                var enumerator = x.GetEnumerator();
                enumerator.MoveNext();
                var current = enumerator.Current;
                var distance = 0;

                while (enumerator.MoveNext())
                {
                    var next = enumerator.Current;
                    distance += getDistance(current, next);
                    current = next;
                }

                return distance;
            };
            log("Done making preparations");

            var paths = GetAllAlternativePaths(locations, new List<string>()).ToList();
            log("Worked through creating all paths");

            var shortest = paths
                .Select(path => new { Distance = getPathDistance(path), Path = path })
                .Aggregate((x, y) => x.Distance <= y.Distance ? x : y);
            log("Got shortest path");

            var longest = paths
                .Select(path => new { Distance = getPathDistance(path), Path = path })
                .Aggregate((x, y) => x.Distance >= y.Distance ? x : y);
            log("Got longest path");

            Console.CursorTop++;
            Console.WriteLine($"Shortest path is {shortest.Distance} long");
            Console.WriteLine("The path is as following:");
            Console.WriteLine(string.Join(" -> ", shortest.Path));
            Console.CursorTop++;
            Console.WriteLine($"Longest path is {longest.Distance} long");
            Console.WriteLine("The path is as following:");
            Console.WriteLine(string.Join(" -> ", longest.Path));
            Console.CursorTop++;
            log("Process finished");
            clock.Stop();

            Console.ReadLine();
        }
    }
}
