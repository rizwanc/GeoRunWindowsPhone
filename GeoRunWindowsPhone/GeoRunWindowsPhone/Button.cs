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
    class Button : Control
    {
        public Button(ContentManager Content, string newText, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("Menus/button");

            Rectangle = newRectangle;

            IsVisible = true;
            IsEnabled = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(texture, Rectangle, Color.White);
            }
        }

    }
}
