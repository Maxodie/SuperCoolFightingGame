using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GUI;

namespace GameEn
{
    public class WindowE : Form {
        public static WindowE instance;
        GameE gameE;
        public List<Sprite> sprites = new List<Sprite>();
        List<Text> texts = new List<Text>();
        List<ButtonGUI> buttons = new List<ButtonGUI>();

        int WIDTH = 800, HEIGHT = 640;
        string windowName;

        Thread _gameThread;
        ManualResetEvent _evExit;

        public WindowE(string windowName, int width, int height) {
            if(instance == null) {
                instance = this;
            }
            else 
                return;
            this.windowName = windowName;

            WIDTH = width;
            HEIGHT = height;

            DoubleBuffered = true;

            InitWindow(windowName);

            gameE = new SuperCoolFightingGame.SuperCoolFightingGame(this);
            gameE.Run();

            Paint += Render;
        }

        /// <summary>
        /// graphicsTimer force refresh of the screen
        /// </summary>

        protected override void OnClosed(EventArgs e) {
            _evExit.Set();
            _gameThread.Abort();
            _evExit.Close();
            base.OnClosed(e);

        }

        public void OnGameTimerTick() {
            gameE.GameLoop();
            Invalidate();
        }

        /// <summary>
        /// init window infos
        /// </summary>
        /// <param name="windowName"></param>
        void InitWindow(string windowName) {
            Text = windowName;
            Size = new Size(WIDTH, HEIGHT);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - WIDTH / 3, Screen.PrimaryScreen.Bounds.Height / 2 - HEIGHT / 3);
            Icon = new Icon(AppDomain.CurrentDomain.BaseDirectory + "Media/tankFace.ico");
        }


        public void CloseWindow() {
            Close();
        }

        /// <summary>
        /// Add sprite to render
        /// </summary>
        /// <param name="newSprite"></param>
        public void AddSprite(Sprite newSprite) {
            sprites.Add(newSprite);
        }

        /// <summary>
        /// Remove sprite from render
        /// </summary>
        /// <param name="newSprite"></param>
        public void RemoveSprite(Sprite newSprite) {
            sprites.Remove(newSprite);
        }

        /// <summary>
        /// Add text to render
        /// </summary>
        /// <param name="newText"></param>
        public void AddText(Text newText) {
            texts.Add(newText);
        }

        /// <summary>
        /// Remove sprite from render
        /// </summary>
        /// <param name="newText"></param>
        public void RemoveText(Text newText) {
            texts.Add(newText);
        }

        /// <summary>
        /// Add button to render
        /// </summary>
        /// <param name="btn"></param>
        public void AddButton(ButtonGUI btn) {
            buttons.Add(btn);
            Controls.Add(btn.btn);
        }

        /// <summary>
        /// Remove button from render
        /// </summary>
        /// <param name="btn"></param>
        public void RemoveButton(ButtonGUI btn) {
            buttons.Remove(btn);
            Controls.Remove(btn.btn);
        }

        /// <summary>
        /// Render sprites and GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Render(object sender, PaintEventArgs e) {
            // Draw Player Sprite
            RenderSprites(e.Graphics);
            // Draw GUIs
            RenderGUI(e.Graphics);
            
        }


        /// <summary>
        /// Render all sprites
        /// </summary>
        /// <param name="g"></param>
        void RenderSprites(Graphics g) {
            for (int i = 0; i < sprites.Count; i++) {
                sprites[i].Render(g);
            }
        }

        /// <summary>
        /// Render all Gui
        /// </summary>
        /// <param name="g"></param>
        void RenderGUI(Graphics g) {
            for (int i = 0; i < texts.Count; i++) {
                texts[i].Render(g);
            }
        }

        /// <summary>
        /// Reset the render of all things
        /// </summary>
        public void ResetRender() {
            while(sprites.Count > 0) {
                sprites[0].rImage.image.Dispose();
                sprites.RemoveAt(0);
            }

            texts.Clear();

            while (buttons.Count > 0) {
                buttons[0].btnSprite.rImage.image.Dispose();
                buttons.RemoveAt(0);
            }

            Controls.Clear();

            Invalidate();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            _evExit = new ManualResetEvent(false);
            _gameThread = new Thread(GameThreadProc);
            _gameThread.Name = "Game Thread";
            _gameThread.Start();
        }

        private void GameThreadProc() {
            IAsyncResult tick = null;
            while (!_evExit.WaitOne(15)) {
                if (tick != null) {
                    if (!tick.AsyncWaitHandle.WaitOne(0)) {
                       
                        if (WaitHandle.WaitAny(new WaitHandle[] { _evExit, tick.AsyncWaitHandle }) == 0) {
                            return;
                        }
                    }
                }
                tick = BeginInvoke(new MethodInvoker(OnGameTimerTick));
            }
        }
    }
}
