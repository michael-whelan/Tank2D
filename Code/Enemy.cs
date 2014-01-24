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
    class Enemy
    {
        public Texture2D enemyImage;
        public Vector2 enemyPos = new Vector2(100,150);
        Vector2 enemyOrigin;
        static Random rand = new Random();
        Vector2 enemySpeed = new Vector2(100, 100);
        Vector2 enemyDirection;
        
        int movementDirection;
        int forward = 1;
        //enemy facing angle
        float angle;
        bool directionF = true;
        bool directionB = false;

        //timer
        int timerMax = 30;
        int timer;
        bool timerStart=  false;
        public bool alive = true;
        
         Bullet enemyBullet;


        public Bullet EnemyBullet
        {
            get{return enemyBullet;}
        }


        public Enemy(ContentManager content)
        {
            enemyBullet = new Bullet(content);
            enemyPos.X = rand.Next(10,1700);
            enemyPos.Y = rand.Next(10, 1700);
        }

        public void Update(GameTime gameTime)
        {
            
            if (alive == true)
            {
                movementDirection = forward;
                enemyDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                //starts timer ticking
                if (timerStart == true)
                {
                    Timer();
                }
                
                enemyBullet.Update(enemyDirection, enemyPos);
            }
        }

        public void MoveBasic(int xMax, int yMax, int xMin, int yMin, GameTime gameTime)
        {
            if (alive == true)
            {
                enemyPos += enemySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                angle = (float)Math.Atan2(enemySpeed.Y, enemySpeed.X);
                // Check for bounce.      
                if (enemyPos.X > xMax)
                {
                    enemySpeed.X = rand.Next(-150, -50);
                    enemySpeed.Y = rand.Next(-150, -50);//test
                    enemyPos.X = xMax;
                }
                else if (enemyPos.X < xMin)
                {
                    enemySpeed.X = rand.Next(50, 150);
                    enemySpeed.Y = rand.Next(50, 150);//test
                    enemyPos.X = xMin;
                }

                if (enemyPos.Y > yMax)
                {
                    enemySpeed.X = rand.Next(50, 150);//test
                    enemySpeed.Y = rand.Next(-150, -50);
                    enemyPos.Y = yMax;
                }
                else if (enemyPos.Y < yMin)
                {
                    enemySpeed.Y = rand.Next(50, 150);
                    enemySpeed.X = rand.Next(-150, -50);//test
                    enemyPos.Y = yMin;
                }
            }
        }

        public void MoveTowardPlayer(Vector2 playerPos)
        {
            if (alive == true)
            {
                Vector2 posDifference = new Vector2(playerPos.X - enemyPos.X, playerPos.Y - enemyPos.Y); // finds the vector for the difference in positions

                float rotation = (float)Math.Atan2(posDifference.Y, posDifference.X);

                if (angle - 0.08f < rotation) 
                {
                    angle += 0.03f;
                }
                if (angle - 0.08f > rotation)
                {
                    angle -= 0.03f;
                }
                if (timer < timerMax +10)
                {
                    enemyBullet.CreateBullet(enemyDirection, enemyPos); //lets the enemy create a bullet at rthe enemies position
                    timer = 0;
                }
            }
        }

        public void SceneryCol()
        {
            if (alive == true)
            {
                timerStart = true;
                enemySpeed *= -1;
                directionF = false;
                directionB = true;
            }
        }

        public void Timer()
        {

            if (timer < timerMax)
            {
                timer++;
            }
            else
            {
                timer = 0;
                
                directionF = true;
                directionB = false;
                timerStart = false;
                enemySpeed.X = rand.Next(50, 150);
                enemySpeed.Y = rand.Next(-150, -50);//test
            }

        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            enemyOrigin.X = enemyImage.Width / 2;
            enemyOrigin.Y = enemyImage.Height / 2;
            if (alive == true)
            {
                enemyBullet.Draw(spriteBatch, color);
                if (directionF == true)
                {
                    spriteBatch.Draw(enemyImage, enemyPos, null, color, angle, enemyOrigin, 1.0f, SpriteEffects.None, 0f);
                }
                else if (directionB == true)
                {
                    //changes the direction the enemy faces. the enemy will reverse when facing the move direction
                    spriteBatch.Draw(enemyImage, enemyPos, null, color, angle + 160, enemyOrigin, 1.0f, SpriteEffects.None, 0f);
                }

              
            }
        }
    }
}
