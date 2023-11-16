using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperCoolFightingGame
{
  public class OperationSelector
  { 
        Difficulty _difficulty;

        public OperationSelector(Difficulty difficulty) {
            SetDifficulty(difficulty);
        }

        public void SetDifficulty(Difficulty newDifficulty)
        {
            _difficulty = newDifficulty;
        }
    
        public void PlayerSelection(Operation newOperation, Character player)
        {
            player.CurrentOperation = newOperation;
        }

        public void ComputerSelection(Character computer, Character player)
        {
          Random rand = new Random();
          List<Operation> possibleOperation = new List<Operation>();
          foreach (var operation in Enum.GetValues(typeof(Operation)))
          {
            possibleOperation.Add((Operation)operation);
          }

          switch (_difficulty)
          {
            case Difficulty.Easy:
              break;
            case Difficulty.Medium:
              switch (computer)
              {
                case Fighter fighter:
                  if (player.CurrentAttack < computer.CurrentAttack)
                    possibleOperation.Remove(Operation.Special);
                  break;
                case Healer healer:
                  if (computer.CurrentHealth <= computer.BaseHealth / 2)
                    possibleOperation.AddRange(Enumerable.Repeat(Operation.Special, 3));
                  break;
                case Tank tank:
                  if (computer.CurrentHealth <= computer.BaseHealth / 3)
                    possibleOperation.Remove(Operation.Special);
                  break;
                case Assassin assassin:
                  if (computer.GetSpecialData() == 1) // Special attack is ready
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
              switch (computer)
              {
                case Fighter fighter:
                  if (player.CurrentAttack < computer.CurrentAttack || player.CurrentOperation == Operation.Defend)
                    possibleOperation.Remove(Operation.Special);
                  else if (player.CurrentOperation == Operation.Attack)
                  {
                    possibleOperation.Remove(Operation.Attack);
                    possibleOperation.AddRange(Enumerable.Repeat(Operation.Special, 7));
                  }

                  break;
                case Healer healer:
                  if (computer.CurrentHealth <= computer.BaseHealth / 2 || player.CurrentOperation == Operation.Defend)
                    possibleOperation.AddRange(Enumerable.Repeat(Operation.Special, 5));
                  if (player.CurrentOperation == Operation.Attack)
                    possibleOperation.AddRange(Enumerable.Repeat(Operation.Attack, 3));
                  else if (player.GetType() == typeof(Fighter) && player.CurrentOperation == Operation.Special)
                    possibleOperation.Remove(Operation.Attack);
                  break;
                case Tank tank:
                  if (computer.CurrentHealth <= computer.BaseHealth / 3)
                    possibleOperation.Remove(Operation.Special);
                  else if (player.CurrentOperation == Operation.Defend)
                    possibleOperation.Remove(Operation.Special);
                  else if (player.CurrentOperation == Operation.Attack)
                    possibleOperation.AddRange(Enumerable.Repeat(Operation.Defend, 2));
                  break;
                case Assassin assassin:
                  if (computer.GetSpecialData() == 1 &&
                      player.CurrentOperation == Operation.Defend) // Special attack is ready
                  {
                    possibleOperation.Remove(Operation.Special);
                    possibleOperation.Remove(Operation.Defend);
                    possibleOperation.Add(Operation.Attack);
                  }
                  else if (player.CurrentOperation == Operation.Defend)
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

          computer.CurrentOperation = possibleOperation[rand.Next(possibleOperation.Count - 1)];
        }
      }
}