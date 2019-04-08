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
    public class NPC : Entity
    {
        SpriteFont font;
        Rectangle rec;
        Rectangle sourceRec;
        Texture2D tex;
        Vector2 pos;
        Vector2 vol;
        private int timer = 0;
        private int t = 0;
        private int r1;
        private int r2;

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

        public NPC(Rectangle r, Texture2D t, Vector2 p, Vector2 v, bool i, char n)
        {
            rec = r;
            tex = t;
            pos = p;
            vol = v;
            interact = i;
            name = n;
            blackSmith = new List<string>();
            armourer = new List<string>();
            shopkeep = new List<string>();
            hero = new List<string>();
            priest = new List<string>();
            ReadFileAsStrings(@"Content/chartext.txt");
        }

        public override void Update(GameTime gameTime)
        {
            timer++;
            Random r = new Random();
            if (timer % 60 == 0 && ranMov == false)
            {
                
                if(r.Next(100) < 80)
                {
                    ranMov = true;
                }
            }
            if(ranMov == true)
            {
                t++;
                if(t % 60 < 6)
                {
                    vol = new Vector2(r.Next(5), r.Next(5));
                }
                else
                {
                    ranMov = false;
                    t = 0;
                    vol = new Vector2(0, 0);
                }
            }
            rec.X += (int)vol.X;
            rec.Y += (int)vol.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,blackSmith[0],new Vector2(0,0), Color.White);
            spriteBatch.Draw(tex, rec, Color.White);
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
            return "no text";
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
    }
}
