using HyVo_ChickenFPS.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace HyVo_ChickenFPS
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Random Random;

        public static int ScreenWidth = 1366;
        public static int ScreenHeight = 768;

        private State currentState;
        private State nextState;

        private Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Random = new Random();

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            song = Content.Load<Song>("Chicken Invaders 4 Full OBS");
            MediaPlayer.Play(song);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            // TODO: use this.Content to load your game content here
            currentState = new MainMenuState(this, Content);
            currentState.LoadContent();
            nextState = null;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                currentState = nextState;
                currentState.LoadContent();
                nextState = null;
            }

            // TODO: Add your update logic here
            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        public void ChangeState(State state)
        {
            nextState = state;
        }

        void MediaPlayer_MediaStateChanged(object sender, EventArgs args)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }
    }
}
