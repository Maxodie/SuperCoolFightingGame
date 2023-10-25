using GameEn;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public class ButtonGUI
    {
        public Button btn { get; private set; }
        Image[] btnImages;
        public Sprite btnSprite;
        Vector2 pos;

        int currentImageListId = 0;

        //Mouse events
        public EventHandler onClick;
        public EventHandler onMouseHover;
        public EventHandler onMouseLeave;

        //Btn colors
        Color normalColor;
        Color pressedColor;
        Color highlightColor;

        bool isImageBtn;
        string text;

        public ButtonGUI(Vector2 pos, Size size, string text, Font font) {
            btn = new Button();
            btn.Location = new Point((int)pos.x, (int)pos.y);
            this.text = text;
            this.pos = pos;

            btn.Size = size;
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.FlatAppearance.BorderSize = 0;

            btn.Text = text;

            if (font != null)
                btn.Font = font;

            btn.Visible = true;
            btn.BringToFront();

            btn.FlatStyle = FlatStyle.Flat;
            InitDefaultBtn();
            InitEvent();
        }

        public ButtonGUI(Vector2 pos, Size size, string text, Font font, Rectangle rect, Image highlightColor, Image normalImg, Image clickedImage)
            : this(pos, size, text, font) {

            isImageBtn = true;
            InitVisualImage(rect, normalImg, clickedImage, highlightColor);
        }

        public ButtonGUI(Vector2 pos, Size size, string text, Font font, Color normalColor, Color pressedColor, Color highlightColor)
            : this(pos, size, text, font) {
            isImageBtn = false;
            InitColor(normalColor, pressedColor, highlightColor);
        }

        void InitDefaultBtn() {
            UpdateDraw();
        }

        public void ChangeImages(Image normalImg, Image clickedImage, Image highlightImage, Rectangle newRectangle = new Rectangle()) {
            btnImages[0] = normalImg;
            btnImages[1] = clickedImage;
            btnImages[2] = highlightImage;

            btnSprite.ChangeImage(btnSprite.rImage.image, new Rectangle(0, btnSprite.Rect.Y, btnSprite.Rect.Width, btnSprite.Rect.Height));
            btnSprite.ChangeImage(btnImages[currentImageListId], newRectangle);
        }

        //Link mouse event
        void InitEvent() {
            btn.Click += PerformClick;
            btn.MouseHover += OnMouseHover;
            btn.MouseLeave += OnMouseLeave;
        }

        //add images in array and put colors to transparent
        void InitVisualImage(Rectangle rect, Image normalImg, Image clickedImage, Image highlightImage) {
            btnImages = new Image[3];

            btnImages[0] = normalImg;
            btnImages[1] = clickedImage;
            btnImages[2] = highlightImage;

            btnSprite = new Sprite(normalImg, rect, pos);
            WindowE.instance.AddSprite(btnSprite);

            currentImageListId = 0;

            btn.BackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }

        void InitColor(Color normalColor, Color pressedColor, Color highlightColor) {
            this.normalColor = normalColor;
            this.pressedColor = pressedColor;
            this.highlightColor = highlightColor;

            btn.BackColor = normalColor;
            btn.FlatAppearance.MouseOverBackColor = highlightColor;
            btn.FlatAppearance.MouseDownBackColor = normalColor;

        }

        public async void PerformClick(object sender, EventArgs e) {
            //Change visual with onClick event
            if (isImageBtn) {
                btnSprite.ChangeImage(btnImages[1]);
                btn.Refresh();
                
            }
            else {
                btn.BackColor = pressedColor;
            }

            onClick?.Invoke(sender, e);

            if(isImageBtn) {//delay for the press effect
                await Task.Run(() => {
                    Task.Delay(10).Wait();
                });
                currentImageListId = 2;
                btnSprite.ChangeImage(btnImages[2]);
            }

            btn.Refresh();
        }

        public void ResetVisual() {
            //reset the btn 
            if (isImageBtn) {
                btnSprite.ChangeImage(btnImages[0]);
            }
            else {
                btn.BackColor = normalColor;
            }

            btn.Refresh();
        }

        private void OnMouseHover(object sender, EventArgs e) {
            //change visual with onMouseHover event
            if (isImageBtn) {
                btnSprite.ChangeImage(btnImages[2]);
            }
            else {
                btn.BackColor = highlightColor;
            }

            onMouseHover?.Invoke(sender, e);
            btn.Refresh();
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            //change visual with onMouseLeaveEvent
            if (isImageBtn) {
                btnSprite.ChangeImage(btnImages[0]);
            }
            else {
                btn.BackColor = normalColor;
            }

            onMouseLeave?.Invoke(sender, e);
            btn.Refresh();
        }

        public void UpdateDraw() {
            WindowE.instance.AddButton(this);
            btn.Invalidate();
        }

    }

    public class Text {
        Font font;
        SolidBrush brush;
        Color textColor;
        Vector2 pos;
        StringFormat drawFormat;

        public string text;

        /// <summary>
        /// to draw you need to connect it with gameE.AddTextToRender(Text text)
        /// </summary>
        /// <param name="textColor"></param>
        /// <param name="pos"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public Text(Color textColor, Vector2 pos, string text, Font font) {
            this.textColor = textColor;
            this.pos = pos;
            this.text = text;
            this.font = font;

            InitText();
        }

        void InitText() {
            brush = new SolidBrush(textColor);
            drawFormat = new StringFormat();
            WindowE.instance.Invalidate();
        }

        public void Render(Graphics g) {
            g.DrawString(text, font, brush, pos.x, pos.y, drawFormat);
        }

        public void ChangePos(Vector2 pos) {
            this.pos = pos;
            WindowE.instance.Invalidate();
        }
    }
}
