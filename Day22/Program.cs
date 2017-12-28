using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    class Program
    {
        static void RunEffects(List<Spell> effects, Character player, Character boss)
        {
            //reset effects
            effects.ForEach(x => x.EffectEnd(player, boss));

            //remove ending effects
            effects.Where(x => !x.IsEffect)
                .ToList()
                .ForEach(x => effects.Remove(x));

            //apply effects
            effects.ForEach(x =>
            {
                x.Effect(player, boss);
                x.Duration--;
            });
        }
        static void Fight(List<Spell> effects, Character player, Character boss, List<SpellName> spellsCast, int maxManaCost, bool hard)
        {
            bool finished() => !player.IsAlive || !boss.IsAlive;
            while (!finished())
            {
                //players turn
                if (hard)
                {
                    player.HitPoints--;
                    if (finished())
                        return;
                }
                RunEffects(effects, player, boss);
                if (finished())
                    return;

                var spell = Spellbook.GetRandomCastableSpell(player.Mana, effects);
                if (spell == null)
                    return;

                if (player.ManaSpent + spell.Cost > maxManaCost)
                    return;

                spellsCast.Add(spell.Name);
                player.CastSpell(spell, effects, boss);


                if (finished())
                    return;

                //boss's turn
                RunEffects(effects, player, boss);
                if (finished())
                    return;

                boss.Attack(player);
            }
        }
        static void Main(string[] args)
        {
            bool hard = false;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" Choose difficulty:");
            ConsoleKey key;
            do
            {
                Console.CursorTop = 3;
                if (!hard)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" {(!hard ? '>' : ' ')}Normal");
                if (hard)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ResetColor();
                Console.WriteLine($" {(hard ? '>' : ' ')}Hard");

                Console.ResetColor();
                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow)
                    hard = !hard;

            } while (key != ConsoleKey.Enter);

            Console.Clear();
            var player = new Character
            {
                IsPlayer = true,
                MaxHitPoints = 50,
                MaxMana = 500,
            };
            var boss = new Character
            {
                IsPlayer = false,
                MaxHitPoints = 51,
                Damage = 9
            };

            var effects = new List<Spell>();
            var manaSpent = int.MaxValue;
            List<List<SpellName>> castOrders = null;
            Console.WindowWidth = 200;

            while (true)
            {
                var spellsCast = new List<SpellName>();
                do
                {
                    spellsCast.Clear();
                    //reset
                    player.Reset();
                    boss.Reset();
                    effects.Clear();
                    //run
                    Fight(effects, player, boss, spellsCast, manaSpent, hard);
                } while (boss.IsAlive);
                if (player.ManaSpent <= manaSpent)
                {
                    if (player.ManaSpent == manaSpent)
                        castOrders?.Add(spellsCast);
                    else
                    {
                        castOrders = new List<List<SpellName>> { spellsCast };
                        manaSpent = player.ManaSpent;
                    }

                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine($" It only took {player.ManaSpent} mana to defeat the boss");
                    Console.WriteLine();
                    foreach (var castOrder in castOrders.Select(x => string.Join(" -> ", x)).GroupBy(x => x).OrderBy(x => x.Key).Select(x => $" {x.Count(),3} x {x.Key}"))
                        Console.WriteLine(castOrder);
                    Console.WriteLine();
                }
                var keys = new List<ConsoleKey>();
                while (Console.KeyAvailable)
                    keys.Add(Console.ReadKey(true).Key);
                if (keys.Any(x => x == ConsoleKey.Enter))
                    break;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Game Over.");

            Console.ReadLine();
        }
    }
}