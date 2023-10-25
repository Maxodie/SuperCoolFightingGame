using System;
using System.Drawing;
using GameEn;

namespace SuperCoolFightingGame
{
    public class MainMenuState : GameState {
        MainMenuManager mainMenuManager;

        Sprite border;
        Sprite titleTextSprite;

        //Audio
        MusicManager musicManager;

        public MainMenuState(GameStateData gameStateData): base(gameStateData) {
            musicManager = new MusicManager("Media/sounds/mainMenu", false);
        }

        public override void InitGUI() {
            window.BackgroundImage = imageLoader.GetImage("background0");
            /*Sprite test = new Sprite(imageLoader.GetImage("nan"), new Rectangle(0, 0, 128, 128), new Vector2(112, 232));
            gameE.AddSpriteToRender(test);

            t = new SpriteAnimation(test, new Rectangle(2560, 0, 128, 128), 20, 1f, 3f);

            AddAnimation(t);
            t.Play();*/

            mainMenuManager.InitGUI();

            border = new Sprite(imageLoader.GetImage("border"), new Rectangle(0, 0, 800, 640), new Vector2(0, 0));
            gameE.AddSpriteToRender(border);

            gameStateData.savedSprite["border"] = border;

            titleTextSprite = new Sprite(imageLoader.GetImage("title"), new Rectangle(0, 0, 592, 152), new Vector2(104, 56));
            superCoolFightingGame.AddSpriteToRender(titleTextSprite);
        }

        /// <summary>
        /// Call on the creation of the state or on the update of all states
        /// </summary>
        public override void Start() {
            mainMenuManager = new MainMenuManager(gameStateData, musicManager);
            base.Start();
        }

        /// <summary>
        /// Call each frames
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt) {
            base.Update(dt);
            mainMenuManager.Update(dt);
        }


        /// <summary>
        /// Event call on the end of a render
        /// </summary>
        public override void OnStopRender() {
            base.OnStopRender();
        }

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public override void OnDestroy() {
            base.OnDestroy();
        }

    }
}
