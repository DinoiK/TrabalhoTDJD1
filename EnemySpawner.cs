using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using IPCA.Camera;

namespace MonoGame2
{
    static class EnemySpawner
    {
        static Random rand = new Random();
        public static float inverseSpawnchance = 60;
        static float spawnFrequency = 15f;
        static float timeSinceLastSpawnRoll = 0;

        public static void Update(GameTime gameTime)
        {

            // if the player is alive and there are less than 200 entities on the screen
            // do this spawn roll every 100millis seconds
            if ((Player.Instance.IsDead == false || PlayerStatus.IsGameOver == true) && EntityManager.Count < 200 && timeSinceLastSpawnRoll > spawnFrequency)
            {
                // reset this back to 0 because we are rolling the spawn now
                timeSinceLastSpawnRoll = 0;

                // get a random number from 0 to the inverseSpawnChance number if that number is 0 spawn an enemy
                if (rand.Next((int)inverseSpawnchance) == 0 && PlayerStatus.IsGameOver == false)
                {
                    EntityManager.Add(Enemy.CreatePolice(GetSpawnPosition(), gameTime));
                }
                if (rand.Next((int)inverseSpawnchance) == 0)
                {
                    EntityManager.Add(Enemy.CreateUFO(GetSpawnPosition(), gameTime));
                }
            }

            // if the inverse spawn chance is more than 20 (lower inverse spawn chance means a higher chance for a spawn)
            // reduce the inverse spawn chance to make it more likely to spawn an enemy
            if (inverseSpawnchance > 20)
            {
                // multiply this by total seconds that has passed to normalize frequency of increase between faster and slower
                // computers. A machine with a faster update loop will have this code run more frequently compared to a slower machine
                // so we multiply this factor by time passed so a slower machine will get a larger decrement of the inverseSpawnChance
                // each loop compared to a faster machine which would get a smaller decrement since it's running more loops per second
                // thus normalizing this increase in spawn probability by time rather than frame rate
                inverseSpawnchance -= 0.5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // increase the timeSinceLastSawnRoll by the number of miliseconds that has passed since the last loop
            timeSinceLastSpawnRoll += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private static Vector2 GetSpawnPosition()
        {
            return new Vector2(Player.Instance.Position.X - 10, Player.Instance.Position.Y - 10);
        }

        public static void Reset()
        {
            inverseSpawnchance = 60;
        } 
    }
}
