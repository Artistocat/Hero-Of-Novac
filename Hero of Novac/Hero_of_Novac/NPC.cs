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
    public class NPC : Entity
    {
        Rectangle rect;
        Rectangle sourceRect;
        Texture2D texture;
        Vector2 pos;

        Boolean interact;
        string text;
        char name;


        public NPC()
        {

        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
        public string speech(char a)
        {
            return "didnt work";
        }
    }
}
