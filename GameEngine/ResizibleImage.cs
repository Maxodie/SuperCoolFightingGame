using System;
using System.Drawing;

namespace GameEn
{
    public class ResizibleImage {
        public Image image;
        Size size;

        public ResizibleImage(Size size) {
            this.size = size;
        }

        public ResizibleImage(string filePath, Size size) : this(size) {
            //try to load image and send error if the path is wrong
            try {
                image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + filePath);
            }
            catch {
                Console.WriteLine("ERROR::Can't load image with path : " + filePath);
            }

            image = new Bitmap(image, this.size);
        }

        public ResizibleImage(Image img, Size size) : this(size) {
            //create new image with new size
            image = new Bitmap(img, this.size);
        }
    }
}
