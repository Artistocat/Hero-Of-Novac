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

        private const int OVERWORLD_SPRITE_WIDTH = 52;
        private const int OVERWORLD_SPRITE_HEIGHT = 72;
        private const int BATTLE_SPRITE_WIDTH = 96;
        private const int BATTLE_SPRITE_HEIGHT = 96;


        private enum GameState
        {
            Overworld, Battlemenu
        }

        private GameState currentGameState;

        private Texture2D overworldTex;
        private Texture2D combatTex;
        private Texture2D pixel;
        private Rectangle sourceRecWorld;
        private Rectangle sourceRecBattle;
        public Rectangle SourceRec
        {
            get { return sourceRecWorld; }
        }
        private Vector2 playerPos;
        private Vector2 battlePos;
        public Vector2 Position
        {
            get { return playerPos; }
            set { playerPos = value; }
        }
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        private PercentageRectangle healthBar;
        private PercentageRectangle magicBar;
        private PercentageRectangle chargeBar;
        private Boolean dead = false;
        
        private Color color;
        private int timer;

        public Player(Texture2D overworldTex, Texture2D combatTex, Texture2D p, Rectangle window)
        {
            currentGameState = GameState.Overworld; 
            this.window = window;

            this.overworldTex = overworldTex;
            this.combatTex = combatTex;
            pixel = p;
            sourceRecWorld = new Rectangle(OVERWORLD_SPRITE_WIDTH, 0, OVERWORLD_SPRITE_WIDTH, OVERWORLD_SPRITE_HEIGHT);
            playerPos = new Vector2((window.Width - OVERWORLD_SPRITE_WIDTH) / 2, (window.Height - OVERWORLD_SPRITE_HEIGHT) / 2);

            healthBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 10, 66, 5), 100, Color.Red);
            magicBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 15, 66, 5), 100, Color.Blue);
            chargeBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 20, 66, 5), 100, Color.Gray);

            battlePos = new Vector2(200, 200);
            color = Color.White;
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);
            timer = 0;

            hitbox = new Rectangle((int)playerPos.X + (sourceRec.Width - 32) / 2, (int)playerPos.Y + sourceRec.Height - 32, sourceRec.Width, sourceRec.Height);
        }

        public void death()
        {
            dead = true;
            sourceRec

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
            rec = new Rectangle((int)playerPos.X, (int)playerPos.Y, sourceRecWorld.Width, sourceRecWorld.Height);
        }

        private void UpdateOverworld(GameTime gameTime, Vector2 speed)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            if (playerPos.Y < 0)
                playerPos.Y = 0;
            else if (playerPos.Y + sourceRecWorld.Height > window.Height)
                playerPos.Y = window.Height - sourceRecWorld.Height;
            if (playerPos.X < 0)
                playerPos.X = 0;
            else if (playerPos.X + sourceRecWorld.Width > window.Width)
                playerPos.X = window.Width - sourceRecWorld.Width;


            if (speed == Vector2.Zero)
                sourceRecWorld.X = OVERWORLD_SPRITE_WIDTH;
            else if (Math.Abs(speed.Y) > Math.Abs(speed.X))
            {
                if (speed.Y > 0)
                    sourceRecWorld.Y = 216;
                else
                    sourceRecWorld.Y = 0;

            }
            else if (Math.Abs(speed.X) > Math.Abs(speed.Y))
            {
                if (speed.X > 0)
                    sourceRecWorld.Y = 144;
                else
                    sourceRecWorld.Y = 72;
            }
            if (speed != Vector2.Zero)
            {
                if (timer % 6 == 0)
                    sourceRecWorld.X = (sourceRecWorld.X + OVERWORLD_SPRITE_WIDTH) % overworldTex.Width;
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

        public void MoveY(int speed)
        {
            playerPos.Y -= speed;
        }

        public void MoveX(int speed)
        {
            playerPos.X += speed;
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
                    if (dead == true)
                    {
                        spriteBatch.Draw(combatTex, playerPos, sourceRecBattle, color);
                    }
                    else
                        spriteBatch.Draw(overworldTex, playerPos, sourceRecWorld, color);
                    healthBar.Draw(spriteBatch);
                    magicBar.Draw(spriteBatch);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(combatTex, battlePos, sourceRecBattle, color);
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
