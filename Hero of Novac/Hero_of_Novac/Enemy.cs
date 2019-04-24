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
        private int timer;
        private GameState currentGameState;
        private Rectangle battleRec;
        private Rectangle battleSourceRec;
        private PercentageRectangle healthBar;
        private Rectangle healthRect;

        /*
         * 146 x 116
         */
        public Enemy(Rectangle rec, Rectangle sourceRec, Texture2D tex, Vector2 pos, Rectangle window/*, Random ran*/) 
        {
            this.rec = rec;
            this.sourceRec = sourceRec;
            this.tex = tex;
            this.pos = pos;
            currentGameState = GameState.Overworld;
            healthBar = new PercentageRectangle(new Rectangle(rec.X - 10, rec.Y - 10, barWidth, barHeight), 50, Color.Red); ;
            timer = 0;
            //TODO
            battleRec = new Rectangle(window.Right - rec.Width * 4, window.Bottom / 4 - rec.Height, rec.Width, rec.Height);
            healthRect = new Rectangle(window.Left + window.Width * 3 / 4 + 25, window.Height / 2 + 100, barWidth * 5, barHeight * 5);
            battleSourceRec = sourceRec;
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
        }

        public void MoveX(int speed)
        {
            rec.X -= speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    spriteBatch.Draw(tex, rec, sourceRec, Color.White);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(tex, battleRec, battleSourceRec, Color.White);
                    healthBar.Draw(spriteBatch, true);
                    break;
            }
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
        //public void randomMove()
        //{
        //    if (timer % 60 == 0 && ranMov == false)
        //    {
        //        // change the second number for how often you want to proc it
        //        if (ran.Next(100) < 30)
        //        {
        //            ranMov = true;
        //        }
        //    }
        //    if (ranMov == true)
        //    {
        //        t++;
        //        // how long it'll move for
        //        if (t / 60 < 2)
        //        {
        //            vol = new Vector2(r1, r2);
        //        }
        //        else
        //        {
        //            //reset
        //            ranMov = false;
        //            t = 0;
        //            vol = new Vector2(0, 0);
        //            r1 = ran.Next(-2, 3);
        //            r2 = ran.Next(-2, 3);
        //        }
        //    }
        //}
    }

}
