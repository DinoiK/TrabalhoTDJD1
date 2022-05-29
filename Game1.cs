using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using IPCA.Camera;
using IPCA.TilingBackground;
using Microsoft.Xna.Framework.Input;
using MonoGame2.State;


namespace MonoGame2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {      
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public bool start = false;

        public bool next = false;

        public States _currentState;

        public Cover menuBk, victoryBk, gameOverBk;
     
        public Camera Camera { get; private set; }
        public TilingBackground gameBackground;
        
        public static Game _instance;

        

        public static int DeadEnemies { get; set; }

        public Game1()
        {
            _instance = this;

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            // these are to unlink it from MonoGame's default frame rate and to use variable timesteps for update cycles
            Instance.IsFixedTimeStep = false;
        }    

        /// <summary>
        /// A helpful property that returns a reference to this game object, this will be
        /// used when classes need a reference to the instance.
        /// </summary>
        public static Game Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// A reference to this object's Viewport
        /// </summary>
        public static Viewport Viewport
        {
            get { return Instance.GraphicsDevice.Viewport; }
        }

        /// <summary>
        /// The screen size of this object's viewport
        /// </summary>
        public static Vector2 ScreenSize
        {
            get { return new Vector2(Viewport.Width, Viewport.Height); }
        }
       
        protected override void Initialize()
        { 
            // here we define the screen
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
            graphics.IsFullScreen = true;

            Camera = new Camera(this, worldWidth: 30f);

            IsMouseVisible = true;

            // load the base initialize first
            base.Initialize();

            // all other initializations come after base
            EntityManager.Add(Player.Instance);

            
            MediaPlayer.Volume = 0.05f;

            // set the music to repeating
            MediaPlayer.IsRepeating = true;

            // play the music defined in the sound class
            MediaPlayer.Play(Sound.Music);   
            
        }

        protected override void LoadContent()
        {
           // Create a new SpriteBatch, which can be used to draw textures.
           spriteBatch = new SpriteBatch(GraphicsDevice);

            Art.Load(this);
            Sound.Load(Instance.Content);

            gameBackground = new TilingBackground(this, "Highway", new Vector2(27));
            menuBk = new CoverNickAdams(this, "NickAdams", Art.NickAdams);
            victoryBk = new VictoryCover(this, "VictoryBk", Art.VictoryBk);
            gameOverBk = new GameOverCover(this, "GameOverBk", Art.GameOverBk);

            _currentState = new MenuState(this, graphics.GraphicsDevice, Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Input.IsPressingExit())
                Exit();

            Input.Update();

            if (start && !PlayerStatus.IsGameOver || start && !PlayerStatus.Victory)
            {               
                _currentState = new Story(this, graphics.GraphicsDevice, Content);
            }
            if (next && !PlayerStatus.IsGameOver || next && !PlayerStatus.Victory)
            {
                _currentState = new GameState(this, graphics.GraphicsDevice, Content);
            }
            if (PlayerStatus.IsGameOver && !(_currentState is GameOver))
            {
                _currentState = new GameOver(this, graphics.GraphicsDevice, Content);
            }

            if (PlayerStatus.Victory && !(_currentState is Victory))
            {
                _currentState = new Victory(this, graphics.GraphicsDevice, Content);
            }


            _currentState.Update(gameTime);
            
            _currentState.PostUpdate(gameTime);

            Player.Instance.Won();

            base.Update(gameTime);
        }

        public void DrawRightAlignedString(string text, float y)
        {
            // this gets the width of the string in pixels
            float textWidth = Art.Font.MeasureString(text).X;
            // draws the string on the top right side of the screen 5 pixels from the right side
            spriteBatch.DrawString(Art.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            _currentState.Draw(gameTime, spriteBatch);   
            
            base.Draw(gameTime);
        }
    }
    }

