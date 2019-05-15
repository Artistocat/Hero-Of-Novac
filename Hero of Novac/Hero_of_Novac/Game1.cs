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

namespace Hero_of_Novac
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle window;

        Save save;
        Load load;
        Area area;

        SpriteFont font;
        SpriteFont fontC;

        Texture2D pix;
        Texture2D FX1;

        enum GameState
        {
            MainMenu, Overworld, Inventory, BattleMenu
        }
        GameState currentGameState;

        BattleMenu battleMenu;
        MainMenu mainMenu;

        const bool TESTING = true;

        Random randomSeed = new Random(1102);
        Random randomNoSeed = new Random();

        GamePadState oldgp;

        /*static Dictionary<string, Texture2D> textures;

        public static Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
        }*/

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;//1920
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;//1080
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            window = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            currentGameState = GameState.MainMenu;
            pix = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pix.SetData(pixelColors);
            save = new Save();
            oldgp = GamePad.GetState(PlayerIndex.One);
            base.Initialize();
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            fontC = Content.Load<SpriteFont>("CharacterTalk");

            FX1 = Content.Load<Texture2D>("combatFX");

            MainMenu.LoadContent(GraphicsDevice, window, font);
            mainMenu = new MainMenu();
            if (TESTING)
            {
                area = new Area(Services, @"Content/Test", pix, window, randomSeed);
                Enemy.LoadContent(area.Player);
                Attack.LoadContent(area.Player);
                List<NPC> npcs = new List<NPC>();
                npcs.Add(CreateNPC("blacksmith", new Rectangle(100, 100, 200, 136), true, 'b'));
                npcs.Add(CreateNPC("shopkeeper", new Rectangle(0, 400, 200, 200), true, 's'));
                npcs.Add(CreateNPC("priestess", new Rectangle(464, 0, 200, 200), true, 'p'));
                npcs.Add(CreateNPC("armorer", new Rectangle(564, 500, 200, 168), true, 'a'));
                area.AddNPCs(npcs);
                List<Enemy> enemies = new List<Enemy>();
                enemies.Add(CreateEnemy("gryphon", new Rectangle(0, 0, 320, 320), false, false));
                enemies.Add(CreateEnemy("wasp", new Rectangle(320, 0, 320, 320), true, true));
                area.AddEnemies(enemies);
            }
            else
            {
                area = new Area(Services, @"Content/Village", pix, window, randomSeed);
                Enemy.LoadContent(area.Player);
                Attack.LoadContent(area.Player);
            }

            NPC.Load(fontC, area.Player, Content.Load<Texture2D>("speechballoons"), Content.Load<Texture2D>("window"), Content.Load<Texture2D>("HeroProfile"));
            NPC.Window = window;
            area.Player.LearnAttack(Attack.Lunge);
            area.Player.LearnAttack(Attack.Slash);
            area.Player.LearnAttack(Attack.Chop);
            area.Player.LearnAttack(Attack.Punch);
            area.Player.LearnAttack(Attack.Whirlwind);
            area.Player.LearnAttack(Attack.AirSlash);
            area.Player.LearnAttack(Attack.WindStrike);
            area.Player.LearnAttack(Attack.FaldorsWind);
            area.Player.LearnAttack(Attack.WallOfFire);
            area.Player.LearnAttack(Attack.FireBall);
            area.Player.LearnAttack(Attack.IncendiaryCloud);
            area.Player.LearnAttack(Attack.OttosFireStorm);
            area.Player.LearnAttack(Attack.ThornWhip);
            area.Player.LearnAttack(Attack.StoneThrow);
            area.Player.LearnAttack(Attack.Earthquake);
            area.Player.LearnAttack(Attack.OtilukesWrath);
            area.Player.LearnAttack(Attack.ConeOfCold);
            area.Player.LearnAttack(Attack.IceStorm);
            area.Player.LearnAttack(Attack.FrostRay);
            area.Player.LearnAttack(Attack.RarysTsunami);
            area.Player.LearnAttack(Attack.MagicMissile);
            area.Player.LearnAttack(Attack.EldritchBlast);
            area.Player.LearnAttack(Attack.ArcaneBeam);
            area.Player.LearnAttack(Attack.TashasLaugh);
            Enemy.Window = window;

            SpriteFont smallBattleFont = Content.Load<SpriteFont>("SmallBattleFont");
            SpriteFont battleFont = Content.Load<SpriteFont>("BattleFont");
            BattleMenu.LoadContent(area.Player, battleFont, smallBattleFont, pix, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            PercentageRectangle.LoadContent(pix, font);

            battleMenu = new BattleMenu(new Enemy[0], BattleMenu.Biome.Plains);
        }

        private Enemy CreateEnemy(string name, Rectangle space, bool constantMove, bool idleAnimation)
        {
            Texture2D enemyTex = Content.Load<Texture2D>(name);
            Texture2D enemyProfileTex = Content.Load<Texture2D>(name + "Profile");
            enemyTex.Name = name;
            enemyProfileTex.Name = name + "Profile";
            return new Enemy(new Rectangle(0, 0, 146, 116), new Rectangle(146, 0, 146, 116), space, enemyTex, new Rectangle(0, 0, 414, 560), enemyProfileTex, new Vector2(0, 0), window, randomNoSeed, constantMove, idleAnimation, new Vector2(0, 0));
        }

        private Enemy LoadEnemy(List<string> enemyInfo)
        {
            Rectangle rec = ParseStringToRectangle(enemyInfo[0]);
            Rectangle sourceRec = ParseStringToRectangle(enemyInfo[1]);
            Texture2D tex = Content.Load<Texture2D>(enemyInfo[2]);
            tex.Name = enemyInfo[2];
            Rectangle sourceRecProfile = ParseStringToRectangle(enemyInfo[3]);
            Texture2D profileTex = Content.Load<Texture2D>(enemyInfo[4]);
            profileTex.Name = enemyInfo[4];
            Vector2 pos = ParseStringToVector(enemyInfo[5]);
            Rectangle space = ParseStringToRectangle(enemyInfo[6]);
            Rectangle battleRec = ParseStringToRectangle(enemyInfo[7]);
            Rectangle battleSourceRec = ParseStringToRectangle(enemyInfo[8]);
            PercentageRectangle healthBar = ParseStringToPercentageRectangle(enemyInfo[9]);
            Rectangle healthRect = ParseStringToRectangle(enemyInfo[10]);
            PercentageRectangle chargeBar = ParseStringToPercentageRectangle(enemyInfo[11]);
            bool constantMove = ParseStringToBool(enemyInfo[12]);
            bool isIdle = ParseStringToBool(enemyInfo[13]);
            Vector2 vol = ParseStringToVector(enemyInfo[14]);
            return new Enemy(rec, sourceRec, space, tex, sourceRecProfile, profileTex, pos, window, randomNoSeed, constantMove, isIdle, vol, battleRec, battleSourceRec, healthBar, healthRect, chargeBar);
        }

        private Rectangle ParseStringToRectangle(string str)
        {
            Rectangle parsedRect;
            int xIndex = str.IndexOf('X');
            int yIndex = str.IndexOf('Y');
            int widthIndex = str.IndexOf('W');
            int heightIndex = str.IndexOf('H');
            int x, y, width, height;
            Int32.TryParse(str.Substring(xIndex + 2, yIndex - xIndex - 3), out x); //x
            Int32.TryParse(str.Substring(yIndex + 2, widthIndex - yIndex - 3), out y); //y
            Int32.TryParse(str.Substring(widthIndex + 6, heightIndex - widthIndex - 7), out width); //width;
            Int32.TryParse(str.Substring(heightIndex + 7, str.Length - heightIndex - 8), out height); //height;
            parsedRect = new Rectangle(x, y, width, height);
            return parsedRect;
        }

        private Vector2 ParseStringToVector(string str)
        {
            Vector2 parsedVector;
            int xIndex = str.IndexOf('X');
            int yIndex = str.IndexOf('Y');
            int x, y;
            Int32.TryParse(str.Substring(xIndex + 2, yIndex - xIndex - 3), out x);
            Int32.TryParse(str.Substring(yIndex + 2, str.Length - yIndex - 3), out y);
            parsedVector = new Vector2(x, y);
            return parsedVector;
        }

        private PercentageRectangle ParseStringToPercentageRectangle(string str)
        {
            PercentageRectangle parsedRect;
            int x, y, width, height, r, g, b, maxValue, currentValue;
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out x);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out y);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out width);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out height);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out r);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out g);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out b);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out maxValue);
            str = str.Substring(str.IndexOf(' ') + 1);
            Int32.TryParse(str.Substring(0, str.IndexOf(' ')), out currentValue);
            str = str.Substring(str.IndexOf(' ') + 1);
            parsedRect = new PercentageRectangle(new Rectangle(x, y, width, height), maxValue, new Color(r, g, b));
            parsedRect.CurrentValue = currentValue;
            return parsedRect;
        }

        private bool ParseStringToBool(string str)
        {
            if (str.Equals("true"))
                return true;
            return false;
        }

        private NPC CreateNPC(string name, Rectangle space, bool interact, char type)
        {
            Texture2D npcTex = Content.Load<Texture2D>(name);
            Texture2D npcProfileTex = Content.Load<Texture2D>(name + "Profile");
            npcTex.Name = name;
            npcProfileTex.Name = name + "Profile";
            return new NPC(new Rectangle(0, 0, 52, 72), npcTex, new Rectangle(52, 0, 52, 72), space, new Vector2(0, 0), interact, type, Speech.None, randomNoSeed, npcProfileTex);
        }

        private NPC LoadNPC(List<string> npcInfo)
        {
            char name = npcInfo[0].ToCharArray()[0];
            Rectangle rec = ParseStringToRectangle(npcInfo[1]);
            Texture2D npcTex = Content.Load<Texture2D>(npcInfo[2]);
            npcTex.Name = npcInfo[2];
            Rectangle space = ParseStringToRectangle(npcInfo[3]);
            Texture2D npcProfileTex = Content.Load<Texture2D>(npcInfo[4]);
            npcProfileTex.Name = npcInfo[4];
            bool isInteractable = ParseStringToBool(npcInfo[5]);
            return new NPC(new Rectangle(0, 0, 52, 72), npcTex, new Rectangle(52, 0, 52, 72), space, new Vector2(0, 0), isInteractable, name, Speech.None, randomNoSeed, npcProfileTex);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || mainMenu.quitGame)
                this.Exit();
            bool willBattle = false;
            List<Enemy> enemiesInBattle = new List<Enemy>();
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) && oldgp.IsButtonUp(Buttons.Start))
            {
                save.SaveAll(area);
            }
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    mainMenu.Update();
                    if (mainMenu.loadOldGame)
                    {
                        //load.LoadAll();
                        
                    }
                    if (mainMenu.startNewGame)
                    {
                        currentGameState = GameState.Overworld;
                    }
                    break;
                case GameState.Overworld:
                    area.Update(gameTime);
                    foreach (Enemy enemy in area.Enemies)
                        if (enemy.IsInBattle())
                        {
                            willBattle = true;
                            enemiesInBattle.Add(enemy);
                        }
                    break;
                case GameState.BattleMenu:
                    battleMenu.Update(gameTime);
                    area.Player.Update(gameTime, new Vector2(0, 0));
                    if (battleMenu.BattleIsOver)
                    {
                        currentGameState = GameState.Overworld;
                        area.RemoveEnemies(battleMenu.Enemies);
                        area.Player.Overworld();
                    }
                    break;
                case GameState.Inventory:
                    break;
            }

            if (willBattle)
            {
                currentGameState = GameState.BattleMenu;
                Console.WriteLine("going to battle");
                battleMenu = new BattleMenu(enemiesInBattle.ToArray(), BattleMenu.Biome.Plains);
            }
            oldgp = GamePad.GetState(PlayerIndex.One);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(53, 65, 73));

            spriteBatch.Begin();
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameState.Overworld:
                    area.DrawFirstLayer(gameTime, spriteBatch);
                    break;
                case GameState.BattleMenu:
                    battleMenu.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            if (currentGameState == GameState.Overworld)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                area.DrawEntities(gameTime, spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin();
                area.DrawSecondLayer(gameTime, spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
