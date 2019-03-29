using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Enemy[] enemies;

        enum BattleState
        {
            BeginningBattle, Charging, Attacking, ChoosingAttack, EndingBattle
        }
        BattleState currentBattleState;

        public BattleMenu()
        {
            currentBattleState = BattleState.BeginningBattle;
        }

        public static void LoadContent(Player player, GraphicsDevice graphicsDevice)
        {
            BattleMenu.player = player;
            BattleMenu.pixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] pixelColors = new Color[1];
            pixelColors[0] = Color.White;
            pixel.SetData(pixelColors);
        }

        public void Update()
        {
            switch (currentBattleState)
            {
                case BattleState.BeginningBattle:
                    break;
                case BattleState.ChoosingAttack:
                    break;
                case BattleState.Charging:
                    break;
                case BattleState.Attacking:
                    break;
                case BattleState.EndingBattle:
                    break;
            }
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
        }
    }
}
