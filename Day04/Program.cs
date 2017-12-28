using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    class Program
    {
        static string Hash(MD5 md5, string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            var hash = md5.ComputeHash(bytes);
            return string.Concat(hash.Select(x => x.ToString("x2")));
        }
        static void Main(string[] args)
        {
            var input = "bgvyzdsv{0}";
            MD5 md5 = MD5.Create();

            var first = true;
            for (int i = 1; ; i++)
            {
                var code = string.Format(input, i);
                var hash = Hash(md5, code);
                if (first && hash.StartsWith("00000"))
                {
                    first = false;
                    Console.WriteLine($"\n Part 1:\r\n {i}");
                }
                else if(!first && hash.StartsWith("000000"))
                {
                    Console.WriteLine($"\n Part 2:\r\n {i}");
                    break;
                }
            }

            Console.ReadLine();
        }
    }
}
