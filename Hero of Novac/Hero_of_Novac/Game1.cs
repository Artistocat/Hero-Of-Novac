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

        static Dictionary<string, Texture2D> textures;

        public static Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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
                Texture2D[,] NPCTex = new Texture2D[4, 2];
                NPCTex[0, 0] = Content.Load<Texture2D>("blacksmith"); NPCTex[0, 0].Name = "blacksmith";
                NPCTex[0, 1] = pix; NPCTex[0, 1].Name = "pix";
                NPCTex[1, 0] = Content.Load<Texture2D>("shopkeeper"); NPCTex[1, 0].Name = "shopkeeper";
                NPCTex[1, 1] = pix; NPCTex[1, 1].Name = "pix";
                NPCTex[2, 0] = Content.Load<Texture2D>("priestess"); NPCTex[2, 0].Name = "priestess";
                NPCTex[2, 1] = Content.Load<Texture2D>("PriestessProfile"); NPCTex[2, 1].Name = "PriestessProfile";
                NPCTex[3, 0] = Content.Load<Texture2D>("armour"); NPCTex[3, 0].Name = "armour";
                NPCTex[3, 1] = Content.Load<Texture2D>("ArmourerProfile"); NPCTex[3, 1].Name = "ArmourerProfile";
                npcs.Add(new NPC(new Rectangle(300, 300, 52, 72), NPCTex[0, 0], new Rectangle(52, 0, 52, 72), new Rectangle(100, 100, 200, 136), new Vector2(0, 0), true, 'b', Speech.None, randomNoSeed, NPCTex[0, 1]));
                npcs.Add(new NPC(new Rectangle(200, 300, 52, 72), NPCTex[1, 0], new Rectangle(52, 0, 52, 72), new Rectangle(0, 400, 200, 200), new Vector2(0, 0), true, 's', Speech.None, randomNoSeed, NPCTex[1, 1]));
                npcs.Add(new NPC(new Rectangle(400, 300, 52, 72), NPCTex[2, 0], new Rectangle(52, 0, 52, 72), new Rectangle(464, 0, 200, 200), new Vector2(0, 0), true, 'p', Speech.None, randomNoSeed, NPCTex[2, 1]));
                npcs.Add(new NPC(new Rectangle(300, 400, 52, 72), NPCTex[3, 0], new Rectangle(52, 0, 52, 72), new Rectangle(564, 500, 200, 168), new Vector2(0, 0), true, 'a', Speech.None, randomNoSeed, NPCTex[3, 1]));
                area.AddNPCs(npcs);
                List<Enemy> enemies = new List<Enemy>();
                Texture2D enemyTex = Content.Load<Texture2D>("gryphon");
                Texture2D enemyProfileTex = Content.Load<Texture2D>("GryphonProfile");
                enemyTex.Name = "gryphon";
                enemyProfileTex.Name = "GryphonProfile";
                enemies.Add(new Enemy(new Rectangle(0, 0, 100, 100), new Rectangle(146, 0, 146, 116), new Rectangle(0, 0, 200, 200), Content.Load<Texture2D>("gryphon"), new Rectangle(0, 0, 414, 560), Content.Load<Texture2D>("GryphonProfile"), new Vector2(0, 0), window, randomNoSeed, new Vector2(0, 0)));
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
            area.Player.LearnAttack(Attack.AirSlash);
            area.Player.LearnAttack(Attack.IncendiaryCloud);
            Enemy.Window = window;

            SpriteFont smallBattleFont = Content.Load<SpriteFont>("SmallBattleFont");
            SpriteFont battleFont = Content.Load<SpriteFont>("BattleFont");
            BattleMenu.LoadContent(area.Player, battleFont, smallBattleFont, pix, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            PercentageRectangle.LoadContent(pix, font);

            battleMenu = new BattleMenu(new Enemy[0], BattleMenu.Biome.Plains);
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
                        //TODO
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
