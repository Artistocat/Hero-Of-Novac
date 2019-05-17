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
using System.IO;

namespace Hero_of_Novac
{
    public class NPC
    {
        private static Player player;
        private static SpriteFont font;
        private static Texture2D bubblez;
        private Rectangle bubblezSourceRec;
        GamePadState gp;
        GamePadState oldGP;
        KeyboardState KB;
        KeyboardState oldKB;
        Direction dir;
        Direction oldDir;

        int test = 0;
        public bool isTalking;
        int talkwindow = 145;
        private static Texture2D talkW;

        private static Rectangle window;
        public static Rectangle Window
        {
            set { window = value; }
        }
        private Rectangle rec;
        public Rectangle Rectangle
        {
            get { return rec; }
            set { rec = value; }
        }
        public Texture2D tex;
        public Rectangle source;
        public Texture2D headshot;
        static Texture2D heroHead;
        //private static Rectangle sourceHome;
        Vector2 vol;
        Color color;
        private int timer = 0;
        private int t = 0;
        Random ran;
        private int r1;
        private int r2;
        private Speech chat;
        bool doneTalk = false;

        public Rectangle space;

        private List<string> blackSmith;
        private List<string> armourer;
        private List<string> shopkeep;
        private List<string> hero;
        private List<string> priest;

        bool interact;
        public bool IsInteractable
        {
            get { return interact; }
        }
        bool ranMov;
        public char name;

        public NPC()
        {

        }

        public NPC(Rectangle r, Texture2D t, Rectangle s, Rectangle sp, Vector2 v, bool i, char n, Speech c, Random ran, Texture2D face)
        {
            rec = r;
            tex = t;
            source = s;
            //sourceHome = source;
            vol = v;
            interact = i;
            name = n;
            chat = c;
            space = sp;
            headshot = face;
            blackSmith = new List<string>();
            armourer = new List<string>();
            shopkeep = new List<string>();
            hero = new List<string>();
            priest = new List<string>();
            r1 = ran.Next(-2, 3);
            r2 = ran.Next(-2, 3);
            ReadFileAsStrings(@"Content/chartext.txt");
            this.ran = ran;
            isTalking = false;
            bubblezSourceRec = new Rectangle(0, 224, 32, 32);
            oldDir = dir = GetInputDirection();
        }

        public void Update(GameTime gameTime)
        {
            gp = GamePad.GetState(PlayerIndex.One);
            KB = Keyboard.GetState();
            oldDir = dir;
            dir = GetInputDirection();
            timer++;
            //if (doneTalk)
            //    doneTalk = false;
            Rectangle r = new Rectangle(0, 0, 0, 0);
            if ((gp.Buttons.A == ButtonState.Pressed && oldGP.Buttons.A != ButtonState.Pressed) || (KB.IsKeyDown(Keys.Enter) && oldKB.IsKeyUp(Keys.Enter)))
            {

                Vector2 v = player.Position;
                r = new Rectangle((int)v.X - 55, (int)v.Y - 55, 125, 125);
                if (rec.Intersects(r))
                {
                    if (doneTalk)
                    {
                        chat = Speech.None;
                        doneTalk = false;
                        talkwindow = 145;

                    }
                    else
                    {

                        switch (talkwindow)
                        {
                            case 145:
                                if (gp.Buttons.A == ButtonState.Pressed && test > 0)
                                    chat = Speech.Flavor;
                                else
                                {
                                    chat = Speech.Greeting;
                                    test++;
                                }
                                break;
                            case 100:
                                chat = Speech.Interactable;
                                break;
                            case 55:
                                chat = Speech.Farewell;
                                doneTalk = true;
                                test = 0;
                                talkwindow = 145;
                                break;
                        }
                    }
                    
                }
                Talk(chat, name);
            }
            
            if (!isTalking)
            {
                randomMove();
                rec.X += (int)vol.X;
                rec.Y += (int)vol.Y;
            }
            if (dir == Direction.Down && oldDir != Direction.Down)
            {

                if (talkwindow <= 55)
                    talkwindow = 145;
                else
                    talkwindow -= 45;
            }
            if (dir == Direction.Up && oldDir != Direction.Up) //up
            {

                if (talkwindow >= 145)
                    talkwindow = 55;
                else
                    talkwindow += 45;
            }
            if (vol.X == 0 && vol.Y == 0)
                source.X = source.Width;
            else if (Math.Abs(vol.Y) >= Math.Abs(vol.X))
            {
                if (vol.Y > 0)
                    source.Y = 0;
                else
                    source.Y = 216;

            }
            else if (Math.Abs(vol.X) > Math.Abs(vol.Y))
            {
                if (vol.X > 0)
                    source.Y = 144;
                else
                    source.Y = 72;
            }
            
            if (timer % 6 == 0 && !isTalking)
            {
                bubblezSourceRec.X = (bubblezSourceRec.X + bubblezSourceRec.Width) % bubblez.Width;
                if (vol != Vector2.Zero)
                    source.X = (source.X + source.Width) % tex.Width;
            }
            
            if (rec.X < space.Left)
                rec.X = space.Left;
            if (rec.X > space.Right)
                rec.X = space.Right;
            if (rec.Y < space.Top)
                rec.Y = space.Top;
            if (rec.Y > space.Bottom)
                rec.Y = space.Bottom;
            oldGP = gp;
            oldKB = KB;
        }
        
