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

        private Texture2D defaultTex;
        private Texture2D overWorldTex;
        private Texture2D combatTex;
        private Texture2D pixel;
        private Rectangle sourceRec;
        private Vector2 playerPos;
        private Vector2 battlePos;

        private PercentageRectangle healthBar;
        private PercentageRectangle magicBar;
        private PercentageRectangle chargeBar;

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

            healthBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 10, 66, 5), 100, Color.Red);
            magicBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 15, 66, 5), 100, Color.Blue);
            chargeBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 20, 66, 5), 100, Color.Gray);

            battlePos = new Vector2(200, 200);
            color = Color.White;
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);

            rec = new Rectangle((int)playerPos.X, (int)playerPos.Y, sourceRec.Width, sourceRec.Height);
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
            rec = new Rectangle((int)playerPos.X, (int)playerPos.Y, sourceRec.Width, sourceRec.Height);
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
                healthBar.CurrentValue--;
            else if (pad1.IsButtonDown(Buttons.DPadUp))
                healthBar.CurrentValue++;
            timer++;

            //New health bar
            healthBar.SetLocation((int)playerPos.X - 10, (int)playerPos.Y - 10);
            magicBar.SetLocation((int)playerPos.X - 10, (int)playerPos.Y - 20);
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
                    healthBar.Draw(spriteBatch);
                    magicBar.Draw(spriteBatch);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(combatTex, battlePos, sourceRec, color);
                    healthBar.Draw(spriteBatch);
                    magicBar.Draw(spriteBatch);
                    chargeBar.Draw(spriteBatch);
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
