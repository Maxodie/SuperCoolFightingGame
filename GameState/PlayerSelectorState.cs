using GUI;
using GameEn;
using System.Drawing;
using System;

namespace SuperCoolFightingGame
{
    public class PlayerSelectorState : GameState{
        CharacterSelector characterSelector;
        GameManager gameManager;

        Character player;
        Character computer;

        AudioListener backMusic;

        ButtonGUI buttonAssassin;
        ButtonGUI buttonFighter;
        ButtonGUI buttonHealer;
        ButtonGUI buttonTank;

        ButtonGUI currentCharacterBtn;
        Image currentCharacterImage;
        Vector2 currentCharacterPos;

        ButtonGUI startBtn;
        SpriteAnimation spriteStartButtonAnimation;

        Sprite selectorBorder;
        Sprite selectedCharacter;
        SpriteAnimation selectedCharacterAnim;

        Text charactersInfosText;

        public PlayerSelectorState(GameStateData data, AudioListener backMusic) : base(data) {
            this.backMusic = backMusic;
            characterSelector = new CharacterSelector();
            gameManager = new GameManager(player, computer, superCoolFightingGame);
        }

        /// <summary>
        /// Call at start
        /// </summary>
        public override void InitGUI()
        {
            base.InitGUI();

            Sprite border = new Sprite(imageLoader.GetImage("border"), new Rectangle(0, 0, 800, 640), new Vector2(0, 0));
            gameE.AddSpriteToRender(border);

            Image assassinFace = imageLoader.GetImage("assassinFace");
            Image enchanterFace = imageLoader.GetImage("enchanterFace");
            Image fighterFace = imageLoader.GetImage("fighterFace");
            Image tankFace = imageLoader.GetImage("tankFace");

            Image assassinIcon = imageLoader.GetImage("assassinIcon");
            Image enchanterIcon = imageLoader.GetImage("enchanterIcon");
            Image fighterIcon = imageLoader.GetImage("fighterIcon");
            Image tankIcon = imageLoader.GetImage("tankIcon");

            buttonAssassin = new ButtonGUI(new Vector2(48, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), assassinFace, assassinFace, assassinFace, false);
            buttonAssassin.onClick += delegate (object sender, EventArgs e) {
                gameStateData.savedAudio["click"].Play();
                SelectCharacter(sender, e, buttonAssassin, assassinIcon, new Vector2(72, 448), new Vector2(48, 424), typeof(Assassin)); 
            };

            buttonFighter = new ButtonGUI(new Vector2(224, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), fighterFace, fighterFace, fighterFace, false);
            buttonFighter.onClick += delegate (object sender, EventArgs e) {
                gameStateData.savedAudio["click"].Play();
                SelectCharacter(sender, e, buttonFighter, fighterIcon, new Vector2(248, 448), new Vector2(224, 424), typeof(Fighter)); 
            };

            buttonHealer = new ButtonGUI(new Vector2(408, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), enchanterFace, enchanterFace, enchanterFace, false);
            buttonHealer.onClick += delegate (object sender, EventArgs e) {
                gameStateData.savedAudio["click"].Play();
                SelectCharacter(sender, e, buttonHealer, enchanterIcon, new Vector2(432, 448), new Vector2(408, 424), typeof(Healer));
            };

            buttonTank = new ButtonGUI(new Vector2(584, 424), new Size(168, 168), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 168, 168), tankFace, tankFace, tankFace, false);
            buttonTank.onClick += delegate (object sender, EventArgs e) {
                gameStateData.savedAudio["click"].Play();
                SelectCharacter(sender, e, buttonTank, tankIcon, new Vector2(608, 448), new Vector2(584, 424), typeof(Tank)); 
            };

            charactersInfosText = new Text(Color.White, new Vector2(48, 250), "Character :\nnot selected", gameE.fonts["Pixel16"]);
            gameE.AddTextToRender(charactersInfosText);
        }

