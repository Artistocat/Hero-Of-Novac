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

        private Texture2D defaultTex;
        private Texture2D combatTex;
        private Texture2D pixel;
        private Rectangle sourceRec;
        public Rectangle SourceRec
        {
            get { return sourceRec; }
        }
        private Vector2 playerPos;
        private Vector2 battlePos;
        private Rectangle healthBarPosTest;
        private Rectangle magicBarPosTest;
        private String hpTest;
        public int healthPoints = 100;
        public int magicPoints = 100;

        public Vector2 Position
        {
            get { return playerPos; }
            set { playerPos = value; }
        }
        private Color color;
        private int timer;

        public Player(Texture2D defaultTex, Texture2D combatTex, Texture2D p, Rectangle window)
        {
            currentGameState = GameState.Overworld;
            this.window = window;

            this.overWorldTex = defaultTex;
            this.combatTex = combatTex;
            this.pixel = p;
            sourceRec = new Rectangle(SPRITE_WIDTH, 2, SPRITE_WIDTH, SPRITE_HEIGHT);
            playerPos = new Vector2((window.Width - SPRITE_WIDTH) / 2, (window.Height - SPRITE_HEIGHT) / 2);
            battlePos = new Vector2(200, 200);
            healthBarPosTest = new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 10, healthPoints, 5);
            magicBarPosTest = new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 15, magicPoints, 5);
            color = Color.White;
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);
            timer = 0;
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
                    sourceRec.X = (sourceRec.X + SPRITE_WIDTH) % overWorldTex.Width;
            }
            //Use to test health bar stuff
            //if (pad1.IsButtonDown(Buttons.DPadDown))
            //    healthPoints--;
            //else if (pad1.IsButtonDown(Buttons.DPadUp))
            //    healthPoints++;
            timer++;
            healthBarPosTest.X = (int)playerPos.X - 10;
            healthBarPosTest.Y = (int)playerPos.Y - 10;
            healthBarPosTest.Width = healthPoints;
            magicBarPosTest.X = (int)playerPos.X - 10;
            magicBarPosTest.Y = (int)playerPos.Y - 15;
            magicBarPosTest.Width = magicPoints;
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
                    spriteBatch.Draw(overWorldTex, playerPos, sourceRec, color);
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
