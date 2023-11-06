﻿using GUI;
using GameEn;
using System.Drawing;
using System;

namespace SuperCoolFightingGame
{
    public class EndGameState : GameState {
        Character winner;
        bool isPlayerWin;
        Text infosText;
        Text titleText;

        ButtonGUI restartBtn;
        SpriteAnimation btnAnim;

        MusicManager musicManager;

        public EndGameState(GameStateData data, Character winner, bool isPlayerWin, MusicManager musicManager) : base(data){ 
            this.winner = winner;
            this.isPlayerWin = isPlayerWin;
            this.musicManager = musicManager;
        }

        public override void InitGUI() {
            Sprite border = new Sprite(imageLoader.GetImage("border"), new Rectangle(0, 0, 800, 640), new Vector2(0, 0));
            gameE.AddSpriteToRender(border);

            string result = isPlayerWin ? "WIN" : "LOOSE";
            Vector2 textPos = isPlayerWin ? new Vector2(320, 50) : new Vector2(300, 50);

            titleText = new Text(Color.White, textPos, result, gameE.fonts["Pixel40"]);
            gameE.AddTextToRender(titleText);

            infosText = new Text(Color.White, new Vector2(320, 300), $"Winner : {winner.Name}", gameE.fonts["Pixel16"]);
            gameE.AddTextToRender(infosText);

            Image scrollStartOpen = imageLoader.GetImage("scrollExitOpen");

            restartBtn = new ButtonGUI(new Vector2(184, 496), new Size(448, 96), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 448, 96), scrollStartOpen, scrollStartOpen, scrollStartOpen, false);
            restartBtn.onClick += delegate (object sender, EventArgs e) {
                gameStateData.savedAudio["click"].Play();
                StartCaracterSelectorBtn(); 
            };

            btnAnim = new SpriteAnimation(window, restartBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            btnAnim.Play();

        }

        public override void Update(float dt) {
            base.Update(dt);

            btnAnim.Update(dt);
        }

        void StartCaracterSelectorBtn() {
            if (!btnAnim.isPaused) return;

            Image scrollStartClose = imageLoader.GetImage("scrollExitClose");

            restartBtn.ChangeImages(scrollStartClose, scrollStartClose, scrollStartClose);

            btnAnim = new SpriteAnimation(window, restartBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            btnAnim.AddFrameEvent(23, Restart);

            btnAnim.Play();
        }

        public override void OnStopRender() {
            base.OnStopRender();

            musicManager.StopMusic();
            musicManager = null;
        }
        void Restart() {
            while(superCoolFightingGame.states.Count > 2) {
                superCoolFightingGame.RemoveState(superCoolFightingGame.states[superCoolFightingGame.states.Count-2]);
            }

            superCoolFightingGame.RemoveState(superCoolFightingGame.states[superCoolFightingGame.states.Count - 1]);
        }
    }
}
