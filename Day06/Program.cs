using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day06
{
    enum Command { on, off, toggle }
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")
                .Select(x => x.Replace("turn ", ""))
                .Select(x => x.Split(' '))
                .Select(x => new
                {
                    command = (Command)Enum.Parse(typeof(Command), x[0]),
                    x1 = int.Parse(x[1].Split(',')[0]),
                    y1 = int.Parse(x[1].Split(',')[1]),
                    x2 = int.Parse(x[3].Split(',')[0]),
                    y2 = int.Parse(x[3].Split(',')[1])
                })
                .ToList();

            var grid1 = new bool[1000, 1000];
            var grid2 = new int[1000, 1000];

            foreach (var instruction in input)
                for (int y = instruction.y1; y <= instruction.y2; y++)
                    for (int x = instruction.x1; x <= instruction.x2; x++)
                        switch (instruction.command)
                        {
                            case Command.on:
                                grid1[y, x] = true;
                                grid2[y, x]++;
                                break;
                            case Command.off:
                                grid1[y, x] = false;
                                if (grid2[y, x] > 0)
                                    grid2[y, x]--;
                                break;
                            case Command.toggle:
                                grid1[y, x] ^= true;
                                grid2[y, x] += 2;
                                break;
                        }

            Console.WriteLine($"\n Part 1:\r\n {grid1.Cast<bool>().Count(x => x)}");
            Console.WriteLine($"\n Part 2:\r\n {grid2.Cast<int>().Sum()}");

            Console.ReadLine();
        }
    }
}
