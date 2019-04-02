using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class BattleMenu
    {
        private static Player player;
        private static Texture2D pixel;
        private static SpriteFont Font;

        private static Rectangle menuRect;
        private static Rectangle basicRect;
        private static Rectangle magicRect;
        private static Rectangle itemsRect;
        private static Rectangle[] attackRects;
        private static Rectangle[] elementRects;

        private Enemy[] enemies;
        private GamePadState gamePad;
        private GamePadState oldGamePad;
        private int tics;

        enum BattleState
        {
            BeginningBattle, Charging, Attacking, ChoosingAttack, EndingBattle
        }
        BattleState currentBattleState;

        public BattleMenu(Enemy[] enemies)
        {
            currentBattleState = BattleState.ChoosingAttack;
            this.enemies = enemies;
            tics = 0;
        }

        public static void LoadContent(Player player, SpriteFont Font, GraphicsDevice graphicsDevice, Rectangle screenRect)
        {
            BattleMenu.player = player;
            BattleMenu.Font = Font;
            BattleMenu.pixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);

            int width = screenRect.Width;
            int height = screenRect.Height;
            menuRect = new Rectangle(0, height / 2, width, height / 2);
            basicRect = new Rectangle(width / 4, height / 2, width / 2, height / 4);
            magicRect = new Rectangle(basicRect.X, basicRect.Y + height / 4, basicRect.Width, basicRect.Height);
            itemsRect = new Rectangle(0, magicRect.Y, width / 4, height / 4);

            attackRects = new Rectangle[4];
            elementRects = new Rectangle[5];
            //attackRects[0] = new Rectangle()
        }

        public void Update()
        {
            oldGamePad = gamePad;
            gamePad = GamePad.GetState(PlayerIndex.One);
            switch (currentBattleState)
            {
                case BattleState.BeginningBattle:
                    BeginningBattle();
                    break;
                case BattleState.ChoosingAttack:
                    ChoosingAttack();
                    break;
                case BattleState.Charging:
                    Charging();
                    break;
                case BattleState.Attacking:
                    Attacking();
                    break;
                case BattleState.EndingBattle:
                    EndingBattle();
                    break;
            }
            tics++;
        }

        private void BeginningBattle()
        {
            
        }

        private void ChoosingAttack()
        {

        }

        private void Charging()
        {

        }

        private void Attacking()
        {

        }

        private void EndingBattle()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
            foreach(Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            //spriteBatch.Draw(pixel, )
        }
    }
}
