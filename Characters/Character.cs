using GameEn;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
  public class Character
  {
        #region InstanceVariables

        public Sprite playerSprite;

        Sprite SpriteEnemyEffect;
        Sprite SpriteSelfEffect;

        public string Name;
        public int BaseHealth;
        public int CurrentHealth;
        public int BaseAttack;
        public int CurrentAttack;
        public int BaseCritRate;
        public int CurrentCritRate;
        public int BaseDodgeRate;
        public int CurrentDodgeRate;
        public int BaseEnergy;
        public int CurrentEnergy;
        public int MaxEnergy;

        public string CharacterSpecialBtnImgPath;
        public string CharacterSpecialBtnHighlightImgPath;
        public string CharacterSpecialBtnPressedImgPath;
        public string CharacterSpecialSelfImgPath;
        public string CharacterSpecialSelfEffectImgPath;
        public string CharacterSpecialProjectilImgPath;
        public string CharacterSpecialEnemyImgPath;
        public string CharacterIdleImgPath;
        public string CharacterDeathImgPath;
        public string CharacterDamagedPath;

        public string attackEffect;
        public string defenseEffect;

        public Operation CurrentOperation;

        protected SpriteAnimation attackAnimEffect;
        protected SpriteAnimation defendAnimEffect;

        protected Animator animator;

        #endregion

        public Character(CharacterStats data)
        {
            Name = data.Name;
            BaseHealth = data.BaseHealth;
            BaseAttack = data.BaseAttack;
            BaseCritRate = data.BaseCritRate;
            BaseDodgeRate = data.BaseDodgeRate;
            BaseEnergy = data.BaseEnergy;
            MaxEnergy = data.MaxEnergy;

            CharacterSpecialBtnImgPath = data.CharacterSpecialBtnImgPath;
            CharacterSpecialBtnHighlightImgPath = data.CharacterSpecialBtnHighlightImgPath;
            CharacterSpecialBtnPressedImgPath = data.CharacterSpecialBtnPressedImgPath;
            CharacterSpecialSelfImgPath = data.CharacterSpecialSelfImgPath;
            CharacterSpecialSelfEffectImgPath = data.CharacterSpecialSelfEffectImgPath;
            CharacterSpecialProjectilImgPath = data.CharacterSpecialProjectilImgPath;
            CharacterSpecialEnemyImgPath = data.CharacterSpecialEnemyImgPath;
            CharacterIdleImgPath = data.CharacterIdleImgPath;
            CharacterDeathImgPath = data.CharacterDeathImgPath;
            CharacterDamagedPath = data.CharacterDamagedPath;

            attackEffect = data.attackEffect;
            defenseEffect = data.defenseEffect;

            InitCharacter();
        }

        public void InitAnimations(ImageLoader imageLoader) {
            animator = new Animator();

            SpriteAnimation characterAnim = new SpriteAnimation(WindowE.instance, playerSprite, new Rectangle(0, 0, 512, 128), 4, 1f, 1.5f, true);
            animator.AddAnimation(characterAnim, imageLoader.GetImage(CharacterIdleImgPath), new Rectangle(0, 0, 128, 128), "Idle");

            characterAnim = new SpriteAnimation(WindowE.instance, playerSprite, new Rectangle(0, 0, 2176, 128), 17, 1f, .5f, false);
            animator.AddAnimation(characterAnim, imageLoader.GetImage(CharacterIdleImgPath), new Rectangle(0, 0, 128, 128), "Death");

            characterAnim = new SpriteAnimation(WindowE.instance, playerSprite, new Rectangle(0, 0, 2176, 128), 17, 1f, .5f, false);
            animator.AddAnimation(characterAnim, imageLoader.GetImage(CharacterDamagedPath), new Rectangle(0, 0, 128, 128), "Damaged");

            SpriteEnemyEffect = new Sprite(imageLoader.GetImage(attackEffect), new Rectangle(0, 0, 104, 48), new Vector2(0, 0));

            animator.PlayAnimation("Idle");
        }

        public void InitCharacter()
        {
          CurrentHealth = BaseHealth;
          CurrentAttack = BaseAttack;
          CurrentCritRate = BaseCritRate;
          CurrentDodgeRate = BaseDodgeRate;
          CurrentEnergy = BaseEnergy;
        }
    
        public virtual void Attack(Character target) {
            // Return amount of damages
            target.TakeDamage(this);

            attackAnimEffect.Play();
        }

        public virtual void Update(float dt) {
            animator.Update(dt);
        }

        public void AttackEffectAnim() {
            attackAnimEffect = new SpriteAnimation(WindowE.instance, SpriteSelfEffect, new Rectangle(0, 0, 832, 48), 8, 1f, .5f);
        }
    
        public virtual int TakeDamage(Character attacker)
        {
              // Return amount of damages
              Random rand = new Random();
              if (rand.Next(1, 100 / CurrentDodgeRate) == 1)
                return 0;

              int damage = attacker.CurrentAttack;
              if (rand.Next(1, 100 / attacker.CurrentCritRate) == 1)
                damage *= 2;
              if (CurrentOperation == Operation.Defend)
              {
                if (damage > 0) damage--;
                Console.WriteLine($"{Name} se défend pv !");
              }
      
              CurrentHealth -= damage;
              return damage;
        }

        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public bool IsAbilityReady()
        {
            return CurrentEnergy == MaxEnergy;
        }

        public virtual void UseAbility(Character optionalTarget = null)
        {
              if (!IsAbilityReady())
                Console.WriteLine($"Erreur : Pas assez d'énergie. Check d'abord IsAbilityReady() avant d'utiliser UseAbility() (maxNrg : {MaxEnergy}, curNrg : {CurrentEnergy})");
      
              CurrentEnergy = 0;
        }

        public void AddEnergy(int value)
        {
              CurrentEnergy += value;
              if (CurrentEnergy > MaxEnergy) CurrentEnergy = MaxEnergy;
        }

        public virtual void NewRound()
        {
          AddEnergy(1);
        }

        public virtual int GetSpecialData() { return 0; }

        public void LoadGFX(ImageLoader imageLoader, GameE gameE, Vector2 pos, Vector2 shadowPos, bool flipX) {
            Sprite shadow = new Sprite(imageLoader.GetImage("shadow"), new Rectangle(0, 0, 128, 40), shadowPos);
            gameE.AddSpriteToRender(shadow);

            playerSprite = new Sprite(imageLoader.GetImage(CharacterIdleImgPath), new Rectangle(0, 0, 128, 128), pos);
            gameE.AddSpriteToRender(playerSprite);
            if (flipX) {
                playerSprite.FlipX();
                playerSprite.ChangeImage(playerSprite.rImage.image, new Rectangle(384, 0, 128, 128));
            }
            
        }
  }
}