using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day19
{
    internal class Replacement
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    class Program
    {
        static Stopwatch stopwatch = new Stopwatch();
        static Action<string> log = x => Console.WriteLine($"{x} at {stopwatch.ElapsedMilliseconds / 1000d}s");
        static void Init(out List<Replacement> replacements, out string medicineMolecule)
        {
            stopwatch.Start();
            log("Init started");
            var input = File.ReadAllLines("input.txt").ToList();
            replacements = input.TakeWhile(x => x.Trim().Length > 0).Select(x => x.Split(' ')).Select(x => new Replacement { Key = x[0], Value = x[2] }).ToList();
            medicineMolecule = input.Last();
            log("Init ended");
            Console.WriteLine();
        }
        static void Part1(List<Replacement> replacements, string medicineMolecule)
        {
            Console.WriteLine("Part 1:");
            stopwatch.Restart();
            List<string> calibrationMolecules = new List<string>();

            foreach (var replacement in replacements)
            {
                for (int i = 0; i < medicineMolecule.Length; i++)
                {
                    if (medicineMolecule.Substring(i).StartsWith(replacement.Key))
                    {
                        var sb = new StringBuilder(medicineMolecule.Substring(0, i));
                        sb.Append(replacement.Value);
                        sb.Append(medicineMolecule.Substring(i + replacement.Key.Length));
                        calibrationMolecules.Add(sb.ToString());
                    }
                }
            }

            Console.WriteLine(calibrationMolecules.Distinct().Count());
            log("Part 1 ended");
            Console.WriteLine();
        }

        static void Part2(List<Replacement> replacements, string medicineMolecule)
        {
            Console.WriteLine("Part 2:");
            stopwatch.Restart();

            var count = GetLenghtOfRoute(replacements, medicineMolecule);

            Console.WriteLine(count);
            log("Part 2 ended");
        }

        static int GetLenghtOfRoute(List<Replacement> replacements, string medicineMolecule)
        {
            var molecule = medicineMolecule;
            var mutations = 0;

            while (molecule != "e")
            {
                var unmodified = molecule;
                foreach (var replacement in replacements)
                {
                    if (molecule.Contains(replacement.Value))
                    {
                        molecule = new Regex(replacement.Value).Replace(molecule, replacement.Key, 1);
                        mutations++;
                    }
                }
                if (molecule == unmodified)
                {
                    mutations = 0;
                    molecule = medicineMolecule;
                    //this is just weird, why does it even work? :S
                    replacements = replacements.OrderBy(x => Guid.NewGuid()).ToList();
                }
            }
            return mutations;

        }


        static void Main(string[] args)
        {
            Init(out var replacements, out var medicineMolecule);

            Part1(replacements, medicineMolecule);

            //stole this solution
            Part2(replacements, medicineMolecule);

            stopwatch.Stop();
            Console.ReadLine();
        }
    }
}
