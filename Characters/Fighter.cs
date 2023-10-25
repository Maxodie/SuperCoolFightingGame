using System;

namespace SuperCoolFightingGame
{
    class Fighter : Character
    {
        int _damageTaken = 0;

        public Fighter(CharacterStats data) : base(data) { }

        public override int TakeDamage(Character attacker)
        {
            _damageTaken += base.TakeDamage(attacker);
            return _damageTaken;
        }
    
        public override void UseAbility(Character optionalTarget = null)
        {
            base.UseAbility();
      
            /*if (optionalTarget == null)
                Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
            else
            {
                CurrentAttack = _damageTaken;
                Console.WriteLine($"{Name} utilise sa capacité ! Il retourne les dégats reçus et inflige donc {Attack(optionalTarget)} pt de dégats à {optionalTarget.Name}.");
                CurrentAttack = BaseAttack;
            }*/
        }

        public override void NewRound()
        {
            base.NewRound();
            _damageTaken = 0;
        }
    }
}