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
        protected Rectangle sourceRecProfile;
        protected Texture2D profileTex;
        protected Vector2 pos;

        public Rectangle Rec
        {
            get { return rec; }
        }

        public Rectangle SourceRec
        {
            get { return sourceRec; }
        }

        public Texture2D Tex
        {
            get { return tex; }
        }

        public Rectangle SourceRecProfile
        {
            get { return sourceRecProfile; }
        }

        public Texture2D ProfileTex
        {
            get { return profileTex; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public Entity()
        { 
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
