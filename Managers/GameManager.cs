using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameEn;
using GUI;

namespace SuperCoolFightingGame
{
    public class GameManager
    {
        SuperCoolFightingGame mainState;
        public int RoundNumber;
        public bool canPlay = true;
        bool isFinished = false;
        public Difficulty Difficulty;
        public Character computer, player;
        public Queue<(Action, Character)> actionsQueue = new Queue<(Action, Character) > ();

        public Text currentGameInfoText;

        public MusicManager musicManager;
        AudioListener defeatSound;

        public GameManager(Character player, Character computer, SuperCoolFightingGame mainState)
        {
            this.player = player;
            this.computer = computer;
            this.mainState = mainState;

            defeatSound = new AudioListener(false, "Media/sounds/SFX/Defeat.wav");
        }

        public void ReferenceMusicManager(MusicManager musicManager) {
            this.musicManager = musicManager;
        }

        public void MakeActions() {
            if (!canPlay) return;

            if(player.GetType() == typeof(Assassin)) actionsQueue.Enqueue((player.StartAbility, player));
            if(computer.GetType() == typeof(Assassin)) actionsQueue.Enqueue((computer.StartAbility, computer));

            if (player.GetType() == typeof(Fighter)) actionsQueue.Enqueue((player.StartAbility, player));
            if (computer.GetType() == typeof(Fighter)) actionsQueue.Enqueue((computer.StartAbility, computer));

            if(player.GetType() == typeof(Healer) || player.GetType() == typeof(Tank))
                actionsQueue.Enqueue((player.StartAbility, player));
            if(computer.GetType() == typeof(Healer) || computer.GetType() == typeof(Tank))
                actionsQueue.Enqueue((computer.StartAbility, computer));

            actionsQueue.Enqueue((player.StartDefense, player));
            actionsQueue.Enqueue((computer.StartDefense, computer));

            if (player.GetType() == typeof(Tank)) actionsQueue.Enqueue((delegate () { player.UseAbility(computer); }, player));
            if (computer.GetType() == typeof(Tank)) actionsQueue.Enqueue((delegate () { computer.UseAbility(player); }, computer));

            if (player.GetType() == typeof(Healer)) actionsQueue.Enqueue((delegate () { player.UseAbility(computer); }, player));
            if (computer.GetType() == typeof(Healer)) actionsQueue.Enqueue((delegate () { computer.UseAbility(player); }, computer));

            actionsQueue.Enqueue((delegate () { player.Attack(computer); }, player));
            actionsQueue.Enqueue((delegate () { computer.Attack(player); }, computer));

            if (player.GetType() == typeof(Assassin)) actionsQueue.Enqueue((delegate () { player.UseAbility(computer); }, player));
            if (computer.GetType() == typeof(Assassin)) actionsQueue.Enqueue((delegate () { computer.UseAbility(player); }, computer));

            if (player.GetType() == typeof(Fighter)) actionsQueue.Enqueue((delegate () { player.UseAbility(computer); }, player));
            if (computer.GetType() == typeof(Fighter)) actionsQueue.Enqueue((delegate () { computer.UseAbility(player); }, computer));

            actionsQueue.Enqueue((player.EndActions, player));
            actionsQueue.Enqueue((computer.EndActions, computer));


            DoActions();
        }

        async void DoActions() {
            canPlay = false;

            while (actionsQueue.Count > 0) {
                (Action, Character) act = actionsQueue.Dequeue();
                act.Item1.Invoke();

                await Task.Run(() => {
                    Task.Delay(act.Item2.currentActionTimeMs).Wait();
                });

                act.Item2.currentActionTimeMs = 0;

                if (isFinished)
                    break;
            }

            if(!isFinished)
                canPlay = true;
        }

        public void UpdateTextInfos(string data) {
            if (currentGameInfoText == null) {
                currentGameInfoText = new Text(Color.Black, new Vector2(375, 404), "", mainState.fonts["Pixel16"]);
                mainState.AddTextToRender(currentGameInfoText);
            }
            
            currentGameInfoText.text = data;
        }

        public async void GameOver(Character winner) {
            if (isFinished) return;

            isFinished = true;
            canPlay = false;

            player.playerHud.CloseCharacterBorder();

            defeatSound.Play();

            await Task.Run(() => { Task.Delay((int)(winner.animator.GetAnimation("Death").duration * 1000) + winner.waitActionTimeOffset).Wait(); });

            mainState.AddState(new EndGameState(mainState.gameStateData, winner, !winner.isComputer, musicManager));
            if(winner.isComputer)
                computer = null;
            else
                player = null;
        }

        //4
        public void StartNewRound()
        {
            RoundNumber++;

        }

        public void Update(float dt) {
            player.Update(dt);
            computer.Update(dt);
        }
    }
}