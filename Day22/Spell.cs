using System;

namespace Day22
{
    class Spell
    {
        public SpellName Name { get; set; }
        public int Cost { get; set; }
        public int Duration { get; set; } = 0;
        public Action<Character, Character> Effect { get; set; }
        public Action<Character, Character> EffectEnd { get; set; } = (x, y) => { };
        public bool IsEffect { get { return Duration > 0; } }

        internal Spell Clone()
        {
            return new Spell
            {
                Name = Name,
                Cost = Cost,
                Duration = Duration,
                Effect = Effect,
                EffectEnd = EffectEnd
            };
        }
    }
}