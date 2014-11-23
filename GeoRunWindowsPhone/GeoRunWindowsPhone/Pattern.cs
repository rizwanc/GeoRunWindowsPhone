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
    class Pattern
    {
        //size of the entire pattern the width is going to be user to determine where the next pattern will come from
        private int width;
        private int height;

        public int weight = 0;
        public int timesDiedOn = 0;
        public int passCount = 0;


        SpriteFont font;

        //this is going to be the top left corner of the pattern.
        public Vector2 origin;

        // Pattern ID
        private String patternID;

        //The actual representation of the pattern in a 2d array
        public int[,] tilePattern;
        Tile[,] tiles;

        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }

        public Pattern(int[,] pattern, int tileSize, String patternName)
        {
            this.patternID = patternName;
            tilePattern = pattern;
            height = tilePattern.GetLength(0) * tileSize;
            width = tilePattern.GetLength(1) * tileSize;
        }

        // Added argument that adds the tile size since if we doing ramp, 32 is too big
        public void Initialize(ContentManager Content, Vector2 origin, int[,] tilePattern, int tileSize, String inID)
        {
            patternID = inID;
            this.origin = origin;
            this.tilePattern = tilePattern;
            tiles = new Tile[tilePattern.GetLength(0), tilePattern.GetLength(1)];
            height = tilePattern.GetLength(0) * tileSize;
            width = tilePattern.GetLength(1) * tileSize;
            // Load in font
            font = Content.Load<SpriteFont>("myFont");
            // go through the array and initialize the tiles based on the int array of 0 and 1s
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j] = new Tile();
                    if (tilePattern[i, j] == 1)
                    {
                        tiles[i, j].Initialize(1, Content, new Vector2(origin.X + (j * tileSize), origin.Y + (i * tileSize)), patternID);
                    }
                    else if (tilePattern[i, j] == 2)
                    {
                        tiles[i, j].Initialize(2, Content, new Vector2(origin.X + (j * tileSize), origin.Y + (i * tileSize)), patternID);
                    }
                    else if (tilePattern[i, j] == 3)
                    {
                        tiles[i, j].Initialize(3, Content, new Vector2(origin.X + (j * tileSize), origin.Y + (i * tileSize)), patternID);
                    }
                    else if (tilePattern[i, j] == 5)
                    {
                        tiles[i, j].Initialize(5, Content, new Vector2(origin.X + (j * tileSize), origin.Y + (i * tileSize)), patternID);
                    }
                    else
                    {
                        //tiles[i, j].Initialize(0, Content, new Vector2(origin.X + (j * tileSize) + tileSize, origin.Y + (i * tileSize) + tileSize));
                    }
                }
            }
        }

        public void Update(GameTime gameTime, float velocity)
        {
            this.origin.X += velocity;
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tilePattern[i, j] != 0)
                    {
                        tiles[i, j].Update(gameTime, velocity);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tilePattern[i, j] != 0)
                    {
                        tiles[i, j].Draw(spriteBatch);
                    }
                }
            }
        }

        public Tile getTile(Vector2 location)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (location.X > tiles[i, j].getPos().X && location.X < tiles[i, j].getPos().X + 32
                        && location.Y > tiles[i, j].getPos().Y && location.Y < tiles[i, j].getPos().Y + 32)
                    {
                        tiles[i, j].ColorThis(Color.Chocolate);
                        return tiles[i, j];
                    }
                }
            }
            return null;
        }

        public Boolean SwitchPat()
        {
            if (origin.X <= (-1 * this.width))
                return true;
            return false;
        }
        // Returns width of the pattern
        public int PatternWidth() { return width; }
        // Returns height of the pattern
        public int PatternHeight() { return height; }
        // Returns ID of the pattern
        public String getPatternID() { return patternID; }
    }
}
