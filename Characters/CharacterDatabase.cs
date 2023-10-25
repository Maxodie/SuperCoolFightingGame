using System;
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
        1,
        1,
        0,
        0,
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
        "attackAnimation",
        "defendAnimationEnd"
      );
      CharacterStatsList.Add(fighter);

      CharacterStats healer = new CharacterStats(
        typeof(Healer),
        "Luna",
        4, //12
        1, //1
        1,
        1, //10
        0,
        0,
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
        "attackAnimation",
        "defendAnimationEnd"
        ); //3
      CharacterStatsList.Add(healer);

      CharacterStats tank = new CharacterStats(
        typeof(Tank),
        "Nan",
        5, //15
        1, //1
        1,
        1, //20
        0,
        0,
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
        "attackAnimation",
        "defendAnimationEnd"
        ); //1
      CharacterStatsList.Add(tank);

      CharacterStats assassin = new CharacterStats(
        typeof(Assassin),
        "Guni",
        3, //8
        2, //3
        1, //20
        1, //10
        0,
        0,
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
        "attackAnimation",
        "defendAnimationEnd"
        ); //3
      CharacterStatsList.Add(assassin);
      }
    }
}