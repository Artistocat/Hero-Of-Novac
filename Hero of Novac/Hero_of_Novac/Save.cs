using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class Save
    {
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

        }
        private void EnemySave(List<Enemy> enemies)
        {

        }

        private void NPCSave(List<NPC> npcs)
        {
            foreach (NPC n in npcs)
            {
                file.WriteLine("" + n.name);
                file.WriteLine("" + n.rec);
                file.WriteLine("" + n.tex.Name);
                file.WriteLine("" + n.source);
                file.WriteLine("" + n.space);
                file.WriteLine("" + n.headshot.Name);
                //file.WriteLine("" + n.);
            }
        }


    }
}
