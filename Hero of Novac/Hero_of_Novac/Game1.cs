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

        Random ran = new Random();

        SpriteFont font;

        Texture2D pix;

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
            pix = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pix.SetData(pixelColors);
            base.Initialize();

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

            area = new Area(Services, @"Content/Village", pix, window);
            
            NPC.Load(font, area.Player);

            Enemy.LoadContent(area.Player);

            BattleMenu.LoadContent(area.Player, font, pix, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            Enemy.LoadContent(area.Player);
            PercentageRectangle.LoadContent(pix);
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

            switch (currentGameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Overworld:
                    area.Update(gameTime);
                    foreach(Enemy enemy in area.Enemies)
                        if (enemy.IsInBattle())
                            willBattle = true;
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
            GraphicsDevice.Clear(new Color(53, 65, 73));

            spriteBatch.Begin();
            
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Overworld:
                    area.Draw(gameTime, spriteBatch);
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
