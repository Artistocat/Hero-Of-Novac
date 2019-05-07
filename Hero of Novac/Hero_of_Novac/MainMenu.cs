using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class MainMenu
    {
        private static NavigableMenuItem newGame;
        private static NavigableMenuItem loadGame;
        private static NavigableMenuItem exitGame;
        private GamePadState gp;
        private GamePadState oldgp;

        public bool startNewGame = false;
        public bool loadOldGame = false;
        public bool quitGame = false;

        public MainMenu()
        {
            gp = GamePad.GetState(PlayerIndex.One);
            oldgp = gp;
            newGame.isSelected = true;
            loadGame.isSelected = false;
            exitGame.isSelected = false;
        }

        public static void LoadContent(GraphicsDevice graphicsDevice, Rectangle window, SpriteFont Font)
        {
            NavigableMenuItem.Font = Font;
            Texture2D pixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            pixel.SetData(data);
            
            int width = window.Width / 4;
            int height = window.Height / 10;
            newGame = new NavigableMenuItem(new Rectangle((window.Width - width) / 2, 200, width, height), pixel, new Rectangle(0, 0, 1, 1), Color.Purple, "New Game");
            loadGame = new NavigableMenuItem(new Rectangle((window.Width - width) / 2, 250 + height, width, height), pixel, new Rectangle(0, 0, 1, 1), Color.Purple, "Load Game");
            exitGame = new NavigableMenuItem(new Rectangle((window.Width - width) / 2, 300 + 2 * height, width, height), pixel, new Rectangle(0, 0, 1, 1), Color.Purple, "Exit");
            newGame.isSelected = true;
        }

        enum Direction
        {
            Up, Down, Left, Right, Neutral
        }
        //Helper Method for Getting Direction
        private Direction GetInputDirection(GamePadState gamePad)
        {
            Direction dir = Direction.Neutral;
            if (gamePad.ThumbSticks.Left.Y >= .9)
                dir = Direction.Up;
            if (gamePad.ThumbSticks.Left.Y <= -.9)
                dir = Direction.Down;
            if (gamePad.ThumbSticks.Left.X <= -.9)
                dir = Direction.Left;
            if (gamePad.ThumbSticks.Left.X >= .9)
                dir = Direction.Right;
            return dir;
        }

        public void Update()
        {
            oldgp = gp;
            gp = GamePad.GetState(PlayerIndex.One);

            if (!gp.IsButtonDown(Buttons.A) && oldgp.IsButtonDown(Buttons.A))
            {
                if (newGame.isSelected)
                {
                    startNewGame = true;
                }
                if (loadGame.isSelected)
                {
                    loadOldGame = true;
                }
                if (exitGame.isSelected)
                {
                    quitGame = true;
                }
            }

            Direction dir = GetInputDirection(gp);
            Direction oldDir = GetInputDirection(oldgp);
            if (dir == Direction.Down && oldDir != Direction.Down)
            {
                if (newGame.isSelected)
                {
                    newGame.isSelected = false;
                    loadGame.isSelected = true;
                }
                else if (loadGame.isSelected)
                {
                    loadGame.isSelected = false;
                    exitGame.isSelected = true;
                }
            }

            if (dir == Direction.Up && oldDir != Direction.Up)
            {
                if (loadGame.isSelected)
                {
                    loadGame.isSelected = false;
                    newGame.isSelected = true;
                }
                else if (exitGame.isSelected)
                {
                    exitGame.isSelected = false;
                    loadGame.isSelected = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //TODO
            //background.draw
            newGame.Draw(spriteBatch);
            loadGame.Draw(spriteBatch);
            exitGame.Draw(spriteBatch);
        }
        private class NavigableMenuItem
        {
            public static SpriteFont Font;
            Rectangle rect;
            Texture2D texture;
            Rectangle sourceRect;
            Color color;
            Color selectedColor;
            public bool isSelected;
            String name;
            Vector2 nameV;

            public String Name
            {
                get { return name; }
                set
                {
                    name = value;
                    if (name.Length > 0)
                    {
                        Vector2 nameDimensions;
                        nameDimensions = Font.MeasureString(name);
                        if (nameDimensions.X > rect.Width ||
                            nameDimensions.Y > rect.Height)
                        {
                            Console.WriteLine(name);
                            throw new Exception(name + " is too long for the navigable menu item");
                        }
                        float x = (rect.Width - nameDimensions.X) / 2;
                        float y = (rect.Height - nameDimensions.Y) / 2;
                        nameV = new Vector2(rect.X + x, rect.Y + y);
                    }
                }
            }

            public NavigableMenuItem(Rectangle rect, Texture2D texture, Rectangle sourceRect, Color color, String name)
            {
                this.rect = rect;
                this.texture = texture;
                this.sourceRect = sourceRect;
                this.color = color;

                selectedColor = Color.White;
                isSelected = false;
                Name = name;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Color drawColor = color;
                if (isSelected)
                {
                    drawColor = Color.LightPink;
                }
                spriteBatch.Draw(texture, rect, sourceRect, drawColor);
                if (name.Length > 0)
                {
                    spriteBatch.DrawString(Font, name, nameV, Color.Black);
                }
            }
        }
    }
}