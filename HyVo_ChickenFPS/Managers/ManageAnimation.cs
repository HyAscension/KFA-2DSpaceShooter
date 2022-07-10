using HyVo_ChickenFPS.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyVo_ChickenFPS.Managers
{
    public class ManageAnimation : ICloneable
    {
        private Animation animation;
        private float counter;
        public Animation CurrentAnimation => animation;
        
        public float Layer { get; set; }
        public Vector2 Center { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public ManageAnimation(Animation spriteAnimation)
        {
            animation = spriteAnimation;
            Scale = 1f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                animation.Texture, Position,
                new Rectangle
                (
                    animation.CurrentFrame * animation.FrameWidth,
                    0,
                    animation.FrameWidth,
                    animation.FrameHeight
                ),
                Color.White, Rotation, Center, Scale, SpriteEffects.None, Layer
            );
        }

        public void Play(Animation spriteAnimation)
        {
            if (animation == spriteAnimation)
                return;
            animation = spriteAnimation;
            animation.CurrentFrame = 0;
            counter = 0;
        }

        public void Stop()
        {
            counter = 0f;
            animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            counter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (counter > animation.FrameSpeed)
            {
                counter = 0f;
                animation.CurrentFrame++;
                if (animation.CurrentFrame >= animation.FrameCount)
                {
                    animation.CurrentFrame = 0;
                }
            }
        }

        public object Clone()
        {
            var animationManager = this.MemberwiseClone() as ManageAnimation;
            animationManager.animation = animationManager.animation.Clone() as Animation;
            return animationManager;
        }
    }
}
