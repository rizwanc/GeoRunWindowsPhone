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

namespace GeoRunWindowsPhone
{
    class Insults
    {
        //Reads in the insult text file and stores the insults in an array
        //returns string array
        public string[] readInsults()
        {
            int count = 0;
            string[] insults = new string[5];
            string line = String.Empty;

            System.IO.Stream st = TitleContainer.OpenStream("Content/Texts/insults.txt");
            using (System.IO.StreamReader sr = new System.IO.StreamReader(st))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    insults[count] = line;
                    count++;
                }
                sr.Close();
                return insults;
            }
        }

    }
}
