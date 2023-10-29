using GameEn;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
    class Healer : Character
    {
        int _healValue = 2;
    
        public Healer(CharacterStats data, bool isComputer, GameManager gm) : base(data, isComputer, gm) { }

        public override void InitAnimations(ImageLoader imageLoader) {
            base.InitAnimations(imageLoader);

            specialSelfEffectPos = !isComputer ? new Vector2(96, 176) : new Vector2(536, 120);

            spriteSpecialSelfEffect = new Sprite(imageLoader.GetImage(characterSpecialSelfEffectImgPath), new Rectangle(0, 0, 160, 200), specialSelfEffectPos);
            specialSelfEffect = new SpriteAnimation(WindowE.instance, spriteSpecialSelfEffect, new Rectangle(0, 0, 3200, 128), 20, 1f, 0.5f);
            specialSelfEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialSelfEffect); };

            animator.AddAnimation(imageLoader.GetImage(characterSpecialSelfImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2560, 128), 20, 1f, .5f, "HealerSpe");

        }

        public override void UseAbility(Character optionalTarget = null) {
            animator.PlayAnimation("HealerSpe");

            Console.WriteLine($"{Name} utilise sa capacité ! Elle se soigne de {_healValue} pv. ({BaseHealth} pv max)");

            CurrentHealth += _healValue;

            if (CurrentHealth > BaseHealth) 
                CurrentHealth = BaseHealth;

            for(int i=0; i < _healValue; i++) {
                playerHud.GetHp();
            }

            base.UseAbility();
        }
    }
}