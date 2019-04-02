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

        enum ChoiceState
        {
            MainChoice, Basic, Magic, Items
        }
        ChoiceState currentChoiceState;

        public BattleMenu(Enemy[] enemies)
        {
            currentBattleState = BattleState.BeginningBattle;
            currentChoiceState = ChoiceState.MainChoice;
            this.enemies = enemies;
            tics = 0;


            //TESTING
            //currentBattleState = BattleState.ChoosingAttack;
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
            attackRects[0] = new Rectangle(basicRect.X, basicRect.Y, basicRect.Width / 2, basicRect.Height / 2);
            attackRects[1] = new Rectangle(basicRect.X + basicRect.Width / 2, basicRect.Y, attackRects[0].Width, attackRects[0].Height);
            attackRects[2] = new Rectangle(attackRects[0].X, attackRects[0].Y + attackRects[0].Height, attackRects[0].Width, attackRects[0].Height);
            attackRects[3] = new Rectangle(attackRects[1].X, attackRects[2].Y, attackRects[0].Width, attackRects[0].Height);

            int elementRectHeight = 40;
            elementRects = new Rectangle[5];
            for (int i = 0; i < elementRects.Length; i++)
            {
                elementRects[i] = new Rectangle(basicRect.X + i * basicRect.Width / 5, basicRect.Y - elementRectHeight, basicRect.Width / 5, elementRectHeight);
            }

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

            spriteBatch.Draw(pixel, menuRect, Color.Blue);
            switch (currentChoiceState)
            {
                case ChoiceState.MainChoice:
                    DrawMainChoice(spriteBatch);
                    break;
                case ChoiceState.Basic:
                    DrawBasic(spriteBatch);
                    break;
                case ChoiceState.Magic:
                    DrawMagic(spriteBatch);
                    break;
                case ChoiceState.Items:
                    DrawItems(spriteBatch);
                    break;
            }
        }

        private void DrawMainChoice(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, basicRect, Color.White);
            spriteBatch.Draw(pixel, magicRect, Color.Purple);
            spriteBatch.Draw(pixel, itemsRect, Color.Green);
                
        }

        private void DrawBasic(SpriteBatch spriteBatch)
        {
            Color c = Color.Blue;
            foreach (Rectangle rect in attackRects)
            {
                spriteBatch.Draw(pixel, rect, c);
                c.R += 10;
                c.G += 10;
                c.B += 10;
            }
        }

        private void DrawMagic(SpriteBatch spriteBatch)
        {
            Color c = Color.Purple;
            foreach (Rectangle rect in attackRects)
            {
                spriteBatch.Draw(pixel, rect, c);
                c.R += 10;
                c.G += 10;
                c.B += 10;
            }
        }

        private void DrawItems(SpriteBatch spriteBatch)
        {
            //TODO
            //Requires item/inventory implementation
        }
    }
}
