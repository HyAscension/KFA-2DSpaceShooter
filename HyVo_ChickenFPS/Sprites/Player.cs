using System;
using System.Collections.Generic;
using System.Text;
using HyVo_ChickenFPS.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyVo_ChickenFPS.Sprites
{
    public class Player : Ship
    {
        private float shootTimer = 0;

        public bool IsDead => Health <= 0;
        public KeyInput Input;
        public Score Score;

        public Player(Texture2D texture) : base(texture)
        {
            Speed = 5f;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDead) return;

            var velocity = Vector2.Zero;
            rotation = 0;

            if (Keyboard.GetState().IsKeyDown(Input.Up))
            {
                velocity.Y -= Speed; //slow down when move up
                rotation = MathHelper.ToRadians(-20);
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
            {
                velocity.Y += Speed; //slow down when move down
                rotation = MathHelper.ToRadians(20);
            }
            if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                velocity.X -= Speed;
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                velocity.X += Speed;
            }

            shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Input.Shoot) && shootTimer > 0.25f)
            {
                Shoot(Speed * 5);
                shootTimer = 0f;
            }

            Position += velocity;

            //prevent ship from moving outside the screen
            Position = Vector2.Clamp(Position, new Vector2(80, 0), new Vector2(Game1.ScreenWidth, Game1.ScreenHeight));

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead) return;

            base.Draw(gameTime, spriteBatch);
        }

        public override void OnCollide(Sprite sprite)
        {
            if (IsDead) return;

            if (sprite is Bullet && sprite.Parent is Enemy)
                Health--;

            if (sprite is Enemy) Health -= 3;
        }
    }
}
