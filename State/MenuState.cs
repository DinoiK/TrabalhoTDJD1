using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame2.Controls;

namespace MonoGame2.State
{
    public class MenuState : States
    {
        public List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Font"); 

            var startButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenSize.X / 2f, Game1.ScreenSize.Y / 3f),
                Text = "Start",
            };

            startButton.Click += startButton_Click;       

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenSize.X / 2, Game1.ScreenSize.Y / 2f),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                startButton,
                quitGameButton,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _game.menuBk.Draw(gameTime);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            _game.start = true;
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
