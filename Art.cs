using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Collections.Generic;

namespace MonoGame2
{
    /// <summary>
    /// A class for loading and storing all the textures in the game 
    /// </summary>
    class Art
    {
        private static Texture2D _player;
        private static Texture2D _police;
        private static Texture2D _ufo;
        private static Texture2D _bullet;
        private static Texture2D _pointer;
        private static Texture2D _NickAdams;
        private static Texture2D _GameOverBk;
        private static Texture2D _VictoryBk;
        private static SpriteFont _font;

        /// <summary>
        /// A helpful and tidy load method to load all the textures from a one line
        /// Art.Load() in the LoadContent() of Game
        /// </summary>
        /// <param name="game"></param>
        public static void Load(Game1 instance)
        {
            _player = instance.Content.Load<Texture2D>("Art\\Carro1");
            _police = instance.Content.Load<Texture2D>("Art\\CarroPolicia_anima");
            _ufo = instance.Content.Load<Texture2D>("Art\\GajoMau_anima");

            _bullet = instance.Content.Load<Texture2D>("Art\\Bullet");
            _pointer = instance.Content.Load<Texture2D>("Art\\Pointer");
            _NickAdams = instance.Content.Load<Texture2D>("Art\\NickAdams");
            _GameOverBk = instance.Content.Load<Texture2D>("Art\\GameOverBk");
            _VictoryBk = instance.Content.Load<Texture2D>("Art\\VictoryBk");
            _font = instance.Content.Load<SpriteFont>("Font");     
        }

        public static Texture2D NickAdams
        {
            get { return _NickAdams; }
        }

        public static Texture2D GameOverBk
        {
            get { return _GameOverBk; }
        }

        public static Texture2D VictoryBk
        {
            get { return _VictoryBk; }
        }

        public static Texture2D Player
        {
            get { return _player; }
        }

        public static Texture2D Police
        {
            get { return _police; }
        }

        public static Texture2D UFO
        {
            get { return _ufo; }
        }

        public static Texture2D Bullet
        {
            get { return _bullet; }
        }

        public static Texture2D Pointer
        {
            get { return _pointer; }
        }

        public static SpriteFont Font
        {
            get { return _font; }
        }
    }
}
