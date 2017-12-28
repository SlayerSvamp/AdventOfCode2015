using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    enum InstructionType { Half, Tripple, Increment, Jump, JumpIfEven, JumpIfOne }
    class Register
    {
        public int Value { get; set; } = 0;
    }
    class Program
    {
        static void Run(Register a, Register b)
        {
            var index = 0;
            var input = File.ReadAllLines("input.txt")
                .Select(x => x.Replace(",", "").Split(' '))
                .Select<string[], Action>(x =>
                {
                    Register r = null;
                    int firstValue = 0;
                    int secondValue = 0;
                    if (x[1] == "a")
                        r = a;
                    else if (x[1] == "b")
                        r = b;
                    else
                        firstValue = int.Parse(x[1]);
                    if (x.Length > 2)
                        secondValue = int.Parse(x[2]);

                    switch (x[0])
                    {
                        case "hlf": return () => { r.Value /= 2; index++; };
                        case "tpl": return () => { r.Value *= 3; index++; };
                        case "inc": return () => { r.Value++; index++; };
                        case "jmp": return () => { index += firstValue; };
                        case "jie": return () => { index += r.Value % 2 == 0 ? secondValue : 1; }; // ska det verkligen vara +1 om den inte skippar?
                        case "jio": return () => { index += r.Value == 1 ? secondValue : 1; };     // utvärdera!
                        default: return null;
                    }
                })
                .ToList();

            while (index < input.Count && index >= 0)
            {
                input[index]();
            }
        }
        static void Main(string[] args)
        {
            var a = new Register();
            var b = new Register();
            Run(a, b);
            Console.WriteLine($"a: {a.Value}, b: {b.Value}");

            a.Value = 1;
            b.Value = 0;
            Run(a, b);
            Console.WriteLine($"a: {a.Value}, b: {b.Value}");

            Console.ReadLine();
        }
    }
}
