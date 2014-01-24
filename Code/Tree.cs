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
    class Tree
    {
        public Texture2D treeImage;
        public Vector2 treePos = new Vector2(300, 400);
        Vector2 origin;
        static Random rand = new Random();



        public Tree()
        {
            treePos.X = rand.Next(10, 2000);
            treePos.Y = rand.Next(10, 2000);
        }

        public void Draw(SpriteBatch spriteBatch , int mode, Color color)
        {
            origin.X = treeImage.Width / 2;
            origin.Y = treeImage.Height / 2;
                spriteBatch.Draw(treeImage, treePos, null, color, 0, origin, 1.0f, SpriteEffects.None, 0f);

        }

    }
}
