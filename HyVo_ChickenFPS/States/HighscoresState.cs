using HyVo_ChickenFPS.Controls;
using HyVo_ChickenFPS.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyVo_ChickenFPS.States
{
    public class HighscoresState : State
    {
        private SpriteFont font;

        private List<Component> components;

        private ManageScore scoreManager;

        public HighscoresState(Game1 game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            font = content.Load<SpriteFont>("Font");

            scoreManager = ManageScore.Load();

            Texture2D buttonTexture = content.Load<Texture2D>("Button");
            SpriteFont buttonFont = content.Load<SpriteFont>("Font");

            components = new List<Component>()
            {
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Main Menu",
                    Layer = 0.1f,
                    Position = new Vector2(Game1.ScreenWidth / 2, 500),
                    Click = new EventHandler(Button_MainMenu_Clicked),
                }
            };
        }

        private void Button_MainMenu_Clicked(object sender, EventArgs args)
        {
            game.ChangeState(new MainMenuState(game, content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            var highscores = scoreManager.HighScores
                .Select(c => c.PlayerName + ": " + c.Value);

            spriteBatch.DrawString(font, "Highscores:\n" + string.Join("\n",highscores), new Vector2(400, 100), Color.White);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Button_MainMenu_Clicked(this, new EventArgs());
            }

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
