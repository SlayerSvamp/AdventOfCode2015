using System;
using System.Collections.Generic;
using System.Linq;

namespace Day22
{
    static class Spellbook
    {
        static List<Spell> _spells = null;
        static List<Spell> Spells { get { return _spells ?? (_spells = CreateSpellbook()); } }
        private static List<Spell> CreateSpellbook()
        {
            var spells = new List<Spell>();

            spells.Add(new Spell
            {
                Name = SpellName.MagicMissile,
                Cost = 53,
                Effect = (x, y) => y.HitPoints -= 4,
            });

            spells.Add(new Spell
            {
                Name = SpellName.Drain,
                Cost = 73,
                Effect = (x, y) => { x.HitPoints += 2; y.HitPoints -= 2; },
            });

            spells.Add(new Spell
            {
                Name = SpellName.Shield,
                Cost = 113,
                Duration = 6,
                Effect = (x, y) => x.Armor = 7,
                EffectEnd = (x, y) => x.Armor = 0
            });

            spells.Add(new Spell
            {
                Name = SpellName.Poison,
                Cost = 173,
                Duration = 6,
                Effect = (x, y) => y.HitPoints -= 3
            });

            spells.Add(new Spell
            {
                Name = SpellName.Recharge,
                Cost = 229,
                Duration = 5,
                Effect = (x, y) => x.Mana += 101
            });

            return spells;
        }
        static IEnumerable<Spell> getCastableSpells(int mana, List<Spell> effects)
        {
            return Spells
                .Where(x => x.Cost <= mana)
                .Where(x => !effects.Any(e => e.Name == x.Name && e.IsEffect));
        }
        public static List<Spell> GetCastableSpells(int mana, List<Spell> effects)
        {
            return getCastableSpells(mana, effects)
                .Select(x => x.Clone())
                .ToList();
        }

        internal static Spell GetRandomCastableSpell(int mana, List<Spell> effects)
        {
            return getCastableSpells(mana, effects).OrderBy(x => Guid.NewGuid()).FirstOrDefault()?.Clone();
        }
    }
}