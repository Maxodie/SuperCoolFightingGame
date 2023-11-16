using System;

namespace SuperCoolFightingGame
{ 
    public class CharacterStats
    {
        public Type characterType;

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
        public string characterSpecialAnimPath;
        public string characterIconPath;
        public string characterTypeIconPath;

        public string attackEffect;
        public string defenseStartEffect;
        public string defenseEndEffect;

        public string Name;
        public int BaseHealth;
        public int BaseAttack;

        public string specialName;
        public string specialDescription;

        public CharacterStats(
            Type characterType,
            string name,
            int baseHealth,
            int baseAttack,

            string characterSpecialBtnImgPath,
            string characterSpecialBtnHighlightImgPath,
            string characterSpecialBtnPressedImgPath,
            string characterSpecialSelfImgPath,
            string characterSpecialSelfEffectImgPath,
            string characterSpecialProjectileImgPath,
            string characterSpecialEnemyImgPath,
            string characterIdleImgPath,
            string characterDeathImgPath,
            string characterDamagedPath,
            string characterSpecialAnimPath,
            string characterIconPath,
            string characterTypeIconPath,

            string attackEffect,
            string defenseStartEffect,
            string defenseEndEffect,

            string specialName,
            string specialDescription
            )
        {
            this.characterType = characterType;
            this.Name = name;
            this.BaseHealth = baseHealth;
            this.BaseAttack = baseAttack;

            this.characterSpecialBtnImgPath = characterSpecialBtnImgPath;
            this.characterSpecialBtnHighlightImgPath = characterSpecialBtnHighlightImgPath;
            this.characterSpecialBtnPressedImgPath = characterSpecialBtnPressedImgPath;
            this.characterSpecialSelfImgPath = characterSpecialSelfImgPath;
            this.characterSpecialSelfEffectImgPath = characterSpecialSelfEffectImgPath;
            this.characterSpecialProjectileImgPath = characterSpecialProjectileImgPath;
            this.characterSpecialEnemyImgPath = characterSpecialEnemyImgPath;
            this.characterIdleImgPath = characterIdleImgPath;
            this.characterDeathImgPath = characterDeathImgPath;
            this.characterDamagedPath = characterDamagedPath;
            this.characterSpecialAnimPath = characterSpecialAnimPath;
            this.characterIconPath = characterIconPath;
            this.characterTypeIconPath = characterTypeIconPath;

            this.attackEffect = attackEffect;
            this.defenseStartEffect = defenseStartEffect;
            this.defenseEndEffect = defenseEndEffect;

            this.specialName = specialName;
            this.specialDescription = specialDescription;
        }
    }
}