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
using Microsoft.Xna.Framework.Input.Touch;

namespace GeoRunWindowsPhone
{
    class Player
    {
        private int width = 78;
        public int Width
        {
            get { return width; }
        }
        private int height = 69;
        public int Height
        {
            get { return height; }
        }
        //Holds the texture used for the player
        private Texture2D playerTexture;
        //the texture that gets drawn
        private Texture2D drawTexture;
        private Texture2D crouchTexture;
        private Texture2D attackTexture;
        private Texture2D shieldTexture;
        //Position of the character
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        //The position where it left the ground
        private Vector2 lastPos;
        //Check if player is on the ground
        public enum PlayerAction
        {
            jumping,
            crouching,
            shielding,
            attacking,
            dashing,
            running
        }
        private PlayerAction playerAction = PlayerAction.running;
        public PlayerAction Action
        {
            get { return playerAction; }
            set { playerAction = value; }
        }
        public enum PlayerState
        {
            falling,
            onGround,
            hitCeiling,
            dead
        }
        private PlayerState playerState = PlayerState.falling;
        public PlayerState State
        {
            get { return playerState; }
            set { playerState = value; }
        }
        private Boolean crouchPosChange = false; // Bool to know when to change height and pos
        //Velocity of the character
        private float velocityY;
        //Strength of gravity
        private const float gravity = 4.0f;
        // Use this AttackCounter to check if the user is still attacking or not
        private int attackCounter;

        //Initialize player variables, this is the spot where he gets dropped in from.
        public void Initialize(ContentManager content)
        {
            crouchTexture = content.Load<Texture2D>("Characters/mrcircle-crouch");
            shieldTexture = content.Load<Texture2D>("Characters/shield");
            attackTexture = content.Load<Texture2D>("Characters/mrcircleAngry");
            playerTexture = content.Load<Texture2D>("Characters/mrcircle");
            drawTexture = playerTexture;
            this.position.X = 95;
            this.position.Y = 710;
            velocityY = 0.0f;
            attackCounter = 0;
            lastPos = new Vector2(96, 100);
        }

        //Player update method
        public void Update(GameTime gameTime)
        {
            //Get key states
            KeyboardState key = Keyboard.GetState();

            UpdateAttack(key);

            UpdateJump(key);

            UpdateCrouch(key);

            UpdateShield(key);

            switch (playerState)
            {
                case PlayerState.falling:
                    UpdateFalling();
                    break;
                case PlayerState.onGround:
                    velocityY = 0.0f;
                    break;
                case PlayerState.hitCeiling:
                    velocityY = 0.0f;
                    playerState = PlayerState.falling;
                    break;
                case PlayerState.dead:
                    break;
                default:
                    break;

            }
        }

        //Player draw method, used to draw the player texture to the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            if (playerAction == PlayerAction.shielding)
            {
                blockProjectiles(spriteBatch);
            }

            spriteBatch.Draw(this.drawTexture, this.position, Color.White);
        }

        //************************************ UPDATE ATTACK ************************************
        private void UpdateAttack(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.A))
            {
                playerAction = PlayerAction.attacking;
                drawTexture = attackTexture;
            }
            //possibly add a timer for attacking so you cant just hold it
        }
        //************************************ UPDATE FALLING ************************************
        private void UpdateFalling()
        {
            //Add gravity to Y axis
            velocityY += gravity;
            //Increase the Y position of the player
            if (velocityY > 20.0f)
            {
                velocityY = 20.0f;
            }
            position.Y += velocityY;
        }

        //************************************ UPDATE CROUCH ************************************
        private void UpdateCrouch(KeyboardState keyboard)
        {
            MouseState ms = Mouse.GetState();
           
                if (ms.LeftButton == ButtonState.Pressed && ms.X >=0 && ms.X <= 100 && ms.Y >= 450 && ms.Y <= 470)
                {
                    if (playerState == PlayerState.onGround)
                    {
                        playerAction = PlayerAction.crouching;
                        if (crouchPosChange == false)
                        {
                            drawTexture = crouchTexture;
                            height = 15;
                            position.Y = position.Y + 54;
                            crouchPosChange = true;
                        }
                    }
                }
                else
                {
                    playerAction = PlayerAction.running;
                    height = 69;
                    drawTexture = playerTexture;
                    if (crouchPosChange == true)
                    {
                        position.Y = position.Y - 54;
                        crouchPosChange = false;
                    }
                }
            
        }
        //************************************ UPDATE JUMP ************************************
        private void UpdateJump(KeyboardState keyboard)
        {
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && ms.X <= 800 && ms.X >= 690 && ms.Y >= 450 && ms.Y <= 470 && playerAction != PlayerAction.crouching)
            {
                if (playerState == PlayerState.onGround)
                {
                    playerState = PlayerState.falling;
                    playerAction = PlayerAction.jumping;
                    velocityY = -40.0f;
                }
            }
            else
            {//if the player lets go of the space key the jump gets slowed down so they jump less
                if (velocityY < -12.0f)
                {
                    velocityY = -12.0f;
                }
            }
        }

        //************************************ UPDATE SHIELDING ************************************
        private void UpdateShield(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.S))
            {
                if (playerAction != PlayerAction.crouching)
                {
                    playerAction = PlayerAction.shielding;
                }
            }
        }
        //for the camera
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 70, 70); }
        }

        //Bounding box used for collision checking of the shield
        public Rectangle BoundingBoxShield
        {
            get { return new Rectangle((int)position.X + 64, (int)position.Y, 64, 64); }
        }

        public void blockProjectiles(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shieldTexture, new Rectangle((int)position.X + 64, (int)position.Y, 64, 64), Color.White);
        }
    }
}
