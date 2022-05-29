using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using IPCA.Camera;

namespace IPCA.TilingBackground
{
    public class TilingBackground
    {
        Texture2D _background;
        Vector2 _realSize;
        Game _game;
        SpriteBatch _sb;

        public TilingBackground(Game game, string texture, Vector2 realSize)
        {
            _game = game;
            _realSize = realSize;
            _background = game.Content.Load<Texture2D>(texture);
            _sb = new SpriteBatch(_game.GraphicsDevice);
        }

        public void Draw(GameTime gameTime)
        {
            Vector2 camTopLeft = Camera.Camera.Target() - Camera.Camera.Size() / 2f;
            Vector2 camBotRight = Camera.Camera.Target() + Camera.Camera.Size() / 2f;


            Vector2 bottomleft = new Vector2(
                 ((int)(camTopLeft.X / _realSize.X) - 1) * _realSize.X,
                 ((int)(camTopLeft.Y / _realSize.Y) - 1) * _realSize.Y
            );


            Vector2 topright = new Vector2(
                 ((int)(camBotRight.X / _realSize.X) + 1) * _realSize.X,
           ((int)(camBotRight.Y / _realSize.Y) + 1) * _realSize.Y
            );


            _sb.Begin();

            for (float x = bottomleft.X; x <= topright.X; x += _realSize.X)
            {
                for (float y = bottomleft.Y; y <= topright.Y; y += _realSize.Y)
                {
                    Rectangle outRectangle = new Rectangle(Camera.Camera.ToPixel(new Vector2(x, y)).ToPoint(),
                                                    (Camera.Camera.ToLength(_realSize) + Vector2.One).ToPoint());
                    _sb.Draw(_background,  // texture
                        outRectangle,   // drawing position
                        null, Color.White, 0f,  // source, color, rotation 
                        _background.Bounds.Size.ToVector2() / 2f, // origin / anchor
                        SpriteEffects.None, 0);
                }
            }
            _sb.End();
        }
    }
}