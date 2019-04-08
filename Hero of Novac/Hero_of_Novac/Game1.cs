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
        Rectangle window;

        Area village;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;
        List<string> lines;

        Texture2D playerMoveSprites;

        Texture2D pix;


        Texture2D playerWalkingSprites;
        Texture2D playerCombatSprites;

        NPC smith;
        NPC shop;
        NPC priest;
        NPC armor;

        List<Enemy> enemies;

        Player player;

        enum GameState
        {
            MainMenu, Overworld, Inventory, BattleMenu
        }
        GameState currentGameState;

        BattleMenu battleMenu;
        MainMenu mainMenu;

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
            playerWalkingSprites = Content.Load<Texture2D>("player_walking");
            playerCombatSprites = Content.Load<Texture2D>("player_combat");
            enemies = new List<Enemy>();
            pix = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;

            pix.SetData(pixelColors);
            player = new Player(playerWalkingSprites, playerCombatSprites, pix,  window);
            smith = new NPC(new Rectangle(100,100,100,100),pix,new Vector2(100,100), new Vector2(0,0),true,'b');
            base.Initialize();

            //TESTING
            Enemy newEnemy = new Enemy(new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, 1, 1), pix, new Vector2(0, 0));
            enemies.Add(newEnemy);
            currentGameState = GameState.Overworld;
            if (currentGameState == GameState.BattleMenu)
            {
                player.Battle();
                foreach (Enemy enemy in enemies)
                {
                    enemy.Battle();
                }
            }


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
            smith.load(font);

            BattleMenu.LoadContent(player, font, GraphicsDevice, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            Enemy.LoadContent(player);
            battleMenu = new BattleMenu(new Enemy[0]);
            // TODO: use this.Content to load your game content here
            village = new Area(Services, @"Content/Village", window);
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

            smith.Update(gameTime);
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Overworld:
                    village.Update(gameTime);
                    player.Update(gameTime);
                    foreach(Enemy enemy in enemies)
                    {
                        enemy.Update(gameTime);
                        if (enemy.IsInBattle())
                            willBattle = true;
                    }
                    base.Update(gameTime);
                    break;
                case GameState.BattleMenu:
                    battleMenu.Update();
                    break;
                case GameState.Inventory:
                    break;
            }

            if (willBattle)
                currentGameState = GameState.BattleMenu;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Overworld:
                    village.Draw(gameTime, spriteBatch);
                    smith.Draw(spriteBatch);
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(spriteBatch);
                    }
                    player.Draw(spriteBatch);
                    break;
                case GameState.BattleMenu:
                    battleMenu.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
