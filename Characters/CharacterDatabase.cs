using System;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SuperCoolFightingGame
{ 
  public class CharacterDatabase 
  { 
    public List<Type> CharacterTypesList = new List<Type>();
    public List<CharacterStats> CharacterStatsList = new List<CharacterStats>();

    public CharacterDatabase()
    {
      GetInheritedClasses();
      InitCharacterDatabase();
    }

    public CharacterStats GetCharacterDataWithType(Type Type) {
        for(int i=0; i < CharacterTypesList.Count; i++) {
            if (CharacterStatsList[i].characterType == Type)
                return CharacterStatsList[i];
        }

        return null;
    }
    
    public void GetInheritedClasses()
    {
      foreach (Type charaType in Assembly.GetAssembly(typeof(Character)).GetTypes()
                 .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Character))))
      {
        CharacterTypesList.Add(charaType);
      }
    }
    
    public void InitCharacterDatabase()
    {
      CharacterStats fighter = new CharacterStats(
        typeof(Fighter),
        "Vlad",
        3,
        2,
        "specialVlad",
        "specialVladH",
        "specialVladP",
        "vladSpecial",
        "vladSpecialSelf",
        "fighterProjectile",
        "vladSpecialEnemy",
        "vladIdle",
        "vladDeath",
        "vladDamaged",
        "vladSpecial",//
        "fighterIcon",//
        "attackAnimation",
        "defendAnimationStart",
        "defendAnimationEnd"
      );
      CharacterStatsList.Add(fighter);

      CharacterStats healer = new CharacterStats(
        typeof(Healer),
        "Luna",
        4, //12
        1, //1
        "specialLuna",
        "specialLunaH",
        "specialLunaP",
        "lunaSpecial",
        "lunaSpecialSelf",
        "",
        "lunaSpecialEnemy",
        "lunaIdle",
        "lunaDeath",
        "lunaDamaged",
        "lunaSpecial",
        "enchanterIcon",
        "attackAnimation",
        "defendAnimationStart",
        "defendAnimationEnd"
        ); //3
      CharacterStatsList.Add(healer);

      CharacterStats tank = new CharacterStats(
        typeof(Tank),
        "Nan",
        5, //15
        1, //1
        "specialNan",
        "specialNanH",
        "specialNanP",
        "nanSpecial",
        "nanSpecialSelf",
        "",
        "nanSpecialEnemy",
        "nanIdle",
        "nanDeath",
        "nanDamaged",
        "nanSpecial",
        "tankIcon",
        "attackAnimation",
        "defendAnimationStart",
        "defendAnimationEnd"
        ); //1
      CharacterStatsList.Add(tank);

      CharacterStats assassin = new CharacterStats(
        typeof(Assassin),
        "Guni",
        3, //8
        2, //3
        "specialGuni",
        "specialGuniH",
        "specialGuniP",
        "assassin",
        "guniSpecialSelf",
        "",
        "guniSpecialEnemy",
        "guniIdle",
        "guniDeath",
        "guniDamaged",
        "",
        "assassinIcon",
        "attackAnimation",
        "defendAnimationStart",
        "defendAnimationEnd"
        ); //3
      CharacterStatsList.Add(assassin);
      }
    }
}