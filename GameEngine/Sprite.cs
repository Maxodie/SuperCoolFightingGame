using System.Drawing;

namespace GameEn
{
    public class Sprite {
        ResizibleImage resizibleImage;
        public ResizibleImage rImage { get { return resizibleImage; } private set { resizibleImage = value; } }
        Vector2 pos;
        public Rectangle Rect { get { return rect; } private set { rect = value; } }
        Rectangle rect;
        Rectangle defaultRect;

        public bool flipX = false;

        /// <summary>
        /// to draw you need to connect it with gameE.AddSpriteToRender(Sprite sprite)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fullSize"></param>
        /// <param name="newPos"></param>
        public Sprite(Image image, Rectangle rect, Vector2 newPos) {
            pos = newPos;

            this.rect = rect;
            defaultRect = rect;


            rImage = new ResizibleImage(image, image.Size);
        }

        public void Render(Graphics g) {
            g.DrawImage(rImage.image, (int)pos.x, (int)pos.y, rect, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// change the scale of a sprite by multiply scale
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        public void ChangeScale(float scaleX, float scaleY) {
            rect.Width = (int)(rect.Width * scaleX);
            rect.Height = (int)(rect.Height * scaleY);
            rImage.image = new Bitmap(rImage.image, new Size((int)(rImage.image.Width * scaleX), (int)(rImage.image.Height * scaleY)));
        }

        //Flip the sprite on x axe
        public void FlipX() {
            rImage.image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            
            flipX = !flipX;

            if (flipX)
                rect.X = rImage.image.Width - defaultRect.Width;
            else
                rect.X = defaultRect.X;

        }

        public void ChangeImage(Image newImg, Rectangle newRect = new Rectangle()) {
            if (newRect != new Rectangle())
                rect = newRect;

            if (newImg != rImage.image)
            {
                rImage.image = new Bitmap(newImg, newImg.Size);
                if(flipX) {
                    flipX = false;
                    FlipX();
                }
            } 
            WindowE.instance.Update();
        }

        public void ChangePos(Vector2 newPos) {
            pos = newPos;
        }
    }
}
