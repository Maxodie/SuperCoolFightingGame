using System;
using System.Drawing;

namespace GameEn
{
    public class WindowShaker {
        int magnitude = 0;
        float duration = 0;
        bool isRunning = false;
        float timer = 0;
        Point defaultLocation = new Point();

        WindowE window;

        public WindowShaker(WindowE window) {
            this.window = window;  
        }

        public void cameraShaker(float duration, int magnitude) {
            this.magnitude = magnitude;
            this.duration = duration;
            isRunning = true;

            defaultLocation = window.Location;

            timer = 0f;
        }

        public void Update(float dt) {
            if (!isRunning) return;

            if(timer > duration) {
                isRunning = false;
                window.Location = defaultLocation;
                return;
            }

            Random rnd = new Random();
            int x = rnd.Next(-1, 1) * magnitude;
            int y = rnd.Next(-1, 1) * magnitude;

            window.Location = new Point(defaultLocation.X + x, defaultLocation.Y + y);

            timer += dt;
        }
    }
}
