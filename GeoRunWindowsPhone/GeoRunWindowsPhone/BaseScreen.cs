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
    public abstract class BaseScreen
    {
        public bool isActive { get; set; }

        protected ContentManager Content;

        public abstract void Update(Game1 game);
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
