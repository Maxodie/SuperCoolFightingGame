using GameEn;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SuperCoolFightingGame
{
    class Healer : Character
    {
        int _healValue = 2;
    
        public Healer(CharacterStats data, bool isComputer, GameManager gm, Dictionary<string, AudioListener> savedAudio) : base(data, isComputer, gm, savedAudio) {
            specialSound = savedAudio["lunaSpe"];
        }

        public override void InitAnimations(ImageLoader imageLoader) {
            base.InitAnimations(imageLoader);

            specialSelfEffectPos = !isComputer ? new Vector2(96, 176) : new Vector2(536, 120);

            spriteSpecialSelfEffect = new Sprite(imageLoader.GetImage(characterSpecialSelfEffectImgPath), new Rectangle(0, 0, 160, 200), specialSelfEffectPos);
            specialSelfEffect = new SpriteAnimation(WindowE.instance, spriteSpecialSelfEffect, new Rectangle(0, 0, 3200, 128), 20, 1f, 0.5f);
            specialSelfEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialSelfEffect); };

            animator.AddAnimation(imageLoader.GetImage(characterSpecialSelfImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2560, 128), 20, 1f, .5f, "HealerSpe");

        }

        public override void UseAbility(Character optionalTarget = null, bool playSelfEffect = true, bool playEnemyEffect = true) {
            if (CurrentOperation != Operation.Special) return;

            animator.PlayAnimation("HealerSpe");

            int currentHeal = _healValue;

            if (CurrentHealth + currentHeal > BaseHealth)
                currentHeal = BaseHealth - CurrentHealth;

            gm.UpdateTextInfos($"{Name} calls upon the \nstars and heals {currentHeal}HP!");
            CurrentHealth += currentHeal;

            for(int i=0; i < currentHeal; i++) {
                playerHud.GetHp();
            }

            specialSound.Play();

            base.UseAbility(optionalTarget, playSelfEffect, playEnemyEffect);
        }
    }
}