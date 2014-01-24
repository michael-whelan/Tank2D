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
    class Radar
    {
        public Texture2D radarImage;
        public Texture2D enemyIcon;
        public Texture2D radarSkin;
        public Texture2D playerIcon;


        Vector2 radarPos = new Vector2(0, 0);
        Vector2 enemyIconPos;



        public void DrawEnemyIcons(GameTime gametime, SpriteBatch spriteBatch, Color color, Vector2 enemyPos, Vector2 playerPos, bool alive)
        {
            enemyIconPos = enemyPos;
            if (alive == true)
            {
                spriteBatch.Draw(enemyIcon, enemyIconPos, color);
            }
            spriteBatch.Draw(playerIcon, playerPos, color);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(radarImage, radarPos,color);
            
        }
    }
}
