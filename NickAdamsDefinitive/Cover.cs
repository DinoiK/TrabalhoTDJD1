using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MonoGame2
{
    public abstract class Cover
    {
       protected Texture2D _cover;
       protected Game _game;
       protected SpriteBatch _sb;

        public Cover(Game game, string image, Texture2D cover) 
        {
            _game = game;
            _cover = cover;
            _sb = new SpriteBatch(_game.GraphicsDevice);  
        }

        public virtual void Draw(GameTime gameTime)
        {
            _sb.Begin();

            _sb.Draw(_cover, new Vector2(Game1.ScreenSize.X, Game1.ScreenSize.Y), Color.White);

            _sb.End();
            
        }   
    }
}
