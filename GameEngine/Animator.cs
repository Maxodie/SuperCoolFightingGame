using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEn {
    public class Animator {
        Dictionary<string, (SpriteAnimation, Image, Rectangle)> animations = new Dictionary<string, (SpriteAnimation, Image, Rectangle)>();
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

        public void AddAnimation(Image animImage, Rectangle oneRect, Rectangle fullRectSize, int spriteNb, float speed, float duration, string key, bool isLooping = false, bool isEntryAnimation = false, bool canGoToEntry = true) {
            animations[key] = (new SpriteAnimation(window, defaultSprite, fullRectSize, spriteNb, speed, duration, isLooping), animImage, oneRect);

            if (isEntryAnimation)
                entryAnimation = key;

            if (!isLooping && canGoToEntry)
                animations[key].Item1.onEndAnimation += delegate(object sender, EventArgs e) { SetDefaultSprite(); };
        }

        public void PlayAnimation(string key) {
            if(currentAnimationItem != null) {
                currentAnimationItem.Reset(true);
            }

            currentAnimationItem = animations[key].Item1;
            currentAnimationItem.sprite.ChangeImage(animations[key].Item2, animations[key].Item3);
            currentAnimationItem.Reset(true);
            currentAnimationItem.Play();
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

        public void AddOnEndAnimation(EventHandler e, string key) {
            animations[key].Item1.onEndAnimation += e;
        }

        public void AddAnimationEvent(Action action, string key, int frameCall) {
            animations[key].Item1.AddFrameEvent(frameCall, action);
        }

        public SpriteAnimation GetAnimation(string key) {
            return animations[key].Item1;
        }
    }
}
