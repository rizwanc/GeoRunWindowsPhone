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
    class Projectiles
    {
        //Variable to hold projectile texture
        public Texture2D texture;

        //Projectile psoition vector
        public Vector2 position;

        //Projectile speed
        private float projectileMoveSpeed;

        //Check if enemy is still active or not
        public Boolean Active;

        //Projectile constructor
        public Projectiles(ContentManager Content, Vector2 newPosition)
        {
            texture = Content.Load<Texture2D>("Levels/projectile"); ;
            position = newPosition;
            projectileMoveSpeed = 6f;
            Active = true;
        }

        //Get projectile X coordinate
        public int getXCoord()
        {
            return (int)position.X;
        }

        //Get projectile y coordinate
        public int getYCoord()
        {
            return (int)position.Y;
        }

        //Get projectile width
        public int Width
        {
            get { return texture.Width; }
        }

        //Get projectile width
        public int Height
        {
            get { return texture.Height; }
        }

        //Bounding box used for collision checking
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, Width, Height); }
        }

        //Projectile update method
        public void Update(GameTime gametime)
        {
            position.X -= projectileMoveSpeed;

            if (position.X <= 0)
            {
                this.Active = false;
            }
            else
            {
                this.Active = true;
            }
        }

        //Projectile draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, BoundingBox, Color.CornflowerBlue);
        }

    }
}
