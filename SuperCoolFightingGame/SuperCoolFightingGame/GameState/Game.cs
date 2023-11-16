using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SuperCoolFightingGame
{
  
  #region GameManagers

  // -- GAME MANAGERS -- //
  internal class Game
  {
    static int _roundNumber;
    static Difficulty _iaDifficulty;
    static Character _player, _enemy;
    static CharacterDatabase _characterDatabase = new CharacterDatabase();
      
    static void Main(string[] args)
    {
      // ----- INIT ----- //
      _iaDifficulty = ChooseDifficulty();
      _player = ChooseCharacter();
      _enemy = ChooseCharacter();
      
      // ----- GAME ----- //
      while (_player.IsAlive() && _enemy.IsAlive())
      {
        StartNewRound();

        _player.CurrentOperation = PlayerChooseOperation();
        _enemy.CurrentOperation = IAChooseOperation();

        MakeActions();
      }
      
      // ----- END GAME ----- //
      Console.WriteLine("\n----- [ FINI ] -----");
      if (!_player.IsAlive() && !_enemy.IsAlive()) Console.WriteLine("TOUS LES JOUEURS SONT MORTS");
      else if (!_player.IsAlive()) Console.WriteLine($"[JOUEUR] {_player.Name} est mort !\n--- TU AS PERDU ---");
      else if (!_enemy.IsAlive()) Console.WriteLine($"[IA] {_enemy.Name} ennemi est mort !\n--- TU AS GAGNÉ ---");
    }

    static Difficulty ChooseDifficulty()
    {
      string choice;

      while (true)
      {
        Console.WriteLine("Choisissez le niveau de difficulté");
        
        for (int i = 0; i < Enum.GetValues(typeof(Difficulty)).Length; i++)
        {
          Console.WriteLine($"[ {i} ] {Enum.GetName(typeof(Difficulty),i)}");
        }
        
        choice = Console.ReadLine();
        if (Int32.TryParse(choice, out int choiceInt) && choiceInt <= Enum.GetValues(typeof(Difficulty)).Length - 1)
        {
          return (Difficulty)choiceInt;
        }
        
        Console.WriteLine($"Erreur : entrez un nombre entre 0 et {Enum.GetValues(typeof(Difficulty)).Length - 1} (compris)");
      }
    }

    static Character ChooseCharacter()
    {
      string choice;

      while (true)
      {
        Random rand = new Random();
        
        if (_player == null)
          Console.WriteLine("\nChoisissez votre personnage !");
        else
          Console.WriteLine("\nChoisissez le personnage adverse !");

        Console.WriteLine($"[ 0 ] Random");
        for (int i = 0; i < _characterDatabase._characterTypesList.Count(); i++)
        {
          Console.WriteLine($"[ {i+1} ] {_characterDatabase._characterStatsList[i].Name} ({_characterDatabase._characterTypesList[i].Name} | {_characterDatabase._characterStatsList[i].BaseAttack} atq | {_characterDatabase._characterStatsList[i].BaseHealth} pv)");
        }
        
        choice = Console.ReadLine();
        if (Int32.TryParse(choice, out int choiceInt) && choiceInt-1 <= _characterDatabase._characterTypesList.Count() - 1 && choiceInt-1 >= 0)
          return (Character)Activator.CreateInstance(_characterDatabase._characterTypesList[choiceInt-1], _characterDatabase._characterStatsList[choiceInt-1]);
        if (choiceInt - 1 == -1)
        {
          int randomChoice = rand.Next(_characterDatabase._characterTypesList.Count());
          return (Character)Activator.CreateInstance(_characterDatabase._characterTypesList[randomChoice], _characterDatabase._characterStatsList[randomChoice]);
        }
        
        Console.WriteLine($"Erreur : entrez un nombre entre 0 et {_characterDatabase._characterTypesList.Count() - 1} (compris)");
      }
    }

    static Operation PlayerChooseOperation()
    {
      string choice;

      while (true)
      {
        Console.WriteLine("\nChoisissez votre prochaine action !");

        for (int i = 0; i < Enum.GetValues(typeof(Operation)).Length; i++)
        {
          Console.WriteLine($"[ {i} ] {Enum.GetName(typeof(Operation), i)}");
        }
        
        choice = Console.ReadLine();
        if (Int32.TryParse(choice, out int choiceInt) && choiceInt <= Enum.GetValues(typeof(Operation)).Length - 1)
        {
          return (Operation)choiceInt;
        }
        
        Console.WriteLine($"Erreur : entrez un nombre entre 0 et {Enum.GetValues(typeof(Operation)).Length - 1} (compris)");
      }
    }

    static Operation IAChooseOperation()
    {
      Random rand = new Random();
      List<Operation> possibleOperation = new List<Operation>();
      foreach (var operation in Enum.GetValues(typeof(Operation)))
      {
        possibleOperation.Add((Operation)operation);
      }

      switch (_iaDifficulty)
      {
        case Difficulty.Easy:
          break;
        case Difficulty.Medium:
          switch (_enemy)
          {
            case Fighter fighter:
              if (_player.CurrentAttack < _enemy.CurrentAttack) 
                possibleOperation.Remove(Operation.Special);
              break;
            case Healer healer:
              if (_enemy.CurrentHealth <= _enemy.BaseHealth / 2)
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Special, 3));
              break;
            case Tank tank:
              if (_enemy.CurrentHealth <= _enemy.BaseHealth / 3)
                possibleOperation.Remove(Operation.Special);
              break;
            case Assassin assassin:
              if (_enemy.GetSpecialData() == 1) // Special attack is ready
              {
                possibleOperation.Remove(Operation.Special);
                possibleOperation.Add(Operation.Attack);
              }
              else
                possibleOperation.Add(Operation.Special);
              break;
          }
          break;
        case Difficulty.Hard:
          switch (_enemy)
          {
            case Fighter fighter:
              if (_player.CurrentAttack < _enemy.CurrentAttack || _player.CurrentOperation == Operation.Defend) 
                possibleOperation.Remove(Operation.Special);
              else if (_player.CurrentOperation == Operation.Attack)
              {
                possibleOperation.Remove(Operation.Attack);
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Special, 7));
              }
              break;
            case Healer healer:
              if (_enemy.CurrentHealth <= _enemy.BaseHealth / 2 || _player.CurrentOperation == Operation.Defend)
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Special, 5));
              if (_player.CurrentOperation == Operation.Attack)
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Attack, 3));
              else if (_player.GetType() == typeof(Fighter) && _player.CurrentOperation == Operation.Special)
                possibleOperation.Remove(Operation.Attack);
              break;
            case Tank tank:
              if (_enemy.CurrentHealth <= _enemy.BaseHealth / 3)
                possibleOperation.Remove(Operation.Special);
              else if (_player.CurrentOperation == Operation.Defend)
                possibleOperation.Remove(Operation.Special);
              else if (_player.CurrentOperation == Operation.Attack)
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Defend, 2));
              break;
            case Assassin assassin:
              if (_enemy.GetSpecialData() == 1 && _player.CurrentOperation == Operation.Defend) // Special attack is ready
              {
                possibleOperation.Remove(Operation.Special);
                possibleOperation.Remove(Operation.Defend);
                possibleOperation.Add(Operation.Attack);
              }
              else if (_player.CurrentOperation == Operation.Defend)
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Attack, 3));
              else
                possibleOperation.AddRange(Enumerable.Repeat(Operation.Defend, 5));
              break;
          }
          break;
        default:
          Console.WriteLine("Une erreur est survenue lors du choix de l'IA. Le niveau n'est pas selectionné.");
          break;
      }

      if (!possibleOperation.Any())
        Console.WriteLine("Une erreur est survenue lors du choix de l'IA. La liste des possibilité est vide");
      
      return possibleOperation[rand.Next(possibleOperation.Count - 1)];
    }
    
    static void MakeActions()
    {
      // Every players is defending
      if (_player.CurrentOperation == Operation.Defend && _enemy.CurrentOperation == Operation.Defend)
      {
        Console.WriteLine($"{_player.Name} et {_enemy.Name} se défendent. Il ne se passe rien.");
        return;
      }
      
      // Normal attack
      if (_player.CurrentOperation == Operation.Attack)
        Console.WriteLine($"{_player.Name} attaque et inflige {_player.Attack(_enemy)} pt de dégat !");
      if (_enemy.CurrentOperation == Operation.Attack)
        Console.WriteLine($"{_enemy.Name} attaque et inflige {_enemy.Attack(_player)} pt de dégat !");
      
      // Abilities
      /* Tank */
      if (_player.CurrentOperation == Operation.Special && _player.GetType() == typeof(Tank)) _player.UseAbility(_enemy);
      if (_enemy.CurrentOperation == Operation.Special && _enemy.GetType() == typeof(Tank)) _enemy.UseAbility(_player);
      
      /* Fighter */
      if (_player.CurrentOperation == Operation.Special && _player.GetType() == typeof(Fighter)) _player.UseAbility(_enemy);
      if (_enemy.CurrentOperation == Operation.Special && _enemy.GetType() == typeof(Fighter)) _enemy.UseAbility(_player);
      
      /* Enchanter */
      if (_player.CurrentOperation == Operation.Special && _player.GetType() == typeof(Healer)) _player.UseAbility(_enemy);
      if (_enemy.CurrentOperation == Operation.Special && _enemy.GetType() == typeof(Healer)) _enemy.UseAbility(_player);
      
      /* Assassin */
      if (_player.CurrentOperation == Operation.Special && _player.GetType() == typeof(Assassin)) _player.UseAbility(_enemy);
      if (_enemy.CurrentOperation == Operation.Special && _enemy.GetType() == typeof(Assassin)) _enemy.UseAbility(_player);
    }

    static void StartNewRound()
    {
      _roundNumber++;

      if (_roundNumber != 1)
      {
        _player.NewRound();
        _enemy.NewRound(); 
      }
      
      Console.WriteLine($"\n----- [ Round {_roundNumber} ] -----");
      Console.WriteLine($"[JOUEUR] {_player.Name} : {_player.CurrentHealth} hp ({_player.CurrentAttack} atk)");
      Console.WriteLine($"[IA] {_enemy.Name} : {_enemy.CurrentHealth} hp ({_enemy.CurrentAttack} atk)");
    }
  }

  #endregion

  #region CharacterManagers

  // -- CHARACTER MANAGERS -- //
  public class Character
  {
    #region InstanceVariables
    
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

    public Operation CurrentOperation;

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
      
      InitCharacter();
    }
    
    public void InitCharacter()
    {
      CurrentHealth = BaseHealth;
      CurrentAttack = BaseAttack;
      CurrentCritRate = BaseCritRate;
      CurrentDodgeRate = BaseDodgeRate;
      CurrentEnergy = BaseEnergy;
    }
    
    public virtual int Attack(Character target)
    {
      // Return amount of damages
      return target.TakeDamage(this);
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
        Console.WriteLine($"{Name} se défend et perd {damage} pv !");
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
  }

  public class CharacterStats
  {
    public string Name;
    public int BaseHealth;
    public int BaseAttack;
    public int BaseCritRate;
    public int BaseDodgeRate;
    public int BaseEnergy;
    public int MaxEnergy;

    public CharacterStats(
      string Name,
      int BaseHealth,
      int BaseAttack,
      int BaseCritRate,
      int BaseDodgeRate,
      int BaseEnergy,
      int MaxEnergy)
    {
      this.Name = Name;
      this.BaseHealth = BaseHealth;
      this.BaseAttack = BaseAttack;
      this.BaseCritRate = BaseCritRate;
      this.BaseDodgeRate = BaseDodgeRate;
      this.BaseEnergy = BaseEnergy;
      this.MaxEnergy = MaxEnergy;
    }
  }

  public class CharacterDatabase
  {
    public List<Type> _characterTypesList = new List<Type>();
    public List<CharacterStats> _characterStatsList = new List<CharacterStats>();

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
        _characterTypesList.Add(charaType);
      }
    }
    
    public void InitCharacterDatabase()
    {
      // Please keep the order !
      
      CharacterStats fighter = new CharacterStats(
        "Vlad",
        3,
        2,
        1,
        1,
        0,
        0);
      _characterStatsList.Add(fighter);

      CharacterStats healer = new CharacterStats(
        "Luna",
        4, //12
        1, //1
        1,
        1, //10
        0,
        0); //3
      _characterStatsList.Add(healer);

      CharacterStats tank = new CharacterStats(
        "Nan",
        5, //15
        1, //1
        1,
        1, //20
        0,
        0); //1
      _characterStatsList.Add(tank);

      CharacterStats assassin = new CharacterStats(
        "Guni",
        3, //8
        2, //3
        1, //20
        1, //10
        0,
        0); //3
      _characterStatsList.Add(assassin);
    }
  }

  #endregion

  #region Characters
  
  // -- CHARACTERS -- // Please keep the order

  class Fighter : Character
  {
    int _damageTaken = 0;

    public Fighter(CharacterStats data) : base(data) { }

    public override int TakeDamage(Character attacker)
    {
      _damageTaken += base.TakeDamage(attacker);
      return _damageTaken;
    }
    
    public override void UseAbility(Character optionalTarget = null)
    {
      base.UseAbility();
      
      if (optionalTarget == null)
        Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
      else
      {
        CurrentAttack = _damageTaken;
        Console.WriteLine($"{Name} utilise sa capacité ! Il retourne les dégats reçus et inflige donc {Attack(optionalTarget)} pt de dégats à {optionalTarget.Name}.");
        CurrentAttack = BaseAttack;
      }
    }

    public override void NewRound()
    {
      base.NewRound();
      _damageTaken = 0;
    }
  }
  
  class Healer : Character
  {
    int _healValue = 2;
    
    public Healer(CharacterStats data) : base(data) { }
    
    public override void UseAbility(Character optionalTarget = null)
    {
      base.UseAbility();

      Console.WriteLine($"{Name} utilise sa capacité ! Elle se soigne de {_healValue} pv. ({BaseHealth} pv max)");
      CurrentHealth += _healValue;
      if (CurrentHealth > BaseHealth) CurrentHealth = BaseHealth;
    }
  }
  
  class Tank : Character
  {
    public Tank(CharacterStats data) : base(data) { }
    
    public override void UseAbility(Character optionalTarget = null)
    {
      base.UseAbility();
      
      if (optionalTarget == null)
        Console.WriteLine($"Erreur : {Name} a besoin d'une cible pour utiliser sa capacité.");
      else
      {
        CurrentAttack++;
        Console.WriteLine($"{Name} utilise sa capacité ! Il inflige {Attack(optionalTarget)} pt de dégats à {optionalTarget.Name} et perd 1 pt de vie.");
        CurrentAttack--;
        CurrentHealth--;
      }
    }
  }
  
  class Assassin : Character
  {
    bool _abilityCharged;
      
    public Assassin(CharacterStats data) : base(data) { }

    public override int Attack(Character target)
    {
      if (_abilityCharged && target.CurrentOperation == Operation.Defend) CurrentAttack++;
      int dmg = base.Attack(target);
      if (_abilityCharged && target.CurrentOperation == Operation.Defend) CurrentAttack--;
      return dmg;
    }

    public override void UseAbility(Character optionalTarget = null)
    {
      base.UseAbility();
      Console.WriteLine($"{Name} utilise sa capcité ! Sa prochaine attaque pourra transpercer la défense adverse.");
      _abilityCharged = true;
    }

    public override int GetSpecialData()
    {
      return Convert.ToInt32(_abilityCharged);
    }
  }

  #endregion
  
  #region Enums
  
  // -- ENUMS -- //
  
  public enum Operation
  {
    Attack,
    Defend,
    Special
  }

  public enum Difficulty
  {
    Easy,
    Medium,
    Hard
  }
  
  #endregion
}