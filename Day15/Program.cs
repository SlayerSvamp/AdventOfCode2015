using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    //Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
    public class Ingredient
    {
        public int Capacity { get; set; }
        public int Durability { get; set; }
        public int Flavor { get; set; }
        public int Texture { get; set; }
        public int Calories { get; set; }

        public Ingredient()
        {

        }
        public Ingredient(string raw)
        {
            var parts = raw.Replace(",", "")
                .Split(' ')
                .Skip(1)
                .Where((x, i) => i % 2 == 1)
                .Select(x => int.Parse(x))
                .ToList();

            Capacity = parts[0];
            Durability = parts[1];
            Flavor = parts[2];
            Texture = parts[3];
            Calories = parts[4];
        }
    }
    class Program
    {
        static int GetScore(List<Ingredient> ingredients, List<int> amounts, bool checkCalories)
        {
            Func<Func<Ingredient, int>, int> getSum = (ing) => Math.Max(ingredients.Select((x, i) => ing(x) * amounts[i]).Sum(), 0);

            if (checkCalories && getSum(x => x.Calories) != 500)
                return 0;

            var val = getSum(x => x.Capacity);
            if (val > 0)
                val *= getSum(x => x.Durability);
            if (val > 0)
                val *= getSum(x => x.Flavor);
            if (val > 0)
                val *= getSum(x => x.Texture);
            return val;

        }
        static int GetHighestScore(List<Ingredient> ingredients, List<int> amounts, int spaceLeft, bool checkCalories)
        {
            if (amounts.Count + 1 == ingredients.Count)
            {
                amounts.Add(spaceLeft);
                return GetScore(ingredients, amounts, checkCalories);
            }

            var max = 0;
            for (var i = 0; i <= spaceLeft; i++)
            {
                var list = amounts.ToList();
                list.Add(i);
                var val = GetHighestScore(ingredients, list, spaceLeft - i, checkCalories);
                if (val > max)
                    max = val;
            }

            return max;

        }
        static void Main(string[] args)
        {
            var ingredients = File.ReadAllLines("input.txt").Select(x => new Ingredient(x)).ToList();
            var max = GetHighestScore(ingredients, new List<int>(), 100, false);
            Console.WriteLine($"Highest score of a cookie: {max}");
            Console.CursorTop++;

            max = GetHighestScore(ingredients, new List<int>(), 100, true);
            Console.WriteLine($"Highest score with 500 calories: {max}");
            Console.CursorTop++;

            Console.ReadLine();
        }
    }
}
