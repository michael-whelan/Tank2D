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
    class Bullet
    {
        public Vector2 position;
        Vector2 velocity;
        DateTime timeOfBirth;
        int maxSpeed = 10;
        public bool alive = false;
        public Texture2D bulletImage;
        int timer;
        int timerMax = 20;


        public Bullet(ContentManager content)
        {
            bulletImage = content.Load<Texture2D>("bulletS");
        }



        public void CreateBullet(Vector2 playerDirection,Vector2 playerPos)
        {
            position = playerPos;
            velocity = playerDirection*maxSpeed;
            timeOfBirth = DateTime.Now;
            alive = true;

        }


        public void Timer()
        {
            if (timer < timerMax)
            {
                timer++;
            }
        }

        public void Update(Vector2 playerDirection, Vector2 playerPos)
        {
                position += velocity;
                Timer();
                if (timer == timerMax)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        CreateBullet(playerDirection, playerPos);
                        timer = 0;
                    }
                }
            
                //kills the bullet after a set time
                if (DateTime.Now.Subtract(timeOfBirth).TotalMilliseconds > 2000)
                {
                    alive = false;
                }
            
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
                spriteBatch.Draw(bulletImage, position, color);
        }
    }
}
