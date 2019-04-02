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
        public Texture2D sprites;
        public Rectangle source;
        public Rectangle destination;
        public Color colour;
        public Vector2 loc;
        public Vector2 vol;

        public Player()
        {

        }


        public override void Update()
        {
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            if (gps.ThumbSticks.Left.X > 0 || gps.ThumbSticks.Left.Y > 0)
            {
                vol = gps.ThumbSticks.Left;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
        //public override void Update()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
