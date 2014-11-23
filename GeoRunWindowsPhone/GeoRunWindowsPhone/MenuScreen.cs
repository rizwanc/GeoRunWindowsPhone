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
    class MenuScreen : BaseScreen
    {
        Button btnPlay;

        List<Control> Controls = new List<Control>();

        MouseState presentMouse;

        public MenuScreen(Game1 game)
        {
            presentMouse = Mouse.GetState();

            int center = game.GraphicsDevice.Viewport.Width / 2;

            btnPlay = new Button(game.Content, "Play", new Rectangle(center - 50, 100, 100, 35));

            Controls.Add(btnPlay);
        }

        public override void Update(Game1 game)
        {
            foreach (Control control in Controls)
            {
                control.Update(presentMouse);
            }

            if (btnPlay.IsLeftClicked)
            {
                this.isActive = false;
                //game.currentGameState = Geo_Run.Game1.GameState.Playing;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

            foreach (Control control in Controls)
            {
                control.Draw(spriteBatch);
            }

            //spriteBatch.End();
        }

    }
}
