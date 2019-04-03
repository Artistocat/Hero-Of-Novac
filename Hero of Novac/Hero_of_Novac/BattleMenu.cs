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

        private static NavigableMenuItem[] MainChoices;
        private static NavigableMenuItem[,] Basic;
        private static NavigableMenuItem[,] Magic;
        private static Rectangle menuRect;

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

        //enum MenuChoice
        //{
        //    One, Two, Three, Four, Basic, Magic, Items
        //}
        //MenuChoice currentMenuChoice;

        public BattleMenu(Enemy[] enemies)
        {
            currentBattleState = BattleState.BeginningBattle;
            currentChoiceState = ChoiceState.MainChoice;
            //currentMenuChoice = MenuChoice.Basic;
            this.enemies = enemies;
            tics = 0;


            foreach (NavigableMenuItem m in MainChoices)
            {
                m.isSelected = false;
            }

            foreach (NavigableMenuItem m in Basic)
            {
                m.isSelected = false;
            }

            foreach (NavigableMenuItem m in Magic)
            {
                m.isSelected = false;
            }

            MainChoices[0].isSelected = true;

            //TESTING
            currentBattleState = BattleState.ChoosingAttack;
        }

        public static void LoadContent(Player player, SpriteFont Font, GraphicsDevice graphicsDevice, Rectangle screenRect)
        {
            BattleMenu.player = player;
            BattleMenu.Font = Font;
            BattleMenu.pixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);


            
            Rectangle basicRect;
            Rectangle magicRect;
            Rectangle itemsRect;
            Rectangle[] attackRects;
            Rectangle[] elementRects;

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

            Rectangle singleRect = new Rectangle(0, 0, 1, 1);
            MainChoices = new NavigableMenuItem[3];
            MainChoices[0] = new NavigableMenuItem(basicRect, pixel, singleRect, Color.Blue);
            MainChoices[1] = new NavigableMenuItem(magicRect, pixel, singleRect, Color.Purple);
            MainChoices[2] = new NavigableMenuItem(itemsRect, pixel, singleRect, Color.Green);

            Basic = new NavigableMenuItem[2, 2];
            Basic[0, 0] = new NavigableMenuItem(attackRects[0], pixel, singleRect, Color.Blue);
            Basic[0, 1] = new NavigableMenuItem(attackRects[1], pixel, singleRect, Color.Blue);
            Basic[1, 0] = new NavigableMenuItem(attackRects[2], pixel, singleRect, Color.Blue);
            Basic[1, 2] = new NavigableMenuItem(attackRects[3], pixel, singleRect, Color.Blue);

            Magic = new NavigableMenuItem[2, 2];
            Magic[0, 0] = new NavigableMenuItem(attackRects[0], pixel, singleRect, Color.Purple);
            Magic[0, 1] = new NavigableMenuItem(attackRects[1], pixel, singleRect, Color.Purple);
            Magic[1, 0] = new NavigableMenuItem(attackRects[2], pixel, singleRect, Color.Purple);
            Magic[1, 1] = new NavigableMenuItem(attackRects[3], pixel, singleRect, Color.Purple);


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
            switch (currentChoiceState)
            {
                case ChoiceState.MainChoice:
                    ChoosingMainChoice();
                    break;
                case ChoiceState.Basic:
                    ChoosingBasic();
                    break;
                case ChoiceState.Magic:
                    ChoosingMagic();
                    break;
                case ChoiceState.Items:
                    ChoosingItems();
                    break;
            }
        }

        private void ChoosingMainChoice()
        {
            int selected = -1;
            if (gamePad.ThumbSticks.Left.Y >= 1)
                selected = 0;
            if (gamePad.ThumbSticks.Left.Y <= -1)
                selected = 1;
            if (gamePad.ThumbSticks.Left.X <= -1)
                selected = 2;
            if (selected != -1)
                for (int i = 0; i < MainChoices.Length; i++)
                {
                    MainChoices[i].isSelected = false;
                    if (i == selected)
                        MainChoices[i].isSelected = true;
                }

        }

        private void ChoosingBasic()
        {
            int selected = -1;
            if (gamePad.ThumbSticks.Left.Y >= 1)
                selected = 0;
            if (gamePad.ThumbSticks.Left.Y <= -1)
                selected = 1;
            if (gamePad.ThumbSticks.Left.X <= -1)
                selected = 2;
            if (selected != -1)
                //for (int i = 0; i < Basic.Length; i++)
                //{
                //    Basic[i].isSelected = false;
                //    if (i == selected)
                //        Basic[i].isSelected = true;
                //}

            //TODO YOU WERE RIGHT HERE

        }

        private void ChoosingMagic()
        {

        }

        private void ChoosingItems()
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
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            spriteBatch.Draw(pixel, menuRect, Color.White);
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
            foreach (NavigableMenuItem menuItem in MainChoices)
                menuItem.Draw(spriteBatch);
        }

        private void DrawBasic(SpriteBatch spriteBatch)
        {
            foreach (NavigableMenuItem menuItem in Basic)
                menuItem.Draw(spriteBatch);
        }

        private void DrawMagic(SpriteBatch spriteBatch)
        {
            foreach (NavigableMenuItem menuItem in Magic)
                menuItem.Draw(spriteBatch);
        }

        private void DrawItems(SpriteBatch spriteBatch)
        {
            //TODO
            //Requires item/inventory implementation
        }

        private class NavigableMenuItem
        {
            Rectangle rect;
            Texture2D texture;
            Rectangle sourceRect;
            Color color;
            Color selectedColor;
            public bool isSelected;

            public NavigableMenuItem(Rectangle rect, Texture2D texture, Rectangle sourceRect, Color color)
            {
                this.rect = rect;
                this.texture = texture;
                this.sourceRect = sourceRect;
                this.color = color;

                selectedColor = color;
                selectedColor.R -= 10;
                selectedColor.G -= 10;
                selectedColor.B -= 10;
                isSelected = false;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Color drawColor = color;
                if (isSelected)
                    drawColor = selectedColor;
                spriteBatch.Draw(texture, rect, sourceRect, drawColor);
            }
        }
    }
}
