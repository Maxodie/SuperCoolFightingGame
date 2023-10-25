using GUI;
using GameEn;
using System.Drawing;
using System;

namespace SuperCoolFightingGame
{
    public class DifficultySelectorState : GameState {
        MusicManager musicManager;

        Sprite selectionBorder;

        ButtonGUI easyDifficulty;
        ButtonGUI mediumDifficulty;
        ButtonGUI hardDifficulty;

        ButtonGUI startBtn;

        SpriteAnimation spriteStartButtonAnimation;
        Text difficultyText;

        public DifficultySelectorState(GameStateData data, MusicManager musicManager) : base(data) {
            this.musicManager = musicManager;
        }


        /// <summary>
        /// Call at start
        /// </summary>
        public override void InitGUI() {
            base.InitGUI();

            gameE.AddSpriteToRender(gameStateData.savedSprite["border"]);

            Image easy = imageLoader.GetImage("easyDifficulty");
            Image medium = imageLoader.GetImage("mediumDifficulty");
            Image hard = imageLoader.GetImage("hardDifficulty");

            //Diffictulty button 
            easyDifficulty = new ButtonGUI(new Vector2(120, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), easy, easy, easy);
            easyDifficulty.onClick += delegate (object sender, EventArgs e) { SelectDifficulty(sender, e, new Vector2(120, 424), Difficulty.Easy); };

            mediumDifficulty = new ButtonGUI(new Vector2(312, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), medium, medium, medium);
            mediumDifficulty.onClick += delegate (object sender, EventArgs e) { SelectDifficulty(sender, e, new Vector2(312, 424), Difficulty.Medium); };

            hardDifficulty = new ButtonGUI(new Vector2(512, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), hard, hard, hard);
            hardDifficulty.onClick += delegate (object sender, EventArgs e) { SelectDifficulty(sender, e, new Vector2(512, 424), Difficulty.Hard); };

            difficultyText = new Text(Color.White, new Vector2(120, 380), "Select a difficulty", superCoolFightingGame.fonts["Pixel16"]);
            superCoolFightingGame.AddTextToRender(difficultyText);
        }

        /// <summary>
        /// Call on the creation of the state or on the update of all states
        /// </summary>
        public override void Start() {
            base.Start();
        }

        /// <summary>
        /// Call each frames
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt) {
            base.Update(dt);

            spriteStartButtonAnimation?.Update(dt);
        }

        /// <summary>
        /// Event call on the end of a render
        /// </summary>
        public override void OnStopRender() { 
        }

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public override void OnDestroy() {
            base.OnDestroy();
        }

        void SwitchToPlayerSelector() {
            superCoolFightingGame.AddState(new PlayerSelectorState(gameStateData, musicManager));

        }

        void SelectDifficulty(object sender, EventArgs e, Vector2 pos, Difficulty newDifficulty) {
            gameStateData.difficulty = newDifficulty;

            if (selectionBorder == null) {
                Console.WriteLine(pos.x);
                selectionBorder = new Sprite(imageLoader.GetImage("selectBorder"), new Rectangle(0, 0, 168, 168), pos);
                superCoolFightingGame.AddSpriteToRender(selectionBorder);
                gameStateData.savedSprite["selectBorder"] = selectionBorder;
            }
            else {
                selectionBorder.ChangePos(pos);
            }

            string result = "Difficulty selected : ";

            switch(newDifficulty) {
                case Difficulty.Easy:
                    result += "Easy";
                    break;

                case Difficulty.Medium:
                    result += "Medium";
                    break;

                case Difficulty.Hard:
                    result += "Hard";
                    break;
            }

            difficultyText.text = result;

            if (startBtn == null) {

                Image scrollStartOpen = imageLoader.GetImage("scrollPlayOpen");

                //Start Button
                startBtn = new ButtonGUI(new Vector2(176, 56), new Size(448, 96), "", superCoolFightingGame.fonts["Pixel40"], new Rectangle(0, 0, 448, 96), scrollStartOpen, scrollStartOpen, scrollStartOpen);
                startBtn.onClick += StartCaracterSelectorBtn;

                spriteStartButtonAnimation = new SpriteAnimation(window, startBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
                spriteStartButtonAnimation.Play();
            }
        }

        void StartCaracterSelectorBtn(object sender, EventArgs e) {
            Image scrollStartClose = imageLoader.GetImage("scrollPlayClose");

            startBtn.ChangeImages(scrollStartClose, scrollStartClose, scrollStartClose);

            spriteStartButtonAnimation = new SpriteAnimation(window, startBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            spriteStartButtonAnimation.AddFrameEvent(23, SwitchToPlayerSelector);

            spriteStartButtonAnimation.Play();
        }
    }
}
