using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace MonoGame2.State
{
    public class GameState : States
    {
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int eneminesRemaning = Math.Abs(Game1.DeadEnemies - 25);

            spriteBatch.Begin(SpriteSortMode.Texture);

            EntityManager.Draw(spriteBatch);

            _game.gameBackground.Draw(gameTime);

            // draw the players lives and hp on the top left of the screen 5,5
            spriteBatch.DrawString(Art.Font, string.Format("Hp: {0}", PlayerStatus.Hp), new Vector2(5), Color.White);
            spriteBatch.DrawString(Art.Font, string.Format("Lives: {0}", PlayerStatus.Lives), new Vector2(5, 45), Color.White);
            _game.DrawRightAlignedString(string.Format("Score: {0}", PlayerStatus.Score), 5);
            _game.DrawRightAlignedString(string.Format("Multiplier: X{0}", PlayerStatus.Multiplier), 35);
            _game.DrawRightAlignedString(string.Format("Enemies remaing: {0}", eneminesRemaning), 70);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.IsPressingExit())
                _game.Exit();

            Input.Update();

            EntityManager.Update(gameTime);
            EnemySpawner.Update(gameTime);
            PlayerStatus.Update(gameTime);

            _game.next = false;

        }
    }
}
