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
    class Control
    {
        protected MouseState presentMouse;
        protected MouseState pastMouse;

        protected SpriteFont font;

        protected ContentManager Content;

        protected string text = " ";

        protected Texture2D texture;

        private bool isEnabled = true;

        private bool isVisible = true;

        private Rectangle rectangle;

        protected List<Control> controls = new List<Control>();

        private Vector2 position = Vector2.Zero;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public bool IsMouseOver
        {
            get
            {
                Rectangle rect = new Rectangle(presentMouse.X, presentMouse.Y, 1, 1);

                return isVisible && rect.Intersects(Rectangle);
            }
        }

        public List<Control> Controls
        {
            get { return controls; }
        }

        public bool IsLeftClicked
        {
            get { return IsMouseOver && (presentMouse.LeftButton == ButtonState.Released && pastMouse.LeftButton == ButtonState.Pressed); }
        }

        public bool IsRightClicked
        {
            get { return IsMouseOver && (presentMouse.RightButton == ButtonState.Released && pastMouse.RightButton == ButtonState.Pressed); }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                rectangle.X = (int)position.X;
                rectangle.Y = (int)position.Y;
            }
        }

        public int Width
        {
            get { return Rectangle.Width; }
        }

        public int Height
        {
            get { return Rectangle.Height; }
        }

        public int X
        {
            get { return Rectangle.X; }
        }

        public int Y
        {
            get { return Rectangle.Y; }
        }

        public virtual void Update(MouseState mouse)
        {
            pastMouse = presentMouse;
            presentMouse = mouse;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
