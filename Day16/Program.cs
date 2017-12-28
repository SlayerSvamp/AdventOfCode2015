using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class AuntSue
    {
        public int Number { get; set; } = 0;
        public int Children { get; set; } = 3;
        public int Cats { get; set; } = 7;
        public int Samoyeds { get; set; } = 2;
        public int Pomeranians { get; set; } = 3;
        public int Akitas { get; set; } = 0;
        public int Vizslas { get; set; } = 0;
        public int Goldfish { get; set; } = 5;
        public int Trees { get; set; } = 3;
        public int Cars { get; set; } = 2;
        public int Perfumes { get; set; } = 1;

        public List<string> UndefinedProperties { get; set; }

        public AuntSue()
        {
            UndefinedProperties = new List<string>();
            foreach(var name in GetType().GetProperties().Select(x => x.Name))
            {
                UndefinedProperties.Add(name);
            }
        }
        public AuntSue(string raw) : this()
        {
            Number = int.Parse(string.Concat(raw.Skip(4).TakeWhile(x => x != ':')));
            var parts = string.Concat(raw.SkipWhile(x => x != ':').Skip(1)).Split(',').Select(x => x.Trim());
            foreach (var part in parts)
            {
                var key = string.Concat(part.Split(':')[0].Select((x, i) => i == 0 ? char.ToUpper(x) : x));
                var value = int.Parse(part.Split(' ')[1]);

                UndefinedProperties.Remove(key);
                GetType().GetProperty(key).SetValue(this, value);
            }
        }

        public bool IsDefined(string name)
        {
            return !UndefinedProperties.Contains(name);
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            //Sue 476: vizslas: 3, perfumes: 5, goldfish: 1
            var aunts = File.ReadAllLines("input.txt").Select(x => new AuntSue(x)).ToList();
            var control = new AuntSue();
            var filtered = aunts
                .Where(x => x.Children == control.Children)
                .Where(x => x.Cats == control.Cats)
                .Where(x => x.Samoyeds == control.Samoyeds)
                .Where(x => x.Pomeranians == control.Pomeranians)
                .Where(x => x.Akitas == control.Akitas)
                .Where(x => x.Vizslas == control.Vizslas)
                .Where(x => x.Goldfish == control.Goldfish)
                .Where(x => x.Trees == control.Trees)
                .Where(x => x.Cars == control.Cars)
                .Where(x => x.Perfumes == control.Perfumes);
            Console.WriteLine("Part 1");
            foreach (var aunt in filtered)
            {
                Console.WriteLine($"Aunt Sue {aunt.Number}");
            }
            Console.WriteLine();

            filtered = aunts
                .Where(x => !x.IsDefined("Children") || x.Children == control.Children)
                .Where(x => !x.IsDefined("Cats") || x.Cats > control.Cats)
                .Where(x => !x.IsDefined("Samoyeds") || x.Samoyeds == control.Samoyeds)
                .Where(x => !x.IsDefined("Pomeranians") || x.Pomeranians < control.Pomeranians)
                .Where(x => !x.IsDefined("Akitas") || x.Akitas == control.Akitas)
                .Where(x => !x.IsDefined("Vizslas") || x.Vizslas == control.Vizslas)
                .Where(x => !x.IsDefined("Goldfish") || x.Goldfish < control.Goldfish)
                .Where(x => !x.IsDefined("Trees") || x.Trees > control.Trees)
                .Where(x => !x.IsDefined("Cars") || x.Cars == control.Cars)
                .Where(x => !x.IsDefined("Perfumes") || x.Perfumes == control.Perfumes);
            Console.WriteLine("Part 2");
            foreach (var aunt in filtered)
            {
                Console.WriteLine($"Aunt Sue {aunt.Number}");
            }
            Console.WriteLine();
            Console.ReadLine();

        }
    }
}
