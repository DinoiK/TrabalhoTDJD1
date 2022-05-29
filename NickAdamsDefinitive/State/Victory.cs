using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame2.Controls;
using MonoGame2.State;


namespace MonoGame2.State
{
    class Victory : States
    {
        private List<Component> _components;
        private int cutsceneCounter = 0;

        public Victory(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Font");
      
                var nextGameButton = new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(Game1.ScreenSize.X / 3, Game1.ScreenSize.Y / 2),
                    Text = "Continue",
                };

                nextGameButton.Click += NextGameButton_Click;

            _components = new List<Component>()
            {
                nextGameButton,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            switch (cutsceneCounter)
            {
                case 0:
                    _game.victoryBk.Draw(gameTime);
                    string victoryText = string.Format("Your Score: {0}\nHigh Score: {1}", PlayerStatus.Score, PlayerStatus.HighScore);
                    Vector2 textSize = Art.Font.MeasureString(victoryText);
                    spriteBatch.DrawString(Art.Font, victoryText, Game1.ScreenSize / 2 - textSize / 2, Color.White);
                    break;
                case 1:
                    _game.sequelBk.Draw(gameTime);
                    break;
                case 2:
                    _game.bossBk.Draw(gameTime);
                    break;
            }   
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);

            if (cutsceneCounter == 3)
                PlayerStatus.trueEnding = true;
        }

        private void NextGameButton_Click(object sender, EventArgs e)
        {
            cutsceneCounter++; 
        }
    }
}

