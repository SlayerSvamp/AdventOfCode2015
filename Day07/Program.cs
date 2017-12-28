using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    class Component
    {
        public Dictionary<string, Component> Components { get; set; }
        int? value = null;
        public string[] WiresIn;
        public string WireOut;
        readonly Func<int> GetOutput;
        public int Input(int index = 0) => int.TryParse(WiresIn[index], out int value) ? value : Components[WiresIn[index]].Output;
        public int Output => value ?? (int)(value = GetOutput());
        Component(string raw)
        {
            var parts = raw.Split(' ');
            {
                switch (parts[1])
                {
                    case "AND":
                        GetOutput = () => Input(0) & Input(1);
                        break;
                    case "OR":
                        GetOutput = () => Input(0) | Input(1);
                        break;
                    case "LSHIFT":
                        GetOutput = () => Input(0) << Input(1);
                        break;
                    case "RSHIFT":
                        GetOutput = () => Input(0) >> Input(1);
                        break;
                    case "->":
                        if (int.TryParse(parts[0], out int _value))
                        {
                            value = _value;
                        }
                        else
                        {
                            GetOutput = () => Input();
                            WiresIn = new string[] { parts[0] };
                        }
                        WireOut = parts[2];
                        return;
                    default: // NOT-gate
                        GetOutput = () => ~Input();
                        WiresIn = new string[] { parts[1] };
                        WireOut = parts[3];
                        return;
                }

                WiresIn = new string[] { parts[0], parts[2] };
                WireOut = parts[4];
            }
        }
        public static Component Create(string raw) => new Component(raw);

        internal void SetValue(int value) => this.value = value;
    }
    class CommandFromString
    {
        static Dictionary<string, Component> Setup()
        {

            var components = File.ReadAllLines("input.txt")
                .Select(Component.Create)
                .ToDictionary(x => x.WireOut);

            foreach (var key in components.Keys)
                components[key].Components = components;

            return components;
        }
        static void Main(string[] args)
        {
            var components = Setup();
            var a = components["a"].Output;
            Console.WriteLine($"\n Part 1:\r\n {a}");

            components = Setup();
            components["b"].SetValue(a);
            Console.WriteLine($"\n Part 2:\r\n {components["a"].Output}");

            Console.ReadLine();
        }
    }
}
