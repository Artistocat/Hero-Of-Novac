using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    class Load
    {
        private List<String> npcInfo = new List<string>();
        private List<String> enemyInfo = new List<string>();
        private List<String> playerInfo = new List<string>();

        StreamReader file;
        public Load()
        {

        }
        public void LoadAll(Area area)
        {
            file = new StreamReader(@"Content/SaveData.save");
            LoadPlayer(area.Player);
            LoadEnemies(area.Enemies);
            LoadNPCs(area.Npc);
            file.Close();
        }
        public void LoadPlayer(Player player)
        {

        }
        public void LoadEnemies(List<Enemy> enemy)
        {

        }
        public void LoadNPCs(List<NPC> npc)
        {

        }
        private void ReadFileAsStrings(string path)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file could not be read:\n" + e.Message);
            }
        }
    }
}
