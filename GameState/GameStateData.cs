using System.Collections.Generic;
using System.Drawing;
using GameEn;

namespace SuperCoolFightingGame
{
    /// <summary>
    /// All the data states needs
    /// </summary>
    public struct GameStateData {
        public Dictionary<string, Font> fonts;
        public WindowE window;
        public GameE gameE;
        public SuperCoolFightingGame superCoolFightingGame;
        public Dictionary<string, Sprite> savedSprite;
        public ImageLoader imageLoader;

        //mainGameInfos
        public Difficulty difficulty;
    }
}
