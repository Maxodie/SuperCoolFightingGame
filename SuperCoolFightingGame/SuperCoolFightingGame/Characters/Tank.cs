using GameEn;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace SuperCoolFightingGame
{
    class Tank : Character
    {
        public Tank(CharacterStats data, bool isComputer, GameManager gm, Dictionary<string, AudioListener> savedAudio) : base(data, isComputer, gm, savedAudio) {
            specialSound = savedAudio["nanSpe"];
        }

        public override void InitAnimations(ImageLoader imageLoader) {
            base.InitAnimations(imageLoader);

            specialSelfEffectPos = !isComputer ? new Vector2(88, 200) : new Vector2(544, 144);

            spriteSpecialSelfEffect = new Sprite(imageLoader.GetImage(characterSpecialSelfEffectImgPath), new Rectangle(0, 0, 160, 176), specialSelfEffectPos);
            specialSelfEffect = new SpriteAnimation(WindowE.instance, spriteSpecialSelfEffect, new Rectangle(0, 0, 3200, 176), 20, 1f, 1f);
            specialSelfEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialSelfEffect); };

            animator.AddAnimation(imageLoader.GetImage(characterSpecialSelfImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2560, 128), 20, 1f, .5f, "TankSpe");
            animator.AddOnEndAnimation(delegate (object sender, EventArgs e) {  }, "TankSpe");
        }

        public async override void UseAbility(Character optionalTarget = null, bool playSelfEffect = true, bool playEnemyEffect = true)
        {

            base.UseAbility(optionalTarget, playSelfEffect);

            if (CurrentOperation != Operation.Special) return;

            if (optionalTarget == null) {
                Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
                return;
            }

            animator.PlayAnimation("TankSpe");
      
            CurrentAttack++;
            playerHud.GetAttackPoint();
            RemoveHp(1);

            gm.UpdateTextInfos($"{Name} draws power from \nher blood, gaining 1AP!");
            //Console.WriteLine($"{Name} utilise sa capacité ! Il inflige {Attack(optionalTarget)} pt de dégats à {optionalTarget.Name} et perd 1 pt de vie.");

            specialSound.Play();

            currentActionTimeMs += (int)(attackAnimEffect.duration * 1000) * 2 + waitActionTimeOffset;
            await Task.Run(() => { Task.Delay((int)(specialSelfEffect.duration * 1000)).Wait();});


            Attack(optionalTarget, playSelfEffect, playEnemyEffect);

            CurrentAttack--;
            playerHud.LoseAttackPoint();

        }
    }
}