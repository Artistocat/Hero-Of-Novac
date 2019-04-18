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

        private enum GameState
        {
            Overworld, Battlemenu
        }

        private GameState currentGameState;
        private Rectangle battleRec;
        private Rectangle battleSourceRec;


        /*
         * 146 x 116
         */
        public Enemy(Rectangle rec, Rectangle sourceRec, Texture2D tex, Vector2 pos) 
        {
            this.rec = rec;
            this.sourceRec = sourceRec;
            this.tex = tex;
            this.pos = pos;
            currentGameState = GameState.Overworld;

            //TODO
            battleRec = new Rectangle(1920 - 200 - rec.Width, 1080 / 2 - rec.Height - 100, rec.Width, rec.Height); ;
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
                currentGameState = GameState.Battlemenu;
            }
        }

        public void MoveY(int speed)
        {
            rec.Y += speed;
        }

        public void MoveX(int speed)
        {
            rec.X -= speed;
        }

        private void BattleMenuUpdate(GameTime gameTime)
        {

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
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
        }
    }
}
