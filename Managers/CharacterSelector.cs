using System;

namespace SuperCoolFightingGame
{
    public class CharacterSelector
    {
        readonly CharacterDatabase _characterDatabase = new CharacterDatabase();
        Type _temporaryCharacterType;

        public void Select(Type characterType = null)
        {
            // characterID = null for random
            Random rand = new Random();

            if (characterType == null)
            {
                int typeIndex = rand.Next(_characterDatabase.CharacterTypesList.Count);
                _temporaryCharacterType = _characterDatabase.CharacterTypesList[typeIndex];
            }
            else
                _temporaryCharacterType = characterType;
        }

        public void Confirm(out Character newCharacter)
        {
            // Set player or enemy as _temporaryCharacter
            int statIndex = _characterDatabase.CharacterStatsList
                .FindIndex(a => a.CharacterType == _temporaryCharacterType);
            
            newCharacter = (Character)Activator.CreateInstance(
                _temporaryCharacterType,
                _characterDatabase.CharacterStatsList[statIndex]);
        }
    }
}