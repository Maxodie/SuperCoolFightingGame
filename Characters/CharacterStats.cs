using System;

namespace SuperCoolFightingGame
{ 
    public class CharacterStats
    {
        public Type CharacterType;

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

        public string Name;
        public int BaseHealth;
        public int BaseAttack;
        public int BaseCritRate;
        public int BaseDodgeRate;
        public int BaseEnergy;
        public int MaxEnergy;

        public CharacterStats(
            Type characterType,
            string name,
            int baseHealth,
            int baseAttack,
            int baseCritRate,
            int baseDodgeRate,
            int baseEnergy,
            int maxEnergy,

            string CharacterSpecialBtnImgPath,
            string CharacterSpecialBtnHighlightImgPath,
            string CharacterSpecialBtnPressedImgPath,
            string CharacterSpecialSelfImgPath,
            string CharacterSpecialSelfEffectImgPath,
            string CharacterSpecialProjectilImgPath,
            string CharacterSpecialEnemyImgPath,
            string CharacterIdleImgPath,
            string CharacterDeathImgPath,
            string CharacterDamagedPath,

            string attackEffect,
            string defenseEffect
            )
        {
            this.CharacterType = characterType;
            this.Name = name;
            this.BaseHealth = baseHealth;
            this.BaseAttack = baseAttack;
            this.BaseCritRate = baseCritRate;
            this.BaseDodgeRate = baseDodgeRate;
            this.BaseEnergy = baseEnergy;
            this.MaxEnergy = maxEnergy;

            this.CharacterSpecialBtnImgPath = CharacterSpecialBtnImgPath;
            this.CharacterSpecialBtnHighlightImgPath = CharacterSpecialBtnHighlightImgPath;
            this.CharacterSpecialBtnPressedImgPath = CharacterSpecialBtnPressedImgPath;
            this.CharacterSpecialSelfImgPath = CharacterSpecialSelfImgPath;
            this.CharacterSpecialSelfEffectImgPath = CharacterSpecialSelfEffectImgPath;
            this.CharacterSpecialProjectilImgPath = CharacterSpecialProjectilImgPath;
            this.CharacterSpecialEnemyImgPath = CharacterSpecialEnemyImgPath;
            this.CharacterIdleImgPath = CharacterIdleImgPath;
            this.CharacterDeathImgPath = CharacterDeathImgPath;
            this.CharacterDamagedPath = CharacterDamagedPath;

            this.attackEffect = attackEffect;
            this.defenseEffect = defenseEffect;
        }
    }
}