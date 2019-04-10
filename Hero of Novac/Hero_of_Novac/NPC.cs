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
        Rectangle window;
        SpriteFont font;
        Rectangle rec;
        Texture2D tex;
        Rectangle source;
        Vector2 vol;
        private int timer = 0;
        private int t = 0;
        Random ran;
        private int r1;
        private int r2;
        private Speech chat;

        private List<string> blackSmith;
        private List<string> armourer;
        private List<string> shopkeep;
        private List<string> hero;
        private List<string> priest;

        bool interact;
        bool ranMov;
        char name;

        public NPC()
        {

        }

        public NPC(Rectangle r, Texture2D t, Rectangle s, Vector2 v, bool i, char n, Speech c)
        {
            ran = new Random();
            rec = r;
            tex = t;
            source = s;
            vol = v;
            interact = i;
            name = n;
            chat = c;
            blackSmith = new List<string>();
            armourer = new List<string>();
            shopkeep = new List<string>();
            hero = new List<string>();
            priest = new List<string>();
            r1 = ran.Next(-2, 3);
            r2 = ran.Next(-2, 3);
            ReadFileAsStrings(@"Content/chartext.txt");
        }

        public void Update(GameTime gameTime)
        {
            timer++;
            randomMove();
            rec.X += (int)vol.X;
            rec.Y += (int)vol.Y;
            if (rec.X < 100)
                rec.X = 100;
            if (rec.X > 800)
                rec.X = 800;
            if (rec.Y < 100)
                rec.Y = 100;
            if (rec.Y > 800)
                rec.Y = 800;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,talk(chat,name),new Vector2(0,0), Color.White);
            spriteBatch.Draw(tex,rec,source,Color.White);
        }

        public void load(SpriteFont f)
        {
            font = f;
        }

        public string talk(Speech s, char c)
        {

            if (s == Speech.Greeting)
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
            else if (s == Speech.Interactable)
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
            else if (s == Speech.Farewell)
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
        public void windowget(Rectangle r)
        {
            window = r;
        }
        public void randomMove()
        {
            if (r1 > 0)
                r1 = 2;
            if (r1 < 0)
                r1 = -2;
            if (r2 > 0)
                r2 = 2;
            if (r2 < 0)
                r2 = -2;
            if (timer % 60 == 0 && ranMov == false)
            {
                // change the second number for how often you want to proc it
                if (ran.Next(100) < 30)
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
    }
}
