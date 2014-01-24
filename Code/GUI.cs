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
using System.Runtime.InteropServices;
using System.IO;

namespace RoboTank
{
    class GUI
    {
        //SpriteFont timerScoreString;
        int stringX = 100;
        int stringY = 10;
        public int score = 0;
        public int gameTime;
        int counter;


        public void Time()
        {
                //makes counter constantly rise
                counter++;
            //stops gametime moving too quickly
            if (counter > 60)
            {
                gameTime++; // increases gametime approx 1/sec
                counter = 0;
            }

        }

        public void DrawString(SpriteBatch spriteBatch,SpriteFont spriteFont,int playerLives, int health)
        {
            Time();
            Vector2 stringPos = new Vector2(stringX, stringY);
            //string for output
            string output = "Lives: " + playerLives +
                "\nHealth: "+ health +
                "%\nScore: " +score +
                "\nSurvival Time: " + gameTime;
            //outputs string
            spriteBatch.DrawString(spriteFont, output, stringPos, Color.Black);
        }

    }
}
