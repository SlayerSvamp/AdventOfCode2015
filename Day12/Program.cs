using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Day12
{
    class Program
    {
        static long ExtractSum(JToken tok, bool includeRed)
        {
            switch (tok.Type)
            {
                case JTokenType.Array:
                    return (tok as JArray).Select(x => ExtractSum(x, includeRed)).Sum();
                case JTokenType.Object:
                    var obj = tok as JObject;

                    var exclude = !includeRed &&
                        obj.Values()
                           .OfType<JValue>()
                           .Select(x => x.Value)
                           .OfType<string>()
                           .Any(x => x.ToLower() == "red");

                    if (exclude)
                        return 0;

                    return (tok as JObject).Cast<KeyValuePair<string, JToken>>().Select(x => ExtractSum(x.Value, includeRed)).Sum();

                case JTokenType.Integer:
                    return (long)(tok as JValue).Value;
                default:
                    return 0;
            }
        }
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");

            var obj = JToken.Parse(input);

            var sum = ExtractSum(obj, true);
            Console.WriteLine($"The sum is {sum}");

            sum = ExtractSum(obj, false);
            Console.WriteLine($"The sum is {sum} not counting anything with \"red\"");


            Console.ReadLine();
        }
    }
}
