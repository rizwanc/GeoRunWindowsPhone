using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;

namespace GeoRunWindowsPhone
{
    class Patterns
    {
        /**
         * The random generator numbers
         **/
        static private int maxRanGen = 200;
        private int[] randomPatIndexes = new int[maxRanGen];
        // minimum a pattern weight can be
        private int minWeight = 0;
        private int defaultWeight = 0;
        //number of times a pattern passes for the weight to get reset to default
        private int resetWeightNum = 3;


        //The size of the tiles
        private int tileSize = 32;


        // the amount of different patterns, we will pick these randomly
        private Pattern[] displayPatterns = new Pattern[3];

        private List<Pattern> allPatterns = new List<Pattern>();

        //the speed at which everything is going to be moving to the left of the screen.
        private float velocity;
        private SpriteFont font;

        ContentManager cont;

        public void Initialize(ContentManager Content, float vel, String Pattern)
        {
            //initialize the fist pattern to 400 down
            cont = Content;
            font = Content.Load<SpriteFont>("myFont");
            velocity = vel;

            //load all of the patterns from the text files
            if (allPatterns.Count == 0)
            {
                LoadPatterns(Pattern);
            }

            for (int i = 0; i < allPatterns.Count; i++)
            {
                Console.WriteLine(i + " " + allPatterns[i].weight);
            }

            displayPatterns[0] = new Pattern(ready, tileSize, "ready");
            displayPatterns[1] = new Pattern(set, tileSize, "set");
            displayPatterns[2] = new Pattern(go, tileSize, "go");

            displayPatterns[0].Initialize(Content, new Vector2(0, 0), displayPatterns[0].tilePattern, tileSize, displayPatterns[0].getPatternID());
            displayPatterns[1].Initialize(Content, new Vector2(displayPatterns[0].getWidth(), 0), displayPatterns[1].tilePattern, tileSize, displayPatterns[1].getPatternID());
            displayPatterns[2].Initialize(Content, new Vector2((displayPatterns[0].getWidth() + displayPatterns[1].getWidth()), 0), displayPatterns[2].tilePattern, tileSize, displayPatterns[2].getPatternID());
        }

