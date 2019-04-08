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

        private Texture2D defaultTex;
        private Texture2D combatTex;
        private Rectangle sourceRec;
        public Rectangle SourceRec
        {
            get { return sourceRec; }
        }

        private Vector2 pos;
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }
        private Color color;
        private int timer;

        public Player(Texture2D defaultTex, Texture2D combatTex, Rectangle window)
        {
            this.window = window;

            this.defaultTex = defaultTex;
            this.combatTex = combatTex;
            sourceRec = new Rectangle(SPRITE_WIDTH, 2, SPRITE_WIDTH, SPRITE_HEIGHT);
            pos = new Vector2((window.Width - SPRITE_WIDTH) / 2, (window.Height - SPRITE_HEIGHT) / 2);
            color = Color.White;
            timer = 0;
        }

        public void Update(GameTime gameTime, Vector2 speed)
        {
            if (pos.Y < 0)
                pos.Y = 0;
            else if (pos.Y + sourceRec.Height > window.Height)
                pos.Y = window.Height - sourceRec.Height;
            if (pos.X < 0)
                pos.X = 0;
            else if (pos.X + sourceRec.Width > window.Width)
                pos.X = window.Width - sourceRec.Width;


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
                    sourceRec.X = (sourceRec.X + SPRITE_WIDTH) % defaultTex.Width;
            }
            timer++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch spriteBatchTwo = spriteBatch;
            spriteBatchTwo.Draw(defaultTex, pos, sourceRec, color);
        }
    }
}
