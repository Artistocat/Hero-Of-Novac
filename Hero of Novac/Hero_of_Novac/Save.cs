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
