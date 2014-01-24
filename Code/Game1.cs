using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices; // allows messageBox
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace RoboTank
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //messageBox use
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        //check win time bonus

        Color color = Color.AntiqueWhite;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Tank playerTank;
        Viewport playerCam;
        Enemy[] testEnemy;
        Bullet[] bullet;
        Mode enviroChange; //controls the different enviroments ---> day/night/fog
        Explosion tankExplode;
        Radar radar;
        GUI gui;
        
        Texture2D background;
        Rectangle backRec;
        Tree[] tree;

        //GUI
        SpriteFont GUISpriteFont;

        int noOfBullets = 1;
        int noOfEnemies = 30;
        int enemiesAlive;
        int noOfTrees=  10;

        //edge of map
        int xMax = 1970;
        int yMax = 1960;
        int yMin = 40;
        int xMin = 40;

        int timer;
        int timerMax = 5;

        //camera control
        Matrix cameraMatrix;
        int xMinCam = 384; //max an min values for when player pos shows outside map
        int yMinCam = 336;
        int xMaxCam = 1575;
        int yMaxCam = 1836;

        //test
        bool contentLoaded= false;
        Texture2D loader;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            backRec = new Rectangle(0, 0, 2000, 2000);
            bullet = new Bullet[noOfBullets];
            tree = new Tree[noOfTrees];
            testEnemy = new Enemy[noOfEnemies];
            enviroChange = new Mode();
            radar = new Radar();
            gui = new GUI();
            tankExplode = new Explosion();
            playerTank = new Tank(Content.Load<Texture2D>("tankS"), GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            enemiesAlive = noOfEnemies;//sets living enemies to enemies initilized
            for (int i = 0; i < noOfTrees; i++)
            {
                tree[i] = new Tree();
            }

            for (int i = 0; i < noOfEnemies; i++)
            {
                testEnemy[i] = new Enemy(Content);
            }
            for (int i = 0; i < noOfBullets; i++)
            {
                bullet[i] = new Bullet(Content);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loader = this.Content.Load<Texture2D>("1");
            enviroChange.fogImage = this.Content.Load<Texture2D>("fog1");
            playerCam = GraphicsDevice.Viewport;
            radar.radarImage = this.Content.Load<Texture2D>("b-scan");
            radar.enemyIcon = this.Content.Load<Texture2D>("redSquareS");
            radar.radarSkin = this.Content.Load<Texture2D>("radarSkin");
            radar.playerIcon = this.Content.Load<Texture2D>("greenSquareS");
            tankExplode.spriteSheet = this.Content.Load<Texture2D>("explosion");
            background = this.Content.Load<Texture2D>("terrrain");
            //tree/landscape
            for (int j = 0; j < noOfTrees; j++)
            {
                tree[j].treeImage = this.Content.Load<Texture2D>("tree");

            }

            //enemy
            for (int i = 0; i < noOfEnemies; i++)
            {
                testEnemy[i].enemyImage = this.Content.Load<Texture2D>("EnemyTank");
            }
            //GUI
            GUISpriteFont = Content.Load<SpriteFont>("SpriteFont1");

            //bullet
            for (int i = 0; i < noOfBullets; i++)
			{
                bullet[i].bulletImage = this.Content.Load<Texture2D>("bulletS");
			}
            contentLoaded = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                EndGame(false);

            if (playerTank.noOfLives <= 0)
            {
                EndGame(false);
            }


            playerTank.Update(gameTime, xMax, yMax, xMin, yMin);
            for (int i = 0; i < noOfEnemies; i++)
            {
                testEnemy[i].Update(gameTime);
            }

            for (int i = 0; i < noOfBullets; i++)
            {
                    bullet[i].Update(playerTank.Direct, playerTank.spritePos);
            }


            if (enemiesAlive <= 0)
            {
                EndGame(true);
            }

            //changes day/night/fog
            color = enviroChange.ChangeEnviroment(color);
            enviroChange.Timer();
            CameraMatrix();
            Enemy_PlayerCol(gameTime);//checks distance between player and enemy
            EnemyFacePlayer(gameTime);
            TankCol();//player colliding with tree
            EnemyCol_Tree();//enemy colliding with tree
            EnemyShootPlayer();
            Timer();//timer controling the time displayed
            BulletWithEnemy();//bullet collides with enemy

            base.Update(gameTime);
        }

        public void Timer()
        {
            if (timer < timerMax)
            {
                timer++;
            }
        }

        public void Enemy_PlayerCol(GameTime gameTime)
        {
            float radius1 = (playerTank.tankImage.Width / 2);
            float radius2 = 0;
            //compare the distance to combined radii
            float dx = 0;
            float dy = 0;
            for (int i = 0; i < noOfEnemies; i++)
            {
                radius2 = testEnemy[i].enemyImage.Width / 2;
                dx = playerTank.spritePos.X - testEnemy[i].enemyPos.X;
                dy = playerTank.spritePos.Y - testEnemy[i].enemyPos.Y;
            

            float radii = radius1 + radius2;
            if ((dx * dx) + (dy * dy) < radii*radii)
            {//if enemy is close enough to player, player will lose health

                    if (testEnemy[i].alive == true)
                    {
                        tankExplode.Place(testEnemy[i].enemyPos);
                        playerTank.health -= 30;
                        enemiesAlive--;
                        testEnemy[i].alive = false;
                        
                    }
                   
                
               
            }
        }
    }

        public void EnemyFacePlayer(GameTime gameTime)
        {
             float radius1 = (playerTank.tankImage.Width / 2);
            float radius2 = 0;
            //compare the distance to combined radii
            float dx = 0;
            float dy = 0;
            for (int i = 0; i < noOfEnemies; i++)
            {
                radius2 = (testEnemy[i].enemyImage.Width / 2) * 8;
                dx = playerTank.spritePos.X - testEnemy[i].enemyPos.X;
                dy = playerTank.spritePos.Y - testEnemy[i].enemyPos.Y;


                float radii = radius1 + radius2;
                if ((dx * dx) + (dy * dy) < radii * radii)
                {//if enemy is close enough to player, player will lose health

                    if (testEnemy[i].alive == true)
                    {
                        testEnemy[i].MoveTowardPlayer(playerTank.spritePos);
                    }
                }
                else
                        testEnemy[i].MoveBasic(xMax, yMax, xMin, yMin, gameTime);//controls enemy random movement
            }
        }

        public void BulletWithEnemy()
        {
            float radius1 = 0;
            float radius2 = 0;//enemyWidth / 2;
           

            float dx =0;
            float dy = 0;


            for (int i = 0; i < noOfEnemies; i++)
            {
                for (int j = 0; j < noOfBullets; j++)
                {
                    radius1 = testEnemy[i].enemyImage.Width / 2;
                    radius2 = bullet[j].bulletImage.Width / 2;
                    dx = testEnemy[i].enemyPos.X - bullet[j].position.X;
                    dy = testEnemy[i].enemyPos.Y - bullet[j].position.Y;


                    float radii = radius1 + radius2;
                    if ((dx * dx) + (dy * dy) < radii * radii)
                    {
                        if (testEnemy[i].alive == true)
                        {
                            gui.score += 10;
                            tankExplode.Place(testEnemy[i].enemyPos);
                            testEnemy[i].alive = false;
                            enemiesAlive--;
                            bullet[j].alive = false;
                        }
                    }
                }
            }
        }

        public void EnemyShootPlayer()
        {
            float radius1 = (playerTank.tankImage.Width / 2);
            float radius2 = 0;//enemyWidth / 2;


            float dx = 0;
            float dy = 0;

            for (int i = 0; i < noOfEnemies; i++)
            {
                if (testEnemy[i].alive == true)
                {
                    radius2 = testEnemy[i].EnemyBullet.bulletImage.Width / 2;

                    dx = playerTank.spritePos.X - testEnemy[i].EnemyBullet.position.X;
                    dy = playerTank.spritePos.Y - testEnemy[i].EnemyBullet.position.Y;

                    float radii = radius1 + radius2;
                    if ((dx * dx) + (dy * dy) < radii * radii)
                    {
                        if (testEnemy[i].EnemyBullet.alive == true)
                        {
                            playerTank.health -= 60;
                            tankExplode.Place(playerTank.spritePos);
                            testEnemy[i].EnemyBullet.alive = false;
                        }

                    }
                }
            }
        }

        public void EnemyCol_Tree()
        {
            float radius1 = 0;
            float radius2 = 0;


            //compare the distance to combined radii
            float dx = 0;
            float dy = 0;

            for (int i = 0; i < noOfEnemies; i++)
            {
                for (int j = 0; j < noOfTrees; j++)
                {
                    radius1 = testEnemy[i].enemyImage.Width / 2;
                    radius2 = tree[j].treeImage.Width / 2;
                    dx = testEnemy[i].enemyPos.X - tree[j].treePos.X;
                    dy = testEnemy[i].enemyPos.Y - tree[j].treePos.Y;

                    float radii = radius1 + radius2;
                    if ((dx * dx) + (dy * dy) < radii * radii)
                    {
                        testEnemy[i].SceneryCol();
                    }
                }
            }
        
        }

        public void TankCol()
        {

            float radius1 = (playerTank.tankImage.Width / 2) - 6f;
            float radius2 = 0;

            playerTank.EndCollision();
            //compare the distance to combined radii
            float dx = 0;
            float dy = 0;

            for (int j = 0; j < noOfTrees; j++)
            {
                dx = playerTank.spritePos.X - tree[j].treePos.X;
                dy = playerTank.spritePos.Y - tree[j].treePos.Y;
                radius2 = tree[j].treeImage.Width / 2;
                float radii = radius1 + radius2;

                if ((dx * dx) + (dy * dy) < radii * radii)
                {
                    playerTank.Collision();
                }
            }
          
  
            //colliding with edge
            if (playerTank.spritePos.X >= xMax || playerTank.spritePos.X <= xMin
                || playerTank.spritePos.Y >= yMax || playerTank.spritePos.Y <= yMin)
            {
                playerTank.Collision();
            }
        }

        public void Message(bool win)
        {
            int timeBonus = 0;
            int finalScore = gui.score * gui.gameTime;
            if (win == false)
            {
                MessageBox(new IntPtr(0), string.Format("Lives Left= " + playerTank.noOfLives +
                    "\nFinal Score = " + finalScore +
                    "\nTime Survived = " + gui.gameTime), "Game Over", 0);
            }
            if (win == true)
            {
                if (gui.gameTime <= 80)
                {
                    timeBonus = 300;
                    finalScore += timeBonus;
                }
                else { timeBonus = 0; }
                MessageBox(new IntPtr(0), string.Format("Lives Left= " + playerTank.noOfLives +
                    "\nTime Bonus = " + timeBonus +
                   "\nFinal Score = " + finalScore +
                   "\nTime Survived = " + gui.gameTime), "Victory", 0);

            }
        }

        public void EndGame(bool win)
        {
                Message(win);     
            this.Exit();
        }

        public void CameraMatrix()
        {
            float matX = (-playerTank.spritePos.X - 20) + (float)(GraphicsDevice.Viewport.Width / 2);//sets camera positions on player
            float matY = (-playerTank.spritePos.Y - 150) + (GraphicsDevice.Viewport.Height);//as variables for change
            //stops the camera if the player is too close to the edge
            //gives camera 1-D movement
            if (playerTank.spritePos.X <= xMinCam)
            {
                matX = 0;
            }
            if (playerTank.spritePos.Y <= yMinCam)
            {
                matY = 0;
            }
            if (playerTank.spritePos.X >= xMaxCam)
            {
                matX = -1200;
            }
            if (playerTank.spritePos.Y >= yMaxCam)
            {
                matY = -1520;
            }
                //if the player is away from the edge the matrix is complete and the camera has 2-D movement
                      cameraMatrix = Matrix.CreateTranslation(new Vector3(matX, matY, 0));
        }

        protected override void Draw(GameTime gameTime)
        {
            //sets initial camera
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if(contentLoaded){
            //sets current viewport to playerCam
            GraphicsDevice.Viewport = playerCam;

            //radar matrix - controls size of radar
            Matrix radarTrans = Matrix.CreateScale(0.04f);
            Matrix radarScreenTrans = Matrix.CreateScale(0.3f);//where the radar appears on screen
            Matrix fogTrans = Matrix.CreateScale(0.9f);
            
            spriteBatch.Begin(default(SpriteSortMode), null, null, null, null, null, cameraMatrix);
            spriteBatch.Draw(background, backRec, color);
            for (int i = 0; i < noOfBullets; i++)
            {
                bullet[i].Draw(spriteBatch, color);
            }
            playerTank.Draw(spriteBatch, enviroChange.CurrentMode, color);

            for (int i = 0; i < noOfEnemies; i++)
            {
                testEnemy[i].Draw(spriteBatch, color);
            }

            for (int j = 0; j < noOfTrees; j++)
            {
                tree[j].Draw(spriteBatch, enviroChange.CurrentMode, color);
            }

            tankExplode.Draw(gameTime,spriteBatch,color);
            spriteBatch.End();

            //draws fog appropriately on screen
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, fogTrans);
            enviroChange.Draw(spriteBatch, playerTank.spritePos, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.End();

            //draws radar
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, radarScreenTrans);
            radar.Draw(spriteBatch, Color.White);
            spriteBatch.End();

            //draw enemies on radar
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, radarTrans);
            for (int i = 0; i < noOfEnemies; i++)
            {
                radar.DrawEnemyIcons(gameTime, spriteBatch, Color.White, testEnemy[i].enemyPos, playerTank.spritePos,testEnemy[i].alive);
            }

            spriteBatch.End();
            //GUI
            spriteBatch.Begin();
            gui.DrawString(spriteBatch, GUISpriteFont,playerTank.noOfLives,playerTank.health);
            spriteBatch.End();
        }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(loader,new Rectangle(0,0,GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height),Color.AntiqueWhite);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
