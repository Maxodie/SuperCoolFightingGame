using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEn
{
    public class SpriteAnimation {
        WindowE window;
        bool isPaused = false;
        public Sprite sprite;
        Rectangle startRect;
        Rectangle endRect;
        Rectangle currentRect;

        Dictionary<int, Action> frameEvents = new Dictionary<int, Action>();
        int currentSpriteFrame = 0;

        float speed;
        float duration;
        float timer = 0f;

        int rectDifference;

        bool isLooping = false;

        float spriteTimer = 0f;

        public EventHandler onEndAnimation;


        public SpriteAnimation(WindowE window, Sprite sprite, Rectangle fullRectSize, int spriteNb, float speed, float duration, bool isLooping = false) {
            this.window = window;
            this.sprite = sprite;
            startRect = fullRectSize;
            startRect.X = sprite.Rect.X;
            startRect.Width = sprite.Rect.Width;
            startRect.Height = sprite.Rect.Height;

            endRect = new Rectangle(fullRectSize.Width - startRect.Width, fullRectSize.Y, fullRectSize.Width, fullRectSize.Height);

            this.speed = speed;
            this.duration = duration;
            this.isLooping = isLooping;

            spriteTimer = duration / (float)spriteNb;

            rectDifference = endRect.X / (spriteNb - 1);

            currentRect = startRect;

            isPaused = true;
        }

        public void Play() {
            isPaused = false;
        }

        /// <summary>
        /// Need to update the animation with deltaTime to play it
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt) {
            if(isPaused) return;

            timer += dt * speed;
            //check if the sprite needs to change
            if(timer >= spriteTimer) {
                timer -= spriteTimer;
                
                currentSpriteFrame++;
                UpdateRect();
                CheckForEvent();

                CheckForAnimationEnd();
            }   
        }
        
        void UpdateRect() {
            //change the rect with the flip state
            if (sprite.flipX) 
                currentRect.X -= rectDifference;
            else
                currentRect.X += rectDifference;
            

            sprite.ChangeImage(sprite.rImage.image, currentRect);
           
        }

        void CheckForEvent() {
            //Sprite frame events
            foreach (var item in frameEvents) {
                if (item.Key == currentSpriteFrame) {
                    item.Value.Invoke();
                }
            }
        }

        void CheckForAnimationEnd() {
            //check for the annimation end
            if (!sprite.flipX) {
                if (currentRect.X >= endRect.X)
                    EndAnimation();
                
            }
            else {
                if (currentRect.X <= 0)
                    EndAnimation();
            }
        }

        void EndAnimation() {
            //play event if exist and reset animation and active looping if true
            onEndAnimation?.Invoke(new object(), new EventArgs());
            Reset();
        }

        public void Pause() {
            isPaused = true;
        }

        /// <summary>
        /// call the action when the sprite is on the spriteFrameEvent count
        /// </summary>
        /// <param name="spriteFrameEvent"></param>
        /// <param name="action"></param>
        public void AddFrameEvent(int spriteFrameEvent, Action action) {
            frameEvents[spriteFrameEvent] = action;
        }

        public void Reset(bool forceStop = false) {
            //restart the animation and reset the rect
            timer = 0f;
            currentSpriteFrame = 0;
            currentRect = startRect;

            if(forceStop) {
                isPaused = true;
                return;
            }

            isPaused = !isLooping;
        }
    }
}
