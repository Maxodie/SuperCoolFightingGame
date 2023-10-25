using System;

namespace SuperCoolFightingGame
{
    class Tank : Character
    {
        public Tank(CharacterStats data) : base(data) { }
    
        public override void UseAbility(Character optionalTarget = null)
        {
            base.UseAbility();
      
            if (optionalTarget == null)
                Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
            else
            {
                CurrentAttack++;
                //Console.WriteLine($"{Name} utilise sa capacité ! Il inflige {Attack(optionalTarget)} pt de dégats à {optionalTarget.Name} et perd 1 pt de vie.");
                CurrentAttack--;
                CurrentHealth--;
            }
        }
    }
}