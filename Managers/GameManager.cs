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

        public GameManager(Character player, Character computer, SuperCoolFightingGame mainState)
        {
            this.player = player;
            this.computer = computer;
            this.mainState = mainState;
        }

        public void ReferenceMusicManager(MusicManager musicManager) {
            this.musicManager = musicManager;
        }

        public void MakeActions() {
            if (!canPlay) return;

            actionsQueue.Enqueue((player.StartDefense, player));
            actionsQueue.Enqueue((computer.StartDefense, computer));

            actionsQueue.Enqueue((delegate () { player.Attack(computer); }, player));
            actionsQueue.Enqueue((delegate () { computer.Attack(player); }, computer));

            actionsQueue.Enqueue((delegate () { player.UseAbility(computer); }, player));
            actionsQueue.Enqueue((player.EndActions, player));
            actionsQueue.Enqueue((delegate () { computer.UseAbility(player); }, computer));
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

            await Task.Run(() => { Task.Delay(2000).Wait(); });

            mainState.AddState(new EndGameState(mainState.gameStateData, winner, !winner.isComputer, musicManager));   
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