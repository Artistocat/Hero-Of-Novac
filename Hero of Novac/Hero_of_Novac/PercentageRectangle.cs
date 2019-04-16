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
        private static SpriteFont Font;
        private int maxValue;
        private int currentValue;
        public int CurrentValue
        {
            get
            {
                return currentValue;
            }

            set
            {
                if (currentValue > MaxValue - 1)
                {
                    currentValue = MaxValue - 1;
                }
                else if (currentValue < 0)
                    currentValue = 0;
                else
                    currentValue = value;
                partialRect.Width = Rect.Width * currentValue / MaxValue;
            }
        }

        public Rectangle Rect
        {
            get
            {
                return rect;
            }

            set
            {
                rect = value;
                partialRect = new Rectangle(rect.X + 1, rect.Y + 1, (rect.Width - 2) * currentValue / MaxValue, rect.Height - 2);
            }
        }

        public int MaxValue
        {
            get
            {
                return maxValue;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
        }

        public PercentageRectangle(Rectangle rect, int maxValue, Color color)
        {
            currentValue = this.maxValue = maxValue;
            this.rect = rect;
            this.color = color;
            partialRect = new Rectangle(rect.X + 1, rect.Y + 1, (rect.Width - 2) * currentValue / MaxValue, rect.Height - 2);
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

        public PercentageRectangle Clone()
        {
            PercentageRectangle newRect = new PercentageRectangle(Rect, MaxValue, color);
            newRect.currentValue = currentValue;
            return newRect;
        }

        public static void LoadContent(Texture2D texture, SpriteFont font)
        {
            pix = texture;
            Font = font;
        }

        public void Draw(SpriteBatch spriteBatch, bool drawVal)
        {
            spriteBatch.Draw(pix, Rect, Color.Black);
            spriteBatch.Draw(pix, partialRect, color);

            if (!drawVal) return;

            spriteBatch.DrawString(Font, "" + currentValue, new Vector2(Rect.Right + 10, Rect.Y), color);
        }
    }
}
