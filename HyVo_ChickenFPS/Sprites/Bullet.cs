using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HyVo_ChickenFPS.Sprites
{
    public class Bullet : Sprite, ICollidable
    {
        private float counter;

        public Explosion Explosion;
        public float LifeSpan { get; set; }
        public Vector2 Velocity { get; set; }

        public Bullet(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            counter = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (counter >= LifeSpan)
            {
                IsRemove = true;
            }

            Position += Velocity;
        }

        public void OnCollide(Sprite sprite)
        {
            //no collision between bullets
            if (sprite is Bullet) return;

            //enemies can't shoot each other
            if (sprite is Enemy && this.Parent is Enemy) return;

            //players can't shoot each other
            if (sprite is Player && this.Parent is Player) return;

            //can't hit player if dead
            if (sprite is Player && ((Player)sprite).IsDead) return;

            if (sprite is Enemy && this.Parent is Player)
            {
                IsRemove = true;
                AddExplosion();
            }

            if (sprite is Player && this.Parent is Enemy)
            {
                IsRemove = true;
                AddExplosion();
            }
        }

        public void AddExplosion()
        {
            if (Explosion == null) return;

            Explosion explosion = Explosion.Clone() as Explosion;
            explosion.Position = this.Position;

            Children.Add(explosion);
        }
    }
}
