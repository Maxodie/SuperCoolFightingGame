using GUI;
using GameEn;
using System.Drawing;
using System;

namespace SuperCoolFightingGame
{
    public class EndGameState : GameState {
        Character winner;
        bool isPlayerWin;
        Text infosText;
        Sprite titleTextSprite;

        ButtonGUI restartBtn;
        SpriteAnimation btnAnim;

        AudioListener backMusic;
        SpriteAnimation selectedCharacterAnim;

        public EndGameState(GameStateData data, Character winner, bool isPlayerWin, AudioListener backMusic) : base(data){ 
            this.winner = winner;
            this.isPlayerWin = isPlayerWin;
            this.backMusic = backMusic;
        }

        public override void InitGUI() {
            Sprite border = new Sprite(imageLoader.GetImage("border"), new Rectangle(0, 0, 800, 640), new Vector2(0, 0));
            gameE.AddSpriteToRender(border);

            string result = isPlayerWin ? "WIN" : "LOOSE";
            Vector2 textPos = isPlayerWin ? new Vector2(320, 50) : new Vector2(300, 50);


            if (isPlayerWin)//232;40 (368x152)
                titleTextSprite = new Sprite(imageLoader.GetImage("winTitle"), new Rectangle(0, 0, 368, 152), new Vector2(232, 40));
            else
                titleTextSprite = new Sprite(imageLoader.GetImage("loseTitle"), new Rectangle(0, 0, 368, 152), new Vector2(232, 40));

            gameE.AddSpriteToRender(titleTextSprite);

            Image scrollStartOpen = imageLoader.GetImage("scrollExitOpen");

            restartBtn = new ButtonGUI(new Vector2(184, 496), new Size(448, 96), "", gameE.fonts["Pixel40"], new Rectangle(0, 0, 448, 96), scrollStartOpen, scrollStartOpen, scrollStartOpen, false);
            restartBtn.onClick += delegate (object sender, EventArgs e) {
                gameStateData.savedAudio["click"].Play();
                StartCaracterSelectorBtn(); 
            };

            btnAnim = new SpriteAnimation(window, restartBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            btnAnim.Play();

            //Image selected character
            Sprite selectedCharacter = new Sprite(imageLoader.GetImage(winner.characterIdleImgPath), new Rectangle(0, 0, 128, 128), new Vector2(336, 256));

            gameE.AddSpriteToRender(selectedCharacter);
            selectedCharacterAnim = new SpriteAnimation(WindowE.instance, selectedCharacter, new Rectangle(0, 0, 512, 128), 4, 1f, 1.5f, true);
            selectedCharacterAnim.Play();

            Sprite shadow = new Sprite(imageLoader.GetImage("shadow"), new Rectangle(0, 0, 128, 40), new Vector2(326, 360));
            gameE.AddSpriteToRender(shadow);
        }

        public override void Update(float dt) {
            base.Update(dt);

            btnAnim.Update(dt);
            selectedCharacterAnim.Update(dt);
        }

        void StartCaracterSelectorBtn() {
            if (!btnAnim.isPaused) return;

            Image scrollStartClose = imageLoader.GetImage("scrollExitClose");

            restartBtn.ChangeImages(scrollStartClose, scrollStartClose, scrollStartClose);

            btnAnim = new SpriteAnimation(window, restartBtn.btnSprite, new Rectangle(0, 0, 10752, 96), 24, 1f, 0.2f);
            btnAnim.AddFrameEvent(23, Restart);

            btnAnim.Play();
        }

        public override void OnStopRender() {
            base.OnStopRender();
        }
        void Restart() {
            while(superCoolFightingGame.states.Count > 2) {
                superCoolFightingGame.RemoveState(superCoolFightingGame.states[superCoolFightingGame.states.Count-2]);
            }

            superCoolFightingGame.RemoveState(superCoolFightingGame.states[superCoolFightingGame.states.Count - 1]);
            backMusic = null;
        }
    }
}
