using GameEn;
using System;
using System.Drawing;
using System.Linq;

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

        int currentHeartNb;
        int maxHearts;
        int currentAttackNb;
        int baseAttaackPoint;

        bool isPlayer;
        Character character;

        public PlayerHud(Vector2 heartsIconPos, int maxHearts, int heartNb, Vector2 attacksIconPos, int attackNb, int baseAttaackPoint, ImageLoader imageLoader, GameE gameE, bool leftToRight, bool isPlayer, Character character) {
            this.attackNb = attackNb;
            this.heartNb = heartNb;
            this.maxHearts = maxHearts;
            this.imageLoader = imageLoader;
            this.gameE = gameE;
            this.heartsPos = heartsIconPos;
            this.attacksPos = attacksIconPos;
            this.baseAttaackPoint = baseAttaackPoint;
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

            Sprite characterBorder = new Sprite(imageLoader.GetImage("characterBorder"), new Rectangle(0, 0, 288, 168), new Vector2(48, 440));
            gameE.AddSpriteToRender(characterBorder);

            Sprite characterTypeIcon = new Sprite(imageLoader.GetImage(character.characterTypeIconPath), new Rectangle(0, 0, 288, 168), new Vector2(48, 440));
            gameE.AddSpriteToRender(characterTypeIcon);
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
                bool condition = leftToRight ? i >= baseAttaackPoint || baseAttaackPoint == attackNb : i < baseAttaackPoint;

                if (condition)
                    attacks[i] = new Sprite(imageLoader.GetImage("filledAttackPoint"), heartRect, new Vector2(attackDistance * i, 0) + attacksPos);
                else
                    attacks[i] = new Sprite(imageLoader.GetImage("emptyAttackPoint"), heartRect, new Vector2(attackDistance * i, 0) + attacksPos);

                gameE.AddSpriteToRender(attacks[i]);
            }

            if (leftToRight)
                Array.Reverse(attacks);
        }

        public void LoseHp() {
            if (currentHeartNb < 0) return;

            Console.WriteLine(currentHeartNb + "hp lose");
            hearts[currentHeartNb - 1].ChangeImage(imageLoader.GetImage("emptyHeart"));
            currentHeartNb--;
        }

        public void GetHp() {
            if (currentHeartNb >= heartNb) return;

            Console.WriteLine(currentHeartNb + "hp Get");
            hearts[currentHeartNb].ChangeImage(imageLoader.GetImage("filledHeart"));
            currentHeartNb++;
        }

        public void LoseAttackPoint() {
            if (currentAttackNb <= 0) return;
            Console.WriteLine(currentAttackNb - 1 + "ap Lose");
            attacks[currentAttackNb - 1].ChangeImage(imageLoader.GetImage("emptyAttackPoint"));
            currentAttackNb--;
        }

        public void GetAttackPoint() {
            if(currentAttackNb >= attackNb) return;
            Console.WriteLine(currentAttackNb + "ap get");
            currentAttackNb++;
            attacks[currentAttackNb - 1].ChangeImage(imageLoader.GetImage("filledAttackPoint"));
        }
    }
}
