using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class Program
    {
        static long GetNextValue(long previous)
        {
            long multiplier = 252533;
            long divisor = 33554393;

            return (previous * multiplier) % divisor;

        }
        static void Main(string[] args)
        {
            //input: To continue, please consult the code grid in the manual.  Enter the code at row 2981, column 3075.
            //*
            var col = 3075;
            var row = 2981;
            /*/
            var col = 6;
            var row = 6;
            //*/
            long current = 20151125;
            for (int x = 1, y = 2; x != col || y != row; y--, x++)
            {
                if (y == 0)
                {
                    y = x;
                    x = 1;
                }
                current = GetNextValue(current);

            }

            current = GetNextValue(current);
            Console.WriteLine($"your code: {current}");
            Console.ReadLine();
        }
    }
}
