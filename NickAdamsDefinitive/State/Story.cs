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
    class Story : States
    {
        public List<Component> _components;

        public Story(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            var startButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.ScreenSize.X / 2f - 70, Game1.ScreenSize.Y / 2f + 170),
                Text = "Next",
            };

            startButton.Click += startButton_Click;

            _components = new List<Component>()
            {
                startButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _game.storyBk.Draw(gameTime);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            _game.next = true;
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);

            _game.start = false;
        }
    }
}
