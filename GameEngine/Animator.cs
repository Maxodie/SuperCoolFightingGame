using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEn {
    public class Animator {
        Dictionary<string, (SpriteAnimation, Image, Rectangle)> animatons = new Dictionary<string, (SpriteAnimation, Image, Rectangle)>();
        SpriteAnimation currentAnimationItem;

        public void AddAnimation(SpriteAnimation animation, Image animImage, Rectangle animRect, string key) { 
            animatons[key] = (animation, animImage, animRect);
        }

        public void PlayAnimation(string key) {
            if(currentAnimationItem != null) {
                currentAnimationItem.Reset(true);
            }
            Console.WriteLine("change");
            animatons[key].Item1.sprite.ChangeImage(animatons[key].Item2, animatons[key].Item3);
            animatons[key].Item1.Play();
            currentAnimationItem = animatons[key].Item1;
        }

        public void Update(float dt) {
            currentAnimationItem?.Update(dt);
        }
    }
}
