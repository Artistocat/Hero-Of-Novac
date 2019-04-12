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
    class Area
    {
        private const int TILE_WIDTH = 32;
        private const int TILE_HEIGHT = 32;

        private List<Tile> tiles;

        private int areaWidth;
        public int Width
        {
            get { return areaWidth; }
        }
        private int areaHeight;
        public int Height
        {
            get { return areaHeight; }
        }
        private Rectangle areaRec;
        private Rectangle window;

        Dictionary<string, Rectangle> sourceRecs;
        Dictionary<string, Texture2D> tileSheets;

        private Texture2D pix;

        private Player player;
        public Player Player
        {
            get { return player; }
        }

        private List<NPC> npcs;
        private List<Enemy> enemies;
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        ContentManager content;
        public ContentManager Content
        {
            get { return content; }
        }

        private Random random = new Random(1102);

        /// <summary>
        /// Creates a new area in the world.
        /// </summary>
        /// <param name="serviceProvider">Provides a service provider.</param>
        /// <param name="path">The path to the folder holding the level data.</param>
        /// <param name="window">A rectangle representing the veiwing window of the game.</param>
        public Area(IServiceProvider serviceProvider, string path, Texture2D p, Rectangle window)
        {
            content = new ContentManager(serviceProvider, "Content");

            this.window = window;

            sourceRecs = new Dictionary<string, Rectangle>();
            sourceRecs.Add("grass", new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("flower", new Rectangle(0, 64, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("dirt", new Rectangle(0, 96, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("rock", new Rectangle(0, 128, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("road", new Rectangle(0, 160, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("sand", new Rectangle(0, 192, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("water", new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT));

            tileSheets = new Dictionary<string, Texture2D>();
            tileSheets.Add("grass", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("flower", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("dirt", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("rock", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("road", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("sand", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("water", Content.Load<Texture2D>("water"));

            tiles = new List<Tile>();
            LoadTiles(path + "/terrain.txt");

            pix = p;
            player = new Player(Content.Load<Texture2D>("player_walking"), Content.Load<Texture2D>("player_combat"), pix, window);

            SpriteFont font = Content.Load<SpriteFont>("SpriteFont1");
            npcs = new List<NPC>();
            AddNPCs(font);
            enemies = new List<Enemy>();
            AddEnemies();
        }

        /// <summary>
        /// Loads all the tiles for an area.
        /// </summary>
        /// <param name="path">Provides the path to the terrain data.</param>
        private void LoadTiles(string path)
        {
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    areaWidth = line.Length;
                    while (line != null)
                    {
                        lines.Add(line);
                        if (line.Length != areaWidth)
                            throw new Exception(string.Format("The length of line {0} is different from all proceeding lines.", lines.Count));
                        line = reader.ReadLine();
                    }
                    areaHeight = lines.Count;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:\n" + e.Message);
            }

            areaRec = new Rectangle(0, 0, Width * TILE_WIDTH, Height * TILE_HEIGHT);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    char type = lines[y][x];
                    switch (type)
                    {
                        case 'g':
                            if (random.Next(10) == 0)
                                LoadTile("flower", x, y, 18);
                            else
                                LoadTile("grass", x, y, 5);
                            break;
                        case 'd':
                            if (random.Next(10) == 0)
                                LoadTile("rock", x, y, 8);
                            else
                                LoadTile("dirt", x, y, 5);
                            AddEdge(lines, x, y);
                            break;
                        case 's':
                            LoadTile("sand", x, y, 7);
                            AddEdge(lines, x, y);
                            break;
                        case 'r':
                            LoadTile("road", x, y, 4);
                            AddEdge(lines, x, y);
                            break;
                        case 'w':
                            AddWater(lines, x, y);
                            AddEdge(lines, x, y);
                            break;
                        case '.':
                            AddEdge(lines, x, y);
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", type, x, y));
                    }
                }
        }

        /// <summary>
        /// Loads from a given number of variations and sets it to its position in the world.
        /// </summary>
        /// <param name="type">Identifies the type of tile being placed.</param>
        /// <param name="x">The x position of the current tile.</param>
        /// <param name="y">The y position of the current tile.</param>
        /// <param name="numVariations">The number of variations of the tile.</param>
        private void LoadTile(string type, int x, int y, int numVariations)
        {
            Rectangle tileSource = sourceRecs[type];
            tileSource.X += random.Next(numVariations) * TILE_WIDTH;
            tiles.Add(new Tile(new Vector2(x, y), tileSource, tileSheets[type]));
        }

        /// <summary>
        /// Adds a overlay to tiles based on what type of tile they're boardering.
        /// </summary>
        /// <param name="lines">A string representation of the terrain.</param>
        /// <param name="x">The x position of the current tile.</param>
        /// <param name="y">The y position of the current tile.</param>
        private void AddEdge(List<string> lines, int x, int y)
        {
            if (y > 0 && lines[y - 1][x] == 'g')
                tiles.Add(new Tile(new Vector2(x, y), new Rectangle(0, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            if (y < lines.Count - 1 && lines[y + 1][x] == 'g')
                tiles.Add(new Tile(new Vector2(x, y), new Rectangle(32, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            if (x > 0 && lines[y][x - 1] == 'g')
                tiles.Add(new Tile(new Vector2(x, y), new Rectangle(64, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            if (x < lines[y].Length - 1 && lines[y][x + 1] == 'g')
                tiles.Add(new Tile(new Vector2(x, y), new Rectangle(96, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            if (y > 0 && x > 0)
            {
                if (lines[y - 1][x] != 'g' && lines[y][x - 1] != 'g' && lines[y - 1][x - 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(128, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
                else if (lines[y - 1][x] == 'g' && lines[y][x - 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(256, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            }
            if (y > 0 && x < lines[y].Length - 1)
            {
                if (lines[y - 1][x] != 'g' && lines[y][x + 1] != 'g' && lines[y - 1][x + 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(160, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
                else if (lines[y - 1][x] == 'g' && lines[y][x + 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(288, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            }
            if (y < lines.Count - 1 && x > 0)
            {
                if (lines[y + 1][x] != 'g' && lines[y][x - 1] != 'g' && lines[y + 1][x - 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(192, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
                else if (lines[y + 1][x] == 'g' && lines[y][x - 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(320, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            }
            if (y < lines.Count - 1 && x < lines[y].Length - 1)
            {
                if (lines[y + 1][x] != 'g' && lines[y][x + 1] != 'g' && lines[y + 1][x + 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(224, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
                else if (lines[y + 1][x] == 'g' && lines[y][x + 1] == 'g')
                    tiles.Add(new Tile(new Vector2(x, y), new Rectangle(352, 32, TILE_WIDTH, TILE_HEIGHT), tileSheets["grass"]));
            }
        }

        /// <summary>
        /// Adds water tiles and connects them to other nearby water tiles.
        /// </summary>
        /// <param name="lines">A string representation of the terrain.</param>
        /// <param name="x">The x position of the current tile.</param>
        /// <param name="y">The y position of the current tile.</param>
        private void AddWater(List<string> lines, int x, int y)
        {
            Rectangle tileSource = sourceRecs["water"];

            if (y > 0 && lines[y - 1][x] != 'w')
                tileSource.X = 32;
            if (y < lines.Count - 1 && lines[y + 1][x] != 'w')
                tileSource.X = 64;
            if (x > 0 && lines[y][x - 1] != 'w')
                tileSource.X = 96;
            if (x < lines[y].Length - 1 && lines[y][x + 1] != 'w')
                tileSource.X = 128;
            if (y > 0 && x > 0)
            {
                if (lines[y - 1][x] == 'w' && lines[y][x - 1] == 'w' && lines[y - 1][x - 1] != 'w')
                    tileSource.X = 160;
                else if (lines[y - 1][x] != 'w' && lines[y][x - 1] != 'w')
                    tileSource.X = 288;
            }
            if (y > 0 && x < lines[y].Length - 1)
            {
                if (lines[y - 1][x] == 'w' && lines[y][x + 1] == 'w' && lines[y - 1][x + 1] != 'w')
                    tileSource.X = 192;
                else if (lines[y - 1][x] != 'w' && lines[y][x + 1] != 'w')
                    tileSource.X = 320;
            }
            if (y < lines.Count - 1 && x > 0)
            {
                if (lines[y + 1][x] == 'w' && lines[y][x - 1] == 'w' && lines[y + 1][x - 1] != 'w')
                    tileSource.X = 224;
                else if (lines[y + 1][x] != 'w' && lines[y][x - 1] != 'w')
                    tileSource.X = 352;
            }
            if (y < lines.Count - 1 && x < lines[y].Length - 1)
            {
                if (lines[y + 1][x] == 'w' && lines[y][x + 1] == 'w' && lines[y + 1][x + 1] != 'w')
                    tileSource.X = 256;
                else if (lines[y + 1][x] != 'w' && lines[y][x + 1] != 'w')
                    tileSource.X = 384;
            }

            tiles.Add(new Tile(new Vector2(x, y), tileSource, tileSheets["water"], true));
        }

        private void AddNPCs(SpriteFont font)
        {
            npcs.Add(new NPC(new Rectangle(300, 300, 52, 72), Content.Load<Texture2D>("blacksmith"), new Rectangle(10, 10, 26, 36), new Vector2(0, 0), true, 'b', Speech.None, random));
            npcs.Add(new NPC(new Rectangle(200, 300, 52, 72), Content.Load<Texture2D>("shopkeeper"), new Rectangle(0, 0, 26, 36), new Vector2(0, 0), true, 's', Speech.None, random));
            npcs.Add(new NPC(new Rectangle(400, 300, 52, 72), Content.Load<Texture2D>("priestess"), new Rectangle(0, 0, 26, 36), new Vector2(0, 0), true, 'p', Speech.None, random));
            npcs.Add(new NPC(new Rectangle(300, 400, 52, 72), Content.Load<Texture2D>("armour"), new Rectangle(26 * 3, 0, 26, 36), new Vector2(0, 0), true, 'a', Speech.None, random));
            foreach (NPC n in npcs)
                n.Window = window;
        }

        private void AddEnemies()
        {
            enemies.Add(new Enemy(new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, 1, 1), pix, new Vector2(0, 0)));
        }

        /// <summary>
        /// Updates the area.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            Vector2 speed = pad1.ThumbSticks.Left * 4;

            if (speed.Y > 0)
            {
                if (areaRec.Top < window.Top && player.Position.Y + player.SourceRec.Height <= window.Height / 3)
                    areaRec.Y += (int)speed.Y;
                else
                    player.Position = player.Position - new Vector2(0, speed.Y);
            }
            else if (speed.Y < 0)
            {
                if (areaRec.Bottom > window.Bottom && player.Position.Y >= 2 * window.Height / 3)
                    areaRec.Y -= 4;
                else
                    player.Position = player.Position - new Vector2(0, speed.Y);
            }
            if (speed.X < 0)
            {
                if (areaRec.Left < window.Left && player.Position.X + player.SourceRec.Width <= window.Width / 3)
                    areaRec.X += 4;
                else
                    player.Position = player.Position + new Vector2(speed.X, 0);
            }
            if (speed.X > 0)
            {
                if (areaRec.Right > window.Right && player.Position.X >= 2 * window.Width / 3)
                    areaRec.X -= 4;
                else
                    player.Position = player.Position + new Vector2(speed.X, 0);
            }

            player.Update(gameTime, speed);

            foreach (Enemy e in enemies)
            {
                e.Update(gameTime);
                    
            }

            foreach (NPC n in npcs)
                n.Update(gameTime);

            foreach (Tile t in tiles)
                if (t.IsAnimated)
                    t.Update(gameTime);
        }

        public void Battle()
        {

            player.Battle();
            foreach (Enemy enemy in enemies)
            {
                enemy.Battle();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
            {
                Rectangle tileRec = new Rectangle((int)(t.Position.X * TILE_WIDTH) + areaRec.X, (int)(t.Position.Y * TILE_HEIGHT) + areaRec.Y, TILE_WIDTH, TILE_HEIGHT);
                if (tileRec.Intersects(window))
                    spriteBatch.Draw(t.Texture, tileRec, t.SourceRec, Color.White);
            }
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach (NPC n in npcs)
                n.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}
