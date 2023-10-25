using System;

namespace SuperCoolFightingGame
{
    public class GameManager
    {

        public int RoundNumber;
        public Difficulty Difficulty;
        public Character computer, player;

        public GameManager(Character player, Character computer)
        {
            this.player = player;
            this.computer = computer;
        }

        public void Init()
        {
            StartNewRound();
        }

        public void MakeActions()
        {
            // Every players is defending
            if (player.CurrentOperation == Operation.Defend && computer.CurrentOperation == Operation.Defend)
            {
                Console.WriteLine($"{player.Name} et {computer.Name} se défendent. Il ne se passe rien.");
                return;
            }

            // Normal attack
            /*if (player.CurrentOperation == Operation.Attack)
                Console.WriteLine($"{player.Name} attaque et inflige {player.Attack(computer)} pt de dégat !");
            if (computer.CurrentOperation == Operation.Attack)
                Console.WriteLine($"{computer.Name} attaque et inflige {computer.Attack(player)} pt de dégat !");*/

            // Abilities
            /* Tank */
            if (player.CurrentOperation == Operation.Special && player.GetType() == typeof(Tank)) player.UseAbility(computer);
            if (computer.CurrentOperation == Operation.Special && computer.GetType() == typeof(Tank)) computer.UseAbility(player);

            /* Fighter */
            if (player.CurrentOperation == Operation.Special && player.GetType() == typeof(Fighter)) player.UseAbility(computer);
            if (computer.CurrentOperation == Operation.Special && computer.GetType() == typeof(Fighter)) computer.UseAbility(player);

            /* Enchanter */
            if (player.CurrentOperation == Operation.Special && player.GetType() == typeof(Healer)) player.UseAbility(computer);
            if (computer.CurrentOperation == Operation.Special && computer.GetType() == typeof(Healer)) computer.UseAbility(player);

            /* Assassin */
            if (player.CurrentOperation == Operation.Special && player.GetType() == typeof(Assassin)) player.UseAbility(computer);
            if (computer.CurrentOperation == Operation.Special && computer.GetType() == typeof(Assassin)) computer.UseAbility(player);
        }

        //4
        public void StartNewRound()
        {
            RoundNumber++;

            if (RoundNumber != 1)
            {
                player.NewRound();
                computer.NewRound();
            }

            Console.WriteLine($"\n----- [ Round {RoundNumber} ] -----");
            Console.WriteLine($"[JOUEUR] {player.Name} : {player.CurrentHealth} hp ({player.CurrentAttack} atk)");
            Console.WriteLine($"[IA] {computer.Name} : {computer.CurrentHealth} hp ({computer.CurrentAttack} atk)");
        }

        public void Update(float dt) {
            player.Update(dt);
            computer.Update(dt);
        }
    }
}