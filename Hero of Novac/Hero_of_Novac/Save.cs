using System;
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
            file.Close();
        }
        private void PlayerSave(Player player)
        {
            file.WriteLine(playerStart);
        }

        private void EnemySave(List<Enemy> enemies)
        {
            file.WriteLine(enemyStart);
            foreach (Enemy enemy in enemies)
            {
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
            }
        }

        private void NPCSave(List<NPC> npcs)
        {
            file.WriteLine(npcStart);
            foreach (NPC n in npcs)
            {
                file.WriteLine("" + n.name);
                file.WriteLine("" + n.Rectangle);
                file.WriteLine("" + n.tex.Name);
                file.WriteLine("" + n.space);
                file.WriteLine("" + n.headshot.Name);
                file.WriteLine("" + n.IsInteractable);
            }
        }
    }
}


