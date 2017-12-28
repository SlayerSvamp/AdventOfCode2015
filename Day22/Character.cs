using System;
using System.Collections.Generic;

namespace Day22
{
    class Character
    {
        public bool IsPlayer { get; set; }
        public int MaxHitPoints { get; set; }
        public int HitPoints { get; set; }
        public bool IsAlive { get { return HitPoints > 0; } }
        public int MaxMana { get; set; }
        public int Mana { get; set; }
        public int ManaSpent { get; set; } = 0;
        public int Damage { get; set; }
        public int Armor { get; set; } = 0;
        public Character()
        {
        }
        public void Reset()
        {
            HitPoints = MaxHitPoints;
            Mana = MaxMana;
            ManaSpent = 0;
            Armor = 0;
        }
        public Character Clone()
        {
            return new Character
            {
                IsPlayer = IsPlayer,
                MaxHitPoints = MaxHitPoints,
                HitPoints = HitPoints,
                MaxMana = MaxMana,
                Mana = Mana,
                ManaSpent = ManaSpent,
                Damage = Damage,
                Armor = Armor,
            };
        }


        internal void CastSpell(Spell spell, List<Spell> effects, Character enemy)
        {
            ManaSpent += spell.Cost;
            Mana -= spell.Cost;

            if (spell.IsEffect)
                effects.Add(spell);
            else
                spell.Effect(this, enemy);

        }

        internal void Attack(Character thisPlayer)
        {
            thisPlayer.HitPoints -= Math.Max(Damage - thisPlayer.Armor, 1);
        }
    }
}