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

        private Texture2D defaultTex;
        private Texture2D combatTex;
        private Rectangle sourceRec;
        private Vector2 pos;
        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }
        private Vector2 vol;
        private Color color;
        private int timer;

        public Player(Texture2D defaultTex, Texture2D combatTex, Rectangle window)
        {
            this.window = window;

            this.defaultTex = defaultTex;
            this.combatTex = combatTex;
            sourceRec = new Rectangle(SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            pos = new Vector2((window.Width - SPRITE_WIDTH) / 2, (window.Height - SPRITE_HEIGHT) / 2);
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            vol = pad1.ThumbSticks.Left * 4;
            pos.X += vol.X;
            pos.Y -= vol.Y;

            if (vol.X == 0 && vol.Y == 0)
                sourceRec.X = SPRITE_WIDTH;
            else if (Math.Abs(vol.Y) > Math.Abs(vol.X))
            {
                if (vol.Y > 0)
                    sourceRec.Y = 216;
                else
                    sourceRec.Y = 0;

            }
            else if(Math.Abs(vol.X) > Math.Abs(vol.Y))
            {
                if (vol.X > 0)
                    sourceRec.Y = 144;
                else 
                    sourceRec.Y = 74;
            }
            if (vol.X != 0 || vol.Y != 0)
            {
                if (timer % 6 == 0)
                    sourceRec.X = (sourceRec.X + SPRITE_WIDTH) % defaultTex.Width;
            }
            timer++;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch spriteBatchTwo = spriteBatch;
            spriteBatchTwo.Draw(defaultTex, pos, sourceRec, color);
        }
    }
}
