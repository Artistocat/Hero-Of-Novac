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
        private static NavigableMenuItem[] Element;
        private static Rectangle menuRect;

        private Enemy[] enemies;
        private List<int> attackingEnemies;
        private GamePadState gamePad;
        private GamePadState oldGamePad;
        private KeyboardState KB;
        private KeyboardState oldKB;
        private int timer;
        private bool animationFlag1;

        enum BattleState
        {
            BeginningBattle, Charging, Attacking, EnemyAttacking, ChoosingAttack, Victory, Defeat
        }
        BattleState currentBattleState;

        enum ChoiceState
        {
            MainChoice, Basic, Magic, Items
        }
        ChoiceState currentChoiceState;

        public enum Biome
        {
            Plains, Forest, Ice, Water
        }
        Biome currentBiome;

        private ChoiceState CurrentChoiceState
        {
            get
            {
                return currentChoiceState;
            }
            set
            {
                currentChoiceState = value;
                foreach (NavigableMenuItem mi in Element)
                {
                    mi.isSelected = false;
                }
                Element[0].isSelected = true;
                for (int i = 0; i < 2; i++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Magic[i, k].isSelected = false;
                    }
                }
                int count = 0;
                foreach (NavigableMenuItem m in Magic)
                {
                    m.isSelected = false;
                    try
                    {
                        Attack[] magicAttacks = player.MagicAttacks[0];
                        m.Name = "" + magicAttacks[count].AttackName;
                    }
                    catch
                    {
                        //do nothing cuz I'm lazy and realize there is no name
                    }
                    count++;
                }
                Magic[0, 0].isSelected = true;
                for (int i = 0; i < 2; i++)
                {
                    for (int k = 0; k < 2; k++)
                        Basic[i, k].isSelected = false;
                }
                Basic[0, 0].isSelected = true;
            }
        }

        //enum MenuChoice
        //{
        //    One, Two, Three, Four, Basic, Magic, Items
        //}
        //MenuChoice currentMenuChoice;

        public BattleMenu(Enemy[] enemies, Biome biome)
        {
            currentBiome = biome;
            currentBattleState = BattleState.BeginningBattle;
            CurrentChoiceState = ChoiceState.MainChoice;
            //currentMenuChoice = MenuChoice.Basic;
            this.enemies = enemies;
            timer = 0;
            animationFlag1 = false;

            int count = 0;
            foreach (NavigableMenuItem m in MainChoices)
            {
                m.isSelected = false;
                count++;
            }
            count = 0;
            foreach (NavigableMenuItem m in Basic)
            {
                m.isSelected = false;
                m.Name = "" + player.BasicAttacks[count].AttackName;
                count++;
            }
            count = 0;
            foreach (NavigableMenuItem m in Magic)
            {
                m.isSelected = false;
                try
                {
                    Attack[] magicAttacks = player.MagicAttacks[0];
                    m.Name = "" + magicAttacks[count].AttackName;
                }
                catch
                {
                    //do nothing cuz I'm lazy and realize there is no name
                }
                count++;
            }
            MainChoices[0].isSelected = true;

            attackingEnemies = new List<int>();
        }

        public static void LoadContent(Player player, SpriteFont Font, SpriteFont SmallFont, Texture2D p, Rectangle screenRect)
        {
            NavigableMenuItem.SmallFont = SmallFont;
            NavigableMenuItem.Font = Font;
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
            MainChoices[0] = new NavigableMenuItem(basicRect, pix, singleRect, Color.Blue, "Basic", false);
            MainChoices[1] = new NavigableMenuItem(magicRect, pix, singleRect, Color.Purple, "Magic", false);
            MainChoices[2] = new NavigableMenuItem(itemsRect, pix, singleRect, Color.Green, "Items", false);

            Basic = new NavigableMenuItem[2, 2];
            Basic[0, 0] = new NavigableMenuItem(attackRects[0], pix, singleRect, Color.Red, "Slash", false);
            Basic[0, 1] = new NavigableMenuItem(attackRects[1], pix, singleRect, Color.Yellow, "Punch", false);
            Basic[1, 0] = new NavigableMenuItem(attackRects[2], pix, singleRect, Color.Yellow, "Chop", false);
            Basic[1, 1] = new NavigableMenuItem(attackRects[3], pix, singleRect, Color.Red, "Bitch slap", false);

            Magic = new NavigableMenuItem[2, 2];
            Magic[0, 0] = new NavigableMenuItem(attackRects[0], pix, singleRect, Color.Purple, "", false);
            Magic[0, 1] = new NavigableMenuItem(attackRects[1], pix, singleRect, Color.Blue, "", false);
            Magic[1, 0] = new NavigableMenuItem(attackRects[2], pix, singleRect, Color.Blue, "", false);
            Magic[1, 1] = new NavigableMenuItem(attackRects[3], pix, singleRect, Color.Purple, "", false);

            int elementHeight = 20;
            int elementY = attackRects[0].Y - elementHeight;
            Element = new NavigableMenuItem[5];
            Element[0] = new NavigableMenuItem(new Rectangle(width / 4, elementY, width / 2 / 5, elementHeight), pix, singleRect, Color.BlanchedAlmond, "" + Hero_of_Novac.Element.Air, true);
            Element[1] = new NavigableMenuItem(new Rectangle(width / 4 + width / 2 / 5, elementY, width / 2 / 5, elementHeight), pix, singleRect, Color.BlanchedAlmond, "" + Hero_of_Novac.Element.Fire, true);
            Element[2] = new NavigableMenuItem(new Rectangle(width / 4 + 2 * width / 2 / 5, elementY, width / 2 / 5, elementHeight), pix, singleRect, Color.BlanchedAlmond, "" + Hero_of_Novac.Element.Aether, true);
            Element[3] = new NavigableMenuItem(new Rectangle(width / 4 + 3 * width / 2 / 5, elementY, width / 2 / 5, elementHeight), pix, singleRect, Color.BlanchedAlmond, "" + Hero_of_Novac.Element.Water, true);
            Element[4] = new NavigableMenuItem(new Rectangle(width / 4 + 4 * width / 2 / 5, elementY, width / 2 / 5, elementHeight), pix, singleRect, Color.BlanchedAlmond, "" + Hero_of_Novac.Element.Earth, true);
            //WHY DIDN'T I DO THIS IN A FOR LOOP?????

        }

        public void Update(GameTime gameTime)
        {
            oldGamePad = gamePad;
            gamePad = GamePad.GetState(PlayerIndex.One);
            oldKB = KB;
            KB = Keyboard.GetState();
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
                case BattleState.Victory:
                    Victory();
                    break;
                case BattleState.Defeat:
                    Defeat();
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
            if (timer >= 120)
            {
                currentBattleState = BattleState.ChoosingAttack;
            }
        }

        private void ChoosingAttack()
        {
            switch (CurrentChoiceState)
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
            if (CurrentChoiceState != ChoiceState.MainChoice)
                if ((oldGamePad.Buttons.B == ButtonState.Pressed && gamePad.Buttons.B != ButtonState.Pressed) || (oldKB.IsKeyDown(Keys.Back) && KB.IsKeyUp(Keys.Back)))
                    CurrentChoiceState = ChoiceState.MainChoice;
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

            if (KB.IsKeyDown(Keys.W))
                dir = Direction.Up;
            if (KB.IsKeyDown(Keys.A))
                dir = Direction.Left;
            if (KB.IsKeyDown(Keys.S))
                dir = Direction.Down;
            if (KB.IsKeyDown(Keys.D))
                dir = Direction.Right;
            return dir;
        }

        private void ChoosingMainChoice()
        {
            const int TOP = 0;
            const int BOTTOM = 1;
            const int LEFT = 2;

            if ((oldGamePad.Buttons.A == ButtonState.Pressed && gamePad.Buttons.A != ButtonState.Pressed) || (oldKB.IsKeyDown(Keys.Enter) && KB.IsKeyUp(Keys.Enter)))
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
                        CurrentChoiceState = ChoiceState.Basic;
                        Basic[0, 0].isSelected = true;
                        break;
                    case BOTTOM:
                        CurrentChoiceState = ChoiceState.Magic;
                        Magic[0, 0].isSelected = true;
                        Element[0].isSelected = true;
                        break;
                    case LEFT:
                        CurrentChoiceState = ChoiceState.Items;
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
            if ((oldGamePad.Buttons.A == ButtonState.Pressed && gamePad.Buttons.A != ButtonState.Pressed) || (oldKB.IsKeyDown(Keys.Enter) && KB.IsKeyUp(Keys.Enter)))
            {
                Vector2 selected = GetSelected(Basic);
                player.CurrentAttack = player.BasicAttacks[(int)(2 * selected.X + selected.Y)];
                currentBattleState = BattleState.Charging;
                Console.WriteLine("attack is " + player.CurrentAttack.AttackName);
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
            if ((oldGamePad.Buttons.A == ButtonState.Pressed && gamePad.Buttons.A != ButtonState.Pressed) || (oldKB.IsKeyDown(Keys.Enter) && KB.IsKeyUp(Keys.Enter)))
            {
                Vector2 selected = GetSelected(Magic);
                Element element = GetSelectedElement();
                Attack selectedAttack = player.MagicAttacks[element][(int)(2 * selected.X + selected.Y)];
                if (selectedAttack != null)
                {
                    player.CurrentAttack = selectedAttack;
                    currentBattleState = BattleState.Charging;
                }
            }

            if ((gamePad.Buttons.RightShoulder == ButtonState.Released && oldGamePad.Buttons.RightShoulder == ButtonState.Pressed) || (KB.IsKeyUp(Keys.E) && oldKB.IsKeyDown(Keys.E)))
            {
                for (int i = 0; i < Element.Length; i++)
                {
                    if (Element[i].isSelected)
                    {
                        int newSelectedElement;
                        Element[i].isSelected = false;
                        if (i == Element.Length - 1)
                        {
                            Element[0].isSelected = true;
                            newSelectedElement = 0;
                        }
                        else
                        {
                            Element[i + 1].isSelected = true;
                            newSelectedElement = i + 1;
                        }
                        int count = 0;
                        for (int k = 0; k < Magic.Length; k++)
                        {
                            int x = k;
                            int y = 0;
                            if (x >= 2)
                            {
                                x -= 2;
                                y++;
                            }
                            NavigableMenuItem m = Magic[y, x];
                            try
                            {
                                Attack[] magicAttacks = player.MagicAttacks[(Element)(newSelectedElement)];
                                m.Name = "" + magicAttacks[count].AttackName;
                            }
                            catch
                            {
                                m.Name = "";
                            }
                            count++;
                        }
                        break;
                    }
                }
            }

            if ((gamePad.Buttons.LeftShoulder == ButtonState.Released && oldGamePad.Buttons.LeftShoulder == ButtonState.Pressed) || (KB.IsKeyUp(Keys.Q) && oldKB.IsKeyDown(Keys.Q)))
            {
                for (int i = 0; i < Element.Length; i++)
                {
                    if (Element[i].isSelected)
                    {
                        int newSelectedElement;
                        Element[i].isSelected = false;
                        if (i == 0)
                        {
                            Element[Element.Length - 1].isSelected = true;
                            newSelectedElement = Element.Length - 1;
                        }
                        else
                        {
                            Element[i - 1].isSelected = true;
                            newSelectedElement = i - 1;
                        }

                        int count = 0;
                        for (int k = 0; k < Magic.Length; k++)
                        {
                            int x = k;
                            int y = 0;
                            if (x >= 2)
                            {
                                x -= 2;
                                y++;
                            }
                            NavigableMenuItem m = Magic[y, x];
                            try
                            {
                                Attack[] magicAttacks = player.MagicAttacks[(Element)(newSelectedElement)];
                                m.Name = "" + magicAttacks[count].AttackName;
                            }
                            catch
                            {
                                m.Name = "";
                            }
                            count++;
                        }
                        break;
                    }
                }
            }

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

        private Element GetSelectedElement()
        {
            for (int i = 0; i < Element.Length; i++)
            {
                if (Element[i].isSelected == true)
                {
                    return (Element)i;
                }
            }
            throw new Exception("nothing selected");
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
            if (!player.isCharging)
            {
                currentBattleState = BattleState.Attacking;
                player.isAttacking = true;
            }
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy enemy = enemies[i];
                if (!enemy.IsCharging)
                {
                    currentBattleState = BattleState.EnemyAttacking;
                    attackingEnemies.Add(i);
                }
            }
        }

        private void Attacking()
        {
            bool doneAttacking = false;
            switch (player.CurrentAttack.AttackName)
            {
                case "Slash":
                    if (player.sourceRecBattle.X <= 96 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                        if (timer % 4 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Lunge":
                    if (player.sourceRecBattle.X <= 96 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Punch":
                    if (player.sourceRecBattle.X <= 96 * 5)
                    {
                        if (timer % 8 == 0)
                        {
                            player.sourceRecBattle.X += 96;
                        }
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Chop":
                    if (player.sourceRecBattle.X <= 96 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Whirlwind":
                    if (player.sourceRecBattle.X <= 96 * 1)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    else
                    {
                        player.sourceRecBattle.Y = 96 * 2;
                        player.sourceRecBattle.X = 0;
                    }
                    if (player.sourceRecFX.X <= 64 * 14)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Air Slash":
                    if (player.sourceRecBattle.X <= 96 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    if (player.sourceRecFX.X <= 64 * 3)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Wind Strike":
                    if (player.sourceRecBattle.X <= 96 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    if (player.sourceRecFX.X <= 64 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Faldor's Wind":
                    if (player.sourceRecBattle.X <= 96 * 1)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    else
                    {
                        player.sourceRecBattle.Y = 96 * 2;
                        player.sourceRecBattle.X = 0;
                    }
                    if (player.sourceRecFX.X <= 64 * 5)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Wall of Fire":
                    if (player.sourceRecBattle.X <= 96 * 1)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    else
                    {
                        player.sourceRecBattle.Y = 96 * 2;
                        player.sourceRecBattle.X = 0;
                    }
                    if (player.sourceRecFX.X <= 64 * 10)
                    {
                        if (timer % 10 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
                    break;
                case "Fire Ball":
                    break;
                case "Incendiary Cloud":
                    if(player.sourceRecBattle.X <= 96 * 1)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecBattle.X += 96;
                    }
                    else
                    {
                        player.sourceRecBattle.Y = 96 * 2;
                        player.sourceRecBattle.X = 0;
                    }
                    if(player.sourceRecFX.X <= 64 * 9)
                    {
                        if (timer % 8 == 0)
                            player.sourceRecFX.X += 64;
                    }
                    else
                    {
                        doneAttacking = true;
                    }
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
                default:
                    if (timer % 200 == 0)
                    {
                        doneAttacking = true;
                    }
                    break;
            }
            if (doneAttacking)
            {
                currentBattleState = BattleState.ChoosingAttack;
                CurrentChoiceState = ChoiceState.MainChoice;
                enemies[0].Damage(player.CurrentAttack.Damage); //TODO if we implement multiple enemies, we will have to then specify which is attacked, and do that in the choice update, where we can choose who is attacked
                if (enemies[0].Health <= 0)
                {
                    currentBattleState = BattleState.Victory;
                }
                player.CurrentAttack = null;
            }
        }

        private void EnemyAttacking()
        {
            bool doneAttacking = true; //do similar to Attacking if we do an animation for the enemy
            if (doneAttacking)
            {
                currentBattleState = BattleState.Charging;
                foreach (int index in attackingEnemies)
                {
                    player.Damage(enemies[index].CurrentAttack.Damage);
                    enemies[index].AttackComplete();
                    if (player.Health <= 0)
                        currentBattleState = BattleState.Defeat;
                }
                attackingEnemies.Clear();
            }
        }

        private void Victory()
        {

        }

        private void Defeat()
        {

        }

        public bool BattleIsOver
        {
            get
            {
                return currentBattleState == BattleState.Defeat ||
                    currentBattleState == BattleState.Victory;
            }
        }

        public List<Enemy> Enemies
        {
            get
            {
                return enemies.ToList();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //TODO Background that looks nice af
            switch (currentBiome)
            {
                case Biome.Plains:
                    break;
                case Biome.Forest:
                    break;
                case Biome.Ice:
                    break;
                case Biome.Water:
                    break;
            }
            if (currentBattleState == BattleState.BeginningBattle)
            {
                int ratio = 50;
                player.DrawProfile(spriteBatch, new Rectangle(50, 50, 10 * ratio, 17 * ratio));
                foreach (Enemy enemy in enemies)
                {
                    enemy.DrawProfile(spriteBatch, new Rectangle(1920 - 50 - 10 * ratio, 50, 10 * ratio, 17 * ratio));
                }
            }
            else
            {
                spriteBatch.Draw(pix, menuRect, Color.Black);
                switch (CurrentChoiceState)
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

            if (currentBattleState == BattleState.Victory)
            {
                //VICTORY
            }
            if (currentBattleState == BattleState.Defeat)
            {
                //DEFEAT
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
            foreach (NavigableMenuItem menuItem in Element)
                menuItem.Draw(spriteBatch);
        }

        private void DrawItems(SpriteBatch spriteBatch)
        {
            //TODO
            //Requires item/inventory implementation
        }

        private class NavigableMenuItem
        {
            public static SpriteFont SmallFont;
            public static SpriteFont Font;
            Rectangle rect;
            Texture2D texture;
            Rectangle sourceRect;
            Color color;
            Color selectedColor;
            public bool isSelected;
            String name;
            Vector2 nameV;
            bool isSmallName;

            public String Name
            {
                get { return name; }
                set
                {
                    name = value;
                    if (name.Length > 0)
                    {
                        Vector2 nameDimensions;
                        if (isSmallName)
                            nameDimensions = SmallFont.MeasureString(name);
                        else
                            nameDimensions = Font.MeasureString(name);
                        if (nameDimensions.X > rect.Width ||
                            nameDimensions.Y > rect.Height)
                        {
                            Console.WriteLine(name);
                            throw new Exception(name + " is too long for the navigable menu item");
                        }
                        float x = (rect.Width - nameDimensions.X) / 2;
                        float y = (rect.Height - nameDimensions.Y) / 2;
                        nameV = new Vector2(rect.X + x, rect.Y + y);
                    }
                }
            }

            public NavigableMenuItem(Rectangle rect, Texture2D texture, Rectangle sourceRect, Color color, String name, bool isSmallName)
            {
                this.rect = rect;
                this.texture = texture;
                this.sourceRect = sourceRect;
                this.color = color;
                this.isSmallName = isSmallName;

                selectedColor = Color.White;
                isSelected = false;
                Name = name;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Color drawColor = color;
                if (isSelected)
                    drawColor = selectedColor;
                spriteBatch.Draw(texture, rect, sourceRect, drawColor);
                if (name.Length > 0)
                {
                    if (isSmallName)
                        spriteBatch.DrawString(SmallFont, name, nameV, Color.Gray);
                    else
                    {
                        spriteBatch.DrawString(Font, name, nameV, Color.Gray);
                    }
                }
            }
        }
    }
}
