using GameEn;
using System.Collections.Generic;

namespace SuperCoolFightingGame
{
    public class GameState {
        protected GameStateData gameStateData;

        protected WindowE window;
        protected GameE gameE;
        protected SuperCoolFightingGame superCoolFightingGame;

        protected ImageLoader imageLoader;

        List<SpriteAnimation> spriteAnimationList = new List<SpriteAnimation>();

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
        /// Add animation to update
        /// </summary>
        /// <param name="animation"></param>
        public void AddAnimation(SpriteAnimation animation) {
            spriteAnimationList.Add(animation);
        }

        /// <summary>
        /// Remove animation from update
        /// </summary>
        /// <param name="animation"></param>
        public void RemoveAnimation(SpriteAnimation animation) {
            spriteAnimationList.Remove(animation);
        }

        /// <summary>
        /// Update fonction called each frame
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(float dt) {
            for(int i=0; i<spriteAnimationList.Count; i++) {
                spriteAnimationList[i].Update(dt);
            }
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
