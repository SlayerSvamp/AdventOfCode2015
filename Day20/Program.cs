using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class Program
    {
        static void Part1(int input)
        {
            int presents;
            for (var house = 790000; house > 0; house--)
            {
                presents = 0;
                for (int elf = 1; elf <= house; elf++)
                    if (house % elf == 0)
                        presents += elf * 10;

                if (presents >= input)
                    Console.WriteLine($"{house}     ");
                else if (house % 100 == 0)
                    Console.Write($"{house}       \r");
            }


        }

        private static void Part2(int input)
        {
            var highest = 0;
            int presents = 0;
            var Elves = new Dictionary<int, int>();
            for (var house = 1; presents < input; house++)
            {
                presents = 0;
                Elves.Add(house, 0);

                foreach (var elf in Elves.Keys.ToArray())
                {
                    if (house % elf == 0)
                    {
                        presents += elf * 11;
                        Elves[elf] = Elves[elf] + 1;

                        if (Elves[elf] >= 50)
                            Elves.Remove(elf);
                    }
                }
                if (highest < presents)
                    highest = presents;

                if (presents >= input)
                    Console.WriteLine($"{house} - {highest}");
                else if (house % 100 == 0)
                    Console.Write($"{house} - {highest}\r");
            }
        }

        static void Main(string[] args)
        {
            var input = 34000000;
            //Part1(input);

            Part2(input);

            Console.WriteLine("Done!");
            while (Console.KeyAvailable)
                Console.ReadKey();
            Console.ReadLine();
        }

    }
}
