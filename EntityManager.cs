using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame2
{
    /// <summary>
    /// A class for managing the updating and drawing of all the entities
    /// A static class is one that cannot be instantiated so only one of this 
    /// class can exist at any time during run time.
    /// </summary>
    static class EntityManager
    {
        // a list to store entity objects
        static List<Entity> entities = new List<Entity>();
        // a list to store all currently active enemies for collision detection
        static List<Enemy> enemies = new List<Enemy>();
        // a list to store all currently active bullets for collision detection
        static List<Bullet> bullets = new List<Bullet>();

        /// <summary>
        /// the isUpdating and addedEntities variables are used to handle the
        /// creation of new entities while the update method is running. If we try
        /// to update the entities list while iterating through it. we will get 
        /// an exception.
        /// 
        /// So to get around this we will instead store any of the newly 
        /// generated entities into the addedEntities list first and then
        /// add them in to the entities list on the next update cycle rather than
        /// try to add them in while iterating through the list of entities.
        /// </summary>
        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        /// <summary>
        /// a property that returns the number of entities at any one time
        /// you could probably do this as a method as well like "getCount()"
        /// but this seems to be a nice way to do this, user can call it with
        /// EntityManager.Count
        /// </summary>
        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }

        private static void AddEntity(Entity entity)
        {
            // add the entity to the general entities list this is to manage
            // draw and update methods
            entities.Add(entity);

            // also add the entity to it's relevent list bullet or enemy so
            // we can handle collision
            if (entity is Bullet)
            {
                bullets.Add(entity as Bullet);
            }
            else if (entity is Enemy)
            {
                enemies.Add(entity as Enemy);
            }       
        }

        /// <summary>
        /// If this method is called while we are iterating through the list
        /// of entities (like in the update method) then it will throw an exception
        /// so we only add to the entities list if we are not updating otherwise 
        /// we will add the entity in the addedEntities list instead
        /// </summary>
        /// <param name="entity"></param>
        public static void Add(Entity entity)
        {
            if (isUpdating == false)
            {
                // use the add entity method to correctly add the entity to the required lists
                AddEntity(entity);
            }
            else
            {
                // add entity to the que
                addedEntities.Add(entity);
            }
        }

        public static void ClearEnemies()
        {
            // this is a linq function that iterates through the list and runs the was shot 
            // method on all enemies in the enemies list to kill them off
            enemies.ForEach(enemy => enemy.IsExpired = true);
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            // find the sum of the two entities radiuses
            float radius = a.Radius + b.Radius;
            
            // return true if both entities are not expired (if they are expired they will disappear anyway) and
            // who's sum of radiuses is less than the distance between them (thus overlapping) 
            // we use distance squared here and square the radius in conjuction with this because it is faster to 
            // compute than using just the actual distance
            return (a.IsExpired == false) && (b.IsExpired == false) && (Vector2.DistanceSquared(a.Position, b.Position) < radius * radius);
        }

        static void HandleCollisions(GameTime gameTime)
        {

            // handle collisions between enemies for each enemy in the enemies list
            for (int enemyOneIndex = 0; enemyOneIndex < enemies.Count; enemyOneIndex++)
            {
                // compare it with all other enemies in the list starting with
                // the next one along in the list. note we don't need to compare it with
                // an enemy that is behind it in the list
                for (int enemyTwoIndex = enemyOneIndex + 1; enemyTwoIndex < enemies.Count; enemyTwoIndex++)
                {
                    if (IsColliding(enemies[enemyOneIndex], enemies[enemyTwoIndex]))
                    {
                        enemies[enemyOneIndex].HandleCollision(enemies[enemyTwoIndex], gameTime);
                        enemies[enemyTwoIndex].HandleCollision(enemies[enemyOneIndex], gameTime);
                    }
                }
            }

            // handle collisions between bullets and enemies
            // for each enemy in the enemies list
            foreach (Enemy enemy in enemies)
            {
                // compare it with every bullet in the bullets list
                foreach (Bullet bullet in bullets)
                {
                    // if the two entities have collided then execute the wasshot code for the enemy
                    // and destroy the bullet
                    if (IsColliding(enemy, bullet))
                    {
                        Game1.DeadEnemies++;
                        enemy.WasShot();
                        bullet.IsExpired = true;
                    }
                }
            }

            foreach (Enemy enemy in enemies)
            {
                // if the enemy is active and has collided with the player
                if (enemy.IsActive && IsColliding(Player.Instance, enemy) && !PlayerStatus.IsGameOver)
                {
                    // run the damage method on the player
                    Player.Instance.DamagePlayer(enemy.Damage);
                    // kill the enemy off when the player is damaged
                    enemy.IsExpired = true;

                    break;
                }
            }
        }

        /// <summary>
        /// The Update method runs through all the entities in the entities list
        /// and runs thier respective update methods.
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            // set this to true as soon as we start running just before running through
            // the entities list to run all the update methods
            isUpdating = true;

            // first handle all the collisions that might be happening between entities
            HandleCollisions(gameTime);

            //update all entities
            foreach (Entity entity in entities)
            {
                entity.Update(gameTime);
            }

            // set back to false after running all the update methods
            isUpdating = false;

            // now that all the update methods have finished running, we can add 
            // all the qued up, newly created entites to the entities list
            foreach (Entity entity in addedEntities)
            {
                AddEntity(entity);
            }
            // empty out the addedEntities list since they have now been added to the
            // production entities list
            addedEntities.Clear();

            
            // essentially we are removing all the entities that have been destroyed
            // since these entities are no longer in the entities list, their update
            // and draw methods will no longer be called
            entities = entities.Where(entity => entity.IsExpired == false).ToList();
            bullets = bullets.Where(bullet => bullet.IsExpired == false).ToList();
            enemies = enemies.Where(enemy => enemy.IsExpired == false).ToList();
        }

        /// <summary>
        /// method for running through all the draw methods of all the entities
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity entity in entities)
            {
                entity.Draw(spriteBatch);
            }
        }
    }
}
