using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class PercentageRectangle
    {
        private Rectangle rect;
        private Rectangle partialRect;
        private Color color;
        private static Texture2D pix;
        private int MaxValue;
        private int currentValue;
        public int CurrentValue
        {
            get
            {
                return currentValue;
            }

            set
            {
                if (currentValue > MaxValue)
                    currentValue = MaxValue;
                else if (currentValue < 0)
                    currentValue = 0;
                else
                    currentValue = value;
                partialRect.Width = rect.Width * currentValue / MaxValue;
            }
        }

        public PercentageRectangle(Rectangle rect, int MaxValue, Color color)
        {
            currentValue = this.MaxValue = MaxValue;
            this.rect = rect;
            this.color = color;
            partialRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width * currentValue / MaxValue, rect.Height - 2);
        }

        public void SetLocation(Vector2 loc)
        {
            SetLocation((int)loc.X, (int)loc.Y);
        }

        public void SetLocation(int x, int y)
        {
            rect.X = x;
            rect.Y = y;
            partialRect.X = x + 1;
            partialRect.Y = y + 1;
        }

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            pix = new Texture2D(graphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pix.SetData(pixelColors);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pix, rect, Color.Black);
            spriteBatch.Draw(pix, partialRect, color);
        }
    }
}
