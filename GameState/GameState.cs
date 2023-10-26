using GameEn;
using System;
using System.Collections.Generic;

namespace SuperCoolFightingGame
{
    public class GameState {
        protected GameStateData gameStateData;

        protected WindowE window;
        protected GameE gameE;
        protected SuperCoolFightingGame superCoolFightingGame;

        protected ImageLoader imageLoader;

        /// <summary>
        /// Load all game states infos
        /// </summary>
        /// <param name="gameStateData"></param>
        public GameState(GameStateData gameStateData) {
            this.gameStateData = gameStateData;
            window = gameStateData.window;
            gameE = gameStateData.gameE;
            superCoolFightingGame = gameStateData.superCoolFightingGame;
            imageLoader = gameStateData.imageLoader;
        }

        /// <summary>
        /// Start and call the initGUI
        /// </summary>
        public virtual void Start() {
            InitGUI();
        }

        /// <summary>
        /// Call the gui
        /// </summary>
        public virtual void InitGUI() {

        }

        /// <summary>
        /// Update fonction called each frame
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(float dt) {
        }

        /// <summary>
        /// Event call when the state is not rendered anymore
        /// </summary>
        public virtual void OnStopRender() { }

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public virtual void OnDestroy() {

        }
    }
}
