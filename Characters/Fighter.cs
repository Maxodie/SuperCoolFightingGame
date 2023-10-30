using GameEn;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
    class Fighter : Character
    {
        Sprite projectil;
        Sprite projectilExplosion;
        SpriteAnimation projectilExplosionAnim;
        Vector2 projectilDir;
        int projectilTravelTimeMs = 1000;
        bool ispwepwe = false;
        Character optionalTarget;
        public Fighter(CharacterStats data, bool isComputer, GameManager gm) : base(data, isComputer, gm) { }

        public override void InitAnimations(ImageLoader imageLoader) {
            base.InitAnimations(imageLoader);

            //Projectil
            if (characterSpecialProjectilImgPath != "") {
                projectil = new Sprite(imageLoader.GetImage(characterSpecialProjectilImgPath), new Rectangle(0, 0, 80, 64), projectilPlayerPos);
                projectilDir = projectilEnemyPos - projectilPlayerPos;

                if (playerSprite.flipX)
                    projectil.FlipX();
            }

            specialEnemyEffectPos = isComputer ? new Vector2(112, 232) : new Vector2(552, 176);//
            specialSelfEffectPos = !isComputer ? new Vector2(96, 208) : new Vector2(536, 152);

            spriteSpecialSelfEffect = new Sprite(imageLoader.GetImage(characterSpecialSelfEffectImgPath), new Rectangle(0, 0, 128, 128), specialSelfEffectPos);
            specialSelfEffect = new SpriteAnimation(WindowE.instance, spriteSpecialSelfEffect, new Rectangle(0, 0, 768, 128), 6, 1f, 0.5f);
            specialSelfEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(spriteSpecialSelfEffect); };

            projectilExplosion = new Sprite(imageLoader.GetImage(characterSpecialEnemyImgPath), new Rectangle(0, 0, 128, 128), specialEnemyEffectPos);
            projectilExplosionAnim = new SpriteAnimation(WindowE.instance, projectilExplosion, new Rectangle(0, 0, 768, 128), 6, 1f, 0.5f);

            animator.AddAnimation(imageLoader.GetImage(characterSpecialSelfImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2560, 128), 20, 1f, .5f, "FighterSpe");
            projectilExplosionAnim.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(projectilExplosion); };

        }

        public override void Update(float dt) {
            base.Update(dt);

            projectilExplosionAnim.Update(dt);

            if (ispwepwe) {
                projectil.Move(projectilDir * dt);

                if (Vector2.Distance(projectil.pos, projectilEnemyPos) < 20) {
                    gameE.RemoveSpriteFromRender(projectil);
                    ispwepwe = false;

                    gameE.AddSpriteToRender(projectilExplosion);
                    projectilExplosionAnim.Play();

                    Attack(optionalTarget, true, false);
                    CurrentAttack = BaseAttack;
                    _damageTaken = 0;
                }

            }
        }

        public override void UseAbility(Character optionalTarget = null)
        {
            if (CurrentOperation != Operation.Special) return;

            if(optionalTarget == null)
            {
                Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
                return;
            }

            this.optionalTarget = optionalTarget;

            animator.PlayAnimation("FighterSpe");

            if (projectil != null)
            {
                if (currentActionTimeMs < projectilTravelTimeMs)
                    currentActionTimeMs += projectilTravelTimeMs;

                projectil.ChangePos(projectilPlayerPos);
                gameE.AddSpriteToRender(projectil);
                ispwepwe = true;

            }

            gm.UpdateTextInfos($"{Name} is sending back\nenemy damages");

            CurrentAttack = _damageTaken;
            

            base.UseAbility();
        }

    }
}