﻿using GameEn;
using GUI;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
    public class MainMenuManager {
        GameStateData gameData;
        ButtonGUI startButton;
        ButtonGUI quitButton;

        MusicManager musicManager;
        SpriteAnimation startBtnAnim;
        SpriteAnimation closeBtnAnim;

        public MainMenuManager(GameStateData gameData, MusicManager musicManager) { 
            this.gameData = gameData;
            this.musicManager = musicManager;
        }

        public void InitGUI() {
            Image scrollExitOpen = gameData.imageLoader.GetImage("scrollExitOpen");
            Image scrollStartOpen = gameData.imageLoader.GetImage("scrollPlayOpen");

            //Start Button
            startButton = new ButtonGUI(new Vector2(184, 296), new Size(448, 96), "", gameData.fonts["Pixel40"], new Rectangle(0, 0, 448, 96), scrollStartOpen, scrollStartOpen, scrollStartOpen);
            startButton.onClick += StartGame;

            startBtnAnim = new SpriteAnimation(gameData.window, startButton.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, .5f);
            startBtnAnim.Play();

            //QuitButton00
            quitButton = new ButtonGUI(new Vector2(184, 408), new Size(448, 96), "", gameData.fonts["Pixel40"], new Rectangle(0, 0, 448, 96), scrollExitOpen, scrollExitOpen, scrollExitOpen);
            quitButton.onClick += QuitGame;

            closeBtnAnim = new SpriteAnimation(gameData.window, quitButton.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 2f);
            closeBtnAnim.Play();
        }

        //Start button
        void StartGame(object sender, EventArgs e) {
            Image scrollStartClose = gameData.imageLoader.GetImage("scrollPlayClose");

            startButton.ChangeImages(scrollStartClose, scrollStartClose, scrollStartClose);

            startBtnAnim = new SpriteAnimation(gameData.window, startButton.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, .2f);
            startBtnAnim.AddFrameEvent(23, StartEvent);

            startBtnAnim.Play();

        }

        void StartEvent() {
            gameData.superCoolFightingGame.AddState(new DifficultySelectorState(gameData, musicManager));
        }

        public void Update(float dt) {
            startBtnAnim.Update(dt);
            closeBtnAnim.Update(dt);
        }

        //Quit button
        void QuitGame(object sender, EventArgs e) {
            Image scrollExitClose = gameData.imageLoader.GetImage("scrollExitClose");

            quitButton.ChangeImages(scrollExitClose, scrollExitClose, scrollExitClose);

            closeBtnAnim = new SpriteAnimation(gameData.window, quitButton.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            closeBtnAnim.AddFrameEvent(23, QuitEvent);

            closeBtnAnim.Play();
        }

        void QuitEvent() {
            gameData.window.CloseWindow();
        }
    }
}
