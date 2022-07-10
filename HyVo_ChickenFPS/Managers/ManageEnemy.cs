using System;
using System.Collections.Generic;
using System.Text;
using HyVo_ChickenFPS.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyVo_ChickenFPS.Managers
{
    public class ManageEnemy
    {
        private float counter;
        private List<Texture2D> enemyChicken;

        public bool MoreChicken { get; set; }
        public Bullet Bullet;
        public int MaxEnemies { get; set; }
        public float SpawnTimer { get; set; }

        public ManageEnemy(ContentManager content)
        {
            enemyChicken = new List<Texture2D>()
            {
                content.Load<Texture2D>("Ships/RedChicken"),
                content.Load<Texture2D>("Ships/BlueChicken"),
            };

            MaxEnemies = 10;

            SpawnTimer = 2.5f;
        }

        public Enemy GetEnemy()
        {
            Texture2D texture = enemyChicken[Game1.Random.Next(0, enemyChicken.Count)];

            return new Enemy(texture)
            {
                Layer = 0.2f,
                Position = new Vector2(Game1.ScreenWidth + texture.Width, Game1.Random.Next(0, Game1.ScreenHeight)),
                Speed = 2 + (float)Game1.Random.NextDouble(),
                ShootTimer = 1.5f + (float)Game1.Random.NextDouble(),
                Bullet = Bullet,
                Health = 5,
            };
        }

        public void Update(GameTime gameTime)
        {
            counter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            MoreChicken = false;

            if (counter > SpawnTimer)
            {
                MoreChicken = true;
                counter = 0;
            }
        }
    }
}
