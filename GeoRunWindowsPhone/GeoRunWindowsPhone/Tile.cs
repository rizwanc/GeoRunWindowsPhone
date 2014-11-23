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
    class Tile
    {
        private Texture2D texture;
        private Vector2 position;
        private int TileID;
        private Color color;
        private String patternID;

        public void Initialize(int TileNumber, ContentManager Content, Vector2 pos, String pattern)
        {
            patternID = pattern;
            color = Color.White;
            TileID = TileNumber;
            // Depending what TileNumber it is, load that image
            if (TileNumber == 1)
                texture = Content.Load<Texture2D>("Levels/tile");
            else if (TileNumber == 2)
                texture = Content.Load<Texture2D>("Levels/leftRamp");
            else if (TileNumber == 3)
                texture = Content.Load<Texture2D>("Levels/rightRamp");
            else if (TileNumber == 5)
                texture = Content.Load<Texture2D>("Levels/breakable");

            this.position = pos;
        }

        public void Update(GameTime gameTime, float velocity)
        {
            position.X += velocity;
        }

        public void ColorThis(Color col)
        {
            color = col;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }

        // Returns position
        public Vector2 getPos() { return this.position; }
        // Returns iD
        public int GetTileID() { return this.TileID; }
        public String GetPatternID() { return this.patternID; }
    }
}
