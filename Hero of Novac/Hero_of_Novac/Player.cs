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

    public class Player// : Entity
    {
        //public static List<Enemy> enemies;
        ContentManager content;
        public ContentManager Content
        {
            get { return content; }
        }

        private Rectangle window;

        private const int OVERWORLD_SPRITE_WIDTH = 52;
        private const int OVERWORLD_SPRITE_HEIGHT = 72;
        private const int BATTLE_SPRITE_WIDTH = 96;
        private const int BATTLE_SPRITE_HEIGHT = 96;


        private enum GameState
        {
            Overworld, Battlemenu
        }

        private GameState currentGameState;

        private Texture2D overworldTex;
        private Texture2D combatTex;
        private Texture2D combatFX;
        private Texture2D profileTex;
        private Texture2D pixel;
        private Rectangle sourceRecWorld;
        private Rectangle sourceRecIdle;
        public Rectangle sourceRecBattle;
        public Rectangle sourceRecFX;
        public Rectangle sourceRecProfile;
        public Rectangle SourceRec
        {
            get { return sourceRecWorld; }
        }
        private bool attackTest = false;
        public bool isAttacking;
        public bool isCharging;
        private Vector2 playerPos;
        public Vector2 battlePos;
        public Vector2 battleFXPos;
        public Vector2 Position
        {
            get { return playerPos; }
            set { playerPos = value; }
        }
        private Rectangle hitbox;
        public Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        //private int healthPoints;
        //private int magicPoints;
        //private int chargePoints;

        public int Health
        {
            get
            {
                return healthBar.CurrentValue;
            }
            set
            {
                healthBar.CurrentValue = value;
            }
        }

        private PercentageRectangle healthBar;
        private PercentageRectangle magicBar;
        private PercentageRectangle chargeBar;
        private const int BattleBarX = 25;
        private const int BattleBarY = 1080 / 2 + 100;
        private const int barWidth = 66;
        private const int barHeight = 5;
        private Rectangle healthRect; //for battles
        private Rectangle magicRect;

        private int xp;
        private PercentageRectangle xpBar;
        private PercentageRectangle[] xpElementBars;
        public int Xp
        {
            get
            {
                return xp;
            }
            set
            {
                xp = value;
                while (xp > xpBar.MaxValue)
                {
                    xp -= xpBar.MaxValue;
                    //xpBar.MaxValue = (int)(Math.Round(xpBar.MaxValue * 1.5));
                    Level++;
                }
                xpBar.CurrentValue = xp;
            }
        }
        private int[] elementLevels;
        private int level;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if (level + 1 == value)
                {
                    xpBar.MaxValue = (int)(Math.Round(xpBar.MaxValue * 1.5));
                }
                else
                {
                    xpBar.MaxValue = (int)(Math.Round(100 * Math.Pow(1.5, value)));
                }
                level = value;
            }
        }
        public int MagicPoints
        {
            get
            {
                return magicBar.CurrentValue;
            }
            set
            {
                //while (magicBar.CurrentValue + value > magicBar.MaxValue ||
                //    magicBar.CurrentValue + value < 0)
                //{
                //    level++;
                //    value -= magicBar.MaxValue;
                //    magicBar.MaxValue = (int)(magicBar.MaxValue * 1.5);
                //}
                magicBar.CurrentValue = value;
            }
        }
        public double LevelModifier
        {
            get
            {
                double modifier = 1.0;
                for (int i = 0; i < level; i++)
                {
                    modifier *= 1.5;
                }
                return modifier;
            }
        }

        public Attack[] BasicAttacks
        {
            get
            {
                return basicAttacks;
            }
        }

        public Dictionary<Element, Attack[]> MagicAttacks
        {
            get
            {
                return magicAttacks;
            }
        }

        public int Elementlvl(Element element)
        {
            return elementLevels[(int)element];
        }

        private Attack[] basicAttacks;
        private Dictionary<Element, Attack[]> magicAttacks;
        private Attack currentAttack;

        public Attack CurrentAttack
        {
            get
            {
                return currentAttack;
            }
            set
            {
                if (value != null)
                {
                    isAttacking = false;
                    switch (value.AttackName)
                    {
                        case "Slash":
                            combatFX = Content.Load<Texture2D>("Slashes1");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 96;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 0;
                            break;
                        case "Lunge":
                            combatFX = Content.Load<Texture2D>("Slashes1");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 0;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128 * 2;
                            break;
                        case "Punch":
                            combatFX = Content.Load<Texture2D>("combatFX");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 96 * 5;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128 * 2;
                            break;
                        case "Chop":
                            combatFX = Content.Load<Texture2D>("Slashes1");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 0;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128;
                            break;
                        case "Whirlwind":
                            combatFX = Content.Load<Texture2D>("WindAttacc");
                            sourceRecBattle.X = 0;
                            sourceRecBattle.Y = 96 * 2;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128 * 2;
                            break;
                        case "Air Slash":
                            combatFX = Content.Load<Texture2D>("WindAttacc");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 96;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 0;
                            break;
                        case "Wind Strike":
                            combatFX = Content.Load<Texture2D>("WindAttacc");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 0;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128;
                            break;
                        case "Faldor's Wind": //Animation might change in the future & twitches
                            combatFX = Content.Load<Texture2D>("WindAttacc");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 96;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128;
                            break;
                        case "Wall of Fire":
                            combatFX = Content.Load<Texture2D>("FireAttacc");
                            sourceRecBattle.X = 96 * 3;
                            sourceRecBattle.Y = 96;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128 * 3;
                            break;
                        case "Fire Ball":
                            break;
                        case "Incendiary Cloud":
                            combatFX = Content.Load<Texture2D>("explosions");
                            sourceRecBattle.X = 0;
                            sourceRecBattle.Y = 96 * 2;
                            sourceRecFX.X = -128;
                            sourceRecFX.Y = 128;
                            break;
                        case "Otto's Firestorm":
                            break;
                        case "Thorn Whip":
                            break;
                        case "Stone Throw":
                            break;
                        case "Earthquake":
                            break;
                        case "Otiluke's Wrath":
                            break;
                        case "Cone of Cold":
                            break;
                        case "Ice Storm":
                            break;
                        case "Frost Ray":
                            break;
                        case "Rary's Tsunami":
                            break;
                        case "Magic Missile":
                            break;
                        case "Eldritch Blast":
                            break;
                        case "Arcane Beam":
                            break;
                        case "Tasha's Laugh":
                            break;
                    }
                    chargeBar.MaxValue = value.ChargeTime;
                    isCharging = true;
                }
                else
                {
                    isCharging = false;
                    isAttacking = false;
                    chargeBar.CurrentValue = 0;
                }
                currentAttack = value;
                chargeBar.CurrentValue = 0;
            }
        }

        //private PercentageRectangle battleHealthBar;
        //private PercentageRectangle battleMagicBar;
        //private PercentageRectangle battleChargeBar;

        private bool dead = false;

        private Color color;
        private int timer;

        public Player(Texture2D overworldTex, Texture2D combatTex, Texture2D combatFX, Texture2D profileTex, Texture2D p, Rectangle window, IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");

            currentGameState = GameState.Overworld;
            this.window = window;

            this.overworldTex = overworldTex;
            this.combatTex = combatTex;
            this.combatFX = combatFX;
            this.profileTex = profileTex;
            pixel = p;

            sourceRecWorld = new Rectangle(OVERWORLD_SPRITE_WIDTH, 0, OVERWORLD_SPRITE_WIDTH, OVERWORLD_SPRITE_HEIGHT);
            sourceRecIdle = new Rectangle(0, 96, BATTLE_SPRITE_WIDTH, BATTLE_SPRITE_HEIGHT);
            sourceRecBattle = new Rectangle(0, 96, BATTLE_SPRITE_WIDTH, BATTLE_SPRITE_HEIGHT);
            sourceRecFX = new Rectangle(-128, 128 * 4, 128, 128);
            sourceRecProfile = new Rectangle(0, 6, 292, 503);
            playerPos = new Vector2((window.Width - OVERWORLD_SPRITE_WIDTH) / 2, (window.Height - OVERWORLD_SPRITE_HEIGHT) / 2);

            healthBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 10, barWidth, barHeight), 100, Color.Red);
            magicBar = new PercentageRectangle(new Rectangle((int)playerPos.X - 10, (int)playerPos.Y - 15, barWidth, barHeight), 100, Color.Blue);

            healthRect = healthBar.Rect;
            healthRect.Width *= 5;
            healthRect.Height *= 5;
            healthRect.X = 25;
            healthRect.Y = window.Height / 2 + 50;
            magicRect = magicBar.Rect;
            magicRect.Width *= 5;
            magicRect.Height *= 5;
            magicRect.X = 25;
            magicRect.Y = window.Height / 2 + 100;
            //battleHealthBar = new PercentageRectangle(healthRect, healthBar.MaxValue, healthBar.Color);
            //battleMagicBar = new PercentageRectangle(magicRect, magicBar.MaxValue, magicBar.Color);
            chargeBar = new PercentageRectangle(new Rectangle(25, window.Height / 2 + 150, 66 * 5, 5 * 5), 100, Color.Gray);
            chargeBar.CurrentValue = 0;
            xpBar = new PercentageRectangle(new Rectangle(chargeBar.Rect.X, chargeBar.Rect.Y + 50, 66 * 5, 5 * 5), 100, Color.Green);
            xpBar.CurrentValue = xp = 0;

            battlePos = new Vector2(window.Width / 5, 200);
            battleFXPos.X = window.Right - window.Width / 3 + 8;
            battleFXPos.Y = battlePos.Y - 12;
            color = Color.White;
            pixel = p;
            timer = 0;

            hitbox = new Rectangle((int)playerPos.X + (sourceRecWorld.Width - 32) / 2, (int)playerPos.Y + sourceRecWorld.Height - 32, 32, 32);
            basicAttacks = new Attack[4];
            magicAttacks = new Dictionary<Element, Attack[]>();
            isAttacking = false;
            isCharging = false;

            elementLevels = new int[5];
            for (int i = 0; i < elementLevels.Length; i++)
            {
                elementLevels[i] = 1;
            }

            for (int i = 0; i < 5; i++)
            {
                magicAttacks.Add((Element)i, new Attack[4]);
            }
        }

        public void death()
        {
            dead = true;
            sourceRecBattle.X = BATTLE_SPRITE_WIDTH * 6;
            sourceRecBattle.Y = BATTLE_SPRITE_HEIGHT * 5;
        }

        public void Update(GameTime gameTime, Vector2 speed)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    UpdateOverworld(gameTime, speed);
                    break;
                case GameState.Battlemenu:
                    UpdateBattlemenu(gameTime);
                    break;
            }
            //rec = new Rectangle((int)playerPos.X, (int)playerPos.Y, sourceRecWorld.Width, sourceRecWorld.Height);
        }

        private void UpdateOverworld(GameTime gameTime, Vector2 speed)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            KeyboardState KB = Keyboard.GetState();
            //World Border
            if (hitbox.Top < 0)
            {
                hitbox.Y = 0;
                playerPos.Y = -sourceRecWorld.Height + hitbox.Height;
            }
            else if (hitbox.Bottom > window.Height)
            {
                hitbox.Y = window.Height - hitbox.Height;
                playerPos.Y = window.Height - sourceRecWorld.Height;
            }
            if (hitbox.Left < 0)
            {
                hitbox.X = 0;
                playerPos.X = -(sourceRecWorld.Width - hitbox.Width) / 2;
            }
            else if (hitbox.Right > window.Width)
            {
                hitbox.X = window.Width - hitbox.Width;
                playerPos.X = window.Width - sourceRecWorld.Width + (sourceRecWorld.Width - hitbox.Width) / 2;
            }

            if (!attackTest)
            {

                if (!dead)
                {
                    if (speed == Vector2.Zero)
                        sourceRecWorld.X = OVERWORLD_SPRITE_WIDTH;
                    else if (Math.Abs(speed.Y) > Math.Abs(speed.X))
                    {
                        if (speed.Y > 0)
                            sourceRecWorld.Y = 216;
                        else
                            sourceRecWorld.Y = 0;
                    }
                    else if (Math.Abs(speed.X) > Math.Abs(speed.Y))
                    {
                        if (speed.X > 0)
                            sourceRecWorld.Y = 144;
                        else
                            sourceRecWorld.Y = 72;
                    }
                    if (speed != Vector2.Zero)
                    {
                        if (timer % 6 == 0)
                            sourceRecWorld.X = (sourceRecWorld.X + OVERWORLD_SPRITE_WIDTH) % overworldTex.Width;
                    }
                    if (healthBar.CurrentValue <= 0)
                        death();
                }
            }

            //Attacc animation tests
            if ((pad1.IsButtonDown(Buttons.X) || KB.IsKeyDown(Keys.F)) && !attackTest)
            {
                attackTest = true;
                sourceRecBattle.X = 96 * 3;
                sourceRecFX.X = 0;
            }

            if (attackTest)
            {
                if (sourceRecBattle.X <= 96 * 5)
                {
                    if (timer % 8 == 0)
                        sourceRecBattle.X += 96;
                    if (timer % 4 == 0)
                        sourceRecFX.X += 128;
                }
                if (sourceRecBattle.X >= 96 * 6)
                {
                    attackTest = false;
                }
            }

            //Use to test health bar stuff
            if (pad1.IsButtonDown(Buttons.DPadDown))
                healthBar.CurrentValue--;
            else if (pad1.IsButtonDown(Buttons.DPadUp))
                healthBar.CurrentValue++;
            timer++;
            //Testing death stuff
            if (pad1.IsButtonDown(Buttons.LeftShoulder) || KB.IsKeyDown(Keys.K))
            {
                healthBar.CurrentValue = 0;
                death();
            }
            if (pad1.IsButtonDown(Buttons.RightShoulder) || KB.IsKeyDown(Keys.L))
            {
                healthBar.CurrentValue = 100;
                dead = false;
            }
            //New health bar
            healthBar.SetLocation((int)playerPos.X - 10, (int)playerPos.Y - 10);
            magicBar.SetLocation((int)playerPos.X - 10, (int)playerPos.Y - 20);

            //healthBar.CurrentValue = healthPoints;
            //magicBar.CurrentValue = magicPoints;
        }

        public void MoveY(int speed)
        {
            if (!dead)
            {
                playerPos.Y -= speed;
                hitbox.Y -= speed;
            }
        }

        public void MoveX(int speed)
        {
            if (!dead)
            {
                playerPos.X += speed;
                hitbox.X += speed;
            }
        }

        private void UpdateBattlemenu(GameTime gameTime)
        {
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
            timer++;
            //battleHealthBar.CurrentValue = healthPoints;
            //battleMagicBar.CurrentValue = magicPoints;
            //battleChargeBar.CurrentValue = chargePoints;
            healthBar.Rect = healthRect;
            magicBar.Rect = magicRect;
            //idle Animation
            if (!isAttacking || isCharging)
            {
                sourceRecIdle.Y = 96;
                if (timer % 5 == 0)
                    sourceRecIdle.X += BATTLE_SPRITE_WIDTH;
                if (sourceRecIdle.X >= BATTLE_SPRITE_WIDTH * 3)
                    sourceRecIdle.X = 0;
            }
            //Charging
            if (isCharging)
            {
                if (chargeBar.CurrentValue == chargeBar.MaxValue)
                {
                    isCharging = false;
                    isAttacking = true;
                }
                if (timer % 2 == 0)
                    chargeBar.CurrentValue++;
            }
        }

        public void Damage(int damage)
        {
            healthBar.CurrentValue -= damage;
        }

        public void LearnAttack(Attack attack)
        {
            if (attack.GetType() == Attack.Chop.GetType())
            {
                for (int i = 0; i < basicAttacks.Length; i++)
                {
                    if (basicAttacks[i] == null)
                    {
                        basicAttacks[i] = attack;
                        return;
                    }
                }
            }
            else
            {
                MagicAttack mAttack = (MagicAttack)attack;
                Attack[] magicArray;
                if (!magicAttacks.TryGetValue(mAttack.Element, out magicArray))
                {
                    Console.WriteLine("Could not find element " + mAttack.Element);
                    throw new Exception("Element not found");
                }
                for (int i = 0; i < magicArray.Length; i++)
                {
                    if (magicArray[i] == null)
                    {
                        magicArray[i] = attack;
                        magicAttacks[mAttack.Element] = magicArray;
                        return;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    if (dead || attackTest)
                    {
                        spriteBatch.Draw(combatTex, playerPos, sourceRecBattle, color);
                        //spriteBatch.Draw(combatFX, battleFXPos, sourceRecFX, color);
                    }
                    else
                        spriteBatch.Draw(overworldTex, playerPos, sourceRecWorld, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f / hitbox.Bottom);
                    break;
                case GameState.Battlemenu:
                    if (!isAttacking)
                        spriteBatch.Draw(combatTex, battlePos, sourceRecIdle, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f); //Idle
                    else
                    {
                        spriteBatch.Draw(combatTex, battlePos, sourceRecBattle, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                        spriteBatch.Draw(combatFX, battleFXPos, sourceRecFX, color, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    }
                    healthBar.Draw(spriteBatch, true);
                    magicBar.Draw(spriteBatch, true);
                    chargeBar.Draw(spriteBatch, true);
                    xpBar.Draw(spriteBatch, true);
                    break;
            }
        }

        //10 X 17
        public void DrawProfile(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(profileTex, rect, sourceRecProfile, Color.White);
        }

        public void Battle()
        {
            currentGameState = GameState.Battlemenu;
            healthBar.Rect = healthRect;
            magicBar.Rect = magicRect;
            chargeBar.CurrentValue = 0;
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
            //TODO resetting the locations for the health and magic bars in the overworld
        }
    }
}
