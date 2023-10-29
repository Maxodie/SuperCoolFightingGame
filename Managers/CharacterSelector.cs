using System;

namespace SuperCoolFightingGame
{
    public class CharacterSelector
    {
        public readonly CharacterDatabase characterDatabase = new CharacterDatabase();
        Type _temporaryCharacterType;

        public void Select(Type characterType = null)
        {
            // characterID = null for random
            Random rand = new Random();

            if (characterType == null)
            {
                int typeIndex = rand.Next(characterDatabase.CharacterTypesList.Count);
                _temporaryCharacterType = characterDatabase.CharacterTypesList[typeIndex];
            }
            else
                _temporaryCharacterType = characterType;
        }

        public void Confirm(out Character newCharacter, bool isComputer, GameManager gm)
        {
            // Set player or enemy as _temporaryCharacter
            int statIndex = characterDatabase.CharacterStatsList
                .FindIndex(a => a.characterType == _temporaryCharacterType);
            
            newCharacter = (Character)Activator.CreateInstance(
                _temporaryCharacterType,
                characterDatabase.CharacterStatsList[statIndex], isComputer, gm);
        }
    }
}