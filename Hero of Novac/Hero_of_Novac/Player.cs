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
        private Vector2 sped;
        private Color color;
        private int counter;

        public Player(Texture2D defaultTex, Texture2D combatTex, Rectangle window)
        {
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            sped = gps.ThumbSticks.Left * 3;//Determines walk speed of character
            destination.X += (int)sped.X;
            destination.Y -= (int)sped.Y;
            protected override void Update(GameTime gameTime)
        {
                if (sped.X == 0 && sped.Y == 0)//If not moving does standing sprite
                {
                    sourceRec.X = 27;

                }
                else if (Math.Abs(sped.Y) > Math.Abs(sped.X))//Basic direction animation locations
                {
                    if (sped.Y > 0)
                    {
                        sourceRec.Y = 108;

                        if (sped.X == 0 && sped.Y == 0)
                            sourceRec.X = SPRITE_WIDTH;
                        else if (Math.Abs(sped.Y) > Math.Abs(sped.X))
                        {
                            if (sped.Y > 0)
                                sourceRec.Y = 216;
                            else
                                sourceRec.Y = 0;

                        }
                        else if (Math.Abs(sped.X) > Math.Abs(sped.Y))
                        {
                            if (sped.X > 0)
                            {
                                sourceRec.Y = 72;
                            }
                            else if (sped.X < 0)
                            {
                                sourceRec.Y = 37;
                            }
                        }
                        else if (Math.Abs(sped.X) > Math.Abs(sped.Y))
                        {
                            if (sped.X > 0)
                                sourceRec.Y = 144;
                            else
                                sourceRec.Y = 74;
                        }
                        if (sped.X != 0 || sped.Y != 0)
                        {
                            if (counter % 6 == 0)
                                sourceRec.X = (sourceRec.X + SPRITE_WIDTH) % defaultTex.Width;

                            if (sped.X != 0 || sped.Y != 0)//Does the mid animations
                            {
                                if (counter % 60 == 0)
                                {
                                    sourceRec.X += 26;
                                }
                                if (sourceRec.X >= 72)
                                {
                                    sourceRec.X = 0;
                                }
                            }
                            counter++;
                        }
                    }
                }

            }
            
        }

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteBatch spriteBatchTwo = spriteBatch;
            spriteBatchTwo.Draw(defaultTex, pos, sourceRec, color);
        }
    }
}
