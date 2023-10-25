using System;

namespace SuperCoolFightingGame
{
    class Healer : Character
    {
        int _healValue = 2;
    
        public Healer(CharacterStats data) : base(data) { }
    
        public override void UseAbility(Character optionalTarget = null)
        {
            base.UseAbility();

            Console.WriteLine($"{Name} utilise sa capacité ! Elle se soigne de {_healValue} pv. ({BaseHealth} pv max)");
            CurrentHealth += _healValue;
            if (CurrentHealth > BaseHealth) CurrentHealth = BaseHealth;
        }
    }
}