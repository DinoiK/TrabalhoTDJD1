using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using IPCA.Camera;


namespace MonoGame2
{
    class Enemy : Entity
    {
        private const float inactiveTime = 500; // time before enemy can start moving
        private float timeUntilStart = inactiveTime; //track how long the enemy has waited
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>(); // list for storing behviours
        private Random rand = new Random();
        public int _pointValue;
        public int _damage;
        private Rectangle sourceRectangle;
        private int frameCounter = 0;     
        private Vector2 origin;

        public int PointValue
        {
            get
            {
                return _pointValue;
            }
            private set
            {
                _pointValue = value;
            }
        }

        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }
        // track when an enemy can start moving
        public bool IsActive
        {
            get
            {
                return timeUntilStart <= 0;
            }
        }

        public Enemy(Texture2D image, Vector2 position, int points, int damage)
        {
            this.image = image;
            Position = position;
            PointValue = points;
            Damage = damage;
            origin = new Vector2(image.Width / 2f, image.Height / 6f);
            sourceRectangle = new Rectangle(0, 0, image.Width, image.Height / 3);
            // this is transparent and not white so enemys start invisibile and will fade in
            // gradually until the IsActive switch returns true
            color = Color.Transparent;
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;

            if (frameCounter == 90)
                frameCounter = 0;

            if (frameCounter < 30)
               sourceRectangle.Y = image.Height / 3;

            else if (frameCounter < 60)
                sourceRectangle.Y = (int)(image.Height * (2f / 3f));

            else 
                sourceRectangle.Y = 0;

            // if the enemy has waited its inactive time start working through behaviours
            // otherwise handle the inactive time (fade the enemy in slowly)
            if (timeUntilStart <= 0)
            {
                // execute next bit of code in all assigned behaviours
                ApplyBehaviours();
            }
            else
            {
                // subtract number of milliseconds since last update was run
                timeUntilStart -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // slowly fade in the enemy over time each frame as timeUntilStart/inactiveTime 
                // approaches 0 (since timeUntilStart is getting smaller each frame) the multiplies
                // (1 - (timeUntilStart / inactiveTime) will slowly incease from 0 - 1 each frame.
                color = Color.White * (1 - (timeUntilStart / inactiveTime));
            }

            // update the position based on the set velocity multiplied by the time that's passed since
            // last update
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // the enemies will have a constant acceleration applied to them each update which will
            // result in the enemy speeding up to infinity over time. This is to limit that accelleration
            // to a terminal velocity. it's like setting friction.
            Velocity = Velocity * 0.9f;
        }

        // when this is run, it will kill off the enemy
        public void WasShot()
        {
            // expire the enemy since it was shot
            IsExpired = true;
            // the player has shot this enemy so increase the players multiplier and add points to the score
            PlayerStatus.IncreaseMultiplier();
            PlayerStatus.AddPoints(PointValue);

            // play and explosion sound
            // add some variblity to the explosion sound and randomly chaning pitch
            Sound.Explosion.Play(0.05f, rand.NextFloat(-0.2f, 0.2f), 0);
        }

        // method for handling enemy to enemy collisions
        // we want them to just bounce of each other
        public void HandleCollision(Enemy other, GameTime gameTime)
        {
            // the difference between the this vector and the other determines the direction that the
            // enemy should bounce toward (away from the other)
            Vector2 direction = Position - other.Position;
            // as the distance between the two enemies increases after a collision we reduce the acceleration
            // applied. As direction.lengthsquard gets larger as the two objects move away rom each other the
            // acceleration applied gradually gets smaller too
            Velocity += 60 * (direction / (direction.LengthSquared() + 1)) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // method for adding behaviours to the behaviour list
        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            // enumerables provide enumerators as a kind of "object" of self that keeps track of 
            // where it is at in the execution order
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            // iterate through the list of behaviours and move them along
            for (int i = 0; i < behaviours.Count; i++)
            {
                // movenext returns false when all the code in the enumerable has been executed within it's
                // created enumerator. so we'll remove the behaviour from the behaviours list if there is not
                // mode code to run. (in most cases behaviours run infinatley in while loops but just in case.
                if (behaviours[i].MoveNext() == false)
                {
                    behaviours.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        ///  a factory for creating Police Car enemies
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>       
        public static Enemy CreatePolice(Vector2 position, GameTime gameTime)
        {        
            Enemy enemy = new Enemy(Art.Police, position, 2, 2);
            enemy.AddBehaviour(enemy.FollowPlayer(1800f, gameTime));
            return enemy;
        }

        /// <summary>
        /// a factory for creating UFO enemies
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public static Enemy CreateUFO(Vector2 position, GameTime gameTime)
        {
            Enemy enemy = new Enemy(Art.UFO, position, 1, 1);
            enemy.AddBehaviour(enemy.MoveRandomly(3500f, gameTime));
            return enemy;
        }

        /// <summary>
        /// Behaviour to make the enemy follow the player
        /// </summary>
        /// <param name="acceleration"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        IEnumerable<int> FollowPlayer(float acceleration, GameTime gameTime)
        {
            while (true)
            {
                // get the vector for the ships position relative to the current postion
                // then scale it down to the acceleration value 
                // need to normalize this increase in velocity by time as well so that it runs the same on all machine speeds
                 Velocity += (Player.Instance.Position - Position).ScaleTo(acceleration) * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.055f;
                // if the enemy is moving (ie its active, then orientate itself to point to the player/the
                // direction it's moving in
                if (Velocity != Vector2.Zero)
                {
                    Orientation = (Player.Instance.Position - Position).ToAngleCar();       
                }
                yield return 0;
            }
        }

        /// <summary>
        /// Behaviour to make the enemy move randomly
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        IEnumerable<int> MoveRandomly(float speed, GameTime gameTime)
        {
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            while (true)
            {
                // set the direction initially after spawn then only run this once every 6 frames
                direction += rand.NextFloat(-0.1f, 0.1f);
                direction = MathHelper.WrapAngle(direction);

                for (int i = 0; i < 6; i++)
                {
                    Velocity += MathUtil.FromPolar(direction, speed) * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.05f;
                    
                    Orientation -= 6f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    yield return 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                spriteBatch.Draw(image, Camera.ToPixel(Position), sourceRectangle, Color.White, Orientation, origin, 1f, SpriteEffects.None, 0);
            }
        }
    }
}
