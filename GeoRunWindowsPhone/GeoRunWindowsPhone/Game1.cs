using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace GeoRunWindowsPhone
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //The first level
        Level level1 = new Level();

        //The font for drawing text
        SpriteFont font;

        //Animation that plays for death
        Animation deathAnimation = new Animation();
        Animation levelOneAnim = new Animation();

        //Camera
        Camera camera;

        //Game states to control menu switches
        enum GameState
        {
            MainMenu,
            Help,
            About,
            Playing,
            GameOver
        }

        MenuButtons playButton;
        MenuButtons pauseButton;
        MenuButtons continueButton;
        MenuButtons backToMainButton;
        MenuButtons jumpButton;
        MenuButtons crouchButton;

        //Indicates if the game has been restarted
        bool restarted;

        //Boolean to check if the game has been paused
        bool paused = false;

        //Set the initial state of the game to the main menu
        GameState currentGameState = GameState.MainMenu;

        //Create new insults object
        static Insults insults;

        //the insult array
        string[] insultArray;

        Random myRand = new Random();
        int rand;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            level1.Initialize(Content, "level1");

            // Load in font
            font = Content.Load<SpriteFont>("myFont");

            //Set the mouse to be visible on the xna game screen
            IsMouseVisible = true;
            //Create the buttons and set their positions
            playButton = new MenuButtons(Content.Load<Texture2D>("Menus/button"), graphics.GraphicsDevice);
            playButton.setPosition(new Vector2(350, 300));

            pauseButton = new MenuButtons(Content.Load<Texture2D>("Menus/button"), graphics.GraphicsDevice);
            pauseButton.setPosition(new Vector2(camera.center.X + 700, camera.center.Y - 200));

            continueButton = new MenuButtons(Content.Load<Texture2D>("Menus/button"), graphics.GraphicsDevice);
            continueButton.setPosition(new Vector2(camera.center.X, level1.getPlayer().Position.Y));

            backToMainButton = new MenuButtons(Content.Load<Texture2D>("Menus/button"), graphics.GraphicsDevice);
            backToMainButton.setPosition(new Vector2(camera.center.X, camera.center.Y));

            jumpButton = new MenuButtons(Content.Load<Texture2D>("Menus/button"), graphics.GraphicsDevice);
            jumpButton.setPosition(new Vector2(camera.center.X + 700, camera.center.Y + 455));

            crouchButton = new MenuButtons(Content.Load<Texture2D>("Menus/button"), graphics.GraphicsDevice);
            crouchButton.setPosition(new Vector2(camera.center.X, camera.center.Y + 455));

            //Sets up the death animation
            deathAnimation.Initialize(Content.Load<Texture2D>("Characters/death"), new Vector2(0, 0), 78, 69, 10, 10, Color.White, 1.0f, true);
            levelOneAnim.Initialize(Content.Load<Texture2D>("Menus/levelone"), new Vector2(200, 600), 415, 101, 5, 50, Color.White, 1.0f, true);

            insults = new Insults();

            //populate the insult array
            insultArray = insults.readInsults();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Get the state of the mouse
            MouseState mouse = Mouse.GetState();

            //Check and update each state of the game
            switch (currentGameState)
            {
                case GameState.MainMenu://~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    //Check if the user pressed the play button
                    if (playButton.isClicked == true)
                    {
                        //Check if the game was restarted by the player
                        //pressing the back to main button
                        if (restarted == true)
                        {
                            restarted = false;
                            level1.Initialize(Content, "level1");
                            camera.center = new Vector2(0, 0);
                            currentGameState = GameState.Playing;
                        }
                        else
                        {
                            currentGameState = GameState.Playing;
                        }
                    }

                    //Update the play button
                    playButton.Update(camera, mouse, new Vector2(350, 300));

                    break;

                case GameState.Playing://~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    //Check is the user pressed the pause button
                    if (pauseButton.isClicked == true)
                    {
                        paused = true;

                        //Update the continue button
                        continueButton.Update(camera, mouse, new Vector2(camera.center.X + 350, camera.center.Y + 300));

                        //Update the back to main button
                        backToMainButton.Update(camera, mouse, new Vector2(camera.center.X + 350, camera.center.Y + 350));

                        if (continueButton.isClicked == true)
                        {
                            paused = false;
                            pauseButton.isClicked = false;
                        }
                        else if (backToMainButton.isClicked == true)
                        {
                            paused = false;
                            currentGameState = GameState.MainMenu;
                            restarted = true;
                        }
                    }
                    else // MAIN GAMEPLAY
                    {
                        levelOneAnim.Update(gameTime);
                        //Update the level returns true if game over
                        if (level1.Update(gameTime, Content))
                        {
                            currentGameState = GameState.GameOver;
                            rand = myRand.Next(0, 4);
                        }

                        //Update the camera
                        camera.Update(gameTime, level1.getPlayer());

                        //Update the pause button
                        pauseButton.Update(camera, mouse, new Vector2((camera.center.X + 700), (camera.center.Y)));

                        //Update the jump button
                        jumpButton.Update(camera, mouse, new Vector2((camera.center.X + 700), (camera.center.Y + 455)));

                        //Update the crouch button
                        crouchButton.Update(camera, mouse, new Vector2((camera.center.X), (camera.center.Y + 455)));

                        //Set paused to false to indicate the game 
                        //is no longer paused
                        paused = false;
                    }
                    levelOneAnim.Position = new Vector2(level1.getPlayer().Position.X + 200, level1.getPlayer().Position.Y - 100);
                    break;

                case GameState.GameOver://~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    //When the player dies, sets up the position of the death animatin.
                    //makes the velocity of the level 0
                    level1.gameOver();
                    deathAnimation.Position = new Vector2(level1.getPlayer().Position.X + 60, level1.getPlayer().Position.Y + 20);
                    deathAnimation.Update(gameTime);

                    KeyboardState GameOverContinue = Keyboard.GetState();
                    if (GameOverContinue.IsKeyDown(Keys.C))
                    {
                        level1.ChangeVelocity(-10.0f);
                        level1.Initialize(Content, "level1");

                        currentGameState = GameState.Playing;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        //Draw insults to the screen
        private void DrawInsults()
        {
            //int rand;



            spriteBatch.DrawString(font, insultArray[rand], new Vector2(level1.getPlayer().Position.X, level1.getPlayer().Position.Y - 40), Color.White);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState)
            {
                case GameState.MainMenu:

                    spriteBatch.Begin();

                    spriteBatch.Draw(Content.Load<Texture2D>("Menus/background"), new Rectangle(0, 0, 800, 480), Color.White);
                    playButton.Draw(spriteBatch);

                    spriteBatch.End();

                    break;

                case GameState.Playing:

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                    MouseState mouse = Mouse.GetState();
                    levelOneAnim.Position = new Vector2(level1.getPlayer().Position.X + 110, level1.getPlayer().Position.Y - 200);
                    levelOneAnim.Draw(spriteBatch);
                    level1.Draw(spriteBatch);
                    //Draw the pause button
                    spriteBatch.DrawString(font, "Score: " + level1.getPlayerScore(), new Vector2(level1.getPlayer().Position.X + 585, level1.getPlayer().Position.Y - 175), Color.White);
                    spriteBatch.DrawString(font, "Level: " + level1.getLevel(), new Vector2(level1.getPlayer().Position.X + 585, level1.getPlayer().Position.Y - 155), Color.White);

                    jumpButton.Draw(spriteBatch);
                    crouchButton.Draw(spriteBatch);

                    pauseButton.Draw(spriteBatch);

                    if (paused == true)
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("Menus/pauseScreen"), new Rectangle((int)camera.center.X, (int)camera.center.Y, 800, 480), Color.White);
                        continueButton.Draw(spriteBatch);
                        backToMainButton.Draw(spriteBatch);
                    }

                    spriteBatch.End();

                    break;

                case GameState.GameOver:
                    //Sets up the death screen
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                    spriteBatch.DrawString(font, "Score: " + level1.getPlayerScore(), new Vector2(level1.getPlayer().Position.X + 585, level1.getPlayer().Position.Y - 175), Color.White);
                    level1.Draw(spriteBatch);
                    DrawInsults();
                    spriteBatch.DrawString(font, "Press 'C' to Play Again ", new Vector2(350, level1.getPlayer().Position.Y + 100), Color.White);
                    deathAnimation.Draw(spriteBatch);


                    spriteBatch.End();
                    break;

            }

            base.Draw(gameTime);
        }
    }
}
