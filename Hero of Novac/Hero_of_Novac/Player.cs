using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Hero_of_Novac
{
    
    public class Player
    {
        private Rectangle window;

        private const int SPRITE_WIDTH = 52;
        private const int SPRITE_HEIGHT = 70;

        private enum GameState
        {
            Overworld, Battlemenu
        }

        private GameState currentGameState;

        private Texture2D overworldTex;
        private Texture2D combatTex;
        private Texture2D pixel;
        private Rectangle sourceRec;
        public Rectangle SourceRec
        {
            get { return sourceRec; }
        }
        private Vector2 playerPos;
        private Vector2 battlePos;
        private Rectangle rec;
        public Rectangle Rec
        {
            get { return rec; }
        }
        private Rectangle healthBarPosTest;
        private int healthBarMaxWidth;
        public double healthPoints = 100;
        private double healthMaximum = 100;
        private Rectangle magicBarPosTest;
        private int magicBarMaxWidth;
        private double magicMaximum = 100;
        public double magicPoints = 100;
        public Vector2 Position
        {
            get { return playerPos; }
            set { playerPos = value; }
        }
        private Color color;
        private int timer;

        public Player(Texture2D overworldTex, Texture2D combatTex, Texture2D p, Rectangle window)
        {
            currentGameState = GameState.Overworld; 
            this.window = window;

            this.overworldTex = overworldTex;
            this.combatTex = combatTex;
            pixel = p;
            sourceRec = new Rectangle(SPRITE_WIDTH, 2, SPRITE_WIDTH, SPRITE_HEIGHT);
            playerPos = new Vector2((window.Width - SPRITE_WIDTH) / 2, (window.Height - SPRITE_HEIGHT) / 2);
            healthBarPosTest = new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 10, 66, 5);
            magicBarPosTest = new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 15, 66, 5);
            healthBarMaxWidth = 66; //Pixels
            magicBarMaxWidth = 66;
            battlePos = new Vector2(200, 200);
            color = Color.White;
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);
            timer = 0;

            rec = new Rectangle((int)playerPos.X, (int)playerPos.Y, sourceRec.Width, sourceRec.Height);
        }

        public void death()
        {

        }

        public void Update(GameTime gameTime, Vector2 speed)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    UpdateOverworld(gameTime, speed);
                    break;
                case GameState.Battlemenu:
                    UpdateBattlemenu();
                    break;
            }
            rec = new Rectangle((int)playerPos.X, (int)playerPos.Y, sourceRec.Width, sourceRec.Height);
        }

        private void UpdateOverworld(GameTime gameTime, Vector2 speed)
        {
            if (playerPos.Y < 0)
                playerPos.Y = 0;
            else if (playerPos.Y + sourceRec.Height > window.Height)
                playerPos.Y = window.Height - sourceRec.Height;
            if (playerPos.X < 0)
                playerPos.X = 0;
            else if (playerPos.X + sourceRec.Width > window.Width)
                playerPos.X = window.Width - sourceRec.Width;


            if (speed == Vector2.Zero)
                sourceRec.X = SPRITE_WIDTH;
            else if (Math.Abs(speed.Y) > Math.Abs(speed.X))
            {
                if (speed.Y > 0)
                    sourceRec.Y = 218;
                else
                    sourceRec.Y = 2;

            }
            else if (Math.Abs(speed.X) > Math.Abs(speed.Y))
            {
                if (speed.X > 0)
                    sourceRec.Y = 146;
                else
                    sourceRec.Y = 74;
            }
            if (speed != Vector2.Zero)
            {
                if (timer % 6 == 0)
                    sourceRec.X = (sourceRec.X + SPRITE_WIDTH) % overworldTex.Width;
            }

            if (healthPoints == 0)
            {
                healthPoints = 100;
            }
            timer++;
            healthBarPosTest.X = (int)playerPos.X - 10;
            healthBarPosTest.Y = (int)playerPos.Y - 10;
            magicBarPosTest.X = (int)playerPos.X - 10;
            magicBarPosTest.Y = (int)playerPos.Y - 5;
            if(healthPoints < healthMaximum)
            {
                healthBarPosTest.Width = (int)(healthBarMaxWidth / (healthMaximum / healthPoints));
            }
            if (healthPoints < healthMaximum)
            {
                magicBarPosTest.Width = (int)(magicBarMaxWidth / (magicMaximum / magicPoints));
            }
        }

        private void UpdateBattlemenu()
        {
            //TODO

            /*
             *Update source rectangle for the battlemenu 
             */
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState) {
                case GameState.Overworld:
                    spriteBatch.Draw(overworldTex, playerPos, sourceRec, color);
                    spriteBatch.Draw(pixel, healthBarPosTest, sourceRec, Color.Red);
                    spriteBatch.Draw(pixel, magicBarPosTest, sourceRec, Color.Blue);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(combatTex, battlePos, sourceRec, color);
                    spriteBatch.Draw(pixel, healthBarPosTest, sourceRec, Color.Red);
                    spriteBatch.Draw(pixel, magicBarPosTest, sourceRec, Color.Blue);
                    break;
            }
        }

        public void Battle()
        {
            currentGameState = GameState.Battlemenu;
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
        }
    }
}
