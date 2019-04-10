using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    //Every Enemy, npc and player is an entity, and will have to have implement the methods listed here
    public abstract class Entity 
    {
        protected Rectangle rec;
        protected Rectangle sourceRec;
        protected Texture2D tex;
        protected Vector2 pos;

        public Rectangle Rec
        {
            get
            {
                return rec;
            }
        }

        enum Stance
        {

        }

        public Entity()
        { 
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
