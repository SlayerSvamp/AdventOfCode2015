using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    public static class GetAll
    {
        public static IEnumerable<IEnumerable<string>> GetAllTableSetups(IEnumerable<string> people, IEnumerable<string> table)
        {
            foreach (var person in people.Except(table))
            {
                var list = table.ToList();
                list.Add(person);

                var setup = GetAllTableSetups(people, list);

                if (!setup.Any())
                    yield return list;
                else
                    foreach (var current in setup)
                        yield return current;
            }
        }
    }
    public class Opinion
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public int Modifier { get; set; }
    }
    public class Person
    {
        public string Name;
        public Person(string line, IEnumerable<Line> lines)
        {
            Name = line;
        }
    }

    public class Line
    {
        public string First { get; set; }
        public string Second { get; set; }
        public int Value { get; set; }
        public Line(string raw)
        {
            var parts = raw
                .Replace("gain ", "+")
                .Replace("lose ", "-")
                .Split(' ');
            for (int i = 0; i < parts.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        First = parts[i];
                        break;
                    case 9:
                        Second = parts[i].Replace(".", "");
                        break;
                    case 2:
                        Value = int.Parse(parts[i]);
                        break;
                }
            }
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            bool includeYourself = false;
            var me = "Linus";

            var lines = File.ReadAllLines("input.txt").Select(x => new Line(x)).ToList();
            var people = lines.Select(x => x.First).Distinct().ToList();
            var opinions = lines.Select(x => new Opinion { Name = x.First, Target = x.Second, Modifier = x.Value }).ToList();

            do
            {
                if (includeYourself)
                {
                    people.Add(me);

                    foreach (var person in people.Where(x => x != me))
                    {
                        opinions.Add(new Opinion { Name = me, Target = person, Modifier = 0 });
                        opinions.Add(new Opinion { Name = person, Target = me, Modifier = 0 });
                    }
                }

                var setups = GetAll.GetAllTableSetups(people, people.Take(1)).Select(x => x.ToList()).ToList();

                List<string> best = null;
                var max = 0;
                foreach (var setup in setups)
                {
                    Func<int, int> getIndex = (x) => x >= 0 ? (x < setup.Count ? x : 0) : setup.Count - 1;
                    var val = 0;
                    for (int i = 0; i < setup.Count; i++)
                    {
                        var p = setup[i];
                        var left = setup[getIndex(i - 1)];
                        var right = setup[getIndex(i + 1)];
                        val += opinions.First(x => x.Name == p && x.Target == left).Modifier;
                        val += opinions.First(x => x.Name == p && x.Target == right).Modifier;
                    }
                    if (max < val)
                    {
                        max = val;
                        best = setup.ToList();
                    }
                }
                Console.WriteLine($"Max total: {max}");
                Console.WriteLine($"Setup: {string.Join(" -> ", best)}");
                Console.WriteLine();
            } while (includeYourself = !includeYourself);
            Console.ReadLine();
        }
    }
}
