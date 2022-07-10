using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyVo_ChickenFPS
{
    public abstract class Component
    {
        //update every frame refresh - use for drawing sprites
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        //update every frame refresh - use for calculations
        public abstract void Update(GameTime gameTime);
    }
}
