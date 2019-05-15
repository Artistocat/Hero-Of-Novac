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
using System.IO;

namespace Hero_of_Novac
{
    public class Load
    {
        private List<String> npcInfo = new List<string>();
        private List<String> enemyInfo = new List<string>();
        private string playerInfo = "";
        StreamReader file;
        public Load()
        {

        }
        public void LoadAll()
        {
            ReadFileAsStrings(@"Content/SaveData.save");
        }
        public void LoadNextPlayer(Player player)
        {

        }
        public void LoadNextEnemies(List<Enemy> enemy)
        {

        }
        public void LoadNextNPCs(List<NPC> npc)
        {
            //npcList.Add(new NPC()
        }
        enum SaveReading
        {
            player, enemy, npc
        }
        private void ReadFileAsStrings(string path)
        {
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

                                break;
                            case SaveReading.enemy:

                                break;
                            case SaveReading.npc:
                                npcInfo.Add(line);
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
