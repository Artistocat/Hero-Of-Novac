using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class DefeatMenu
    {
        private static SpriteFont font;
        private static Rectangle window;
        private static Texture2D pixel;
        private const string DEFEAT_MESSAGE = "Game Over";

        GamePadState gp;
        GamePadState oldgp;
        KeyboardState KB;
        KeyboardState oldKB;

        int tics;
        bool giveChoices;

        public bool quitGame;
        public bool loadSave;

        NavigableMenuItem loadLastSave;
        NavigableMenuItem exit;

        public DefeatMenu()
        {
            gp = GamePad.GetState(PlayerIndex.One);
            oldgp = gp;
            KB = Keyboard.GetState();
            oldKB = KB;
            tics = 0;
            giveChoices = false;

            int width = window.Width / 4;
            int height = window.Height / 10;
            loadLastSave = new NavigableMenuItem(new Rectangle((window.Width - width) / 2, 200, width, height), pixel, new Rectangle(0, 0, 1, 1), Color.Black, "Load last Save");
            exit = new NavigableMenuItem(new Rectangle((window.Width - width) / 2, 250 + height, width, height), pixel, new Rectangle(0, 0, 1, 1), Color.Black, "Exit");
            loadLastSave.isSelected = true;
            quitGame = loadSave = false;
        }

        public static void LoadContent(SpriteFont font, Rectangle window, SpriteFont font2, GraphicsDevice GraphicsDevice)
        {
            DefeatMenu.font = font;
            DefeatMenu.window = window;
            NavigableMenuItem.Font = font2;
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);
        }

        public void Update()
        {
            oldgp = gp;
            gp = GamePad.GetState(PlayerIndex.One);
            oldKB = KB;
            KB = Keyboard.GetState();
            if (gp.Buttons.A == ButtonState.Released && oldgp.Buttons.A == ButtonState.Pressed ||
                !KB.IsKeyDown(Keys.Enter) && oldKB.IsKeyDown(Keys.Enter))
            {
                if (exit.isSelected)
                    quitGame = true;
                if (loadLastSave.isSelected)
                    loadSave = true;
            }
            if (giveChoices)
            {
                Direction dir = GetInputDirection(gp, KB);
                Direction oldDir = GetInputDirection(oldgp, oldKB);
                if (dir == Direction.Down && oldDir != Direction.Down ||
                    dir == Direction.Up && oldDir != Direction.Up)
                {
                    if (loadLastSave.isSelected == true)
                    {
                        exit.isSelected = true;
                        loadLastSave.isSelected = false;
                    }
                    else
                    {
                        loadLastSave.isSelected = true;
                        exit.isSelected = false;
                    }
                }
            }
            tics++;
            if (tics >= 60)
            {
                giveChoices = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!giveChoices)
                spriteBatch.DrawString(font, DEFEAT_MESSAGE, new Vector2(window.Width / 2 - font.MeasureString(DEFEAT_MESSAGE).X / 2, window.Height / 3), Color.Red);
            else
            {
                loadLastSave.Draw(spriteBatch);
                exit.Draw(spriteBatch);
            }
        }

        enum Direction
        {
            Up, Down, Left, Right, Neutral
        }
        //Helper Method for Getting Direction
        private Direction GetInputDirection(GamePadState gamePad, KeyboardState KB)
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

            if (KB.IsKeyDown(Keys.W))
                dir = Direction.Up;
            if (KB.IsKeyDown(Keys.A))
                dir = Direction.Left;
            if (KB.IsKeyDown(Keys.S))
                dir = Direction.Down;
            if (KB.IsKeyDown(Keys.D))
                dir = Direction.Right;
            return dir;
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
                    drawColor = Color.White;
                }
                spriteBatch.Draw(texture, rect, sourceRect, drawColor);
                if (name.Length > 0)
                {
                    spriteBatch.DrawString(Font, name, nameV, Color.Goldenrod);
                }
            }
        }
    }
}
