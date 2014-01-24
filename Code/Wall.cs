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
    class Wall
    {
        public Texture2D wallImage;
        int xPos,yPos,xPos2,yPos2;
           int  width = 100;
          //int  height = 100;


        public Rectangle wallRec;

        public Vector2 origin;



        public int X
        {
            get { return xPos; }
            set { xPos = value; }
        }

        public int Y
        {
            get { return yPos; }
            set { yPos = value; }
        }

        public int X2
        {
            get { return xPos2; }
            set { xPos2 = value; }
        }

        public int Y2
        {
            get { return yPos2; }
            set { yPos2 = value; }
        }

        public void InitializeVariables()
        {
            xPos = 0;
            yPos = 0;

            //rectangle variables
            
            xPos2 = xPos + width;

            origin.X = xPos + (wallImage.Width / 2);
            origin.Y = yPos +(wallImage.Height/2);
            

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            wallRec = new Rectangle(xPos, yPos, width, width);
            spriteBatch.Draw(wallImage,wallRec,Color.AntiqueWhite);

        }


    }
}
