using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {
        public class Reindeer
        {
            public string Name { get; set; }
            public int Speed { get; set; }
            public int FlightDuration { get; set; }
            public int SleepDuration { get; set; }
            //New point system
            public int Score { get; set; } = 0;
            public int Distance { get; set; } = 0;
            public bool Sleeping { get; set; } = false;
            public int TimeUntilStateChange { get; set; }
            
            public int Tick()
            {
                if (!Sleeping)
                    Distance += Speed;

                TimeUntilStateChange--;
                if (TimeUntilStateChange == 0)
                {
                    Sleeping ^= true;
                    TimeUntilStateChange = Sleeping ? SleepDuration : FlightDuration;
                }
                return Distance;
            }
            public int GetDistanceBySeconds(int duration)
            {
                var distance = 0;
                var sleeping = false;
                var elapsed = 0;
                while (elapsed < duration)
                {
                    elapsed += sleeping ? SleepDuration : FlightDuration;
                    if (!sleeping)
                    {
                        distance += Speed * FlightDuration;
                        if (elapsed > duration)
                            distance -= (elapsed - duration) * Speed;
                    }
                    sleeping = !sleeping;
                }

                return distance;
            }
        }

        static void Main(string[] args)
        {
            var duration = 2503;
            //Dancer can fly 7 km/s for 20 seconds, but then must rest for 119 seconds.
            var reindeer = new List<Reindeer>();
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var values = line
                    .Split(' ')
                    .Where(x => int.TryParse(x, out _))
                    .Select(x => int.Parse(x))
                    .ToList();

                reindeer.Add(new Reindeer
                {
                    Name = string.Concat(line.TakeWhile(x => x != ' ')),
                    Speed = values[0],
                    FlightDuration = values[1],
                    SleepDuration = values[2],
                    TimeUntilStateChange = values[1]
                });
            }
            var max = 0;
            Reindeer best = null;
            foreach (var deer in reindeer)
            {
                var distance = deer.GetDistanceBySeconds(duration);
                if (distance > max)
                {
                    max = distance;
                    best = deer;
                }
            }
            Console.WriteLine("Part 1:");
            Console.WriteLine($"The longest distance is {max} km, ran by {best?.Name}");
            Console.CursorTop++;

            for (int i = 0; i < duration; i++)
            {
                foreach(var deer in reindeer)
                    deer.Tick();

                var leadingDistance = reindeer.Max(x => x.Distance);
                foreach (var deer in reindeer)
                {
                    if (deer.Distance == leadingDistance)
                        deer.Score++;
                }
            }
            var order = reindeer.OrderByDescending(x => x.Score);
            var winner = order.First();
            var nr2 = order.Skip(1).First();
            Console.WriteLine("Part 2:");
            Console.WriteLine($"{winner.Name} won with {winner.Score} points total.");
            Console.WriteLine($"The total distance traveled for {winner.Name} is {winner.Distance}");
            Console.WriteLine($"In second place came {nr2.Name}");
            Console.WriteLine($"Only {winner.Score - nr2.Score} points behind, with a total of {nr2.Score} points");

            Console.ReadLine();

        }
    }
}
