using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Grid
    {
        public List<bool[,]> Frames { get; set; }
        public int Count { get { return Frames.Count; } }
        int CountAjacentLights(bool[,] frame, int x, int y)
        {
            var count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var _x = x + j;
                    var _y = y + i;
                    if (_x < 0 || _x > 99 || _y < 0 || _y > 99)
                        continue;
                    if (frame[_y, _x])
                    {
                        if (i == 0 && j == 0)
                            continue;
                        count++;
                    }
                }
            }
            return count;
        }

        public Grid(bool[,] initialFrame)
        {
            Frames = new List<bool[,]>();
            Frames.Add(initialFrame);
        }
        public void Next(bool forceCornersOn)
        {
            var grid = new bool[100, 100];
            var previous = Frames.Last();

            Frames.Add(grid);
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    var count = CountAjacentLights(previous, x, y);
                    if (count == 3)
                        grid[y, x] = true;
                    else if (count == 2 && previous[y, x])
                        grid[y, x] = true;
                    else
                        grid[y, x] = false;
                }
            }

            if (forceCornersOn)
                grid[0, 0] = grid[0, 99] = grid[99, 0] = grid[99, 99] = true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var init = new bool[100, 100];
            var input = File.ReadAllLines("input.txt").ToList();

            input.SelectMany((row, y) =>
                row.Select((c, x) => new
                {
                    X = x,
                    Y = y,
                    On = c == '#'
                }))
                .ToList()
                .ForEach(a => init[a.Y, a.X] = a.On);

            var grid = new Grid(init);

            while (grid.Count <= 100)
            {
                grid.Next(false);
            }
            Console.WriteLine("Part 1:");
            Console.WriteLine(grid.Frames.Last().Cast<bool>().Count(x => x));
            Console.WriteLine();

            grid.Frames = grid.Frames.Take(1).ToList();
            init[0, 0] = init[0, 99] = init[99, 0] = init[99, 99] = true;

            while (grid.Count <= 100)
            {
                grid.Next(true);
            }

            Console.WriteLine("Part 2:");
            Console.WriteLine(grid.Frames.Last().Cast<bool>().Count(x => x));

            Console.ReadLine();
        }
    }
}
