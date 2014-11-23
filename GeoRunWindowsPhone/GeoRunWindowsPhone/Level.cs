using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GeoRunWindowsPhone
{
    class Level
    {
        private Player player = new Player();
        private Patterns patterns = new Patterns();
        private String levelID;
        private ContentManager content;

        //Enemy arrow list
        public List<Projectiles> projectileList;

        //The rate at which the projectiles appear
        public TimeSpan projectileSpawnTime;
        public TimeSpan previousSpawnTime;

        //Random variable to generate coordinates
        public Random myRand;

        //This is the veolocity that all of the patterns will move to the left
        private float velocity = -10.0f;
        // Player's Score
        private int playerScore;
        private int level;

        //Particle engine 
        ParticleEngine particleEngine;

        //if game is over make the velocity 0
        public void gameOver()
        {
            velocity = 0.0f;
        }

        // Change the velocity Manually, also returns the velocity
        public float ChangeVelocity(float inV) { this.velocity = inV; return this.velocity; }

        //Returns the player object
        public Player getPlayer()
        {
            return player;
        }
        //sets up the patterns and the player
        public void Initialize(ContentManager Content, String levelName)
        {
            levelID = levelName;
            patterns.Initialize(Content, velocity, "Pattern1.txt");
            player.Initialize(Content);
            // Initialize Score and score increment
            playerScore = 0;
            level = 1;

            // projectileList = new List<Projectiles>();
            //projectileSpawnTime = TimeSpan.FromSeconds(1.0f);
            //previousSpawnTime = TimeSpan.Zero;
            myRand = new Random();

            this.content = Content;

            //particle engine
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("Characters/circle"));
            textures.Add(Content.Load<Texture2D>("Characters/star"));
            textures.Add(Content.Load<Texture2D>("Characters/diamond"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));


        }
        //Will return true if game is over and false if its not
        public Boolean Update(GameTime gameTime, ContentManager Content)
        {
            patterns.Update(gameTime, Content);
            player.Update(gameTime);
            UpdateParticles();
            //UpdateProjectiles(gameTime);
            UpdateGameAttributes();
            Boolean gameOver = DetectCollision();

            if (player.Position.Y > 1000)
            {
                patterns.addDeath(patterns.getPatternID(player.Position));
                return true;
            }
            return gameOver;
        }

        //Draws the tiles and player to the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            patterns.Draw(spriteBatch);
            player.Draw(spriteBatch);
            particleEngine.Draw(spriteBatch);
            // Draw the projectiles
            // for (int i = 0; i < projectileList.Count; i++)
            //  {
            // projectileList[i].Draw(spriteBatch);

            //   if (player.BoundingBoxShield.Intersects(projectileList[i].BoundingBox))
            //    {
            //    if (player.isBlocking == true)
            //     {
            //projectileList[i].position.X += 800;
            // projectileList[i].Active = false;
            //    }
            //  }
            //  }


            //patterns.CheckingBetween(spriteBatch);
        }

        private void addProjectiles()
        {
            Vector2 position = new Vector2(800 + 64 / 2, myRand.Next(100, 480 - 100));

            Projectiles newProjectile = new Projectiles(this.content, position);

            projectileList.Add(newProjectile);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > projectileSpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                addProjectiles();
            }

            // Update the Enemies
            for (int i = projectileList.Count - 1; i >= 0; i--)
            {
                projectileList[i].Update(gameTime);

                if (projectileList[i].Active == false)
                {
                    projectileList.RemoveAt(i);
                }
            }
        }

        //Checks to see if the player position is inside of tiles
        public Boolean DetectCollision()
        {
            Vector2 playerPos = player.Position;
            playerPos.X = playerPos.X + player.Width;
            playerPos.Y = playerPos.Y + player.Height;

            //bottom right corner 
            Tile bottomRightTile = patterns.getPattern(playerPos);
            //bottom middle
            Tile bottomMiddleTile = patterns.getPattern(new Vector2(playerPos.X - (player.Width / 2), playerPos.Y));
            //top right
            Tile topRightTile = patterns.getPattern(new Vector2(playerPos.X, playerPos.Y - player.Height));
            //if this is not null then there is a collision
            if (bottomRightTile != null)
            {
                player.State = Player.PlayerState.onGround;
                //sets the player position to be above the ground to avoid game over detection
                player.Position = new Vector2(player.Position.X, patterns.getPattern(playerPos).getPos().Y - player.Height);
                //checks if the player is inside of a tile in 3 places
                //top right
                Tile gameOverTile1 = patterns.getPattern(new Vector2(playerPos.X, player.Position.Y + player.Height));
                //middle right
                Tile gameOverTile2 = patterns.getPattern(new Vector2(playerPos.X, player.Position.Y + player.Height - (player.Height / 4)));
                //bottom right
                Tile gameOverTile3 = patterns.getPattern(new Vector2(playerPos.X, player.Position.Y));
                // If player is inside of any tiles then its game over

                if (gameOverTile1 != null || gameOverTile2 != null)
                {
                    if (gameOverTile1 != null)
                    {
                        if (gameOverTile1.GetTileID() == 5 && player.Action == Player.PlayerAction.attacking)
                        {
                            playerScore += 10;
                            return false;
                        }
                        else
                        {
                            patterns.addDeath(gameOverTile1.GetPatternID());
                            return true;
                        }
                    }
                    if (gameOverTile2 != null)
                    {
                        if (gameOverTile2.GetTileID() == 5 && player.Action == Player.PlayerAction.attacking)
                        {
                            playerScore += 10;
                            return false;
                        }
                        else
                        {
                            patterns.addDeath(gameOverTile2.GetPatternID());
                            return true;
                        }
                    }
                }

            }
            else if (bottomMiddleTile != null)
            {
                player.State = Player.PlayerState.onGround;
                player.Position = new Vector2(player.Position.X, bottomMiddleTile.getPos().Y - player.Height);
            }
            else if (topRightTile != null && player.State == Player.PlayerState.falling)
            {
                player.State = Player.PlayerState.hitCeiling;
                player.Position = new Vector2(player.Position.X, topRightTile.getPos().Y + 32);
                //checks if the player is inside of a tile in 3 places
                //bottom right
                Tile gameOverTile1 = patterns.getPattern(new Vector2(playerPos.X, player.Position.Y));
                //middle right
                Tile gameOverTile2 = patterns.getPattern(new Vector2(playerPos.X, player.Position.Y + player.Height / 2));
                // If player is inside of any tiles then its game over

                if (gameOverTile1 != null || gameOverTile2 != null)
                {
                    if (gameOverTile1 != null)
                    {
                        if (gameOverTile1.GetTileID() == 5 && player.Action == Player.PlayerAction.attacking)
                        {
                            playerScore += 10;
                            return false;
                        }
                        else
                        {
                            patterns.addDeath(gameOverTile1.GetPatternID());
                            return true;
                        }
                    }
                    if (gameOverTile2 != null)
                    {
                        if (gameOverTile2.GetTileID() == 5 && player.Action == Player.PlayerAction.attacking)
                        {
                            playerScore += 10;
                            return false;
                        }
                        else
                        {
                            patterns.addDeath(gameOverTile2.GetPatternID());
                            return true;
                        }
                    }
                }
            }
            else if (topRightTile != null && player.Action != Player.PlayerAction.jumping)
            {
                Console.WriteLine("IT HIT");
                patterns.addDeath(topRightTile.GetPatternID());
                return true;
            }
            // else if infront of player is tile
            // Check if it is a 1 or 2 tile
            else if (bottomMiddleTile == null)
            {
                if (player.Action != Player.PlayerAction.jumping)
                {
                    player.State = Player.PlayerState.falling;
                }
                //checks if the player is inside of a tile in 3 places
                Tile gameOverTile1 = patterns.getPattern(new Vector2(player.Position.X + player.Width, player.Position.Y + player.Height));
                Tile gameOverTile2 = patterns.getPattern(new Vector2(player.Position.X + player.Width, player.Position.Y + player.Height / 2));
                Tile gameOverTile3 = patterns.getPattern(new Vector2(player.Position.X + player.Width, player.Position.Y));
                //if player is inside of any tiles then its game over
                if (gameOverTile1 != null || gameOverTile2 != null || gameOverTile3 != null)
                {
                    if (gameOverTile1 != null)
                    {
                        patterns.addDeath(gameOverTile1.GetPatternID());
                    }
                    if (gameOverTile2 != null)
                    {
                        patterns.addDeath(gameOverTile2.GetPatternID());
                    }
                    if (gameOverTile3 != null)
                    {
                        patterns.addDeath(gameOverTile3.GetPatternID());
                    }
                    return true;
                }
            }
            else
            {
                player.State = Player.PlayerState.falling;
                //checks if the player is inside of a tile in 3 places
                Tile gameOverTile1 = patterns.getPattern(new Vector2(player.Position.X + player.Width, player.Position.Y + player.Height));
                Tile gameOverTile2 = patterns.getPattern(new Vector2(player.Position.X + player.Width, player.Position.Y + player.Height / 2));
                Tile gameOverTile3 = patterns.getPattern(new Vector2(player.Position.X + player.Width, player.Position.Y));
                //if player is inside of any tiles then its game over
                if (gameOverTile1 != null || gameOverTile2 != null || gameOverTile3 != null)
                {
                    if (gameOverTile1 != null)
                    {
                        patterns.addDeath(gameOverTile1.GetPatternID());
                    }
                    if (gameOverTile2 != null)
                    {
                        patterns.addDeath(gameOverTile2.GetPatternID());
                    }
                    if (gameOverTile3 != null)
                    {
                        patterns.addDeath(gameOverTile3.GetPatternID());
                    }
                    return true;
                }
            }

            //Check for collision between projectile and player
            /* for (int i = 0; i < projectileList.Count; i++)
             {
                 if (player.BoundingBox.Intersects(projectileList[i].BoundingBox))
                 {
                     if (player.isBlocking == false)
                     {
                         projectileList[i].Active = false;
                         return true;
                     }
                 }
             }*/

            return false;
        }
        private void UpdateGameAttributes()
        {
            playerScore += 1;
        }

        private void UpdateParticles()
        {
            if (playerScore < 1000)
            {
            }
            else if (1000 <= playerScore && playerScore < 2000)
            {
                particleEngine.EmitterLocation = player.Position;
                particleEngine.Update(1, 10, 1);
            }
            else if (2000 <= playerScore && playerScore < 3000)
            {
                particleEngine.EmitterLocation = player.Position;
                particleEngine.Update(2, 15, 2);
            }
            else if (3000 <= playerScore && playerScore < 4000)
            {
                particleEngine.EmitterLocation = player.Position;
                particleEngine.Update(3, 20, 3);
            }
            else if (4000 <= playerScore && playerScore < 5000)
            {
                particleEngine.EmitterLocation = player.Position;
                particleEngine.Update(4, 25, 3);
            }
            else if (5000 <= playerScore && playerScore < 6000)
            {
                particleEngine.EmitterLocation = player.Position;
                particleEngine.Update(5, 30, 4);
            }
        }





        public int getPlayerScore() { return (int)playerScore; }
        public int getLevel() { return level; }
        public float getSpeed() { return -velocity; }

    }
}
