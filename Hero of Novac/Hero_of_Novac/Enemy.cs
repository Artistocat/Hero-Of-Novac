
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

        private Rectangle space; //save

        private GameState currentGameState;
        private Rectangle battleRec; //save
        private Rectangle battleSourceRec; //save
        private PercentageRectangle healthBar; //save
        private Rectangle healthRect; //save
        private PercentageRectangle chargeBar; //save
        private BattleState currentBattleState;
        bool constantMove;
        bool isIdle;

        private Attack currentAttack;

        /*public string SaveInfo
        {
            get
            {
                string str = "";
                str += rec.X + " " + rec.Y + " " + rec.Width + " " + rec.Height; //rec
                str += "\n";
                str += sourceRec.X + " " + sourceRec.Y + " " + sourceRec.Width + " " + sourceRec.Height; //sourceRec
                str += "\n";
                str += tex.Name; //texName
                str += "\n";
                str += sourceRecProfile.X + " " + sourceRecProfile.Y + " " + sourceRecProfile.Width + " " + sourceRecProfile.Height; //sourceRecProfile
                str += "\n";
                str += profileTex.Name;
                str += "\n";
                str += pos.X + " " + pos.Y;
                str += "\n";

                str += space.X + " " + space.Y + " " + space.Width + " " + space.Height;
                str += "\n";
                str += battleRec.X + " " + battleRec.Y + " " + battleRec.Width + " " + battleRec.Height;
                str += "\n";
                str += battleSourceRec.X + " " + battleSourceRec.Y + " " + battleSourceRec.Width + " " + battleSourceRec.Height;
                str += "\n";
                str += healthBar.SaveData;
                str += "\n";
                str += healthRect.X + " " + healthRect.Y + " " + healthRect.Width + " " + healthRect.Height;
                str += "\n";
                str += chargeBar.SaveData;
                str += "\n";
                return str;
            }
        }
       */

        /*
         * 146 x 116
         */
        public Enemy(Rectangle rec, Rectangle sourceRec, Rectangle space, Texture2D tex, Rectangle sourceRecProfile, Texture2D profileTex, Vector2 pos, Rectangle window, Random ran, bool constantMove, bool idleAnimation, Vector2 vol)
        {
            this.space = space;
            this.vol = vol;
            this.rec = rec;
            this.sourceRec = sourceRec;
            this.tex = tex;
            this.pos = pos;
            this.ran = ran;
            this.constantMove = constantMove;
            this.isIdle = idleAnimation;
            this.sourceRecProfile = sourceRecProfile;
            this.profileTex = profileTex;
            currentGameState = GameState.Overworld;
            healthBar = new PercentageRectangle(new Rectangle(rec.X - 10, rec.Y - 10, barWidth, barHeight), 50, Color.Red);
            chargeBar = new PercentageRectangle(new Rectangle(healthBar.Rect.X, healthBar.Rect.Y + 50, barWidth, barHeight), 100, Color.Gray);
            timer = 0;
            battleRec = new Rectangle(window.Right - rec.Width * 4, window.Bottom / 4 - rec.Height, rec.Width, rec.Height);
            healthRect = new Rectangle(window.Left + window.Width * 3 / 4 + 25, window.Height / 2 + 100, barWidth * 5, barHeight * 5);
            chargeBar.Rect = healthRect;
            Rectangle chargeRect = healthRect;
            chargeRect.Y += 50;
            chargeBar.Rect = chargeRect;
            chargeBar.CurrentValue = 0;
            battleSourceRec = sourceRec;
            battleSourceRec.Y = 116;
            currentBattleState = BattleState.Charging;
            currentAttack = Attack.Slash;
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

            if (vol == Vector2.Zero && !constantMove)
                sourceRec.X = sourceRec.Width;
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
            timer++;
            //healthBar.Rect = healthRect;
            if (player.isCharging)
            {
                if (timer % 2 == 0)
                {
                    chargeBar.CurrentValue++;
                }
                if (chargeBar.CurrentValue == chargeBar.MaxValue)
                {
                    currentBattleState = BattleState.Attacking;
                }
            }
            if (isIdle)
            {
                sourceRec.Y = 96;
                if (timer % 5 == 0)
                    sourceRec.X += sourceRec.Width;
                if (sourceRec.X >= sourceRec.Width * 3)
                    sourceRec.X = 0;
            }
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
