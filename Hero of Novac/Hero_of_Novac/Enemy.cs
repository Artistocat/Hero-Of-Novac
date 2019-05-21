
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
    public class Enemy : Entity
    {
        private static Player player;
        private static Rectangle window;
        public static Rectangle Window
        {
            get { return window; }
            set { window = value; }
        }

        private const int barWidth = 66;
        private const int barHeight = 5;
        private enum GameState
        {
            Overworld, Battlemenu
        }
        private enum BattleState
        {
            Charging, Attacking
        }

        private Random ran;
        private int timer = 0;
        private int t = 0;
        private int r1;
        private int r2;
        private Vector2 vol;
        private bool ranMov;

        public Rectangle Space
        {
            get { return space; }
        }

        public Rectangle BattleRec
        {
            get { return battleRec; }
        }

        public Rectangle BattleSourceRec
        {
            get { return battleSourceRec; }
        }

        public string HealthBar
        {
            get { return healthBar.SaveData; }
        }

        public Rectangle HealthRect
        {
            get { return healthRect; }
        }

        public string ChargeBar
        {
            get { return chargeBar.SaveData; }
        }

        public bool ConstantMove
        {
            get { return constantMove; }
        }

        public bool IsIdle
        {
            get { return isIdle; }
        }

        public Vector2 Vol
        {
            get { return vol; }
        }
        public Rectangle Rectangle
        {
            get { return rec; }
            set { rec = value; }
        }
        public int Xp
        {
            get { return xp; }
        }

        private Rectangle space; //save

        private GameState currentGameState;
        private Rectangle battleRec; //save
        private Rectangle battleSourceRec; //save
        private PercentageRectangle healthBar; //save
        private Rectangle healthRect; //save
        private PercentageRectangle chargeBar; //save
        private BattleState currentBattleState;
        private int xp;
        bool constantMove;
        bool isIdle;

        private Attack currentAttack;

        /*
         * 146 x 116
         */
        public Enemy(Rectangle rec, Rectangle sourceRec, Rectangle space, Texture2D tex, Rectangle sourceRecProfile, Texture2D profileTex, Vector2 pos, Rectangle window, Random ran, bool constantMove, Vector2 vol)
        {
            this.space = space;
            this.vol = vol;
            this.rec = rec;
            this.sourceRec = sourceRec;
            this.tex = tex;
            this.pos = pos;
            this.ran = ran;
            this.constantMove = constantMove;
            this.sourceRecProfile = sourceRecProfile;
            this.profileTex = profileTex;
            currentGameState = GameState.Overworld;
            healthBar = new PercentageRectangle(new Rectangle(rec.X - 10, rec.Y - 10, barWidth, barHeight), 50, Color.Red);
            chargeBar = new PercentageRectangle(new Rectangle(healthBar.Rect.X, healthBar.Rect.Y + 50, barWidth, barHeight), 100, Color.Gray);
            timer = 0;
            battleRec = new Rectangle(window.Right - window.Width / 3, 180, rec.Width, rec.Height);
            healthRect = new Rectangle(window.Left + window.Width * 3 / 4 + 25, window.Height / 2 + 100, barWidth * 5, barHeight * 5);
            chargeBar.Rect = healthRect;
            Rectangle chargeRect = healthRect;
            chargeRect.Y += 50;
            chargeBar.Rect = chargeRect;
            chargeBar.CurrentValue = 0;
            battleSourceRec = sourceRec;
            battleSourceRec.Y = 116;
            currentBattleState = BattleState.Charging;
            currentAttack = new Attack(12, 300, "enemyAttack");
            //xp = (int)Math.Round(player.LevelModifier);
            UpdateXP();
        }

        public Enemy(Rectangle rec, Rectangle sourceRec, Rectangle space, Texture2D tex, Rectangle sourceRecProfile, Texture2D profileTex, Vector2 pos, Rectangle window, Random ran, 
            bool constantMove, bool idleAnimation, Vector2 vol, Rectangle battleRec, Rectangle battleSourceRec, PercentageRectangle healthBar, Rectangle healthRect, PercentageRectangle chargeBar) 
            : this(rec, sourceRec, space, tex, sourceRecProfile, profileTex, pos, window, ran, constantMove, vol)
        {
            this.battleRec = battleRec;
            this.battleSourceRec = battleSourceRec;
            this.healthBar = healthBar;
            this.healthRect = healthRect;
            this.chargeBar = chargeBar;
        }

        public static void LoadContent(Player player)
        {
            Enemy.player = player;
        }

        public void Update(GameTime gametime)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    OverworldUpdate(gametime);
                    break;
                case GameState.Battlemenu:
                    BattleMenuUpdate(gametime);
                    break;
            }
            randomMove();
            rec.X += (int)vol.X;
            rec.Y += (int)vol.Y;
            if (rec.X < space.Left)
                rec.X = space.Left;
            if (rec.X > space.Right)
                rec.X = space.Right;
            if (rec.Y < space.Top)
                rec.Y = space.Top;
            if (rec.Y > space.Bottom)
                rec.Y = space.Bottom;

            if (vol == Vector2.Zero)
            {
                if (!constantMove)
                    sourceRec.X = sourceRec.Width;
            }
            else if (Math.Abs(vol.Y) >= Math.Abs(vol.X))
            {
                if (vol.Y > 0)
                    sourceRec.Y = 0;
                else
                    sourceRec.Y = 348;

            }
            else if (Math.Abs(vol.X) > Math.Abs(vol.Y))
            {
                if (vol.X > 0)
                    sourceRec.Y = 232;
                else
                    sourceRec.Y = 116;
            }

            if (timer % 6 == 0 && (constantMove || vol != Vector2.Zero))
                sourceRec.X = (sourceRec.X + sourceRec.Width) % tex.Width;
            timer++;
        }

        private void OverworldUpdate(GameTime gameTime)
        {
            if (rec.Intersects(player.Hitbox))
            {
                player.Battle();
                Battle();
                currentGameState = GameState.Battlemenu;
            }
            //healthBar.SetLocation(rec.X - 10, rec.Y - 10);
        }

        private void BattleMenuUpdate(GameTime gameTime)
        {
            //healthBar.Rect = healthRect;
            if (player.isCharging)
            {
                //Console.WriteLine("This shit is happening for the " + tex.Name);
                if (timer % 2 == 0)
                {
                    chargeBar.CurrentValue++;
                }
                if (chargeBar.CurrentValue == chargeBar.MaxValue)
                {
                    currentBattleState = BattleState.Attacking;
                }
            }
            if (timer % 6 == 0 && constantMove)
                battleSourceRec.X = (battleSourceRec.X + battleSourceRec.Width) % tex.Width;
        }

        public void AttackComplete()
        {
            currentBattleState = BattleState.Charging;
            chargeBar.CurrentValue = 0;
            chargeBar.CurrentValue = 0;
        }

        public void MoveY(int speed)
        {
            rec.Y += speed;
            space.Y += speed;
        }

        public void MoveX(int speed)
        {
            rec.X -= speed;
            space.X -= speed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState)
            {
                case GameState.Overworld:
                    spriteBatch.Draw(tex, rec, sourceRec, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f / rec.Bottom);
                    break;
                case GameState.Battlemenu:
                    spriteBatch.Draw(tex, battleRec, battleSourceRec, Color.White);
                    healthBar.Draw(spriteBatch, true);
                    chargeBar.Draw(spriteBatch, true);
                    break;
            }
        }

        public void DrawProfile(SpriteBatch spritebatch, Rectangle rect)
        {
            spritebatch.Draw(profileTex, rect, sourceRecProfile, Color.White);
        }

        public void Damage(int damage)
        {
            healthBar.CurrentValue -= damage;
        }

        public int Health
        {
            get
            {
                return healthBar.CurrentValue;
            }
        }

        public Attack CurrentAttack
        {
            get
            {
                return currentAttack;
            }
            set
            {
                currentAttack = value;
            }
        }

        public bool IsInBattle()
        {
            return currentGameState == GameState.Battlemenu;
        }

        public bool IsAttacking()
        {
            return currentBattleState == BattleState.Attacking;
        }

        public bool IsCharging
        {
            get
            {
                return currentBattleState == BattleState.Charging;
            }
        }

        public void Battle()
        {
            currentGameState = GameState.Battlemenu;
            healthBar.Rect = healthRect;
            chargeBar.CurrentValue = 0;
        }

        public void UpdateXP()
        {
            xp = (int)(Math.Round(13.5 * player.LevelModifier));
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
        }
        public void randomMove()
        {
            if (timer % 60 == 0 && ranMov == false)
            {
                // change the second number for how often you want to proc it
                if (ran.Next(100) < 50)
                {
                    ranMov = true;
                }
            }
            if (ranMov == true)
            {
                t++;
                // how long it'll move for
                if (t / 60 < 2)
                {
                    vol = new Vector2(r1, r2);
                }
                else
                {
                    //reset
                    ranMov = false;
                    t = 0;
                    vol = new Vector2(0, 0);
                    r1 = ran.Next(-2, 3);
                    r2 = ran.Next(-2, 3);
                }
            }
        }
    }

}
