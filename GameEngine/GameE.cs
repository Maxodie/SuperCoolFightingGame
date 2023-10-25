using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using GUI;
using System;
using System.Drawing.Text;
using System.Threading;

namespace GameEn
{
    public class GameE {
        public WindowE window { get; private set; }
        PrivateFontCollection pfc = new PrivateFontCollection();
        public Dictionary<string, Font> fonts = new Dictionary<string, Font>();

        //dt
        DateTime lastDtTime;
        DateTime currentDtTime;

        Thread _gameThread;

        public GameE(WindowE window) {
            this.window = window;
        }


        public virtual void Run() {
            lastDtTime = DateTime.Now;
            Start();
        }

        public virtual void Start() { }


        public void GameLoop() {
            currentDtTime = DateTime.Now;
            //calcule the dt and give it to the update
            Update((currentDtTime.Ticks - lastDtTime.Ticks)/ 10000000f);

            lastDtTime = currentDtTime;
        }


        protected virtual void Update(float dt) {
        }

        public void AddSpriteToRender(Sprite sprite) {
            window.AddSprite(sprite);
        }

        public void RemoveSpriteFromRender(Sprite sprite) {
            window.RemoveSprite(sprite);
        }

        public void RemoveTextFromRender(Text text) {
            window.RemoveText(text);
        }

        public void AddTextToRender(Text text) {
            window.AddText(text);
        }

        public void InitFont(string fontPath, int size, string fontName) {
            //load a font in the dictionary
            fonts[fontName] = UseCustomFont(AppDomain.CurrentDomain.BaseDirectory + fontPath, size);
        }


        Font UseCustomFont(string name, int size) {
            //add custom font in the pcf
            pfc.AddFontFile(name);

            return new Font(pfc.Families[pfc.Families.Length-1], size);
        }
    }
}
