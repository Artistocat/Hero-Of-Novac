using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class Load
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
        enum SaveReading
        {
            player, enemy, npc
        }

        private void ReadFileAsStrings(string path)
        {
            int lines = 0;
            SaveReading currentRead = SaveReading.player;
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        switch (line)
                        {
                            case Save.playerStart:
                                currentRead = SaveReading.player;
                                break;
                            case Save.enemyStart:
                                currentRead = SaveReading.enemy;
                                break;
                            case Save.npcStart:
                                currentRead = SaveReading.npc;
                                break;
                        }

                        switch (currentRead)
                        {
                            case SaveReading.player:
                                //read player stuff
                                break;
                            case SaveReading.enemy:

                                break;
                            case SaveReading.npc:
                                break;
                        }

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
