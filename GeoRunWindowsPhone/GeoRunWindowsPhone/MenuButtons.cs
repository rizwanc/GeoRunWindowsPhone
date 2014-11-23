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
    class MenuButtons
    {
        //Variable to hold the button texture
        Texture2D buttonTexture;

        //Vector2 variable to hold the position of the button
        public Vector2 buttonPosition;

        //Rectangle variable to which we draw the button on
        public Rectangle buttonRectangle;
        public Rectangle mouseRectange;
        //Color variable to allow the button to flash
        Color color = new Color(255, 255, 255, 255);

        //Vector2 variable to hold the size of the button
        public Vector2 buttonSize;

        //Boolean variable to allow the button to flash
        bool down;

        //Boolean variable to indicate if the button has been clicked
        public bool isClicked;

        //Button constructor
        public MenuButtons(Texture2D newTexture, GraphicsDevice graphics)
        {
            this.buttonTexture = newTexture;

            this.buttonSize = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 24);
        }

        //Button uodate method
        public void Update(Camera camera, MouseState mouse, Vector2 updatePosition)
        {
            this.buttonPosition = updatePosition;
            this.buttonRectangle = new Rectangle((int)updatePosition.X, (int)updatePosition.Y, (int)buttonSize.X, (int)buttonSize.Y);
            //Mouse rectangle which we draw to the location of the mouse in order
            //to check if the mouse has intersected with the button

            // the 31 is the X offset of the camera when it follows the player
            if ((int)camera.center.X == 0)
            {
                mouseRectange = new Rectangle(mouse.X, mouse.Y + (int)camera.center.Y, 1, 1);
            }
            else
            {
                mouseRectange = new Rectangle(mouse.X + 31, mouse.Y + (int)camera.center.Y, 1, 1);
            }

            //If the mouse rectangle intersects the button rectangle then
            //We enter our if statement loop where if color.A == 255 meaning
            //the button if fully visible then we set down to false and
            //subtract away 5 until we reach 0, which will make the button
            //no longer visible.  When this happens we then set down to true
            //and begin adding 5 until we reach 255 and the button is once
            //again visible.  We repeat while the mouse is on top of the 
            //button. If the mouse is completely away from the button
            //and color.A is less then 255 we add 5 until the button is
            //visible again.
            if (mouseRectange.Intersects(buttonRectangle))
            {
                if (color.A == 255)
                {
                    down = false;
                }

                if (color.A == 0)
                {
                    down = true;
                }

                if (down)
                {
                    color.A += 5;
                }
                else
                {
                    color.A -= 5;
                }

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                }
            }
            else if (color.A < 255)
            {
                color.A += 5;
                isClicked = false;
            }
        }

        //Set the position of the button
        public void setPosition(Vector2 newPosition)
        {
            this.buttonPosition = newPosition;
        }

        //Draw the button
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.buttonTexture, this.buttonRectangle, this.color);
        }
    }
}
