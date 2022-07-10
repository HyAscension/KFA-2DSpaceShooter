using HyVo_ChickenFPS.Managers;
using HyVo_ChickenFPS.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyVo_ChickenFPS.States
{
    public class GameState : State
    {
        private SpriteFont font;

        private ManageScore manageScore;

        private ManageEnemy manageEnemy;

        private List<Sprite> sprites;

        private List<Player> players;

        private Song song;

        public int PlayerCount;

        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
            song = content.Load<Song>("Battle Music");
        }

        public override void LoadContent()
        {
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            Texture2D playerTexture = content.Load<Texture2D>("Ships/CalorieGuy");
            Texture2D bulletTexture = content.Load<Texture2D>("Bullet");

            font = content.Load<SpriteFont>("Font");

            manageScore = ManageScore.Load();

            sprites = new List<Sprite>()
            {
                new Sprite(content.Load<Texture2D>("Background/Space"))
                {
                    Layer = 0.0f,
                    Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                }
            };

            Bullet massBullet = new Bullet(bulletTexture)
            {
                Explosion = new Explosion(new Dictionary<string, Models.Animation>()
                {
                    {
                        "Explode",
                        new Models.Animation(content.Load<Texture2D>("Explosion"), 1)
                        {
                            FrameSpeed = 0.1f,
                        }
                    }
                })
                {
                    Layer = 0.5f,
                }
            };

            if (PlayerCount >= 1)
            {
                sprites.Add(new Player(playerTexture)
                {
                    Position = new Vector2(100, Game1.ScreenHeight / 3),
                    Layer = 0.3f,
                    Bullet = massBullet,
                    Input = new Models.KeyInput()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Shoot = Keys.Space,
                    },
                    Health = 10,
                    Score = new Models.Score()
                    {
                        PlayerName = "Player 1",
                        Value = 0,
                    },
                });
            }

            if (PlayerCount >= 2)
            {
                sprites.Add(new Player(playerTexture)
                {
                    Color = Color.Lime,
                    Position = new Vector2(100, Game1.ScreenHeight / 2),
                    Layer = 0.4f,
                    Bullet = massBullet,
                    Input = new Models.KeyInput()
                    {
                        Up = Keys.Up,
                        Down = Keys.Down,
                        Left = Keys.Left,
                        Right = Keys.Right,
                        Shoot = Keys.Enter,
                    },
                    Health = 10,
                    Score = new Models.Score()
                    {
                        PlayerName = "Player 2",
                        Value = 0,
                    },
                });
            }

            players = sprites.Where(c => c is Player)
                .Select(c => (Player)c)
                .ToList();

            manageEnemy = new ManageEnemy(content)
            {
                Bullet = massBullet,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //draw sprites
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            //draw status
            spriteBatch.Begin();
            float x = 10f;
            foreach (Player player in players)
            {
                spriteBatch.DrawString(font, "Ship: " + player.Score.PlayerName, new Vector2(x, 10f), Color.White);
                spriteBatch.DrawString(font, "Health: " + player.Health, new Vector2(x, 30f), Color.White);
                spriteBatch.DrawString(font, "Score: " + player.Score.Value, new Vector2(x, 50f), Color.White);
                x += 150;
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            //exit game state
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ChangeState(new MainMenuState(game, content));
            }

            //else keep running and add enemies
            foreach (Sprite sprite in sprites)
            {
                sprite.Update(gameTime);
            }

            manageEnemy.Update(gameTime);

            int enemyCount = sprites.Where(c => c is Enemy).Count();

            if (manageEnemy.MoreChicken && enemyCount < manageEnemy.MaxEnemies)
            {
                sprites.Add(manageEnemy.GetEnemy());
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            var spritesContacted = sprites.Where(c => c is ICollidable);

            foreach (Sprite spriteA in spritesContacted)
            {
                foreach (Sprite spriteB in spritesContacted)
                {
                    //no action if both sprite are the same
                    if (spriteA == spriteB) continue;

                    if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea)) continue;

                    if (spriteA.Contact(spriteB))
                    {
                        ((ICollidable)spriteA).OnCollide(spriteB);
                    }
                        
                }
            }

            //add bullet sprites into the game
            int spriteCount = sprites.Count;
            for (int i = 0; i < spriteCount; i++)
            {
                Sprite sprite = sprites[i];
                foreach (Sprite child in sprite.Children)
                    sprites.Add(child);

                sprite.Children = new List<Sprite>();
            }

            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].IsRemove)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }

           //save score after player died
            if (players.All(p => p.IsDead))
            {
                foreach (Player player in players)
                    manageScore.Add(player.Score);

                ManageScore.Save(manageScore);

                game.ChangeState(new HighscoresState(game, content));
            }
        }

        void MediaPlayer_MediaStateChanged(object sender, EventArgs args)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }
    }
}
