using GameEn;
using System;
using System.Drawing;
using GUI;

namespace SuperCoolFightingGame
{
    public class PlayerHud {
        ImageLoader imageLoader;
        GameE gameE;

        int heartNb;
        int attackNb;

        int heartDistance = 64;
        int attackDistance = 48;

        Rectangle heartRect = new Rectangle(0, 0, 56, 48);
        Rectangle attackRect = new Rectangle(0, 0, 32, 32);

        Vector2 heartsPos;
        Vector2 attacksPos;

        Sprite[] hearts;
        Sprite[] attacks;
        Sprite characterBorder;

        int currentHeartNb;
        int maxHearts;
        int currentAttackNb;
        int baseAttackPoint;

        bool isPlayer;
        Character character;
        Animator borderAnim;
        Text nameText;

        public PlayerHud(Vector2 heartsIconPos, int maxHearts, int heartNb, Vector2 attacksIconPos, int attackNb, int baseAttackPoint, ImageLoader imageLoader, GameE gameE, bool leftToRight, bool isPlayer, Character character) {
            this.attackNb = attackNb;
            this.heartNb = heartNb;
            this.maxHearts = maxHearts;
            this.imageLoader = imageLoader;
            this.gameE = gameE;
            this.heartsPos = heartsIconPos;
            this.attacksPos = attacksIconPos;
            this.baseAttackPoint = baseAttackPoint;
            this.isPlayer = isPlayer;
            this.character = character;

            InitHearts(leftToRight);
            InitAttack(leftToRight);

            if(isPlayer)
                InitBorder();
        }

        void InitBorder() {
            Sprite characterIcon = new Sprite(imageLoader.GetImage(character.characterIcon), new Rectangle(0, 0, 120, 120), new Vector2(72, 464));// 72;464 (120x120)
            gameE.AddSpriteToRender(characterIcon);

            characterBorder = new Sprite(imageLoader.GetImage("characterBorder"), new Rectangle(0, 0, 288, 168), new Vector2(48, 440));
            gameE.AddSpriteToRender(characterBorder);

            borderAnim = new Animator(WindowE.instance, characterBorder);
            borderAnim.AddAnimation(imageLoader.GetImage("characterBorderOpen"), new Rectangle(0, 0, 288, 168), new Rectangle(0, 0, 3456, 168), 12, 1f, 1f, "Open");
            borderAnim.AddOnEndAnimation(delegate(object sender, EventArgs e) { InitText(); }, "Open");

            borderAnim.AddAnimation(imageLoader.GetImage("characterBorderClose"), new Rectangle(0, 0, 288, 168), new Rectangle(0, 0, 3456, 168), 12, 1f, 1f, "Close");
            borderAnim.AddAnimationEvent(DestroyText, "Close", 1);
            borderAnim.PlayAnimation("Open");

            Sprite characterTypeIcon = new Sprite(imageLoader.GetImage(character.characterTypeIconPath), new Rectangle(0, 0, 48, 48), new Vector2(232, 464));
            gameE.AddSpriteToRender(characterTypeIcon);
        }

        void InitText() {
            nameText = new Text(Color.Black, new Vector2(220, 550), character.Name, gameE.fonts["Pixel16"]);
            gameE.AddTextToRender(nameText);
        }

        public void CloseCharacterBorder() {
            if(isPlayer)
                borderAnim.PlayAnimation("Close");
        }

        void DestroyText() {
            gameE.RemoveTextFromRender(nameText);
            nameText.text = "";
        }

        void InitHearts(bool leftToRight) {
            hearts = new Sprite[heartNb];
            currentHeartNb = heartNb;

            int offset = leftToRight ? maxHearts - heartNb : 0;

            for (int i = 0; i < heartNb; i++) {
                hearts[i] = new Sprite(imageLoader.GetImage("filledHeart"), heartRect, new Vector2(heartDistance * (i + offset), 0) + heartsPos);
                gameE.AddSpriteToRender(hearts[i]);
            }

            if (leftToRight)
                Array.Reverse(hearts);
            
        }

        void InitAttack(bool leftToRight) {
            attacks = new Sprite[attackNb];
            currentAttackNb = attackNb-1;
            
            for (int i = 0; i < attackNb; i++) {
                bool condition = leftToRight ? i >= baseAttackPoint || baseAttackPoint == attackNb : i < baseAttackPoint;

                if (condition)
                    attacks[i] = new Sprite(imageLoader.GetImage("filledAttackPoint"), heartRect, new Vector2(attackDistance * i, 0) + attacksPos);
                else
                    attacks[i] = new Sprite(imageLoader.GetImage("emptyAttackPoint"), heartRect, new Vector2(attackDistance * i, 0) + attacksPos);

                gameE.AddSpriteToRender(attacks[i]);
            }

            if (leftToRight)
                Array.Reverse(attacks);
        }

        public void Update(float dt) {
            borderAnim?.Update(dt);
        }

        public void LoseHp() {
            if (currentHeartNb <= 0) return;

            hearts[currentHeartNb - 1].ChangeImage(imageLoader.GetImage("emptyHeart"));
            currentHeartNb--;
        }

        public void GetHp() {
            if (currentHeartNb >= heartNb) return;

            hearts[currentHeartNb].ChangeImage(imageLoader.GetImage("filledHeart"));
            currentHeartNb++;
        }

        public void LoseAttackPoint() {
            if (currentAttackNb <= 0) return;

            attacks[currentAttackNb - 1].ChangeImage(imageLoader.GetImage("emptyAttackPoint"));
            currentAttackNb--;
        }

        public void GetAttackPoint() {
            if(currentAttackNb >= attackNb) return;

            currentAttackNb++;
            attacks[currentAttackNb - 1].ChangeImage(imageLoader.GetImage("filledAttackPoint"));
        }
    }
}
