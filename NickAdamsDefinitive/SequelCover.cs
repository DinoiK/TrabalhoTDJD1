using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame2
{
    class SequelCover : Cover
    {
        public SequelCover(Game game, string image, Texture2D cover) : base(game, image, cover)
        {
            _game = game;
            _cover = cover;
            _sb = new SpriteBatch(_game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            _sb.Begin();

            _sb.Draw(_cover, new Vector2(0, Game1.ScreenSize.Y / 4 - 300), Color.White);

            _sb.End();

        }
    }
}