        enum Direction
        {
            Up, Down, Left, Right, Neutral
        }
        //Helper Method for Getting Direction
        private Direction GetInputDirection()
        {
            Direction dir = Direction.Neutral;
            if (gp.ThumbSticks.Left.Y >= .9)
                dir = Direction.Up;
            if (gp.ThumbSticks.Left.Y <= -.9)
                dir = Direction.Down;
            if (gp.ThumbSticks.Left.X <= -.9)
                dir = Direction.Left;
            if (gp.ThumbSticks.Left.X >= .9)
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

        public void MoveY(int speed)
        {
            rec.Y += speed;
            space.Y += speed;
        }

        public void MoveX(int speed)
        {
            rec.X -= speed;
            space.X -= speed;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rec, source, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f / rec.Bottom);
            if (!isTalking)
                spriteBatch.Draw(bubblez, new Rectangle(rec.X + 10, rec.Y - 20, 30, 30), bubblezSourceRec, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f / rec.Bottom);

        }

        public void DrawWindow(SpriteBatch spriteBatch)
        {
            if (isTalking)
            {

                drawTalkingMenu(spriteBatch);
                spriteBatch.Draw(tex, rec, source, Color.White);
                spriteBatch.Draw(headshot, new Rectangle(window.Width - 360, window.Height / 4 * 3 - 480, 360, 480), Color.White);
                spriteBatch.Draw(heroHead, new Rectangle(0, window.Height / 4 * 3 - 480, 360, 480), Color.White);
                spriteBatch.Draw(talkW, new Rectangle(0, window.Height / 4 * 3, window.Width, window.Height / 4), null, Color.White);
                spriteBatch.DrawString(font, Talk(chat, name), new Vector2(35, window.Height / 4 * 3 + 20), Color.White);
            }
        }

        public static void Load(SpriteFont f, Player player, Texture2D b, Texture2D w, Texture2D heroFace)
        {
            bubblez = b;
            font = f;
            NPC.player = player;
            talkW = w;
            heroHead = heroFace;
        }

        public string Talk(Speech s, char c)
        {
            if (s == Speech.Greeting)
            {
                isTalking = true;
                switch (c)
                {
                    case 'b':
                        return blackSmith[0];
                    case 's':
                        return shopkeep[0];
                    case 'a':
                        return armourer[0];
                    case 'h':
                        return hero[0];
                    case 'p':
                        return priest[0];

                }
            }
            else if (s == Speech.Interactable)
            {
                isTalking = true;
                switch (c)
                {
                    case 'b':
                        return blackSmith[1];
                    case 's':
                        return shopkeep[1];
                    case 'a':
                        return armourer[1];
                    case 'h':
                        return hero[1];
                    case 'p':
                        return priest[1];
                }
            }
            else if (s == Speech.Farewell)
            {
                isTalking = true;
                switch (c)
                {
                    case 'b':
                        return blackSmith[2];
                    case 's':
                        return shopkeep[2];
                    case 'a':
                        return armourer[2];
                    case 'h':
                        return hero[2];
                    case 'p':
                        return priest[2];

                }
            }
            else if (s == Speech.Flavor)
            {
                isTalking = true;
                switch (c)
                {
                    case 'b':
                        return blackSmith[3];
                    case 's':
                        return shopkeep[3];
                    case 'a':
                        return armourer[3];
                    case 'h':
                        return hero[3];
                    case 'p':
                        return priest[3];

                }
            }
            isTalking = false;
            return "";
        }

        private void ReadFileAsStrings(string path)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        char firstChar = line[0];
                        line = line.Substring(2);
                        switch (firstChar)
                        {
                            case 'b':
                                blackSmith.Add(line);
                                break;
                            case 's':
                                shopkeep.Add(line);
                                break;
                            case 'a':
                                armourer.Add(line);
                                break;
                            case 'h':
                                hero.Add(line);
                                break;
                            case 'p':
                                priest.Add(line);
                                break;

                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file could not be read:\n" + e.Message);
            }
        }

        public void randomMove()
        {
            if (timer % 60 == 0 && ranMov == false)
            {
                // change the second number for how often you want to proc it
                if (ran.Next(100) < 50)
                {
                    ranMov = true;
                }
            }
            if (ranMov == true)
            {
                t++;
                // how long it'll move for
                if (t / 60 < 2)
                {
                    vol = new Vector2(r1, r2);
                }
                else
                {
                    //reset
                    ranMov = false;
                    t = 0;
                    vol = new Vector2(0, 0);
                    r1 = ran.Next(-2, 3);
                    r2 = ran.Next(-2, 3);
                }
            }
        }
        public void drawTalkingMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(talkW, new Rectangle(360, window.Height / 4 * 3 - 150, 480, 160), Color.White);
            spriteBatch.DrawString(font, Talk(Speech.Greeting, 'h'), new Vector2(370, window.Height / 4 * 3 - 150), Color.Red, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font, Talk(Speech.Interactable, 'h'), new Vector2(370, window.Height / 4 * 3 - 100), Color.Red, 0f, new Vector2(0, 0), .5f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font,Talk(Speech.Farewell,'h'), new Vector2(370, window.Height / 4 * 3 - 50), Color.Red,0f,new Vector2(0,0),.5f,SpriteEffects.None,1);
            spriteBatch.Draw(talkW, new Rectangle(360, window.Height / 4 * 3 - talkwindow, 480, 50), Color.AliceBlue * .5f);
            oldGP = gp;
        }
        //spriteBatch.Draw(headshot, new Rectangle(window.Width - 360, window.Height / 4 * 3 - 480, 360, 480), Color.White);
        //spriteBatch.Draw(heroHead, new Rectangle(0, window.Height / 4 * 3 - 480, 360, 480), Color.White);
        //spriteBatch.Draw(talkW, new Rectangle(0, window.Height / 4 * 3, window.Width, window.Height / 4), null, Color.White);

    }
}

