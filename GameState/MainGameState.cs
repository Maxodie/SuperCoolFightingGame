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
             
        public MainGameState(GameStateData data, GameManager gm, Character player, Character computer) : base(data) {
            musicManager = new MusicManager("Media/sounds/game");
            operationSelector = new OperationSelector(data.difficulty);
            gameManager = gm;
            gameManager.ReferenceMusicManager(musicManager);
            gameManager.player = player;
            gameManager.computer = computer;
        }


        public override void InitGUI() {
            Sprite border = new Sprite(imageLoader.GetImage("border"), new Rectangle(0, 0, 800, 640), new Vector2(0, 0));
            gameE.AddSpriteToRender(border);

            Image attackImageBtn = imageLoader.GetImage("attack");
            Image attackImageHighlightBtn = imageLoader.GetImage("attackH");
            Image attackImagePressedBtn = imageLoader.GetImage("attackP");

            Image defenseImageBtn = imageLoader.GetImage("defend");
            Image defenseHighlightImageBtn = imageLoader.GetImage("defendH");
            Image defensePressedImageBtn = imageLoader.GetImage("defendP");

            Image specialImageBtn = imageLoader.GetImage(gameManager.player.characterSpecialBtnImgPath);
            Image specialImageHighlightBtn = imageLoader.GetImage(gameManager.player.characterSpecialBtnHighlightImgPath);
            Image specialImagePressedBtn = imageLoader.GetImage(gameManager.player.characterSpecialBtnPressedImgPath);


            hearthSeparator = new Sprite(imageLoader.GetImage("separator"), new Rectangle(0, 0, 32, 88), new Vector2(384, 40));
            gameE.AddSpriteToRender(hearthSeparator);

            attackOperation = new ButtonGUI(new Vector2(352, 480), new Size(128, 128), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 128, 128), attackImageHighlightBtn, attackImageBtn, attackImagePressedBtn, true);
            attackOperation.onClick += delegate(object sender, EventArgs e) { StartOperations(sender, e, Operation.Attack); };

            defenseOperation = new ButtonGUI(new Vector2(496, 480), new Size(128, 128), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 128, 128), defenseHighlightImageBtn, defenseImageBtn, defensePressedImageBtn, true);
            defenseOperation.onClick += delegate (object sender, EventArgs e) { StartOperations(sender, e, Operation.Defend); };

            specialOperation = new ButtonGUI(new Vector2(640, 480), new Size(128, 128), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 128, 128), specialImageHighlightBtn, specialImageBtn, specialImagePressedBtn, true);
            specialOperation.onClick += delegate (object sender, EventArgs e) { StartOperations(sender, e, Operation.Special); };

            //Dialog scroll
            Sprite dialogScroll = new Sprite(imageLoader.GetImage("scroll"), new Rectangle(0, 0, 448, 96), new Vector2(336, 384));
            gameE.AddSpriteToRender(dialogScroll);


        }

        /// <summary>
        /// Call on the creation of the state or on the update of all states
        /// </summary>
        public override void Start() {
            base.Start();

            //gameManager.Init();
            gameManager.player.LoadCharacter(imageLoader, gameE, new Vector2(112, 232), new Vector2(104, 336), false, new Vector2(96, 216), 
                new Vector2(520, 248), new Vector2(168, 272), new Vector2(544, 216), new Vector2(56, 40), new Vector2(280, 96));

            gameManager.computer.LoadCharacter(imageLoader, gameE, new Vector2(552, 176), new Vector2(560, 280), true, new Vector2(544, 160), 
                new Vector2(176, 304), new Vector2(544, 216), new Vector2(168, 272), new Vector2(432, 40), new Vector2(440, 96));

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
            gameManager = null;
        }

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public override void OnDestroy() {
            base.OnDestroy();
        }
        
        void StartOperations(object sender, EventArgs e, Operation operation) {
            if (!gameManager.canPlay) return;

            operationSelector.PlayerSelection(operation, gameManager.player);
            operationSelector.ComputerSelection(gameManager.computer, gameManager.player);

            gameManager.MakeActions();
            gameManager.StartNewRound();
        }
    }
}
