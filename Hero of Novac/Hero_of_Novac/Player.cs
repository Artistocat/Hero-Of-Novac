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
    
    public class Player : Entity
    {
        private Rectangle window;

        private const int SPRITE_WIDTH = 52;
        private const int SPRITE_HEIGHT = 72;

        private enum GameState
        {
            Overworld, Battlemenu
        }

        private GameState currentGameState;

        private Texture2D overWorldTex;
        private Texture2D combatTex;
        private Texture2D pixel;
        private Rectangle sourceRec;
        private Vector2 playerPos;
        private Vector2 battlePos;
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
        private Vector2 vol;
        private Color color;
        private int timer;

        public Player(Texture2D defaultTex, Texture2D combatTex, Texture2D p, Rectangle window)
        {
            currentGameState = GameState.Overworld;
            this.window = window;
            this.overWorldTex = defaultTex;
            this.combatTex = combatTex;
            this.pixel = p;
            sourceRec = new Rectangle(SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
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
        }

        public void death()
        {

        }

        public override void Update(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    UpdateOverworld(gameTime);
                    break;
                case GameState.Battlemenu:
                    UpdateBattlemenu();
                    break;
            }
        }

        private void UpdateOverworld(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            vol = pad1.ThumbSticks.Left * 4;
            playerPos.X += vol.X;
            playerPos.Y -= vol.Y;
            if (playerPos.Y < 0)
                playerPos.Y = 0;
            else if (playerPos.Y + sourceRec.Height > window.Height)
                playerPos.Y = window.Height - sourceRec.Height;
            if (playerPos.X < 0)
                playerPos.X = 0;
            else if (playerPos.X + sourceRec.Width > window.Width)
                playerPos.X = window.Width - sourceRec.Width;


            if (vol.X == 0 && vol.Y == 0)
                sourceRec.X = SPRITE_WIDTH;
            else if (Math.Abs(vol.Y) > Math.Abs(vol.X))
            {
                if (vol.Y > 0)
                    sourceRec.Y = 216;
                else
                    sourceRec.Y = 0;

            }
            else if (Math.Abs(vol.X) > Math.Abs(vol.Y))
            {
                if (vol.X > 0)
                    sourceRec.Y = 144;
                else
                    sourceRec.Y = 74;
            }
            if (vol.X != 0 || vol.Y != 0)
            {
                if (timer % 6 == 0)
                    sourceRec.X = (sourceRec.X + SPRITE_WIDTH) % overWorldTex.Width;
            }
            //Use to test health bar stuff
            if (pad1.IsButtonDown(Buttons.DPadDown))
                healthPoints--;
            else if (pad1.IsButtonDown(Buttons.DPadUp) && healthPoints < healthMaximum)
                healthPoints++;

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

        public override void Draw(SpriteBatch spriteBatch)
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
