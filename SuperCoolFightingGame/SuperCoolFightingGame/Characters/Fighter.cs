using GameEn;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SuperCoolFightingGame
{
    class Fighter : Character
    {
        Sprite projectile;
        Sprite projectileExplosion;
        SpriteAnimation projectileExplosionAnim;
        Vector2 projectileDir;
        int projectileTravelTimeMs = 1000;
        bool ispwepwe = false;
        Character optionalTarget;

        AudioListener projectileSound;
        AudioListener explosionSound;

        public Fighter(CharacterStats data, bool isComputer, GameManager gm, Dictionary<string, AudioListener> savedAudio) : base(data, isComputer, gm, savedAudio) {
            specialSound = savedAudio["vladSpe"];
            projectileSound = savedAudio["vladProject"];
            explosionSound = savedAudio["vladExplo"];
        }

        public override void InitAnimations(ImageLoader imageLoader) {
            base.InitAnimations(imageLoader);

            //Projectile
            if (characterSpecialProjectileImgPath != "") {
                projectile = new Sprite(imageLoader.GetImage(characterSpecialProjectileImgPath), new Rectangle(0, 0, 80, 64), projectilePlayerPos);
                projectileDir = projectileEnemyPos - projectilePlayerPos;

                if (playerSprite.flipX)
                    projectile.FlipX();
            }

            specialEnemyEffectPos = isComputer ? new Vector2(112, 232) : new Vector2(552, 176);
            specialSelfEffectPos = !isComputer ? new Vector2(96, 208) : new Vector2(536, 152);

            spriteSpecialSelfEffect = new Sprite(imageLoader.GetImage(characterSpecialSelfEffectImgPath), new Rectangle(0, 0, 160, 176), specialSelfEffectPos);
            specialSelfEffect = new SpriteAnimation(WindowE.instance, spriteSpecialSelfEffect, new Rectangle(0, 0, 3200, 176), 20, 1f, 2f);
            specialSelfEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialSelfEffect); };

            projectileExplosion = new Sprite(imageLoader.GetImage(characterSpecialEnemyImgPath), new Rectangle(0, 0, 128, 128), specialEnemyEffectPos);
            projectileExplosionAnim = new SpriteAnimation(WindowE.instance, projectileExplosion, new Rectangle(0, 0, 768, 128), 6, 1f, 0.5f);

            animator.AddAnimation(imageLoader.GetImage(characterSpecialSelfImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2560, 128), 20, 1f, .5f, "FighterSpe");
            projectileExplosionAnim.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(projectileExplosion); };

        }

        public override void Update(float dt) {
            base.Update(dt);

            projectileExplosionAnim.Update(dt);
            specialSelfEffect.Update(dt);

            if (ispwepwe) {
                projectile.Move(projectileDir * dt);

                if (Vector2.Distance(projectile.pos, projectileEnemyPos) < 20) {
                    gameE.RemoveSpriteFromRender(projectile);
                    ispwepwe = false;

                    gameE.AddSpriteToRender(projectileExplosion);
                    projectileExplosionAnim.Play();

                    explosionSound.Play();

                    Attack(optionalTarget, true, false, false);
                    CurrentAttack = BaseAttack;
                    _damageTaken = 0;
                }

            }
        }

        public override void StartAbility() {
            base.StartAbility();

            if (CurrentOperation != Operation.Special) return;

            specialSelfEffect.Play();
            gameE.AddSpriteToRender(spriteSpecialSelfEffect);
            specialSound.Play();

            gm.UpdateTextInfos($"{Name} is firing up!");

            currentActionTimeMs = (int)(specialSelfEffect.duration * 1000) + waitActionTimeOffset;
        }

        public override void UseAbility(Character optionalTarget = null, bool playSelfEffect = true, bool playEnemyEffect = true)
        {
            if (CurrentOperation != Operation.Special) return;

            if(optionalTarget == null)
            {
                Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
                return;
            }

            this.optionalTarget = optionalTarget;

            animator.PlayAnimation("FighterSpe");

            if (projectile != null)
            {
                if (currentActionTimeMs < projectileTravelTimeMs)
                    currentActionTimeMs += projectileTravelTimeMs;

                projectile.ChangePos(projectilePlayerPos);
                gameE.AddSpriteToRender(projectile);
                ispwepwe = true;

                projectileSound.Play();
            }

            CurrentAttack = _damageTaken;

            base.UseAbility(optionalTarget, false, playEnemyEffect);

            gm.UpdateTextInfos($"{Name} exerts payback!\n{optionalTarget.Name} loses {CurrentAttack}HP.");
        }

    }
}