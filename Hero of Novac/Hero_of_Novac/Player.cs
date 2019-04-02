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
        //Each separate sprite is 26 x 40
        public Texture2D combatSprites;
        public Rectangle overSource;
        public Rectangle combatSource;
        public Rectangle destination;
        public Color colour;
        public Vector2 loc;
        public Vector2 sped;

        public Player(Texture2D tex, int sW, int sH)
        {
            overSprites = tex;
            colour = Color.White;
            overSource = new Rectangle(0, 0, 26, 40);
            destination = new Rectangle((sW / 2) - 26, (sH / 2) - 40, 26, 40);
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            sped = gps.ThumbSticks.Left * 3;
            destination.X += (int) sped.X;
            destination.Y -= (int) sped.Y;
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
