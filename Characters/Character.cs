using GameEn;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
  public class Character
  {
        #region InstanceVariables

        protected GameE gameE;
        GameManager gm;

        protected PlayerHud playerHud;

        public Sprite playerSprite;
        bool isFliped;
        const int waitActionTimeOfset = 500;
        public bool isComputer { get; private set; }

        Sprite SpriteEnemyEffect;

        protected Sprite spriteSpecialSelfEffect;
        protected Sprite spriteSpecialEnemyEffect;
        protected SpriteAnimation specialSelfEffect;
        protected SpriteAnimation specialEnemyEffect;
        protected Vector2 specialSelfEffectPos;
        protected Vector2 specialEnemyEffectPos;

        public string Name;
        public int BaseHealth;
        public int CurrentHealth;
        public int BaseAttack;
        public int CurrentAttack;
        public int CurrentCritRate;
        public int BaseDodgeRate;
        public int CurrentDodgeRate;

        public string characterSpecialBtnImgPath;
        public string characterSpecialBtnHighlightImgPath;
        public string characterSpecialBtnPressedImgPath;
        public string characterSpecialSelfImgPath;
        public string characterSpecialSelfEffectImgPath;
        public string characterSpecialProjectilImgPath;
        public string characterSpecialEnemyImgPath;
        public string characterIdleImgPath;
        public string characterDeathImgPath;
        public string characterDamagedPath;
        public string characterIcon;
        public string characterTypeIconPath;

        public string attackEffect;
        public string defenseEndEffect;
        public string defenseStartEffect;

        public Operation CurrentOperation;

        protected int _damageTaken = 0;

        protected SpriteAnimation attackAnimEffect;
        Vector2 enemyAttackEffectPos;

        protected SpriteAnimation defendStartAnimEffect;
        protected SpriteAnimation defendEndAnimEffect;
        Sprite defendStartSprite;
        Sprite defendEndSprite;
        Vector2 playerDefendEffectPos;
        protected Vector2 projectilPlayerPos;
        protected Vector2 projectilEnemyPos;

        protected Animator animator;
        public int currentActionTimeMs = 0;

        #endregion

        public Character(CharacterStats data, bool isComputer, GameManager gm)
        {
            Name = data.Name;
            BaseHealth = data.BaseHealth;
            BaseAttack = data.BaseAttack;

            characterSpecialBtnImgPath = data.characterSpecialBtnImgPath;
            characterSpecialBtnHighlightImgPath = data.characterSpecialBtnHighlightImgPath;
            characterSpecialBtnPressedImgPath = data.characterSpecialBtnPressedImgPath;
            characterSpecialSelfImgPath = data.characterSpecialSelfImgPath;
            characterSpecialSelfEffectImgPath = data.characterSpecialSelfEffectImgPath;
            characterSpecialProjectilImgPath = data.characterSpecialProjectilImgPath;
            characterSpecialEnemyImgPath = data.characterSpecialEnemyImgPath;
            characterIdleImgPath = data.characterIdleImgPath;
            characterDeathImgPath = data.characterDeathImgPath;
            characterDamagedPath = data.characterDamagedPath;
            characterIcon = data.characterIconPath;
            characterTypeIconPath = data.characterTypeIconPath;

            attackEffect = data.attackEffect;
            defenseEndEffect = data.defenseEndEffect;
            defenseStartEffect = data.defenseStartEffect;

            this.isComputer = isComputer;
            this.gm = gm;

            InitCharacter();
        }

        public virtual void InitAnimations(ImageLoader imageLoader) {
            //Character Animator
            animator = new Animator(WindowE.instance, playerSprite);

            animator.AddAnimation(imageLoader.GetImage(characterIdleImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 512, 128), 4, 1f, 1.5f, "Idle", true, true);

            animator.AddAnimation(imageLoader.GetImage(characterIdleImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2176, 128), 17, 1f, .5f, "Death");

            //Defend effect
            defendStartSprite = new Sprite(imageLoader.GetImage(defenseStartEffect), new Rectangle(0, 0, 152, 152), playerDefendEffectPos);
            defendStartAnimEffect = new SpriteAnimation(WindowE.instance, defendStartSprite, new Rectangle(0, 0, 2432, 152), 16, 1f, 1f);
            defendEndSprite = new Sprite(imageLoader.GetImage(defenseEndEffect), new Rectangle(0, 0, 152, 152), playerDefendEffectPos);
            defendEndAnimEffect = new SpriteAnimation(WindowE.instance, defendEndSprite, new Rectangle(0, 0, 1520, 152), 10, 1f, 1f);

            animator.AddAnimation(imageLoader.GetImage(characterDamagedPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2176, 128), 17, 1f, 1f, "Damaged");
            animator.PlayAnimation("Idle");

            //Attack pos
            SpriteEnemyEffect = new Sprite(imageLoader.GetImage(attackEffect), new Rectangle(0, 0, 104, 47), enemyAttackEffectPos);
            if(isFliped)
                SpriteEnemyEffect.FlipX();
            
            attackAnimEffect = new SpriteAnimation(WindowE.instance, SpriteEnemyEffect, new Rectangle(0, 0, 832, 47), 8, 1f, .3f);

            defendEndAnimEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(defendEndSprite); };
            attackAnimEffect.onEndAnimation += delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(SpriteEnemyEffect); };
        }

        void InitCharacter()
        {
          CurrentHealth = BaseHealth;
          CurrentAttack = BaseAttack;
          CurrentDodgeRate = BaseDodgeRate;
        }
    
        public virtual void Attack(Character target, bool forceAttack = false) {
            if (!forceAttack && CurrentOperation != Operation.Attack) return;

            // Return amount of damages
            target.TakeDamage(this);


            attackAnimEffect.Play();
            gameE.AddSpriteToRender(SpriteEnemyEffect);
            
            currentActionTimeMs = (int)(attackAnimEffect.duration * 1000) + waitActionTimeOfset;
        }


        public virtual void Update(float dt) {
            animator.Update(dt);
            attackAnimEffect.Update(dt);
            defendStartAnimEffect.Update(dt);
            defendEndAnimEffect.Update(dt);
            specialSelfEffect.Update(dt);
            specialEnemyEffect?.Update(dt);
        }
    
        public virtual void TakeDamage(Character attacker)
        {
            // Return amount of damages
            Random rand = new Random();

            int damage = attacker.CurrentAttack;
            if (CurrentOperation == Operation.Defend)
            {
                if (damage > 0) 
                    damage--;
            } else {
                animator.PlayAnimation("Damaged");
            }

            RemoveHp(damage, attacker);
        }

        protected void RemoveHp(int damage, Character attacker = null) {
            CurrentHealth -= damage;
            _damageTaken = damage;

            for (int i = 0; i < damage; i++)
            {
                playerHud.LoseHp();
            }

            if (CurrentHealth <= 0)
            {
                Console.WriteLine("End");
                if(attacker == null) {
                    if(isComputer)
                        gm.GameOver(gm.player);
                    else
                        gm.GameOver(gm.computer);
                }
                else
                    gm.GameOver(attacker);
            }
        }

        public void EndActions() {
            if(CurrentOperation == Operation.Defend) {
                gameE.AddSpriteToRender(defendEndSprite);
                gameE.RemoveSpriteFromRender(defendStartSprite);
                defendEndAnimEffect.Play();
            }
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public void StartDefense() {
            if (CurrentOperation != Operation.Defend) return;

            defendStartAnimEffect.Play();
            gameE.AddSpriteToRender(defendStartSprite);

            currentActionTimeMs = (int)(defendStartAnimEffect.duration * 1000) + 1 + waitActionTimeOfset;
        }

        public virtual void UseAbility(Character optionalTarget = null)
        {
            if (CurrentOperation != Operation.Special) return;

            if (spriteSpecialSelfEffect != null) {
                gameE.AddSpriteToRender(spriteSpecialSelfEffect);
                specialSelfEffect.Play();

            }

            if (spriteSpecialEnemyEffect != null) {
                gameE.AddSpriteToRender(spriteSpecialEnemyEffect);
                specialEnemyEffect.Play();

            }

            currentActionTimeMs = (int)((specialSelfEffect?.duration + specialSelfEffect?.duration) * 1000) + 1 + waitActionTimeOfset;
        }


        public virtual int GetSpecialData() { return 0; }

        public virtual void LoadCharacter(ImageLoader imageLoader, GameE gameE, Vector2 pos, Vector2 shadowPos, bool flipX, Vector2 playerDefendEffectPos, Vector2 enemyAttackEffectPos,
            Vector2 projectilPlayerPos, Vector2 projectilEnemyPos, Vector2 heartPos, Vector2 attackPos) {

            this.gameE = gameE;
            this.isFliped = flipX;
            Sprite shadow = new Sprite(imageLoader.GetImage("shadow"), new Rectangle(0, 0, 128, 40), shadowPos);
            gameE.AddSpriteToRender(shadow);

            playerSprite = new Sprite(imageLoader.GetImage(characterIdleImgPath), new Rectangle(0, 0, 128, 128), pos);
            gameE.AddSpriteToRender(playerSprite);
            if (flipX) {
                playerSprite.FlipX();
                playerSprite.ChangeImage(playerSprite.rImage.image, new Rectangle(384, 0, 128, 128));
            }

            this.playerDefendEffectPos = playerDefendEffectPos;
            this.enemyAttackEffectPos = enemyAttackEffectPos;

            this.projectilPlayerPos = projectilPlayerPos;
            this.projectilEnemyPos = projectilEnemyPos;

            //HUD
            playerHud = new PlayerHud(heartPos, 5, BaseHealth, attackPos, 2, BaseAttack, imageLoader, gameE, !isComputer, !isComputer, this);
        }
  }
}