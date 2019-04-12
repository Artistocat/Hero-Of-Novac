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
        private Player player;

        private enum GameState
        {
            Overworld, Battlemenu
        }

        private GameState currentGameState;

        public Enemy()
        {
            currentGameState = GameState.Overworld;
        }

        public override void Update(GameTime gametime)
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
        }

        private void OverworldUpdate(GameTime gameTime)
        {
            if (Rec.Intersects(player.Rec))
            {
                player.Battle();
                currentGameState = GameState.Battlemenu;
            }
        }

        private void BattleMenuUpdate(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Battle()
        {
            currentGameState = GameState.Battlemenu;
        }

        public void Overworld()
        {
            currentGameState = GameState.Overworld;
        }
    }
}
