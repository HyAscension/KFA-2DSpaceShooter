using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyVo_ChickenFPS.States
{
    public abstract class State
    {
        protected Game1 game;
        protected ContentManager content;
        
        public State(Game1 gameState, ContentManager contentState)
        {
            game = gameState;
            content = contentState;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
