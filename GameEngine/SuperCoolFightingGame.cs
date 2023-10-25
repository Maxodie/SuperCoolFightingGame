﻿using GameEn;
using System;
using System.Collections.Generic;

namespace SuperCoolFightingGame
{
    public class SuperCoolFightingGame : GameE {
        List<GameState> states = new List<GameState>();

        GameStateData gameStateData;

        public SuperCoolFightingGame(WindowE window) : base(window) {
            InitFonts();

            //load all gameStates data
            gameStateData = new GameStateData();
            gameStateData.fonts = fonts;
            gameStateData.gameE = this;
            gameStateData.superCoolFightingGame = this;
            gameStateData.imageLoader = new ImageLoader();
            gameStateData.savedSprite = new Dictionary<string, Sprite>();
        }

        /// <summary>
        /// start the program
        /// </summary>
        public override void Run() {
            base.Run();
        }

        /// <summary>
        /// call in the run GameE methode
        /// </summary>
        public override void Start() {
            gameStateData.window = window;
            gameStateData.imageLoader.InitImages("Media/img");
            AddState(new MainMenuState(gameStateData));

            base.Start();
        }

        /// <summary>
        /// call after the start methode and update all states
        /// </summary>
        /// <param name="dt"></param>
        protected override void Update(float dt) {
            if (states.Count > 0)
                states[states.Count - 1].Update(dt);
            else
                window.CloseWindow();

            base.Update(dt);
        }

        /// <summary>
        /// Add a new state in the states list and call the start methode of the last states
        /// </summary>
        /// <param name="state"></param>
        public void AddState(GameState state) {
            if (states.Count > 0) {
                window.ResetRender();
                states[states.Count - 1].OnStopRender();
            }
 
            states.Add(state);
            states[states.Count - 1].Start();

        }

        /// <summary>
        /// Remove a state and you can call the start of the last state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="playStart"></param>
        public void RemoveState(GameState state, bool playStart = true) {
            window.ResetRender();

            state.OnStopRender();
            state.OnDestroy();

            states.Remove(state);
            if (states.Count > 0)
                states[states.Count - 1].Start();
        }

        /// <summary>
        /// init all the font of the game
        /// </summary>
        void InitFonts() {
            InitFont("Media/font/pixelFont.ttf", 16, "Pixel16");
            InitFont("Media/font/pixelFont.ttf", 40, "Pixel40");
        }
    }
}
