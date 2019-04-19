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

        //private int healthPoints;
        //private int magicPoints;
        //private int chargePoints;

        private PercentageRectangle healthBar;
        private PercentageRectangle magicBar;
        private PercentageRectangle chargeBar;
        private const int BattleBarX = 25;
        private const int BattleBarY = 1080 / 2 + 100;
        private const int barWidth = 66;
        private const int barHeight = 5;
        private Rectangle healthRect; //for battles
        private Rectangle magicRect;

        //private PercentageRectangle chargeBar;

        //private PercentageRectangle battleHealthBar;
        //private PercentageRectangle battleMagicBar;
        //private PercentageRectangle battleChargeBar;
        
        private bool dead = false;
        
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
            sourceRecBattle = new Rectangle((int)playerPos.X, (int)playerPos.Y, BATTLE_SPRITE_WIDTH, BATTLE_SPRITE_HEIGHT);
            playerPos = new Vector2((window.Width - OVERWORLD_SPRITE_WIDTH) / 2, (window.Height - OVERWORLD_SPRITE_HEIGHT) / 2);

            healthBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 10, barWidth, barHeight), 100, Color.Red);
            magicBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 15, barWidth, barHeight), 100, Color.Blue);

            healthRect = healthBar.Rect;
            healthRect.Width *= 5;
            healthRect.Height *= 5;
            healthRect.X = 25;
            healthRect.Y = window.Height / 2 + 100;
            magicRect = magicBar.Rect;
            magicRect.Width *= 5;
            magicRect.Height *= 5;
            magicRect.X = 25;
            magicRect.Y = window.Height / 2 + 150;
            //battleHealthBar = new PercentageRectangle(healthRect, healthBar.MaxValue, healthBar.Color);
            //battleMagicBar = new PercentageRectangle(magicRect, magicBar.MaxValue, magicBar.Color);
            chargeBar = new PercentageRectangle(new Rectangle(25, window.Height / 2 + 200, 66 * 5, 5 * 5), 100, Color.Gray);

            battlePos = new Vector2(200, 200);
            color = Color.White;
            pixel = p;
            timer = 0;

            hitbox = new Rectangle((int)playerPos.X + (sourceRecWorld.Width - 32) / 2, (int)playerPos.Y + sourceRecWorld.Height - 32, 32, 32);
        }

        public void death()
        {
            dead = true;
            sourceRecBattle.X = BATTLE_SPRITE_WIDTH * 6;
            sourceRecBattle.Y = BATTLE_SPRITE_HEIGHT * 5;
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
            //World Border
            if (hitbox.Top < 0)
            {
                hitbox.Y = 0;
                playerPos.Y = -sourceRecWorld.Height + 32;
            }
            else if (hitbox.Bottom > window.Height)
            {
                hitbox.Y = window.Height - hitbox.Height;
                playerPos.Y = window.Height - sourceRecWorld.Height;
            }
            if (hitbox.Left < 0)
            {
                hitbox.X = 0;
                playerPos.X = -(sourceRecWorld.Width - 32) / 2;
            }
            else if (hitbox.Right > window.Width)
            {
                hitbox.X = window.Width - hitbox.Width;
                playerPos.X = window.Width - sourceRecWorld.Width + (sourceRecWorld.Width - 32) / 2;
            }

            if (!dead)
            {
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
            if (healthBar.CurrentValue <= 0)
                death();
        }
            //Use to test health bar stuff
            if (pad1.IsButtonDown(Buttons.DPadDown))
                healthBar.CurrentValue--;
            else if (pad1.IsButtonDown(Buttons.DPadUp))
                healthBar.CurrentValue++;
            timer++;
            //Testing death stuff
            if (pad1.IsButtonDown(Buttons.LeftShoulder))
            {
                healthBar.CurrentValue = 0;
                death();
            }
            if (pad1.IsButtonDown(Buttons.RightShoulder))
            {
                healthBar.CurrentValue = 100;
                dead = false;
            }
            //New health bar
            healthBar.SetLocation((int)playerPos.X - 10, (int)playerPos.Y - 10);
            magicBar.SetLocation((int)playerPos.X - 10, (int)playerPos.Y - 20);

            //healthBar.CurrentValue = healthPoints;
            //magicBar.CurrentValue = magicPoints;
        }

        public void MoveY(int speed)
        {
            if (!dead)
            {
                playerPos.Y -= speed;
                hitbox.Y -= speed;
            }
        }

        public void MoveX(int speed)
        {
            if (!dead)
            {
                playerPos.X += speed;
                hitbox.X += speed;
            }
        }

        private void UpdateBattlemenu()
        {
            //TODO
            /*
             *Update source rectangle for the battlemenu 
             */
            //battleHealthBar.CurrentValue = healthPoints;
            //battleMagicBar.CurrentValue = magicPoints;
            //battleChargeBar.CurrentValue = chargePoints;
            healthBar.Rect = healthRect;
            magicBar.Rect = magicRect;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState) {
                case GameState.Overworld:
                    if (dead)
                        spriteBatch.Draw(combatTex, playerPos, sourceRecBattle, color);
                    else
                        spriteBatch.Draw(overworldTex, playerPos, sourceRecWorld, color);
                    healthBar.Draw(spriteBatch, false);
                    magicBar.Draw(spriteBatch, false);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(combatTex, battlePos, sourceRecBattle, color);
                    healthBar.Draw(spriteBatch, true);
                    magicBar.Draw(spriteBatch, true);
                    chargeBar.Draw(spriteBatch, true);
                    break;
            }
        }

        public void Battle()
        {
            currentGameState = GameState.Battlemenu;
            healthBar.Rect = healthRect;
            magicBar.Rect = magicRect;
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
            //TODO resetting the values for the health and magic bars in the overworld
        }
    }
}
