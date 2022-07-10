using System;
using System.Collections.Generic;
using System.Text;
using HyVo_ChickenFPS.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HyVo_ChickenFPS.Sprites
{
    public class Explosion : Sprite
    {
        private float counter = 0f;
        
        public Explosion(Dictionary<string, Animation> animations) : base(animations)
        {
        }

        public override void Update(GameTime gameTime)
        {
            manageAnimation.Update(gameTime);
            counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //remove sprite after finish animating
            if (counter > manageAnimation.CurrentAnimation.FrameCount * manageAnimation.CurrentAnimation.FrameSpeed)
                IsRemove = true;
        }
    }
}
