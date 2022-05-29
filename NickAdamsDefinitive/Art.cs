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
        private static Texture2D _SequelBk;
        private static Texture2D _VictoryBk;
        private static SpriteFont _font;
        private static Texture2D _storyBk;
        private static Texture2D _AlienPutin;
        private static Texture2D _BossFight;
        private static Texture2D _Fin;

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
            _AlienPutin = instance.Content.Load<Texture2D>("Art\\AlienPutin");
            _bullet = instance.Content.Load<Texture2D>("Art\\Bullet");
            _pointer = instance.Content.Load<Texture2D>("Art\\Pointer");
            _NickAdams = instance.Content.Load<Texture2D>("Art\\NickAdams");
            _storyBk = instance.Content.Load<Texture2D>("Art\\StoryBk");
            _BossFight = instance.Content.Load<Texture2D>("Art\\bucko");
            _Fin = instance.Content.Load<Texture2D>("Art\\fin");
            _SequelBk = instance.Content.Load<Texture2D>("Art\\theEnd");
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

        public static Texture2D StoryBk
        {
            get { return _storyBk; }
        }

        public static Texture2D FinBk
        {
            get { return _Fin; }
        }

        public static Texture2D BossFightBk
        {
            get { return _BossFight; }
        }

        public static Texture2D SequelBk
        {
            get { return _SequelBk; }
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

        public static Texture2D AlienPutin
        {
            get { return _AlienPutin; }
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
