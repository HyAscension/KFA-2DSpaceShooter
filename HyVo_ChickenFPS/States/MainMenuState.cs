using System;
using System.Collections.Generic;
using System.Text;
using HyVo_ChickenFPS.Controls;
using HyVo_ChickenFPS.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HyVo_ChickenFPS.States
{
    public class MainMenuState : State
    {
        private List<Component> components;

        int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

        public MainMenuState(Game1 game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            Texture2D buttonTexture = content.Load<Texture2D>("Button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Font");

            Texture2D backgroundImage = content.Load<Texture2D>("Background/MainMenu_480");


            int[] heightPosition = new int[4];

            heightPosition[0] = 275;
            heightPosition[1] = 325;
            heightPosition[2] = 375;
            heightPosition[3] = 425;

            components = new List<Component>()
            {
                new Sprite(backgroundImage)
                {
                    Layer = 0.0f,
                    Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Singleplayer",
                    Layer = 0.1f,
                    Position = new Vector2(Game1.ScreenWidth / 2, heightPosition[0]),
                    Click = new EventHandler(Button_1stPlayer_Clicked),
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Multiplayer",
                    Position = new Vector2(Game1.ScreenWidth / 2, heightPosition[1]),
                    Click = new EventHandler(Button_2ndPlayer_Clicked),
                    Layer = 0.1f,
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Highscores",
                    Layer = 0.1f,
                    Position = new Vector2(Game1.ScreenWidth / 2, heightPosition[2]),
                    Click = new EventHandler(Button_Highscores_Clicked),
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Quit",
                    Layer = 0.1f,
                    Position = new Vector2(Game1.ScreenWidth / 2, heightPosition[3]),
                    Click = new EventHandler(Button_Quit_Clicked),
                },
            };
        }

        private void Button_1stPlayer_Clicked(object sender, EventArgs args)
        {
            GameState stPlayer = new GameState(game, content);
            stPlayer.PlayerCount = 1;
            game.ChangeState(stPlayer);
        }

        private void Button_2ndPlayer_Clicked(object sender, EventArgs args)
        {
            GameState ndPlayer = new GameState(game, content);
            ndPlayer.PlayerCount = 2;
            game.ChangeState(ndPlayer);
        }

        private void Button_Highscores_Clicked(object sender, EventArgs args)
        {
            HighscoresState highscoresState = new HighscoresState(game, content);
            game.ChangeState(highscoresState);
        }

        private void Button_Quit_Clicked(object sender, EventArgs args)
        {
            game.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in components)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }
    }
}
