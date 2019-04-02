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
        Rectangle rect;
        Rectangle sourceRect;
        Texture2D texture;
        Vector2 pos;
        Vector2 vol;
        
        private List<string> blackSmith;
        private List<string> Armourer;
        private List<string> Shopkeep;
        private List<string> Hero;
        private List<string> Priest;

        Boolean interact;
        string text;
        char name;

        public NPC()
        {

        }

        public NPC(Rectangle r, Texture2D t, Vector2 p, Vector2 v, Boolean i, char n)
        {
            rect = r;
            texture = t;
            pos = p;
            vol = v;
            interact = i;
            name = n;
            ReadFileAsStrings(@"Content/chartext.txt");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,blackSmith[0],new Vector2(0,0), Color.White);
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
                        return Shopkeep[0];
                    case 'a':
                        return Armourer[0];
                    case 'h':
                        return Hero[0];
                    case 'p':
                        return Priest[0];

                }
            else if (s == Speech.Interactable)
                switch (c)
                {
                    case 'b':
                        return blackSmith[1];
                    case 's':
                        return Shopkeep[1];
                    case 'a':
                        return Armourer[1];
                    case 'h':
                        return Hero[1];
                    case 'p':
                        return Priest[1];
                }
            else if (s == Speech.Farewell)
                switch (c)
                {
                    case 'b':
                        return blackSmith[2];
                    case 's':
                        return Shopkeep[2];
                    case 'a':
                        return Armourer[2];
                    case 'h':
                        return Hero[2];
                    case 'p':
                        return Priest[2];

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
                        char firstChar = line.ElementAt(0);
                        switch(firstChar)
                        {
                            case 'b':
                                blackSmith.Add(line);
                                break;
                            case 's':
                                Shopkeep.Add(line);
                                break;
                            case 'a':
                                Armourer.Add(line);
                                break;
                            case 'h':
                                Hero.Add(line);
                                break;
                            case 'p':
                                Priest.Add(line);
                                break;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read");
                Console.WriteLine(e.Message);
            }
        }
    }
}
