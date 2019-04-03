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
        Vector2 loc;
        SpriteFont font;
        List<string> lines;
        int inputX, inputY;

        Texture2D playerMoveSprites;
        Player Jhon;

        NPC Smith;
        NPC Shop;
        NPC Priest;
        NPC Armour;

        Player player;
        int counter;
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
            graphics.IsFullScreen = true;
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
            playerMoveSprites = this.Content.Load<Texture2D>("chara1");
            player = new Player(playerMoveSprites, window);
            battleMenu = new BattleMenu(new Enemy[0]);
            Smith = new NPC();
            base.Initialize();
            lines = new List<string>();

            //TESTING
            currentGameState = GameState.BattleMenu;


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
            Smith.load(font);

            BattleMenu.LoadContent(Jhon, font, GraphicsDevice, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
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
            player.counter++;
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Overworld:
                    village.Update(gameTime);
                    player.Update(gameTime);
                    base.Update(gameTime);
                    break;
                case GameState.BattleMenu:
                    battleMenu.Update();
                    break;
                case GameState.Inventory:
                    break;
            }
            player.Update(gameTime);
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
                    player.Draw(spriteBatch);
                    break;
                case GameState.BattleMenu:
                    battleMenu.Draw(spriteBatch);
                    break;
            }
            Smith.Draw(spriteBatch);
            village.Draw(gameTime, spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
