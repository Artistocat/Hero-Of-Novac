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
    public class Enemy : Entity
    {
        private static Player player;
        private static Rectangle window;
        public static Rectangle Window
        {
            set { window = value; }
        }

        private const int barWidth = 66;
        private const int barHeight = 5;
        private enum GameState
        {
            Overworld, Battlemenu
        }

        private Random ran;
        private int timer = 0;
        private int t = 0;
        private int r1;
        private int r2;
        private Vector2 vol;
        private bool ranMov;
        private Rectangle space;

        private GameState currentGameState;
        private Rectangle battleRec;
        private Rectangle battleSourceRec;
        private PercentageRectangle healthBar;
        private Rectangle healthRect;
        private PercentageRectangle chargeBar;

        /*
         * 146 x 116
         */
        public Enemy(Rectangle rec, Rectangle sourceRec,Rectangle space, Texture2D tex, Vector2 pos, Rectangle window, Random ran, Vector2 vol) 
        {
            this.space = space;
            this.vol = vol;
            this.rec = rec;
            this.sourceRec = sourceRec;
            this.tex = tex;
            this.pos = pos;
            this.ran = ran;
            currentGameState = GameState.Overworld;
            healthBar = new PercentageRectangle(new Rectangle(rec.X - 10, rec.Y - 10, barWidth, barHeight), 50, Color.Red);
            chargeBar = new PercentageRectangle(new Rectangle(healthBar.Rect.X, healthBar.Rect.Y + 50, barWidth, barHeight), 100, Color.Gray);
            timer = 0;
            battleRec = new Rectangle(window.Right - rec.Width * 4, window.Bottom / 4 - rec.Height, rec.Width, rec.Height);
            healthRect = new Rectangle(window.Left + window.Width * 3 / 4 + 25, window.Height / 2 + 100, barWidth * 5, barHeight * 5);
            Rectangle chargeRect = healthRect;
            chargeRect.Y += 50;
            chargeBar.Rect = chargeRect;                 
            battleSourceRec = sourceRec;
            battleSourceRec.Y = 116;
        }

        public static void LoadContent(Player player)
        {
            Enemy.player = player;

        }

        public void Update(GameTime gametime)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    OverworldUpdate(gametime);
                    break;
                case GameState.Battlemenu:
                    BattleMenuUpdate(gametime);
                    break;
            }
            randomMove();
            rec.X += (int)vol.X;
            rec.Y += (int)vol.Y;
            if (rec.X < space.Left)
                rec.X = space.Left;
            if (rec.X > space.Right)
                rec.X = space.Right;
            if (rec.Y < space.Top)
                rec.Y = space.Top;
            if (rec.Y > space.Bottom)
                rec.Y = space.Bottom;

            if (vol.X == 0 && vol.Y == 0)
                sourceRec.X = sourceRec.Width;
            else if (Math.Abs(vol.Y) >= Math.Abs(vol.X))
            {
                if (vol.Y > 0)
                    sourceRec.Y = 0;
                else
                    sourceRec.Y = 348;

            }
            else if (Math.Abs(vol.X) > Math.Abs(vol.Y))
            {
                if (vol.X > 0)
                    sourceRec.Y = 232;
                else
                    sourceRec.Y = 116;
            }

            if (timer % 6 == 0 && vol != Vector2.Zero)
                    sourceRec.X = (sourceRec.X + sourceRec.Width) % tex.Width;
            timer++;
        }

        private void OverworldUpdate(GameTime gameTime)
        {
            if (rec.Intersects(player.Hitbox))
            {
                player.Battle();
                Battle();
                currentGameState = GameState.Battlemenu;
            }
            healthBar.SetLocation(rec.X - 10, rec.Y - 10);
        }

        private void BattleMenuUpdate(GameTime gameTime)
        {
            healthBar.Rect = healthRect;
        }

        public void MoveY(int speed)
        {
            rec.Y += speed;
            space.Y += speed;
        }

        public void MoveX(int speed)
        {
            rec.X -= speed;
            space.X -= speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    spriteBatch.Draw(tex, rec, sourceRec, Color.White, 0f, Vector2.Zero, SpriteEffects.None, (float)(window.Height - rec.Bottom) / window.Height);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(tex, battleRec, battleSourceRec, Color.White);
                    healthBar.Draw(spriteBatch, true);
                    chargeBar.Draw(spriteBatch, true);
                    break;
            }
        }

        public void Damage(int damage)
        {
            healthBar.CurrentValue -= damage;
        }

        public bool IsInBattle()
        {
            return currentGameState == GameState.Battlemenu;
        }

        public void Battle()
        {
            currentGameState = GameState.Battlemenu;
            healthBar.Rect = healthRect;
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
        }
        public void randomMove()
        {
            if (timer % 60 == 0 && ranMov == false)
            {
                // change the second number for how often you want to proc it
                if (ran.Next(100) < 50)
                {
                    ranMov = true;
                }
            }
            if (ranMov == true)
            {
                t++;
                // how long it'll move for
                if (t / 60 < 2)
                {
                    vol = new Vector2(r1, r2);
                }
                else
                {
                    //reset
                    ranMov = false;
                    t = 0;
                    vol = new Vector2(0, 0);
                    r1 = ran.Next(-2, 3);
                    r2 = ran.Next(-2, 3);
                }
            }
        }
    }

}
