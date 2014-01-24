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
    class Mode
    {
        public Texture2D fogImage;
        Vector2 fogPos = new Vector2(0,0);
        int fogMode = 3;
        int dayMode = 1;
        int nightMode = 2;
        int mode;
        int timer;
        int timerMax = 400;

        public int CurrentMode
        {
            get { return mode; }
        }

        public void Timer()
        {
            if (timer < timerMax)
            {
                timer++;
            }
            if (timer == timerMax)
            {
                if (mode < 3)
                {
                    mode++;
                }
                else mode = 1;
                timer = 0;
            }
        }

        public Color ChangeEnviroment(Color color)
        {
            if (mode == nightMode)
            {
                color = Color.DarkGreen;
            }

            else if (mode == dayMode)
            {
                color = Color.White;
                mode = 1;
            }
            else if (mode == fogMode)
            {
                color = Color.DimGray;
            }
            return color;
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 tankPos, float viewW, float viewH)
        {

            if (mode == fogMode)
            {
                spriteBatch.Draw(fogImage, fogPos, Color.White);
            }
        }

    }
}
