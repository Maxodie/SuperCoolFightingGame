using GameEn;
using GUI;
using System;
using System.Drawing;

namespace SuperCoolFightingGame
{
    public class MainGameState : GameState {
        //SpriteAnimation animation;
        Sprite hearthSeparator;
        OperationSelector operationSelector;
        GameManager gameManager;

        //Selector button
        ButtonGUI attackOperation;
        ButtonGUI defenseOperation;
        ButtonGUI specialOperation;

        //Audio
        MusicManager musicManager;
             
        public MainGameState(GameStateData data, Character player, Character computer) : base(data) {
            musicManager = new MusicManager("Media/sounds/game");
            operationSelector = new OperationSelector(data.difficulty);
            gameManager = new GameManager(player, computer);
        }


        public override void InitGUI() {
            gameE.AddSpriteToRender(gameStateData.savedSprite["border"]);

            Image attackImageBtn = imageLoader.GetImage("attack");
            Image attackImageHighlightBtn = imageLoader.GetImage("attackH");
            Image attackImagePressedBtn = imageLoader.GetImage("attackP");

            Image defenseImageBtn = imageLoader.GetImage("defend");
            Image defenseHighlightImageBtn = imageLoader.GetImage("defendH");
            Image defensePressedImageBtn = imageLoader.GetImage("defendP");

            Image specialImageBtn = imageLoader.GetImage(gameManager.player.CharacterSpecialBtnImgPath);
            Image specialImageHighlightBtn = imageLoader.GetImage(gameManager.player.CharacterSpecialBtnHighlightImgPath);
            Image specialImagePressedBtn = imageLoader.GetImage(gameManager.player.CharacterSpecialBtnPressedImgPath);


            hearthSeparator = new Sprite(imageLoader.GetImage("separator"), new Rectangle(0, 0, 32, 88), new Vector2(384, 40));
            gameE.AddSpriteToRender(hearthSeparator);

            attackOperation = new ButtonGUI(new Vector2(352, 480), new Size(128, 128), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 128, 128), attackImageHighlightBtn, attackImageBtn, attackImagePressedBtn, true);
            attackOperation.onClick += delegate(object sender, EventArgs e) { StartOperations(sender, e, Operation.Attack); };

            defenseOperation = new ButtonGUI(new Vector2(496, 480), new Size(128, 128), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 128, 128), defenseHighlightImageBtn, defenseImageBtn, defensePressedImageBtn, true);
            defenseOperation.onClick += delegate (object sender, EventArgs e) { StartOperations(sender, e, Operation.Defend); };

            specialOperation = new ButtonGUI(new Vector2(640, 480), new Size(128, 128), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 128, 128), specialImageHighlightBtn, specialImageBtn, specialImagePressedBtn, true);
            specialOperation.onClick += delegate (object sender, EventArgs e) { StartOperations(sender, e, Operation.Special); };

            //Image btnImage1 = imageLoader.GetImage("Scroll");
            //Image btnImage2 = imageLoader.GetImage("ScrollHighlight");
            //quitButton = new ButtonGUI(window, new Vector2(184, 400), new Size(448, 96), "Quit", gameStateData.fonts["Pixel40"], btnImage2, btnImage1, btnImage2);

            //quitButton.onClick += delegate (object sender, EventArgs e) { ReturnToMainMenu(sender, e, 1); };

            //shieldTest = new Sprite(imageLoader.GetImage("DefendAnimationStart"), new Rectangle(0, 0, 152, 152), new Vector2(96, 216));
            //gameE.AddSpriteToRender(shieldTest);

            //animation = new SpriteAnimation(shieldTest, new Rectangle(2432, 0, 152, 152), 16, 1f, 0.5f);
            //animation.onEndAnimation += Testc;

            //AddAnimation(animation);
            //animation.Play();
        }

        /// <summary>
        /// Call on the creation of the state or on the update of all states
        /// </summary>
        public override void Start() {
            base.Start();

            //gameManager.Init();
            gameManager.player.LoadGFX(imageLoader, gameE, new Vector2(112, 232), new Vector2(104, 336), false);
            gameManager.computer.LoadGFX(imageLoader, gameE, new Vector2(552, 176), new Vector2(560, 280), true);
            gameManager.player.InitAnimations(imageLoader);
            gameManager.computer.InitAnimations(imageLoader);

        }

        /// <summary>
        /// Call each frames
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt) {
            base.Update(dt);

            gameManager.Update(dt);
        }

        /// <summary>
        /// Event call on the end of a render
        /// </summary>
        public override void OnStopRender() {
            base.OnStopRender();

            musicManager.StopMusic();
        }

        /*public async void Testc(object sender, EventArgs e)
        {
            await Task.Run(() => {
                Task.Delay(10).Wait();
            });

            RemoveAnimation(animation);
            gameE.RemoveSpriteFromRender(shieldTest);

        }*/

        /*public void ReturnToMainMenu(object sender, EventArgs e, int selection)
        {
            superCoolFightingGame.RemoveState(this);

            RemoveAnimation(animation);
            gameE.RemoveSpriteFromRender(shieldTest);
            superCoolFightingGame.RemoveState(this);
            Console.WriteLine(selection);
        }*/

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public override void OnDestroy() {
            base.OnDestroy();
        }
        
        void StartOperations(object sender, EventArgs e, Operation operation) {
            operationSelector.PlayerSelection(operation, gameManager.player);
            operationSelector.ComputerSelection(gameManager.computer, gameManager.player);

            gameManager.MakeActions();
            gameManager.StartNewRound();
        }
    }
}
