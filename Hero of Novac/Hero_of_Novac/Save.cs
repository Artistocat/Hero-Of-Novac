﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class Save
    {
        public const string playerStart = "Player info starts here";
        public const string enemyStart = "Enemies' info starts here";
        public const string npcStart = "Npc info starts here";
        public const string areaStart = "Area info starts here";
        StreamWriter file;
        public Save()
        {

        }
        public void SaveAll(Area area)
        {
            file = new StreamWriter(@"Content/SaveData.save");
            PlayerSave(area.Player);
            EnemySave(area.Enemies);
            NPCSave(area.Npc);
            AreaSave(area);
            file.Close();
            Console.WriteLine("Game saved");
        }
        private void PlayerSave(Player player)
        {
            file.WriteLine(playerStart);
            file.WriteLine(player.Health);
            file.WriteLine(player.Level);
            file.WriteLine(player.Position);
            file.WriteLine(player.Hitbox);
            file.WriteLine(player.Xp);
        }

        private void EnemySave(List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                file.WriteLine(enemyStart);
                file.WriteLine(enemy.Rec);
                file.WriteLine(enemy.SourceRec);
                file.WriteLine(enemy.Tex.Name);
                file.WriteLine(enemy.SourceRecProfile);
                file.WriteLine(enemy.ProfileTex.Name);
                file.WriteLine(enemy.Pos);
                file.WriteLine(enemy.Space);
                file.WriteLine(enemy.BattleRec);
                file.WriteLine(enemy.BattleSourceRec);
                file.WriteLine(enemy.HealthBar);
                file.WriteLine(enemy.HealthRect);
                file.WriteLine(enemy.ChargeBar);
                file.WriteLine(enemy.ConstantMove);
                file.WriteLine(enemy.IsIdle);
                file.WriteLine(enemy.Vol);
                file.WriteLine(enemy.Element);
            }
        }

        private void NPCSave(List<NPC> npcs)
        {
            foreach (NPC n in npcs)
            {
                file.WriteLine(npcStart);
                file.WriteLine("" + n.name);
                file.WriteLine("" + n.Rectangle);
                file.WriteLine("" + n.tex.Name);
                file.WriteLine("" + n.space);
                file.WriteLine("" + n.headshot.Name);
                file.WriteLine("" + n.IsInteractable);
            }
        }

        private void AreaSave(Area area)
        {
            file.WriteLine(areaStart);
            file.WriteLine(area.Window);
            file.WriteLine(area.AreaRec);
            int count = area.Tiles.Count;
            file.WriteLine(count);
            foreach (Tile t in area.Tiles)
            {
                file.WriteLine(t.Rectangle);
            }
        }
    }
}


