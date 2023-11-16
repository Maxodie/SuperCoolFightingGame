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
        AudioListener backMusic;

        public MainMenuState(GameStateData gameStateData): base(gameStateData) {
            backMusic = new AudioListener(true, "Media/sounds/mainMenu/Jeopardies.mp3", true);
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

            titleTextSprite = new Sprite(imageLoader.GetImage("title"), new Rectangle(0, 0, 592, 152), new Vector2(104, 56));
            superCoolFightingGame.AddSpriteToRender(titleTextSprite);
        }

        /// <summary>
        /// Call on the creation of the state or on the update of all states
        /// </summary>
        public override void Start() {
            if (!gameStateData.savedAudio.ContainsKey("click"))
            {
                gameStateData.savedAudio["click"] = new AudioListener(false, "Media/sounds/SFX/Click.wav", false);
                gameStateData.savedAudio["attack"] = new AudioListener(false, "Media/sounds/SFX/Attack.wav");
                gameStateData.savedAudio["defendStart"] = new AudioListener(false, "Media/sounds/SFX/DefendStart.wav");
                gameStateData.savedAudio["defendEnd"] = new AudioListener(false, "Media/sounds/SFX/DefendEnd.wav");

                gameStateData.savedAudio["guniSpe"] = new AudioListener(false, "Media/sounds/SFX/GuniSpecial.wav");

                gameStateData.savedAudio["vladSpe"] = new AudioListener(false, "Media/sounds/SFX/VladSpecialStart.wav");
                gameStateData.savedAudio["vladProject"] = new AudioListener(false, "Media/sounds/SFX/VladProjectile.wav");
                gameStateData.savedAudio["vladExplo"] = new AudioListener(false, "Media/sounds/SFX/VladSpecialExplosion.wav");

                gameStateData.savedAudio["lunaSpe"] = new AudioListener(false, "Media/sounds/SFX/LunaSpecial.wav");
                gameStateData.savedAudio["nanSpe"] = new AudioListener(false, "Media/sounds/SFX/NanSpecial.wav");
            }
            
            mainMenuManager = new MainMenuManager(gameStateData, backMusic);
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
            mainMenuManager = null;
        }

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public override void OnDestroy() {
            base.OnDestroy();
        }

    }
}
