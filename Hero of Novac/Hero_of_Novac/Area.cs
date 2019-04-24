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

        private bool inMenu;

        private List<Tile> tiles;
        private int objectTilesStart;

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
        Dictionary<Texture2D, List<string>> objectData;

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
            sourceRecs.Add("snow", new Rectangle(0, 224, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("water", new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("lilypad", new Rectangle(320, 0, TILE_WIDTH, TILE_HEIGHT));

            sourceRecs.Add("tree1", new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree2", new Rectangle(128, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree3", new Rectangle(192, 128, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree4", new Rectangle(0, 128, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree5", new Rectangle(224, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree6", new Rectangle(288, 128, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree7", new Rectangle(192, 224, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("tree8", new Rectangle(288, 224, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("stump1", new Rectangle(320, 64, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("stump2", new Rectangle(384, 128, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("stump3", new Rectangle(384, 192, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("stump4", new Rectangle(384, 256, TILE_WIDTH, TILE_HEIGHT));

            sourceRecs.Add("house1", new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("house2", new Rectangle(288, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("house3", new Rectangle(640, 0, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("house4", new Rectangle(0, 192, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("house5", new Rectangle(288, 320, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("house6", new Rectangle(704, 320, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("barrel1", new Rectangle(0, 480, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("barrel2", new Rectangle(32, 480, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("barrel3", new Rectangle(64, 480, TILE_WIDTH, TILE_HEIGHT));
            sourceRecs.Add("barrel4", new Rectangle(96, 480, TILE_WIDTH, TILE_HEIGHT));

            tileSheets = new Dictionary<string, Texture2D>();
            tileSheets.Add("grass", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("flower", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("dirt", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("rock", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("road", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("sand", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("snow", Content.Load<Texture2D>("terrain"));
            tileSheets.Add("water", Content.Load<Texture2D>("water"));
            tileSheets.Add("lilypad", Content.Load<Texture2D>("trees"));

            tileSheets.Add("tree1", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree2", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree3", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree4", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree5", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree6", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree7", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree8", Content.Load<Texture2D>("trees"));
            tileSheets.Add("tree9", Content.Load<Texture2D>("trees"));
            tileSheets.Add("stump1", Content.Load<Texture2D>("trees"));
            tileSheets.Add("stump2", Content.Load<Texture2D>("trees"));
            tileSheets.Add("stump3", Content.Load<Texture2D>("trees"));
            tileSheets.Add("stump4", Content.Load<Texture2D>("trees"));

            tileSheets.Add("house1", Content.Load<Texture2D>("houses"));
            tileSheets.Add("house2", Content.Load<Texture2D>("houses"));
            tileSheets.Add("house3", Content.Load<Texture2D>("houses"));
            tileSheets.Add("house4", Content.Load<Texture2D>("houses"));
            tileSheets.Add("house5", Content.Load<Texture2D>("houses"));
            tileSheets.Add("house6", Content.Load<Texture2D>("houses"));
            tileSheets.Add("barrel1", Content.Load<Texture2D>("houses"));
            tileSheets.Add("barrel2", Content.Load<Texture2D>("houses"));
            tileSheets.Add("barrel3", Content.Load<Texture2D>("houses"));
            tileSheets.Add("barrel4", Content.Load<Texture2D>("houses"));

            tiles = new List<Tile>();
            LoadTerrainTiles(ReadFile(path + "/terrain.txt"));
            objectTilesStart = tiles.Count;
            objectData = new Dictionary<Texture2D, List<string>>();
            objectData.Add(Content.Load<Texture2D>("trees"), ReadFile(@"Content/trees_data.txt"));
            objectData.Add(Content.Load<Texture2D>("houses"), ReadFile(@"Content/houses_data.txt"));
            LoadObjectTiles(ReadFile(path + "/objects.txt"));

            pix = p;
            player = new Player(Content.Load<Texture2D>("player_walking"), Content.Load<Texture2D>("player_combat"), pix, window);
            SpriteFont font = Content.Load<SpriteFont>("SpriteFont1");
            npcs = new List<NPC>();
            AddNPCs(font);
            enemies = new List<Enemy>();
            AddEnemies();
        }

        /// <summary>
        /// Reads a file at the indicated path.
        /// </summary>
        /// <param name="path">The location of the file to be read.</param>
        /// <returns></returns>
        private List<string> ReadFile(string path)
        {
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        lines.Add(line);
                        if (line.Length != lines[0].Length)
                            throw new Exception(string.Format("The length of line {0} is different from all proceeding lines.", lines.Count));
                        line = reader.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:\n" + e.Message);
            }
            return lines;
        }

        /// <summary>
        /// Loads all the terrain tiles for an area.
        /// </summary>
        /// <param name="path">Provides the path to the terrain data.</param>
        private void LoadTerrainTiles(List<string> lines)
        {
            areaWidth = lines[0].Length;
            areaHeight = lines.Count;
            areaRec = new Rectangle(0, 0, Width * TILE_WIDTH, Height * TILE_HEIGHT);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    switch (lines[y][x])
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
                        case 'c':
                            LoadTile("snow", x, y, 8);
                            AddEdge(lines, x, y);
                            break;
                        case 'w':
                            LoadWaterTiles(lines, x, y);
                            AddEdge(lines, x, y);
                            break;
                        case '.':
                            AddEdge(lines, x, y);
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", lines[y][x], x, y));
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
        private void LoadWaterTiles(List<string> lines, int x, int y)
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

        /// <summary>
        /// Loads all the object tiles (buildings, trees, etc) for an area.
        /// </summary>
        /// <param name="path">Provides the path to the object data.</param>
        private void LoadObjectTiles(List<string> lines)
        {
            int objectNum = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    switch (lines[y][x])
                    {
                        case 't':
                            x++;
                            switch (lines[y][x])
                            {
                                case '1':
                                    LoadObject("tree1", x - 1, y, 4, 4);
                                    break;
                                case '2':
                                    LoadObject("tree2", x - 1, y, 3, 4);
                                    break;
                                case '3':
                                    LoadObject("tree3", x - 1, y, 3, 3);
                                    break;
                                case '4':
                                    LoadObject("tree4", x - 1, y, 6, 6);
                                    break;
                                case '5':
                                    LoadObject("tree5", x - 1, y, 3, 4);
                                    break;
                                case '6':
                                    LoadObject("tree6", x - 1, y, 3, 3);
                                    break;
                                case '7':
                                    LoadObject("tree7", x - 1, y, 3, 3);
                                    break;
                                case '8':
                                    LoadObject("tree8", x - 1, y, 3, 3);
                                    break;
                                default:
                                    throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", lines[y][x], x, y));
                            }
                            objectNum++;
                            break;
                        case 's':
                            int r = random.Next(4);
                            switch (r)
                            {
                                case 0:
                                    LoadObject("stump1", x, y, 3, 2);
                                    break;
                                case 1:
                                    LoadObject("stump2", x, y, 2, 2);
                                    break;
                                case 2:
                                    LoadObject("stump3", x, y, 2, 2);
                                    break;
                                case 3:
                                    LoadObject("stump4", x, y, 2, 2);
                                    break;
                                default:
                                    throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", lines[y][x], x, y));
                            }
                            objectNum++;
                            break;
                        case 'l':
                            LoadTile("lilypad", x, y, 4);
                            break;
                        case 'h':
                            x++;
                            switch (lines[y][x])
                            {
                                case '1':
                                    LoadObject("house1", x - 1, y, 9, 6);
                                    break;
                                case '2':
                                    LoadObject("house2", x - 1, y, 11, 10);
                                    break;
                                case '3':
                                    LoadObject("house3", x - 1, y, 12, 10);
                                    break;
                                case '4':
                                    LoadObject("house4", x - 1, y, 9, 9);
                                    break;
                                case '5':
                                    LoadObject("house5", x - 1, y, 13, 7);
                                    break;
                                case '6':
                                    LoadObject("house6", x - 1, y, 7, 7);
                                    break;
                                default:
                                    throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", lines[y][x], x, y));
                            }
                            objectNum++;
                            break;
                        case 'b':
                            r = random.Next(4);
                            switch (r)
                            {
                                case 0:
                                    LoadObject("barrel1", x, y, 1, 2);
                                    break;
                                case 1:
                                    LoadObject("barrel2", x, y, 1, 2);
                                    break;
                                case 2:
                                    LoadObject("barrel3", x, y, 1, 2);
                                    break;
                                case 3:
                                    LoadObject("barrel4", x, y, 1, 2);
                                    break;
                                default:
                                    throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", lines[y][x], x, y));
                            }
                            objectNum++;
                            break;
                        case '.':
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", lines[y][x], x, y));
                    }
                }
        }

        private void LoadObject(string type, int x, int y, int width, int height)
        {
            for (int r = 0; r < height; r++)
                for (int c = 0; c < width; c++)
                {
                    Rectangle sourceRec = sourceRecs[type];
                    sourceRec.X += c * TILE_WIDTH;
                    sourceRec.Y += (height - (r + 1)) * TILE_HEIGHT;
                    int i = sourceRec.Y / TILE_HEIGHT;
                    int j = sourceRec.X / TILE_WIDTH;
                    switch (objectData[tileSheets[type]][i][j])
                    {
                        case '-':
                            tiles.Add(new Tile(new Vector2(x + c, y - r), sourceRec, tileSheets[type], true, false));
                            break;
                        case 'x':
                            tiles.Add(new Tile(new Vector2(x + c, y - r), sourceRec, tileSheets[type], false, false));
                            break;
                        case 'o':
                            tiles.Add(new Tile(new Vector2(x + c, y - r), sourceRec, tileSheets[type], true, true));
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Unsupported data character '{0}' at position {1}, {2}.", objectData[tileSheets[type]][i][j], j, i));
                    }
                }
        }

        private void AddNPCs(SpriteFont font)
        {
            npcs.Add(new NPC(new Rectangle(300, 300, 52, 72), Content.Load<Texture2D>("blacksmith"), new Rectangle(52, 0, 52, 72), new Rectangle(100, 100, 200, 200), new Vector2(0, 0), true, 'b', Speech.None, random));
            npcs.Add(new NPC(new Rectangle(200, 300, 52, 72), Content.Load<Texture2D>("shopkeeper"), new Rectangle(52, 0, 52, 72), new Rectangle(0, 400, 200, 200), new Vector2(0, 0), true, 's', Speech.None, random));
            npcs.Add(new NPC(new Rectangle(400, 300, 52, 72), Content.Load<Texture2D>("priestess"), new Rectangle(52, 0, 52, 72), new Rectangle(400, 0, 200, 200), new Vector2(0, 0), true, 'p', Speech.None, random));
            npcs.Add(new NPC(new Rectangle(300, 400, 52, 72), Content.Load<Texture2D>("armour"), new Rectangle(52, 0, 52, 72), new Rectangle(500, 500, 200, 200), new Vector2(0, 0), true, 'a', Speech.None, random));
            foreach (NPC n in npcs)
                n.Window = window;
        }

        private void AddEnemies()
        {
            enemies.Add(new Enemy(new Rectangle(50, 50, 100, 100), new Rectangle(0, 0, 1, 1), pix, new Vector2(0, 0), window));
        }

        /// <summary>
        /// Updates the area.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            inMenu = false;
            foreach (NPC n in npcs)
            {
                if (n.isTalking)
                    inMenu = true;
            }
            if (!inMenu)
            {
                Vector2 speed = pad1.ThumbSticks.Left * 4;
                speed.X = (int)Math.Round(speed.X);
                speed.Y = (int)Math.Round(speed.Y);
                if (speed.Y > 0)
                {
                    if (areaRec.Top < window.Top && player.Hitbox.Bottom <= window.Height / 3)
                    {
                        areaRec.Y += (int)speed.Y;
                        foreach (NPC n in npcs)
                            n.MoveY((int)speed.Y);
                        foreach (Enemy e in enemies)
                            e.MoveY((int)speed.Y);
                    }
                    else
                        player.MoveY((int)speed.Y);
                }
                else if (speed.Y < 0)
                {
                    if (areaRec.Bottom > window.Bottom && player.Hitbox.Top >= 2 * window.Height / 3)
                    {
                        areaRec.Y += (int)speed.Y;
                        foreach (NPC n in npcs)
                            n.MoveY((int)speed.Y);
                        foreach (Enemy e in enemies)
                            e.MoveY((int)speed.Y);
                    }
                    else
                        player.MoveY((int)speed.Y);
                }
                if (speed.X < 0)
                {
                    if (areaRec.Left < window.Left && player.Hitbox.Right <= window.Width / 3)
                    {
                        areaRec.X -= (int)speed.X;
                        foreach (NPC n in npcs)
                            n.MoveX((int)speed.X);
                        foreach (Enemy e in enemies)
                            e.MoveX((int)speed.X);
                    }
                    else
                        player.MoveX((int)speed.X);
                }
                if (speed.X > 0)
                {
                    if (areaRec.Right > window.Right && player.Hitbox.Left >= 2 * window.Width / 3)
                    {
                        areaRec.X -= (int)speed.X;
                        foreach (NPC n in npcs)
                            n.MoveX((int)speed.X);
                        foreach (Enemy e in enemies)
                            e.MoveX((int)speed.X);
                    }
                    else
                        player.MoveX((int)speed.X);
                }

                foreach (Tile t in tiles)
                    if (!t.IsPassable)
                    {
                        Rectangle tileRec = t.Rectangle;
                        tileRec.X += areaRec.X;
                        tileRec.Y += areaRec.Y;
                        if (tileRec.Intersects(player.Hitbox))
                        {
                            Vector2 depth = player.Hitbox.GetIntersectionDepth(tileRec);
                            if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                                player.MoveY(-(int)depth.Y);
                            else
                                player.MoveX((int)depth.X);
                        }
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
            else
            {
                foreach(NPC n in npcs)
                {
                    if (n.isTalking)
                    {
                        n.Update(gameTime);
                    }
                }
            }
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
                Rectangle tileRec = t.Rectangle;
                tileRec.X += areaRec.X;
                tileRec.Y += areaRec.Y;
                if (tileRec.Intersects(window) && !t.IsOnTop)
                    spriteBatch.Draw(t.Texture, tileRec, t.SourceRec, Color.White);
            }
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (NPC n in npcs)
                n.Draw(spriteBatch);
            player.Draw(spriteBatch);
            for (int i = objectTilesStart; i < tiles.Count; i++)
            {
                Rectangle tileRec = tiles[i].Rectangle;
                tileRec.X += areaRec.X;
                tileRec.Y += areaRec.Y;
                if (tileRec.Intersects(window) && tiles[i].IsOnTop)
                    spriteBatch.Draw(tiles[i].Texture, tileRec, tiles[i].SourceRec, Color.White);
            }
        }
    }
}
