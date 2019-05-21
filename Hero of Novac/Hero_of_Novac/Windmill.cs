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
    class Windmill
    {
        private Texture2D tex;
        private Rectangle rec;
        private Vector2 origin;
        private float angle;

        public Windmill(Texture2D t, Rectangle r)
        {
            tex = t;
            rec = r;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            angle = 0;
        }

        public void Update(GameTime gameTime)
        {
            angle = (angle + .25f) % 360;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle areaRec)
        {
            Rectangle tempRec = rec;
            tempRec.X += areaRec.X;
            tempRec.Y += areaRec.Y;
            if (tempRec.Intersects(areaRec)) 
                spriteBatch.Draw(tex, tempRec, null, Color.White, MathHelper.ToRadians(angle), origin, SpriteEffects.None, 0f);
        }
    }
}
