using GameEn;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
  public class Character
  {
        #region InstanceVariables

        protected GameE gameE;
        protected GameManager gm;

        public PlayerHud playerHud { get; protected set; }

        public Sprite playerSprite;
        Sprite shadow;
        bool isFliped;
        public int waitActionTimeOffset = 500;
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
        public string characterSpecialProjectileImgPath;
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

        WindowShaker windowDamageShaker;

        public Animator animator { get; private set; }
        public int currentActionTimeMs = 0;

        //Sounds
        AudioListener attackSound;
        AudioListener defendStartSound;
        AudioListener defendEndSound;
        protected AudioListener specialSound;

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
            characterSpecialProjectileImgPath = data.characterSpecialProjectileImgPath;
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

            this.windowDamageShaker = new WindowShaker(WindowE.instance);
            InitCharacter();
        }

        public virtual void InitAnimations(ImageLoader imageLoader) {
            this.attackSound = new AudioListener(false, "Media/sounds/SFX/Attack.wav");
            this.defendStartSound = new AudioListener(false, "Media/sounds/SFX/DefendStart.wav");
            this.defendEndSound = new AudioListener(false, "Media/sounds/SFX/DefendEnd.wav");

            //Character Animator
            animator = new Animator(WindowE.instance, playerSprite);

            animator.AddAnimation(imageLoader.GetImage(characterIdleImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 512, 128), 4, 1f, 1.5f, "Idle", true, true);

            animator.AddAnimation(imageLoader.GetImage(characterDeathImgPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2176, 128), 17, 1f, 1f, "Death", false, false, false);

            animator.AddOnEndAnimation(delegate (object sender, EventArgs e) { gameE.RemoveSpriteFromRender(shadow); }, "Death");

            //Defend effect
            defendStartSprite = new Sprite(imageLoader.GetImage(defenseStartEffect), new Rectangle(0, 0, 152, 152), playerDefendEffectPos);
            if (!isFliped)
                defendStartSprite.FlipX();
            defendStartAnimEffect = new SpriteAnimation(WindowE.instance, defendStartSprite, new Rectangle(0, 0, 2432, 152), 16, 1f, 1f);
            defendEndSprite = new Sprite(imageLoader.GetImage(defenseEndEffect), new Rectangle(0, 0, 152, 152), playerDefendEffectPos);
            if (!isFliped)
                defendEndSprite.FlipX();
            defendEndAnimEffect = new SpriteAnimation(WindowE.instance, defendEndSprite, new Rectangle(0, 0, 1520, 152), 10, 1f, 1f);

            animator.AddAnimation(imageLoader.GetImage(characterDamagedPath), new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, 2176, 128), 17, 1f, 0.5f, "Damaged");
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
    
        public virtual void Attack(Character target, bool forceAttack = false, bool doAnimation = true) {
            if (!forceAttack && CurrentOperation != Operation.Attack) return;

            // Return amount of damages
            int damage = target.TakeDamage(this);
            gm.UpdateTextInfos($"{Name} uses Attack! {target.Name} \nloses {damage}HP.");

            if (doAnimation) {
                attackAnimEffect.Play();
                gameE.AddSpriteToRender(SpriteEnemyEffect);
                attackSound.Play();
            }
            
            currentActionTimeMs = (int)(attackAnimEffect.duration * 1000) + waitActionTimeOffset;
        }


        public virtual void Update(float dt) {
            animator.Update(dt);
            attackAnimEffect.Update(dt);
            defendStartAnimEffect.Update(dt);
            defendEndAnimEffect.Update(dt);
            specialSelfEffect.Update(dt);
            specialEnemyEffect?.Update(dt);

            windowDamageShaker.Update(dt);

            playerHud.Update(dt);
        }
    
        public virtual int TakeDamage(Character attacker)
        {
            // Return amount of damages
 
            int damage = attacker.CurrentAttack;
            if (CurrentOperation == Operation.Defend)
            {
                if (damage > 0)
                    damage--;
            }

            if (damage > 0) {
                animator.PlayAnimation("Damaged");
                RemoveHp(damage, attacker);
                windowDamageShaker.cameraShaker(0.2f, 10);
            }

            return damage;
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
                if(attacker == null) {
                    if(isComputer)
                        Death(gm.player);
                    else
                        Death(gm.computer);
                }
                else
                    Death(attacker);
            }
        }

        void Death(Character winner) {
            EndActions();
            animator.PlayAnimation("Death");
            gm.GameOver(winner);
        }

        public void EndActions() {
            if(CurrentOperation == Operation.Defend) {
                gameE.AddSpriteToRender(defendEndSprite);
                gameE.RemoveSpriteFromRender(defendStartSprite);
                defendEndAnimEffect.Play();
                defendEndSound.Play();
                currentActionTimeMs = (int)(defendEndAnimEffect.duration * 1000) + waitActionTimeOffset;
            }
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public void StartDefense() {
            if (CurrentOperation != Operation.Defend) return;

            gm.UpdateTextInfos($"{Name} uses Defend!");

            defendStartAnimEffect.Play();
            gameE.AddSpriteToRender(defendStartSprite);
            defendStartSound.Play();

            currentActionTimeMs = (int)(defendStartAnimEffect.duration * 1000) + 1 + waitActionTimeOffset;
        }

        public virtual void StartAbility() {
            //if (CurrentOperation != Operation.Special) return;
        }

        public virtual void UseAbility(Character optionalTarget = null, bool playSelfEffect = true, bool playEnemyEffect = true)
        {
            if (CurrentOperation != Operation.Special) return;

            if (playSelfEffect && spriteSpecialSelfEffect != null) {
                gameE.AddSpriteToRender(spriteSpecialSelfEffect);
                specialSelfEffect.Play();
                currentActionTimeMs += (int)(specialSelfEffect?.duration * 1000);
                specialSound.Play();
            }

            if (playEnemyEffect && spriteSpecialEnemyEffect != null) {
                gameE.AddSpriteToRender(spriteSpecialEnemyEffect);
                specialEnemyEffect.Play();
                currentActionTimeMs += (int)(specialEnemyEffect?.duration * 1000);
            }

            currentActionTimeMs += waitActionTimeOffset;
        }


        public virtual int GetSpecialData() { return 0; }

        public virtual void LoadCharacter(ImageLoader imageLoader, GameE gameE, Vector2 pos, Vector2 shadowPos, bool flipX, Vector2 playerDefendEffectPos, Vector2 enemyAttackEffectPos,
            Vector2 projectilePlayerPos, Vector2 projectileEnemyPos, Vector2 heartPos, Vector2 attackPos) {

            this.gameE = gameE;
            this.isFliped = flipX;
            shadow = new Sprite(imageLoader.GetImage("shadow"), new Rectangle(0, 0, 128, 40), shadowPos);
            gameE.AddSpriteToRender(shadow);

            playerSprite = new Sprite(imageLoader.GetImage(characterIdleImgPath), new Rectangle(0, 0, 128, 128), pos);
            gameE.AddSpriteToRender(playerSprite);
            if (flipX) {
                playerSprite.FlipX();
                playerSprite.ChangeImage(playerSprite.rImage.image, new Rectangle(384, 0, 128, 128));
            }

            this.playerDefendEffectPos = playerDefendEffectPos;
            this.enemyAttackEffectPos = enemyAttackEffectPos;

            this.projectilPlayerPos = projectilePlayerPos;
            this.projectilEnemyPos = projectileEnemyPos;

            //HUD
            playerHud = new PlayerHud(heartPos, 5, BaseHealth, attackPos, 2, BaseAttack, imageLoader, gameE, !isComputer, !isComputer, this);
        }
  }
}