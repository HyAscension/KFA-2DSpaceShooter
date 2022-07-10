using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HyVo_ChickenFPS.Managers;
using HyVo_ChickenFPS.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HyVo_ChickenFPS.Sprites
{
    public class Sprite : Component, ICloneable
    {
        protected Dictionary<string, Animation> animations;
        protected ManageAnimation manageAnimation;

        protected float layer { get; set; }
        protected Vector2 center { get; set; }
        protected Vector2 position { get; set; }
        protected float rotation { get; set; }
        protected float scale { get; set; }
        protected Texture2D texture { get; set; }

        //list for bullet sprites
        public List<Sprite> Children { get; set; }
        public Color Color { get; set; }
        public bool IsRemove { get; set; }

        public float Layer
        {
            get => layer;
            set
            {
                layer = value;
                if (manageAnimation != null)
                    manageAnimation.Layer = layer;
            }
        }

        //center of sprite
        public Vector2 Center
        {
            get => center;
            set
            {
                center = value;
                if (manageAnimation != null)
                    manageAnimation.Center = center;
            }
        }

        //position of sprite on the screen
        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                if (manageAnimation != null)
                    manageAnimation.Position = position;
            }
        }

        //the invisible rectangle for each sprite
        public Rectangle Rectangle
        {
            get
            {
                if (texture != null)
                {
                    var x = (int)Position.X - (int)Center.X;
                    var y = (int)Position.Y - (int)Center.Y;
                    return new Rectangle(x, y, texture.Width, texture.Height);
                }
                if (manageAnimation != null)
                {
                    var animation = animations.FirstOrDefault().Value;
                    var x = (int)Position.X - (int)Center.X;
                    var y = (int)Position.Y - (int)Center.Y;
                    return new Rectangle(x, y, animation.FrameWidth, animation.FrameHeight);
                }
                throw new Exception("Unknown sprite");
            }
        }

        public float Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                if (manageAnimation != null)
                    manageAnimation.Rotation = rotation;
            }
        }

        public readonly Color[] TextureData;

        //matrix for when bullets travel from one place to another
        public Matrix Transform => Matrix.CreateTranslation(new Vector3(-Center, 0))
            * Matrix.CreateRotationZ(rotation) 
            * Matrix.CreateTranslation(new Vector3(Position, 0));

        public Sprite Parent;

        //area with potential sprite collsions
        public Rectangle CollisionArea => new Rectangle
        (
            Rectangle.X,
            Rectangle.Y,
            MathHelper.Max(Rectangle.Width, Rectangle.Height),
            MathHelper.Max(Rectangle.Width, Rectangle.Height)
        );

        //pass in texture
        public Sprite(Texture2D gameTexture)
        {
            texture = gameTexture;
            Children = new List<Sprite>();
            Center = new Vector2(texture.Width / 2, texture.Height / 2);
            Color = Color.White;
            TextureData = new Color[texture.Width * texture.Height];
            texture.GetData(TextureData);
        }
        
        //pass in dictionary
        public Sprite(Dictionary<string, Animation> gameAnimations)
        {
            animations = gameAnimations;
            Animation animation = animations.FirstOrDefault().Value;
            texture = null;
            Children = new List<Sprite>();
            Color = Color.White;
            TextureData = null;
            manageAnimation = new ManageAnimation(animation);
            Center = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Draw(texture, Position, null, Color, rotation, Center, 1f, SpriteEffects.None, Layer);
            }
            else if (manageAnimation != null)
            {
                manageAnimation.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        //collision detection
        public bool Contact(Sprite sprite)
        {
            //check if there's no color data
            if (this.TextureData == null)
            {
                return false;
            }
            if (sprite.TextureData == null)
            {
                return false;
            }

            //find the matrix when a turns into b (when bullet leave ship and hit chicken)
            Matrix transformAToB = this.Transform * Matrix.Invert(sprite.Transform);
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            //loop through each rectangle (scan pixels of both bullet and chicken)
            //if both pixel returns colors (bullet's pixels overlap the chicken's pixel)
            //then return true (one point deducted from the chicken)
            for (int yA = 0; yA < this.Rectangle.Height; yA++)
            {
                Vector2 posInB = yPosInB;
                for (int xA = 0; xA < this.Rectangle.Width; xA++)
                {
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);
                    if (0 <= xB && xB < sprite.Rectangle.Width && 0 <= yB && yB < sprite.Rectangle.Height)
                    {
                        Color colorA = this.TextureData[xA + yA * this.Rectangle.Width];
                        Color colorB = sprite.TextureData[xB + yB * sprite.Rectangle.Height];
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            return true;
                        }
                    }
                    posInB += stepX;
                }
                yPosInB += stepY;
            }
            return false;
        }

        public object Clone()
        {
            Sprite sprite = this.MemberwiseClone() as Sprite;
            return sprite;
        }
    }
}
