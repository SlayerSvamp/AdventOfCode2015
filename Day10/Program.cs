using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        static string Apply(string value)
        {
            var res = new StringBuilder();
            char last = '-';
            int len = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if(value[i] == last)
                {
                    len++;
                }
                else
                {
                    if(last != '-')
                    {
                        res.Append($"{len}{last}");
                    }
                    last = value[i];
                    len = 1;
                }
            }
            res.Append($"{len}{last}");
            return res.ToString();
        }
        static string Process(string value, int times)
        {
            var seq = value;
            for (int i = 0; i < times; i++)
            {
                seq = Apply(seq);
            }
            return seq;
        }
        static void Main(string[] args)
        {
            var input = "1321131112";
            if (Process("1", 5) != "312211")
            {
                Console.WriteLine($"Test failed, 'Process(\"1\", 5)' returns {Process("1",5)}");
                Console.ReadLine();
                return;
            }

            var res = Process(input, 40);
            var res2 = Process(input, 50);

            Console.WriteLine($"With {input} as input, {40} runs ends up with a string, {res.Length} long");
            Console.WriteLine($"With {input} as input, {50} runs ends up with a string, {res2.Length} long");

            Console.ReadLine();
        }
    }
}