        /// <summary>
        /// Call on the creation of the state or on the update of all states
        /// </summary>
        public override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Call each frames
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);

            spriteStartButtonAnimation?.Update(dt);
            selectedCharacterAnim?.Update(dt);
        }

        /// <summary>
        /// Event call on the end of a render
        /// </summary>
        public override void OnStopRender()
        {
        }

        /// <summary>
        /// Event called on the destroy of the scene
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        void StartMainGame() {
            backMusic.Stop();
            backMusic = null;
            superCoolFightingGame.AddState(new MainGameState(gameStateData, gameManager, player, computer));
            player = null;
            computer = null;
        }

        void ConfirmSelection() {
            if (!spriteStartButtonAnimation.isPaused) return;

            ConfirmCharacterSelection();

            Image scrollStartClose = imageLoader.GetImage("scrollPlayClose");

            startBtn.ChangeImages(scrollStartClose, scrollStartClose, scrollStartClose);

            spriteStartButtonAnimation = new SpriteAnimation(window, startBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            spriteStartButtonAnimation.AddFrameEvent(23, StartMainGame);

            spriteStartButtonAnimation.Play();
        }

        void SelectCharacter(object sender, EventArgs e, ButtonGUI currentIcon, Image newImage, Vector2 newImagePos, Vector2 pos, Type characterType) {
            characterSelector.Select(characterType);

            if (selectorBorder == null) {
                selectorBorder = new Sprite(imageLoader.GetImage("selectBorder"), new Rectangle(0, 0, 168, 168), pos);
                superCoolFightingGame.AddSpriteToRender(selectorBorder);

                selectorBorder.ChangePos(pos);
                superCoolFightingGame.AddSpriteToRender(selectorBorder);
            }
            else {
                selectorBorder.ChangePos(pos);
            }

            if (currentCharacterBtn != null) {
                currentCharacterBtn.btnSprite.ChangePos(currentCharacterPos);
                currentCharacterBtn.ChangeImages(currentCharacterImage, currentCharacterImage, currentCharacterImage, new Rectangle(0, 0, 168, 168));
            }

            currentCharacterBtn = currentIcon;
            currentCharacterImage = currentIcon.btnSprite.rImage.image;
            currentCharacterPos = pos;
            currentIcon.btnSprite.ChangePos(newImagePos);
            currentIcon.ChangeImages(newImage, newImage, newImage, new Rectangle(0, 0, 120, 120));

            CharacterStats characterData = characterSelector.characterDatabase.GetCharacterDataWithType(characterType);
            charactersInfosText.text = $"Character :\nName : {characterData.Name}\nLife : {characterData.BaseHealth}\nAttack Point : {characterData.BaseAttack}";

            if (startBtn == null) {

                Image scrollStartOpen = imageLoader.GetImage("scrollPlayOpen");

                //Start Button
                startBtn = new ButtonGUI(new Vector2(176, 56), new Size(448, 96), "", superCoolFightingGame.fonts["Pixel40"], new Rectangle(0, 0, 448, 96), scrollStartOpen, scrollStartOpen, scrollStartOpen, false);
                startBtn.onClick += delegate(object o, EventArgs ev) {
                    gameStateData.savedAudio["click"].Play();
                    ConfirmSelection(); 
                };

                spriteStartButtonAnimation = new SpriteAnimation(window, startBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
                spriteStartButtonAnimation.Play();

                Sprite shadow = new Sprite(imageLoader.GetImage("shadow"), new Rectangle(0, 0, 128, 40), new Vector2(326, 360));
                gameE.AddSpriteToRender(shadow);
            }

            //Image selected character
            gameE.RemoveSpriteFromRender(selectedCharacter);
            selectedCharacter = new Sprite(imageLoader.GetImage(characterData.characterIdleImgPath), new Rectangle(0, 0, 128, 128), new Vector2(336, 256));

            gameE.AddSpriteToRender(selectedCharacter);
            selectedCharacterAnim = new SpriteAnimation(WindowE.instance, selectedCharacter, new Rectangle(0, 0, 512, 128), 4, 1f, 1.5f, true);
            selectedCharacterAnim.Play();
        }

        public void ConfirmCharacterSelection() {

            characterSelector.Confirm(out player, false, gameManager);

            characterSelector.Select();
            characterSelector.Confirm(out computer, true, gameManager);
        }
    }
}
