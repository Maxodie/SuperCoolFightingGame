using System;

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

        public static double Distance(Vector2 a, Vector2 b) {
            return Math.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y));
        }

        public override bool Equals(object o) {
            return o == (object)this;
        }

        public override int GetHashCode() {
            return 0;
        }

        public static Vector2 operator+(Vector2 a, Vector2 b) {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static bool operator==(Vector2 a, Vector2 b) {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2 a, Vector2 b) {
            return !(a.x == b.x && a.y == b.y);
        }

        
    }
}
