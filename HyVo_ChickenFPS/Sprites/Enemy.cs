using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HyVo_ChickenFPS.Sprites
{
    public class Enemy : Ship
    {
        private float counter;
        public float ShootTimer = 1.75f;

        public Enemy(Texture2D texture) : base(texture)
        {
            Speed = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            counter = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (counter >= ShootTimer)
            {
                Shoot(-5);
                counter = 0;
            }

            Position += new Vector2(-Speed, 0);

            //remove chicken after they pass the left of the screen
            if (Position.X < -texture.Width)
            {
                IsRemove = true;
            }
        }

        public override void OnCollide(Sprite sprite)
        {
            if (sprite is Player && !((Player)sprite).IsDead)
            {
                ((Player)sprite).Score.Value++;

                IsRemove = true;
            }

            if (sprite is Bullet && sprite.Parent is Player)
            {
                Health--;

                if (Health <= 0)
                {
                    IsRemove = true;
                    ((Player)sprite.Parent).Score.Value++;
                }
            }
        }
    }
}
