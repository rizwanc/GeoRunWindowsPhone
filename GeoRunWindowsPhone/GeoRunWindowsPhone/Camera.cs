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
    class Camera
    {
        //Used to draw the camera on screen
        public Matrix transform;

        //Variable to set where the camera is looking
        public Viewport view;

        //Coordinates of the center of the camera
        public Vector2 center;

        //Initialize the camera
        public Camera(Viewport newView)
        {
            view = newView;
        }

        //Update the camera position when the player moves
        public void Update(GameTime gametime, Player player)
        {
            center = new Vector2(player.Position.X + (player.BoundingBox.Width / 2 - 100), player.Position.Y - 200);

            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }

    }
}
