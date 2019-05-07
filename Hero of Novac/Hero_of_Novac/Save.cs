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
        public Save(String savePath)
        {
            file = new StreamWriter(@"C:\Users\Public\SaveData.save");
        }

        public void playerSave(Player player)
        {
            file.WriteLine("stuff");
        }

        public void enemySave(List<Enemy> enemies)
        {

        }


    }
}
