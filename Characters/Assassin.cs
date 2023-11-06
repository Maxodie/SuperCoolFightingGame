using GameEn;
using System.Drawing;
using System;

namespace SuperCoolFightingGame
{
    class Assassin : Character
    {
        bool _abilityCharged;
      
        public Assassin(CharacterStats data, bool isComputer, GameManager gm) : base(data, isComputer, gm) { }

        public override void InitAnimations(ImageLoader imageLoader) {
            base.InitAnimations(imageLoader);

            specialSound = new AudioListener(false, "Media/sounds/SFX/GuniSpecial.wav");

            specialEnemyEffectPos = isComputer ? new Vector2(104, 216) : new Vector2(560, 160); //
            specialSelfEffectPos = !isComputer ? new Vector2(112, 232) : new Vector2(552, 176);

            spriteSpecialSelfEffect = new Sprite(imageLoader.GetImage(characterSpecialSelfEffectImgPath), new Rectangle(0, 0, 128, 128), specialSelfEffectPos);
            if (isComputer)
                spriteSpecialSelfEffect.FlipX();
            specialSelfEffect = new SpriteAnimation(WindowE.instance, spriteSpecialSelfEffect, new Rectangle(0, 0, 768, 128), 6, 1f, .8f);
            specialSelfEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialSelfEffect); };

            spriteSpecialEnemyEffect = new Sprite(imageLoader.GetImage(characterSpecialEnemyImgPath), new Rectangle(0, 0, 128, 128), specialEnemyEffectPos);
            specialEnemyEffect = new SpriteAnimation(WindowE.instance, spriteSpecialEnemyEffect, new Rectangle(0, 0, 2560, 128), 20, 1f, 1f);
            specialEnemyEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialEnemyEffect); };
        }

        public override void Attack(Character target, bool isForce = false, bool doAnim = true)
        {

            if (_abilityCharged && target.CurrentOperation == Operation.Defend) CurrentAttack++;
            
            base.Attack(target, isForce, doAnim);
            
            if (_abilityCharged && target.CurrentOperation == Operation.Defend) CurrentAttack--;
            return;
        }

        public override void StartAbility() {
            base.StartAbility();

            if (CurrentOperation != Operation.Special) return;

            gameE.AddSpriteToRender(spriteSpecialSelfEffect);
            specialSelfEffect.Play();

            gameE.AddSpriteToRender(spriteSpecialEnemyEffect);
            specialEnemyEffect.Play();

            _abilityCharged = true;

            specialSound.Play();

            gm.UpdateTextInfos($"{Name} is identifying the\n enemy's weakness...");

            currentActionTimeMs = (int)(specialSelfEffect.duration * 1000) + waitActionTimeOffset;
        }

        public override void UseAbility(Character optionalTarget = null, bool playSelfEffect = true, bool playEnemyEffect = true) {
            if (CurrentOperation != Operation.Special) return;

            base.UseAbility(optionalTarget, false, false);

            Attack(optionalTarget, true);
            _abilityCharged = false;

        }

        public override int GetSpecialData()
        {
            return Convert.ToInt32(_abilityCharged);
        }
    }
}