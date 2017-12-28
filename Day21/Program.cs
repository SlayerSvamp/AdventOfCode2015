using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    public enum EquipmentType { Weapon, Armor, Ring }
    class Shop
    {
        public List<Equipment> Items { get; set; }
        public IEnumerable<Equipment> Weapons { get { return Items.Where(x => x.Type == EquipmentType.Weapon); } }
        public IEnumerable<Equipment> Armors { get { return Items.Where(x => x.Type == EquipmentType.Armor); } }
        public IEnumerable<Equipment> Rings { get { return Items.Where(x => x.Type == EquipmentType.Ring); } }
        public List<Equipment> InStock { get; set; }
        public Shop()
        {
            Items = GetItems();
            Restock();
        }
        public List<Equipment> GetItems()
        {
            var items = new List<Equipment>();
            EquipmentType type = 0;
            foreach (var line in File.ReadAllLines("shop_items.txt").Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                if (line.StartsWith("Weapons"))
                    type = EquipmentType.Weapon;
                else if (line.StartsWith("Armor"))
                    type = EquipmentType.Armor;
                else if (line.StartsWith("Rings"))
                    type = EquipmentType.Ring;
                else
                {
                    var parts = line.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    var item = new Equipment
                    {
                        Type = type,
                        Name = parts[0],
                        Cost = int.Parse(parts[1]),
                        Damage = int.Parse(parts[2]),
                        Armor = int.Parse(parts[3])
                    };

                    items.Add(item);
                }
            }

            return items;
        }
        public void Restock()
        {
            InStock = Items.ToList();
        }
        public List<List<Equipment>> GetCombinationsByGold(int gold)
        {
            //add weapons
            var combinations = Weapons.Where(x => x.Cost <= gold)
                                      .Select(x => new List<Equipment> { x })
                                      .ToList();
            //add armors
            foreach (var combination in combinations.ToList())
            {
                var goldLeft = gold - combination.First().Cost;
                foreach (var armor in Armors.Where(a => a.Cost <= goldLeft))
                {
                    var newCombination = combination.ToList();
                    newCombination.Add(armor);
                    combinations.Add(newCombination);
                }
            }
            //add first rings
            foreach (var combination in combinations.ToList())
            {
                var goldLeft = gold - combination.Sum(x => x.Cost);
                foreach (var ring in Rings.Where(x => x.Cost <= goldLeft))
                {
                    var newCombination = combination.ToList();
                    newCombination.Add(ring);
                    combinations.Add(newCombination);
                }
            }
            //add second rings
            foreach (var combination in combinations.Where(x => x.Any(y => y.Type == EquipmentType.Ring)).ToList())
            {
                var firstRing = combination.Single(x => x.Type == EquipmentType.Ring);
                var goldLeft = gold - combination.Sum(x => x.Cost);
                foreach (var ring in Rings.SkipWhile(x => x != firstRing).Skip(1).Where(x => x.Cost <= goldLeft))
                {
                    var newCombination = combination.ToList();
                    newCombination.Add(ring);
                    combinations.Add(newCombination);
                }
            }
            return combinations.Where(x => x.Sum(y => y.Cost) == gold).ToList();
        }
    }
    class Arena
    {
        public bool Debug { get; set; }
        public Character Player { get; set; }
        public Character Boss { get; set; }
        public bool PlayerWon { get { return Player.IsAlive; } }

        public void Setup(List<Equipment> items)
        {
            Player.Heal();
            Boss.Heal();

            Player.Equip(items);
        }
        public void Fight()
        {
            while (Player.IsAlive)
            {
                Player.Attack(Boss);

                if (!Boss.IsAlive)
                    break;

                Boss.Attack(Player);
            }
        }
    }
    class Character
    {
        public int Gold { get; set; }
        public int BaseDamage { get; set; }
        public int Damage { get { return BaseDamage + Items.Sum(x => x.Damage); } }
        public int BaseArmor { get; set; }
        public int Armor { get { return BaseArmor + Items.Sum(x => x.Armor); } }
        public int MaxHitPoints { get; set; }
        public int HitPoints { get; set; } = 0;
        public bool IsAlive { get { return HitPoints > 0; } }
        public List<Equipment> Items { get; set; } = new List<Equipment>();
        public void Attack(Character enemy)
        {
            enemy.HitPoints -= Math.Max(Damage - enemy.Armor, 1);
        }
        public void Heal()
        {
            HitPoints = MaxHitPoints;
        }
        public void Equip(List<Equipment> items)
        {
            Items = items;
        }
    }

    public class Equipment
    {
        public EquipmentType Type { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Armor { get; set; }
        public int Damage { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //input:
            //Hit Points: 100
            //Damage: 8
            //Armor: 2
            var arena = new Arena
            {
                Player = new Character
                {
                    MaxHitPoints = 100,
                    BaseDamage = 0,
                    BaseArmor = 0
                },
                Boss = new Character
                {
                    MaxHitPoints = 100,
                    BaseDamage = 8,
                    BaseArmor = 2,
                },
                Debug = true
            };

            var shop = new Shop();


            var won = false;
            for (arena.Player.Gold = 8; !won; arena.Player.Gold++)
            {
                var combinations = shop.GetCombinationsByGold(arena.Player.Gold);
                if (combinations.Count == 0)
                    continue;
                foreach (var items in combinations)
                {
                    arena.Setup(items);
                    arena.Fight();

                    if (arena.PlayerWon)
                    {
                        Console.WriteLine($"Boss: {arena.Boss.Damage}/{arena.Boss.Armor}");
                        Console.WriteLine($"Player: {arena.Player.Damage}/{arena.Player.Armor} - Gold: {arena.Player.Gold}");
                        foreach (var item in items)
                            Console.WriteLine($"\t{item.Name} {item.Damage}/{item.Armor} - ${item.Cost}");

                        won = true;
                        break;
                    }
                }
            }

            Console.WriteLine();

            var limit = shop.Weapons.Max(x => x.Cost) +
                shop.Armors.Max(x => x.Cost) + shop.Rings.Max(x => x.Cost) +
                shop.Rings.OrderByDescending(x => x.Cost).Skip(1).First().Cost;

            var lost = false;
            for (arena.Player.Gold = limit; !lost; arena.Player.Gold--)
            {
                var combinations = shop.GetCombinationsByGold(arena.Player.Gold);
                foreach (var items in combinations)
                {
                    arena.Setup(items);
                    arena.Fight();

                    if (!arena.PlayerWon)
                    {
                        Console.WriteLine($"Boss: {arena.Boss.Damage}/{arena.Boss.Armor}");
                        Console.WriteLine($"Player: {arena.Player.Damage}/{arena.Player.Armor} - Gold: {arena.Player.Gold}");
                        foreach (var item in items)
                            Console.WriteLine($"\t{item.Name} {item.Damage}/{item.Armor} - ${item.Cost}");

                        lost = true;
                        break;
                    }
                }

            }

            Console.ReadLine();
        }
    }
}
