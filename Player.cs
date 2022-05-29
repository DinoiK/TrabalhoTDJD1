using System;
using IPCA.Camera;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MonoGame2
{
    class Player : Entity
    {
        private static Player _instance;
        const float shootCooldown = 200; //number of milli seconds between shots
        float cooldownRemaining = 0; // keep track of how many frames has past since last shot
        static Random rand = new Random(); // need this to generate random floats
        float timeUntilRespawn = 0;
        public bool win = false;

        // the player is dead if there is some time left on his respawn timer
        public bool IsDead
        {
            get
            {
                return timeUntilRespawn > 0;
            }
        }

        // if the player instance property hasn't already been created then create one
        // and return that value, otherwise just return the object that had been assigned
        // earlier
        public static Player Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Player();
                }

                return _instance;
            }
        }

        /// <summary>
        /// it inherits everything from the parent class "entity"
        /// and we define anything else we would want as well then it can be used
        /// as the creation of the player class. 
        /// </summary>
        private Player()
        {
            image = Art.Player;
            Position = Game1.ScreenSize / 2;
           // Radius = 10;
        }

        // if the player is killed set the respawn timer to 1 second
        public void Kill()
        {
             EntityManager.ClearEnemies();
              //remove a life from the player
              PlayerStatus.RemoveLife();
                
            // turn on the game over state if the player has 0 lives
            if (PlayerStatus.Lives < 1)
            {
                PlayerStatus.IsGameOver = true;
            }
            
            // if player has died but is not in game over state then make him wait for 1s
            if (!PlayerStatus.IsGameOver)
            {
                timeUntilRespawn = 1000;
            }
            if (PlayerStatus.Hp == 0)
                PlayerStatus.AddHp(5);
            else
                PlayerStatus.AddHp(6);

            // reset the enemy spawner
            EnemySpawner.Reset();
        }

        public void Won()
        {
            if(!PlayerStatus.IsGameOver && Game1.DeadEnemies >= 25)
            {
                PlayerStatus.Victory = true;
            }
        }


        /// <summary>
        /// If an enemy collides with a player he will lose hp
        /// </summary>
        public void DamagePlayer(int dm)
        {
            PlayerStatus.RemoveHp(dm);

            if (PlayerStatus.Hp <= 0)
                     Kill();
        }

        public override void Update(GameTime gameTime)
        {
            // if the game is in a game over state, don't do anything just wait until the player presses
            // A or Enter before continuing on with a new game
            if (PlayerStatus.IsGameOver)
            {
                // this is the time before the player with spawn again into a new game after he presses the A
                // button or the enter key aftter a game over screen
                timeUntilRespawn = 250;

                if (Input.WasKeyPressed(Keys.Enter))
                {
                    // set the game over switch to false and reset all the player stats to start a new game
                    PlayerStatus.IsGameOver = false;
                    EntityManager.ClearEnemies();
                    PlayerStatus.Reset();
                }
                // keep exiting the update loop immediately since the player is in a game over state
                return;
            }

            // if the player is dead subtract some time away from the respawn timer
            // eventually the respawn time will get to 0 and the IsDead getter will return
            // true. immediatly exit out of the players update loop after doing this since
            // the player can't do anything when hes dead.
            if (IsDead)
            {
                timeUntilRespawn -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                return;
            }
          
            const float speed = 15; // speed is the multiplier for the movement direction
            Velocity = speed * Input.GetMovementDirection(); // velocity is the final delta value to move the player from his current position to his new position between update cycles
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Position = Vector2.Clamp(Position, Size / 2, Game1.ScreenSize - Size / 2);

            // here we are using our helper extension function from the extensions.cs file
            // to return radians of velocity so that we can assign the orientation value but
            //we are using one specific for the car because rotation would be inversed otherwise
            if (Velocity.LengthSquared() > 0)
            {
                Orientation = Velocity.ToAngleCar();
            }

            Vector2 aim = Input.GetAimDirection();

            // if the player is aiming in a direction and there is no cooldown left on shooting then
            // create bullets objects, we can also add in a requirement for a button down here as well
            // at the moment as long as the player has an aim button pushed (arrow keys, mouse movement)
            // then we will shoot.
            if (aim.LengthSquared() > 0 && cooldownRemaining <= 0 && gameTime.TotalGameTime.TotalMilliseconds >= 500 && Input.IsPressingShoot())
            {
                cooldownRemaining = shootCooldown; // reset the cooldown remaining to full cd
                float aimAngle = aim.ToAngleCar(); // get the angle that the bullets should be moving in
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle); // a quanternion is used for determining orientation in a 3D space and is required by the vector2 transform method

                //float randomSpread = rand.NextFloat(-0.3f, 0.3f); // generate some variability in the bullet angle to give it a machinegun effect
                Vector2 bulletVelocity = MathUtil.FromPolar(aimAngle /* + randomSpread*/, 20f); // generate the velocity of the bullet based on the angle it leaves the ship at plus a magnitude for bullet speed

                // the starting position of the bullet is the position of the player car plus an offset of 25pix in the x axis  
                // and 8 pix in the y axis but oriented in the aim direction of the player 
                // B   B
                //   P    
                Vector2 offset = Vector2.Transform(new Vector2(1, 0.06f), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, bulletVelocity));

                offset = Vector2.Transform(new Vector2(1, -0.06f), aimQuat);
                EntityManager.Add(new Bullet(Position + offset, bulletVelocity));

                // play a sound for shooting bullets!
                // add some variability to the pitch on each play
                Sound.Shot.Play(0.05f, rand.NextFloat(-0.2f, 0.2f), 0);
            }

            // reduce the cooldown remaining by 1 frame
            if (cooldownRemaining > 0)
            {
                cooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            
            //Check camera position
            Camera.LookAt(Position);
        }

        // only draw the player if he is alive
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead == false)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
