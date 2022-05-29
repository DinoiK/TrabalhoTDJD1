﻿using System;
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
       

        public Victory(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenSize.X / 2, Game1.ScreenSize.Y / 3f),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;
            _components = new List<Component>()
            {
                quitGameButton,
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            
                    _game.victoryBk.Draw(gameTime);
                    string victoryText = string.Format("Your Score: {0}\nHigh Score: {1}", PlayerStatus.Score, PlayerStatus.HighScore);
                    Vector2 textSize = Art.Font.MeasureString(victoryText);
                    spriteBatch.DrawString(Art.Font, victoryText, Game1.ScreenSize / 2 - textSize / 2, Color.White);
                   
             
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }


    }
}

