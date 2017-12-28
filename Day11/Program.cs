using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class PasswordGenerator
    {
        readonly Func<string, bool> Rules;
        public string Current { get; set; } = "aaaaaabc";

        public PasswordGenerator()
        {
            //Passwords must be exactly eight lowercase letters (for security reasons)
            Rules += x => x.Length == 8;
            Rules += x => x.All(char.IsLower);
            //Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
            Rules += x => !x.Contains("i");
            Rules += x => !x.Contains("o");
            Rules += x => !x.Contains("l");
            //Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
            Rules += x =>
            {
                var count = 0;
                for (int i = 1; i < 8; i++)
                {
                    if (x[i - 1] + 1 == x[i])
                        count++;
                    else
                        count = 0;

                    if (count >= 2)
                        return true;
                }
                return false;
            };
            //Passwords must contain at least two different, non - overlapping pairs of letters, like aa, bb, or zz.
            Rules += x =>
            {
                var count = 0;
                for (int i = 1; i < 8; i++)
                {
                    if (x[i - 1] == x[i])
                    {
                        count++;
                        i++;

                        if (count >= 2)
                            return true;
                    }
                }
                return false;
            };
        }

        bool IsValid()
        {
            foreach (var rule in Rules.GetInvocationList())
                if (!(bool)rule.DynamicInvoke(Current))
                    return false;
            return true;
        }
        void NextUnvalidated()
        {
            var curr = Current.ToList();

            while (curr.Last() == 'z')
                curr.RemoveAt(curr.Count - 1);

            var temp = curr.Last();
            curr.RemoveAt(curr.Count - 1);

            temp++;
            curr.Add(temp);

            while (curr.Count < Current.Length)
                curr.Add('a');

            Current = string.Concat(curr);
        }
        public void Next()
        {
            do
            {
                NextUnvalidated();
            }
            while (!IsValid());
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var input = "hepxcrrq";
            //var input = "vzbxkghb"; //niklas input
            var gen = new PasswordGenerator { Current = input };
            Console.WriteLine($"old pass: {gen.Current}");
            gen.Next();
            Console.WriteLine($"new pass: {gen.Current}");
            gen.Next();
            Console.WriteLine($"new pass: {gen.Current}");
            Console.ReadLine();
        }
    }
}
