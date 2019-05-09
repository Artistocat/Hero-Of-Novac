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
        public const string enemyStart = "Enemies' info begins here";
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
            }
        }

        private void NPCSave(List<NPC> npcs)
        {
            file.WriteLine(npcStart);
            foreach (NPC n in npcs)
            {
                file.WriteLine("" + n.name);
                file.WriteLine("" + n.rec.X + " " + n.rec.Y + " " + n.rec.Width + " " + n.rec.Height);
                file.WriteLine("" + n.tex.Name);
                file.WriteLine("" + n.space.X + " " + n.space.Y + " " + n.space.Width + " " + n.space.Height);
                file.WriteLine("" + n.headshot.Name);
                file.WriteLine("next");
                //file.WriteLine("" + n.);
            }
        }
        

    }
}
