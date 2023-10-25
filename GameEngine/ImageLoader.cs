using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GameEn
{
    public class ImageLoader {
        //store all the loaded images
        Dictionary<string, Image> images = new Dictionary<string, Image>();
        Dictionary<string, string> imagesPath = new Dictionary<string, string>();

        /// <summary>
        /// Load a new Image and store it in images dictionary
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        public void InitImages(string path) {
            string[] allPath = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

            string[] splitedPath;
            string key;

            foreach (var item in allPath) {
                splitedPath = item.Split(Path.DirectorySeparatorChar);
                key = splitedPath[splitedPath.Length - 1];

                if (key.Contains("."))
                    key = key.Substring(0, key.LastIndexOf('.'));
                else//If it's folder and not file
                    continue;

                imagesPath[key] = item;
            }
            
        }

        /// <summary>
        /// Get existing image from the loaded images dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Image GetImage(string key) {
            if(!images.ContainsKey(key)) {
                if (!imagesPath.ContainsKey(key)) {
                    Console.WriteLine($"Error::GetImage::Key {key} doesn't exist");
                    return null;
                }
                else
                    images[key] = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + imagesPath[key]);
            }


            return images[key];
        }
    }
}
