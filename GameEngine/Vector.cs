using System;
using System.CodeDom;

namespace GameEn
{
    /// <summary>
    /// Vector2 foat
    /// </summary>
    public class Vector2 {
        public float x, y;

        public Vector2(float x, float y){
            this.x = x; 
            this.y = y;
        }

        public override string ToString() {
            return "X : " + x + "; Y : " + y;
        }

        public static Vector2 operator+(Vector2 a, Vector2 b) {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

    }
}
