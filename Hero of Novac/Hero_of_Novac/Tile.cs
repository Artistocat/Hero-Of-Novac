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
    public class Tile
    {
        private const int WIDTH = 32;
        private const int HEIGHT = 32;

        private Rectangle rec;
        public Rectangle Rectangle
        {
            get { return rec; }
            set { rec = value; }
        }

        private Rectangle sourceRec;
        public Rectangle SourceRec
        {
            get { return sourceRec; }
        }
        private Texture2D tex;
        public Texture2D Texture
        {
            get { return tex; }
        }
        private bool isAnimated;
        public bool IsAnimated
        {
            get { return isAnimated; }
        }
        private bool isPasssable;
        public bool IsPassable
        {
            get { return isPasssable; }
        }
        private bool isOnTop;
        public bool IsOnTop
        {
            get { return isOnTop; }
        }
        private int timer;

        /// <summary>
        /// Creates a new tile.
        /// </summary>
        /// <param name="position">A vector that indicates where the tile is located.</param>
        /// <param name="source">A rectangle that indicates where on the texture the tile is located.</param>
        /// <param name="texture">The texture for the tile.</param>
        public Tile(Vector2 position, Rectangle source, Texture2D texture)
        {
            rec = new Rectangle((int)position.X * WIDTH, (int)position.Y * HEIGHT, WIDTH, HEIGHT);
            sourceRec = source;
            tex = texture;
            isAnimated = false;
            isPasssable = true;
            isOnTop = false;
            timer = 0;
        }

        /// <summary>
        /// Creates a new tile.
        /// </summary>
        /// <param name="position">A vector that indicates where the tile is located.</param>
        /// <param name="source">A rectangle that indicates where on the texture the tile is located.</param>
        /// <param name="texture">The texture for the tile.</param>
        /// <param name="animated">A boolean indicating if the tile is animated.</param>
        public Tile(Vector2 position, Rectangle source, Texture2D texture, bool animated)
        {
            rec = new Rectangle((int)position.X * WIDTH, (int)position.Y * HEIGHT, WIDTH, HEIGHT);
            sourceRec = source;
            tex = texture;
            isAnimated = animated;
            isPasssable = false;
            isOnTop = false;
            timer = 0;
        }

        /// <summary>
        /// Creates a new tile.
        /// </summary>
        /// <param name="position">A vector that indicates where the tile is located.</param>
        /// <param name="source">A rectangle that indicates where on the texture the tile is located.</param>
        /// <param name="texture">The texture for the tile.</param>
        /// <param name="passable">A boolean indicating if the tile is passable.</param>
        /// <param name="onTop">A boolean indicating if the tile is displayed above the player.</param>
        /// <param name="id">An integer that repesents the ID of the object the tile is a part of.</param>
        public Tile(Vector2 position, Rectangle source, Texture2D texture, bool passable, bool onTop)
        {
            rec = new Rectangle((int)position.X * WIDTH, (int)position.Y * HEIGHT, WIDTH, HEIGHT);
            sourceRec = source;
            tex = texture;
            isAnimated = false;
            isPasssable = passable;
            isOnTop = onTop;
            timer = 0;
        }

        /// <summary>
        /// Updates the tile.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (timer % 6 == 0)
                sourceRec.Y = (sourceRec.Y + sourceRec.Height) % tex.Height;
            timer++;
        }
    }
}
