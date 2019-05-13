
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
        private List<Enemy> enemies = new List<Enemy>();
        private String playerInfo = "";

        StreamReader file;
        public Load()
        {

        }
        public void LoadAll(Area area)
        {
            file = new StreamReader(@"Content/SaveData.save");
            ReadFileAsStrings(@"Content/SaveData.save");
            file.Close();
        }
        private void LoadPlayer(StreamReader reader)
        {
            //playerInfo+=nextLine;
        }
        private void LoadNextEnemy(StreamReader reader)
        {
            string rec = reader.ReadLine();
            string sourceRec = reader.ReadLine();
            string texName = reader.ReadLine();
            string sourceRecProfile = reader.ReadLine();
            string profileTexName = reader.ReadLine();
            string pos = reader.ReadLine();
            string space = reader.ReadLine();
            string battleRec = reader.ReadLine();
            string battleSourceRec = reader.ReadLine();
            string healthBar = reader.ReadLine();
            string healthRect = reader.ReadLine();
            string chargeBar = reader.ReadLine();

            enemies.Add(new Enemy(rec, sourceRec, texName, sourceRecProfile, profileTexName, pos, space, battleRec, battleSourceRec, healthBar, healthRect, chargeBar));

        }
        private void LoadNextNPC(StreamReader reader)
        {

        }
        enum SaveReading
        {
            player, enemy, npc, none
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
                            default:
                                currentRead = SaveReading.none;
                                break;
                        }
                        if (currentRead == SaveReading.none)
                        {
                            throw new Exception("Save file has incorrect format, or code can't read correctly");
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
