using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyVo_ChickenFPS.Controls
{
    public class Button : Component
    {
        //fields
        private Texture2D texture;
        private SpriteFont font;
        
        private MouseState previousMouse;
        private MouseState currentMouse;

        private bool isHovering;

        //properties
        public EventHandler Click;
        public bool Clicked { get; set; }
        public float Layer { get; set; }
        public Vector2 Center => new Vector2(texture.Width / 2, texture.Height / 2);
        public Color PenColor { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle => new Rectangle((int)Position.X - ((int)Center.X), (int)Position.Y - (int)Center.Y, texture.Width, texture.Height);
        public string Text;

        //constructor
        public Button(Texture2D gameTexture, SpriteFont gameFont)
        {
            texture = gameTexture;
            font = gameFont;
            PenColor = Color.White;
        }

        //methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = Color.White;

            if (isHovering)
                color = Color.Green;

            spriteBatch.Draw(texture, Position, null, color, 0f, Center, 1f, SpriteEffects.None, Layer);

            if (!string.IsNullOrEmpty(Text))
            {
                float x = Rectangle.X + (Rectangle.Width / 2) - (font.MeasureString(Text).X / 2);
                float y = Rectangle.Y + (Rectangle.Height / 2) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        
    }
}
