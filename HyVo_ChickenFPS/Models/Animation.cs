using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyVo_ChickenFPS.Models
{
    public class Animation : ICloneable
    {
        public int FrameCount { get; set; }

        public Texture2D Texture { get; set; }

        public int FrameWidth => Texture.Width / FrameCount;

        public int FrameHeight => Texture.Height;

        public int CurrentFrame { get; set; }

        public bool IsLooping { get; set; }

        public float FrameSpeed { get; set; }

        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;
            FrameCount = frameCount;
            IsLooping = true;
            FrameSpeed = 0.2f;
        }

        public object Clone() => MemberwiseClone();
    }
}
