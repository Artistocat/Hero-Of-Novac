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
        Rectangle window;

        const int SPRITE_WIDTH = 52;
        const int SPRITE_HEIGHT = 72;

        public Texture2D overSprites;
        public Texture2D combatSprites;
        public Rectangle overSource;
        public Rectangle combatSource;
        public Rectangle destination;
        public Color colour;
        public Vector2 loc;
        public Vector2 sped;
        public int counter;

        public Player(Texture2D tex, Rectangle window)
        {
            this.window = window;
            overSprites = tex;
            colour = Color.White;
            overSource = new Rectangle(SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            destination = new Rectangle((window.Width - SPRITE_WIDTH) / 2, (window.Height - SPRITE_HEIGHT) / 2, SPRITE_WIDTH, SPRITE_HEIGHT);
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            sped = gps.ThumbSticks.Left * 4;
            destination.X += (int) sped.X;
            destination.Y -= (int) sped.Y;
            if (sped.X == 0 && sped.Y == 0)
            {
                overSource.X = 27;
                overSource.Y = 0;

            }else if (Math.Abs(sped.Y) > Math.Abs(sped.X))
            {
                if(sped.Y > 0)
                {
                    overSource.Y = 108;

                }else if(sped.Y < 0)
                {
                    overSource.Y = 0;
                }

            } else if(Math.Abs(sped.X) > Math.Abs(sped.Y))
            {
                if (sped.X > 0)
                {
                    overSource.Y = 72;
                }
                else if (sped.X < 0)
                {
                    overSource.Y = 37;
                }
            }
            if (sped.X != 0 || sped.Y != 0)
            {
                if (counter % 6 == 0)
                {
                    overSource.X += 26;
                }
                if (overSource.X >= 72)
                {
                    overSource.X = 0;
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch spriteBatchTwo = spriteBatch;
            spriteBatchTwo.Draw(overSprites, destination, overSource, colour);
        }
    }
}
