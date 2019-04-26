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
        private static Texture2D pix;
        private static SpriteFont Font;

        private static NavigableMenuItem[] MainChoices;
        private static NavigableMenuItem[,] Basic;
        private static NavigableMenuItem[,] Magic;
        private static Rectangle menuRect;

        private Enemy[] enemies;
        private GamePadState gamePad;
        private GamePadState oldGamePad;
        private int timer;

        enum BattleState
        {
            BeginningBattle, Charging, Attacking, EnemyAttacking, ChoosingAttack, EndingBattle
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
            timer = 0;


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

        public static void LoadContent(Player player, SpriteFont Font, Texture2D p, Rectangle screenRect)
        {
            BattleMenu.player = player;
            BattleMenu.Font = Font;
            pix = p;

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
            attackRects[0] = new Rectangle(basicRect.X, basicRect.Y, basicRect.Width / 2, basicRect.Height);
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
            MainChoices[0] = new NavigableMenuItem(basicRect, pix, singleRect, Color.Blue);
            MainChoices[1] = new NavigableMenuItem(magicRect, pix, singleRect, Color.Purple);
            MainChoices[2] = new NavigableMenuItem(itemsRect, pix, singleRect, Color.Green);

            Basic = new NavigableMenuItem[2, 2];
            Basic[0, 0] = new NavigableMenuItem(attackRects[0], pix, singleRect, Color.Red);
            Basic[0, 1] = new NavigableMenuItem(attackRects[1], pix, singleRect, Color.Yellow);
            Basic[1, 0] = new NavigableMenuItem(attackRects[2], pix, singleRect, Color.Yellow);
            Basic[1, 1] = new NavigableMenuItem(attackRects[3], pix, singleRect, Color.Red);

            Magic = new NavigableMenuItem[2, 2];
            Magic[0, 0] = new NavigableMenuItem(attackRects[0], pix, singleRect, Color.Purple);
            Magic[0, 1] = new NavigableMenuItem(attackRects[1], pix, singleRect, Color.Blue);
            Magic[1, 0] = new NavigableMenuItem(attackRects[2], pix, singleRect, Color.Blue);
            Magic[1, 1] = new NavigableMenuItem(attackRects[3], pix, singleRect, Color.Purple);


        }

        public void Update(GameTime gameTime)
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
                case BattleState.EnemyAttacking:
                    EnemyAttacking();
                    break;
                case BattleState.EndingBattle:
                    EndingBattle();
                    break;
            }
            timer++;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }
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
            if (currentChoiceState != ChoiceState.MainChoice)
                if (oldGamePad.Buttons.B == ButtonState.Pressed && gamePad.Buttons.B != ButtonState.Pressed)
                    currentChoiceState = ChoiceState.MainChoice;
        }

        enum Direction
        {
            Up, Down, Left, Right, Neutral
        }
        //Helper Method for Getting Direction
        private Direction GetInputDirection()
        {
            Direction dir = Direction.Neutral;
            if (gamePad.ThumbSticks.Left.Y >= .9)
                dir = Direction.Up;
            if (gamePad.ThumbSticks.Left.Y <= -.9)
                dir = Direction.Down;
            if (gamePad.ThumbSticks.Left.X <= -.9)
                dir = Direction.Left;
            if (gamePad.ThumbSticks.Left.X >= .9)
                dir = Direction.Right;
            return dir;
        }

        private void ChoosingMainChoice()
        {
            const int TOP = 0;
            const int BOTTOM = 1;
            const int LEFT = 2;

            if (oldGamePad.Buttons.A == ButtonState.Pressed && gamePad.Buttons.A != ButtonState.Pressed)
            {
                int selected = -1;
                for (int i = 0; i < MainChoices.Length; i++)
                {
                    if (MainChoices[i].isSelected)
                    {
                        selected = i;
                        break;
                    }
                }

                switch (selected)
                {
                    case TOP:
                        currentChoiceState = ChoiceState.Basic;
                        Basic[0, 0].isSelected = true;
                        break;
                    case BOTTOM:
                        currentChoiceState = ChoiceState.Magic;
                        Magic[0, 0].isSelected = true;
                        break;
                    case LEFT:
                        currentChoiceState = ChoiceState.Items;
                        break;
                }

                return;
            }

            Direction dir = GetInputDirection();
            if (dir == Direction.Neutral)
                return;

            int previousSelected = -1;
            for (int i = 0; i < MainChoices.Length; i++)
            {
                if (MainChoices[i].isSelected)
                {
                    previousSelected = i;
                }
                MainChoices[i].isSelected = false;
            }

            switch (previousSelected)
            {
                case TOP:
                    if (dir == Direction.Down)
                        MainChoices[BOTTOM].isSelected = true;
                    else if (dir == Direction.Left)
                        MainChoices[LEFT].isSelected = true;
                    else
                        MainChoices[TOP].isSelected = true;
                    break;
                case BOTTOM:
                    if (dir == Direction.Up)
                        MainChoices[TOP].isSelected = true;
                    else if (dir == Direction.Left)
                        MainChoices[LEFT].isSelected = true;
                    else
                        MainChoices[BOTTOM].isSelected = true;
                    break;
                case LEFT:
                    if (dir == Direction.Up)
                        MainChoices[TOP].isSelected = true;
                    else if (dir == Direction.Right)
                        MainChoices[BOTTOM].isSelected = true;
                    else
                        MainChoices[LEFT].isSelected = true;
                    break;
                default:
                    throw new Exception("Nothing was previously selected");
            }
        }

        private void ChoosingBasic()
        {
            if (oldGamePad.Buttons.A == ButtonState.Pressed && gamePad.Buttons.A != ButtonState.Pressed)
            {
                Vector2 selected = GetSelected(Basic);
                currentBattleState = BattleState.Charging;
                //TODO
            }

            Direction dir = GetInputDirection();
            if (dir == Direction.Neutral)
                return;
            bool newSelection = false;
            for (int i = 0; i < 2 && !newSelection; i++)
            {
                for (int k = 0; k < 2 && !newSelection; k++)
                {
                    if (Basic[i, k].isSelected)
                    {
                        bool didSwitch = false;
                        switch (dir)
                        {
                            case Direction.Up:
                                if (i > 0)
                                {
                                    Basic[i - 1, k].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                            case Direction.Down:
                                if (i != 1)
                                {
                                    Basic[i + 1, k].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                            case Direction.Left:
                                if (k > 0)
                                {
                                    Basic[i, k - 1].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                            case Direction.Right:
                                if (k != 1)
                                {
                                    Basic[i, k + 1].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                        }
                        Basic[i, k].isSelected = !didSwitch;
                        newSelection = true;
                    }
                }

            }
        }

        private void ChoosingMagic()
        {
            Direction dir = GetInputDirection();
            if (dir == Direction.Neutral)
                return;
            bool newSelection = false;
            for (int i = 0; i < 2 && !newSelection; i++)
            {
                for (int k = 0; k < 2 && !newSelection; k++)
                {
                    if (Magic[i, k].isSelected)
                    {
                        bool didSwitch = false;
                        switch (dir)
                        {
                            case Direction.Up:
                                if (i > 0)
                                {
                                    Magic[i - 1, k].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                            case Direction.Down:
                                if (i != 1)
                                {
                                    Magic[i + 1, k].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                            case Direction.Left:
                                if (k > 0)
                                {
                                    Magic[i, k - 1].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                            case Direction.Right:
                                if (k != 1)
                                {
                                    Magic[i, k + 1].isSelected = true;
                                    didSwitch = true;
                                }
                                break;
                        }
                        Magic[i, k].isSelected = !didSwitch;
                        newSelection = true;
                    }
                }
            }
        }

        //Give it Basic[,] or Magic[,]
        private Vector2 GetSelected(NavigableMenuItem[,] attacks)
        {
            Vector2 selected = new Vector2();
            bool foundSelected = false;
            for (int i = 0; i < 2 && !foundSelected; i++)
                for (int k = 0; k < 2 && !foundSelected; k++)
                    if (attacks[i, k].isSelected)
                    {
                        selected.X = i;
                        selected.Y = k;
                        foundSelected = true;
                    }
            if (!foundSelected)
                throw new Exception("Nothing selected");
            return selected;
        }

        private void ChoosingItems()
        {

        }

        private void Charging()
        {

        }

        private void Attacking()
        {
            //switch (currentAttack)
            //{
            //    case Attack.Slash:
            //        player.sourceRecBattle.X = 96 * 3;
            //        if (player.sourceRecBattle.X >= 96 * 6)
            //        {
            //            if (timer % 4 == 0)
            //                player.sourceRecBattle.X += 96;
            //        }
            //        break;
            //    case Attack.Lunge:
            //        if (player.sourceRecBattle.X >= 96 * 6)
            //        {
            //            if (timer % 4 == 0)
            //                player.sourceRecBattle.X += 96;
            //        }
            //        break;
            //    case Attack.Punch:
            //        break;
            //}
        }

        private void EnemyAttacking()
        {

        }

        private void EndingBattle()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(pix, menuRect, Color.Black);
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
            player.Draw(spriteBatch);
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
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

                selectedColor = Color.White;
                //selectedColor.A++;
                //selectedColor.R -= 10;
                //selectedColor.G -= 10;
                //selectedColor.B -= 10;
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