        public void Update(GameTime gameTime, ContentManager Content)
        {
            RandomizePatterns(Content);
            displayPatterns[0].Update(gameTime, velocity);
            displayPatterns[1].Update(gameTime, velocity);
            displayPatterns[2].Update(gameTime, velocity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            displayPatterns[0].Draw(spriteBatch);
            displayPatterns[1].Draw(spriteBatch);
            displayPatterns[2].Draw(spriteBatch);
        }

        public Tile getPattern(Vector2 location)
        {
            for (int i = 0; i < displayPatterns.Length; i++)
            {
                if (location.X > displayPatterns[i].origin.X && location.X < displayPatterns[i].origin.X + displayPatterns[i].getWidth())
                {
                    return displayPatterns[i].getTile(location);

                }
            }
            return null;
        }
        public String getPatternID(Vector2 location)
        {
            for (int i = 0; i < displayPatterns.Length; i++)
            {
                if (location.X > displayPatterns[i].origin.X && location.X < displayPatterns[i].origin.X + displayPatterns[i].getWidth())
                {
                    return displayPatterns[i].getPatternID();

                }
            }
            return "NULL";
        }
        // Randomize an array of size 2
        public void RandomizePatterns(ContentManager Content)
        {
            Random random = new Random();
            int rNum = random.Next(0, maxRanGen);

            for (int i = 0; i < displayPatterns.Length; i++)
            {
                if (displayPatterns[i].SwitchPat())
                {
                    //~~~~~~~~~begin~~~~~~~~
                    // check if to reset the weights
                    for (int j = 0; j < allPatterns.Count; j++)
                    {
                        if (allPatterns[j].getPatternID() == displayPatterns[i].getPatternID())
                        {
                            //reset the weight if the passcount is 3 and the weight is more than default
                            if (allPatterns[j].passCount >= resetWeightNum && allPatterns[j].weight > defaultWeight + minWeight)
                            {
                                allPatterns[j].passCount = 0;
                                int subWeight = allPatterns[j].weight - defaultWeight;
                                allPatterns[j].weight = defaultWeight;
                                while (subWeight > 0)
                                {
                                    for (int k = 0; k < allPatterns.Count; k++)
                                    {
                                        if (subWeight <= 0)
                                        {
                                            break;
                                        }
                                        if (j != k)
                                        {
                                            allPatterns[k].weight++;
                                            subWeight--;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (allPatterns[j].passCount <= resetWeightNum)
                                {
                                    allPatterns[j].passCount++;
                                }
                            }
                        }
                    }
                    //~~~~~~~end~~~~~~~~



                    // fills the pattern random number generator array based on the weight numbers,, fills the array with the pattern indexes based on the weights
                    //begin
                    int te = 0;
                    for (int k = 0; k < randomPatIndexes.Length; k++)
                    {
                        for (int j = 0; j < allPatterns.Count; j++)
                        {
                            for (int w = 0; w < allPatterns[j].weight; w++)
                            {
                                if (k < randomPatIndexes.Length)
                                {
                                    randomPatIndexes[k] = j;
                                    k++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        te = k;
                        break;
                    }

                    if (te < randomPatIndexes.Length)
                    {
                        for (int k = te; k < randomPatIndexes.Length; k++)
                        {
                            randomPatIndexes[k] = allPatterns.Count - 1;
                        }
                    }
                    // end

                    //allPatterns[randomPatIndexes[rNum]] using the random number the spot in the randomPatIndexes is an index to all patterns
                    switch (i)
                    {
                        case 0:
                            displayPatterns[i].Initialize(Content, new Vector2(displayPatterns[2].origin.X + displayPatterns[2].getWidth(), 0), allPatterns[randomPatIndexes[rNum]].tilePattern, tileSize, allPatterns[randomPatIndexes[rNum]].getPatternID());
                            break;
                        case 1:
                            displayPatterns[i].Initialize(Content, new Vector2(displayPatterns[0].origin.X + displayPatterns[0].getWidth(), 0), allPatterns[randomPatIndexes[rNum]].tilePattern, tileSize, allPatterns[randomPatIndexes[rNum]].getPatternID());
                            break;
                        case 2:
                            displayPatterns[i].Initialize(Content, new Vector2(displayPatterns[1].origin.X + displayPatterns[1].getWidth(), 0), allPatterns[randomPatIndexes[rNum]].tilePattern, tileSize, allPatterns[randomPatIndexes[rNum]].getPatternID());
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // Loads all the pattern text files for that level folder
        private void LoadPatterns(String Pattern)
        {
            System.IO.Stream stream =  TitleContainer.OpenStream("Content/Patterns/Level1/" + Pattern);
            System.IO.StreamReader sreader = new System.IO.StreamReader(stream);

            String[] patternTexts = new String[25];
            int counter = 0;
            string fileLine;

            while((fileLine = sreader.ReadLine()) != null)
            {
                if (counter == 25)
                {
                    break;
                }
                patternTexts[counter] = fileLine;
                counter++;
            }
    
               //System.IO.Directory.GetFiles("Content/Patterns/" + level);
            foreach (string s in patternTexts)
            {
                int count = 0;
                int row = 0;
                int checkSum = 0;
                int col = 0;
                string line = String.Empty;
                int weight = maxRanGen / patternTexts.Length;
                defaultWeight = weight;

                //get the size of the level array rows and columns from the file
                System.IO.Stream st = TitleContainer.OpenStream("Content/Patterns/Level1/" + Pattern);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(st))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        col = line.Length;
                        checkSum += col;
                        row++;
                    }
                    sr.Close();

                }
                try
                {
                    if (checkSum / row != col)
                    {
                        Console.WriteLine("FORMATTING FOR PATTERN FILE: " + s + " IS WRONG");
                    }
                    // reads the text file and creates the int 2d array
                    System.IO.Stream st2 = TitleContainer.OpenStream("Content/Patterns/Level1/" + Pattern);
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(st2))
                    {
                        int[,] temp = new int[row, col];
                        while ((line = sr.ReadLine()) != null)
                        {
                            for (int i = 0; i < col; i++)
                            {
                                temp[count, i] = int.Parse(line[i] + "");
                            }
                            count++;
                        }
                        Pattern t = new Pattern(temp, tileSize, s);
                        t.weight = weight;
                        Console.WriteLine(weight);
                        allPatterns.Add(t);
                        sr.Close();

                    }



                }
                catch (Exception e)
                {
                    Console.WriteLine("FORMATTING FOR PATTERN FILE: " + s + " IS WRONG", e);
                }


            }
            // min weight is 10% of the max random number divided by the amount of patterns rounded up
            minWeight = (int)Math.Ceiling((maxRanGen / allPatterns.Count) * .1);
            return;

        }
        /** 
         *   if you pass a pattern 3 times and if the weight is above the default then you put it to the default and add the difference to the other patterns
         *
         *   every time you die:
         *   you add the amount of patterns to the weight and subtract from the others min 10% of norm rounded up
         * */
        public void addDeath(String patternID)
        {
            for (int i = 0; i < allPatterns.Count; i++)
            {
                if (allPatterns[i].getPatternID() == patternID)
                {
                    int counter = 0;
                    // goes through the patterns subtracts one if the weight is above the minWeight counts how many times happened and adds that to the died on pattern
                    for (int j = 0; j < allPatterns.Count; j++)
                    {
                        if (j != i)
                        {
                            if (allPatterns[j].weight > minWeight)
                            {
                                allPatterns[j].weight--;
                                counter++;
                            }
                        }
                    }
                    allPatterns[i].weight += counter;
                    allPatterns[i].timesDiedOn++;
                    return;
                }
            }
        }

        private int[,] ready = new int[,]
        {
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {1,1,1,1,0,1,1,1,1,0,0,0,1,1,0,0,0,1,1,1,0,0,1,0,0,0,1,0},
         {1,0,0,1,0,1,0,0,0,0,0,0,1,1,0,0,0,1,0,0,1,0,0,1,0,1,0,0},
         {1,1,1,1,0,1,0,0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,0,1,0,0,0},
         {1,1,0,0,0,1,1,1,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,0,1,0,0,0},
         {1,0,1,0,0,1,0,0,0,0,0,1,1,1,1,0,0,1,0,0,1,0,0,0,1,0,0,0},
         {1,0,0,1,0,1,1,1,1,0,1,0,0,0,0,1,0,1,1,1,0,0,0,0,1,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        private int[,] set = new int[,]
        {
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {1,1,1,1,0,1,1,1,1,0,1,1,1,1,1,0},
         {1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0},
         {0,1,1,0,0,1,0,0,0,0,0,0,1,0,0,0},
         {0,0,0,1,0,1,1,1,1,0,0,0,1,0,0,0},
         {0,0,0,1,0,1,0,0,0,0,0,0,1,0,0,0},
         {1,1,1,1,0,1,1,1,1,0,0,0,1,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };
        private int[,] go = new int[,]
        {
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0},
         {1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,0,0},
         {1,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,0},
         {1,0,0,0,0,0,1,0,0,0,1,0,1,1,0,0,0},
         {1,0,1,1,1,0,1,0,0,0,1,0,1,1,0,0,0},
         {1,0,0,0,1,0,1,0,0,0,1,0,1,1,0,0,0},
         {1,1,1,1,1,0,1,1,1,1,1,0,0,0,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0},
         {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
         {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };
    }
}
