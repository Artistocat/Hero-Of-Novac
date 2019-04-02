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
        public Texture2D overSprites;
        //Each separate sprite is 26 x 36
        public Texture2D combatSprites;
        public Rectangle overSource;
        public Rectangle combatSource;
        public Rectangle destination;
        public Color colour;
        public Vector2 loc;
        public Vector2 sped;
        public int counter;

        public Player(Texture2D tex, int sW, int sH, int c)
        {
            counter = c;
            overSprites = tex;
            colour = Color.White;
            overSource = new Rectangle(27, 0, 26, 36);
            destination = new Rectangle((sW / 2) - (26 * 2), (sH / 2) - (36 * 2), 26 * 2, 36 * 2);
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            sped = gps.ThumbSticks.Left * 3;//Determines walk speed of character
            destination.X += (int) sped.X;
            destination.Y -= (int) sped.Y;
            if (sped.X == 0 && sped.Y == 0)//If not moving does standing sprite
            {
                overSource.X = 27;

            }else if (Math.Abs(sped.Y) > Math.Abs(sped.X))//Basic direction animation locations
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

            if (sped.X != 0 || sped.Y != 0)//Does the mid animations
            {
                if (counter % 60 == 0)
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
        //public override void Update()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
