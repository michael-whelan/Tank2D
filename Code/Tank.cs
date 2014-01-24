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


namespace RoboTank
{
    class Tank
    {
        public Texture2D tankImage;
         public Vector2 origin;
        public Vector2 spritePos;
        float rotationAngle = 0.02f;
        public Vector2 Direct;
        float xPos, yPos;
        //direction variables
        int forward = 1;
        int backward = -1;
        int movementDirection;
        int tankSpeed = 4;
        public int noOfLives = 3;
        public int health = 100;

   
        bool collisionF = false;//is collision was in the forward or backward direction
        bool collisionB = false;


        public Tank(Texture2D texture , int width, int height)
        {
            tankImage = texture;
            //sets the initial tank position
            spritePos = new Vector2(600,600);
            origin.X = tankImage.Width / 2;
            origin.Y = tankImage.Height / 2;
        }

        public float RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
        }

        public void Update(GameTime gameTime, int xMax, int yMax, int xMin, int yMin)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Direct = new Vector2((float)Math.Cos(rotationAngle), (float)Math.Sin(rotationAngle));

            xPos = origin.X - (tankImage.Width / 2);
            yPos = origin.Y - (tankImage.Width / 2);

            //health
            if (health <= 0)
            {
                noOfLives--;
                health = 100;
            }
            


            //button presses
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                //forward collision
                if (collisionF == false)
                {
                    spritePos += Direct * tankSpeed;
                    movementDirection = forward;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                //backward collision
                if (collisionB == false)
                {// minus speed changes direction
                    spritePos += Direct * - tankSpeed;
                    movementDirection = backward;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (collisionB == false && collisionF == false)
                {
                    rotationAngle += 0.02f;  
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (collisionB == false && collisionF == false)
                {
                    rotationAngle += -0.02f;
                }
            }

        }

        public void Collision()
        {
            if (movementDirection == forward)
            {
                collisionF = true;
            }
            else if (movementDirection == backward)
            {
                collisionB = true;
            }
        }
        public void EndCollision()
        {
            collisionB = false;
            collisionF = false;
        }

        public void Draw(SpriteBatch spriteBatch, int mode, Color color)
        {
                spriteBatch.Draw(tankImage, spritePos, null, color, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
        }
        
    }
}
