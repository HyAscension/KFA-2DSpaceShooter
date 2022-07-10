using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HyVo_ChickenFPS.Sprites
{
    public class Ship : Sprite, ICollidable
    {
        public int Health { get; set; }
        public Bullet Bullet { get; set; }
        public float Speed { get; set; }

        public Ship(Texture2D texture) : base(texture)
        {
        }

        public void Shoot(float speed)
        {
            //check if there's a bullet
            if (Bullet == null) return;

            Bullet bulletCloned = Bullet.Clone() as Bullet;
            bulletCloned.Color = this.Color;
            bulletCloned.Layer = 0.1f;
            bulletCloned.Position = this.Position;
            bulletCloned.LifeSpan = 5f;
            bulletCloned.Velocity = new Vector2(speed, 0f);
            bulletCloned.Parent = this;

            Children.Add(bulletCloned);
        }

        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
