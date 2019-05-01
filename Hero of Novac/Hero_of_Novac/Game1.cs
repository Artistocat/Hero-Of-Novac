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

        Area area;

        SpriteFont font;
        SpriteFont fontC;

        Texture2D pix;

        enum GameState
        {
            MainMenu, Overworld, Inventory, BattleMenu
        }
        GameState currentGameState;

        BattleMenu battleMenu;
        MainMenu mainMenu;

        Random randomSeed = new Random(1102);
        Random randomNoSeed = new Random();

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
            currentGameState = GameState.Overworld;
            pix = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pix.SetData(pixelColors);
            currentGameState = GameState.Overworld;
            //TESTING
            if (currentGameState == GameState.BattleMenu)
                area.Battle();
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

            area = new Area(Services, @"Content/Test", pix, window, randomSeed);
            List<NPC> npcs = new List<NPC>();
            npcs.Add(new NPC(new Rectangle(300, 300, 52, 72), Content.Load<Texture2D>("blacksmith"), new Rectangle(52, 0, 52, 72), new Rectangle(100, 100, 200, 200), new Vector2(0, 0), true, 'b', Speech.None, randomNoSeed, Content.Load<Texture2D>("speechballoons")));
            npcs.Add(new NPC(new Rectangle(200, 300, 52, 72), Content.Load<Texture2D>("shopkeeper"), new Rectangle(52, 0, 52, 72), new Rectangle(0, 400, 200, 200), new Vector2(0, 0), true, 's', Speech.None, randomNoSeed, Content.Load<Texture2D>("speechballoons")));
            npcs.Add(new NPC(new Rectangle(400, 300, 52, 72), Content.Load<Texture2D>("priestess"), new Rectangle(52, 0, 52, 72), new Rectangle(400, 0, 200, 200), new Vector2(0, 0), true, 'p', Speech.None, randomNoSeed, Content.Load<Texture2D>("speechballoons")));
            npcs.Add(new NPC(new Rectangle(300, 400, 52, 72), Content.Load<Texture2D>("armour"), new Rectangle(52, 0, 52, 72), new Rectangle(500, 500, 200, 200), new Vector2(0, 0), true, 'a', Speech.None, randomNoSeed, Content.Load<Texture2D>("speechballoons")));
            area.AddNPCs(npcs);
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(new Enemy(new Rectangle(0, 0, 100, 100), new Rectangle(146, 0, 146, 116), new Rectangle(0, 0, 200, 200), Content.Load<Texture2D>("gryphon"), new Vector2(0, 0), window, randomNoSeed, new Vector2(0, 0)));
            area.AddEnemies(enemies);

            NPC.Load(fontC, area.Player, Content.Load<Texture2D>("speechballoons"), Content.Load<Texture2D>("window"), Content.Load<Texture2D>("player_walking"));
            NPC.Window = window;

            Enemy.LoadContent(area.Player);
            Attack.LoadContent(area.Player);
            area.Player.LearnAttack(Attack.Lunge);
            area.Player.LearnAttack(Attack.Slash);
            area.Player.LearnAttack(Attack.Chop);
            area.Player.LearnAttack(Attack.Punch);
            Enemy.Window = window;

            BattleMenu.LoadContent(area.Player, font, pix, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            PercentageRectangle.LoadContent(pix, font);

            battleMenu = new BattleMenu(new Enemy[0]);

            //TESTING
            currentGameState = GameState.Overworld;
            if (currentGameState == GameState.BattleMenu)
            {
                area.Battle();
            }
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            bool willBattle = false;
            List<Enemy> enemiesInBattle = new List<Enemy>();

            switch (currentGameState)
            {
                case GameState.MainMenu:
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
                    break;
                case GameState.Inventory:
                    break;
            }

            if (willBattle)
            {
                currentGameState = GameState.BattleMenu;
                Console.WriteLine("going to battle");
                battleMenu = new BattleMenu(enemiesInBattle.ToArray());
            }
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
