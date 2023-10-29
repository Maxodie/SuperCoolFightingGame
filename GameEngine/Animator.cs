using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Animation;

namespace GameEn {
    public class Animator {
        Dictionary<string, (SpriteAnimation, Image, Rectangle)> animatons = new Dictionary<string, (SpriteAnimation, Image, Rectangle)>();
        SpriteAnimation currentAnimationItem;
        Rectangle defaultRect;
        Sprite defaultSprite;
        Image defaultImage;

        WindowE window;
        string entryAnimation;

        public Animator(WindowE window, Sprite sprite) {
            this.window = window;
            
            defaultRect = sprite.Rect;
            defaultSprite = sprite;
            defaultImage = sprite.rImage.image;
        }

        public void AddAnimation(Image animImage, Rectangle oneRect, Rectangle fullRectSize, int spriteNb, float speed, float duration, string key, bool isLooping = false, bool isEntryAnimation = false) { 
            animatons[key] = (new SpriteAnimation(window, defaultSprite, fullRectSize, spriteNb, speed, duration, isLooping), animImage, oneRect);

            if (isEntryAnimation)
                entryAnimation = key;
        }

        public void PlayAnimation(string key) {
            if(currentAnimationItem != null) {
                currentAnimationItem.Reset(true);
            }
          
            animatons[key].Item1.sprite.ChangeImage(animatons[key].Item2, animatons[key].Item3);
            animatons[key].Item1.Play();
            currentAnimationItem = animatons[key].Item1;

            if(!animatons[key].Item1.isLooping)
                animatons[key].Item1.onEndAnimation += delegate (object sender, EventArgs e) { SetDefaultSprite(); };
        }

        void SetDefaultSprite() {
            if (entryAnimation != null)
                PlayAnimation(entryAnimation);
            /*defaultSprite.ChangeImage(defaultImage, defaultRect);
            if(defaultSprite.flipX) {
                defaultSprite.flipX = false;
                defaultSprite.FlipX();
            }*/
        }

        public void Update(float dt) {
            currentAnimationItem?.Update(dt);
        }

        void AddOnEndAnimation(EventHandler e, string key) {
            animatons[key].Item1.onEndAnimation += e;
        }

        public void AddAnimationEvent(Action action, string key, int frameCall) {
            animatons[key].Item1.AddFrameEvent(frameCall, action);
        }
    }
}
