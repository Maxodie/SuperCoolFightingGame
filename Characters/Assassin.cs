using System;

namespace SuperCoolFightingGame
{
    class Assassin : Character
    {
        bool _abilityCharged;
      
        public Assassin(CharacterStats data) : base(data) { }

        public override void Attack(Character target)
        {
            if (_abilityCharged && target.CurrentOperation == Operation.Defend) CurrentAttack++;
                base.Attack(target);
            if (_abilityCharged && target.CurrentOperation == Operation.Defend) CurrentAttack--;
            return;
        }

        public override void UseAbility(Character optionalTarget = null)
        {
            base.UseAbility();
            Console.WriteLine($"{Name} utilise sa capcité ! Sa prochaine attaque pourra transpercer la défense adverse.");
            _abilityCharged = true;
        }

        public override int GetSpecialData()
        {
            return Convert.ToInt32(_abilityCharged);
        }
    }
}