
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
        private List<string> npcInfo;
        private List<List<string>> enemyInfo;
        private string playerInfo;

        public List<string> NpcInfo
        {
            get { return npcInfo; }
        }
        public List<List<string>> EnemyInfo
        {
            get { return enemyInfo; }
        }
        public string PlayerInfo
        {
            get { return playerInfo; }
        }

        StreamReader reader;
        public Load(int selectedSave)
        {
            npcInfo = new List<string>();
            enemyInfo = new List<List<string>>();
            playerInfo = "";
            LoadAll();
        }
        private void LoadAll()
        {
            reader = new StreamReader(@"Content/SaveData.save");
            //ReadFileAsStrings(@"Content/SaveData.save");
            ReadFile();
            reader.Close();
        }
        private void LoadPlayer()
        {
            //playerInfo+=nextLine;
        }
        private void LoadNextEnemy()
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
            string constantMove = reader.ReadLine();
            string isIdle = reader.ReadLine();
            string vol = reader.ReadLine();
            List<string> addedEnemy = new List<string>();
            addedEnemy.Add(rec);
            addedEnemy.Add(sourceRec);
            addedEnemy.Add(texName);
            addedEnemy.Add(sourceRecProfile);
            addedEnemy.Add(profileTexName);
            addedEnemy.Add(pos);
            addedEnemy.Add(space);
            addedEnemy.Add(battleRec);
            addedEnemy.Add(battleSourceRec);
            addedEnemy.Add(healthBar);
            addedEnemy.Add(healthRect);
            addedEnemy.Add(chargeBar);
            addedEnemy.Add(constantMove);
            addedEnemy.Add(isIdle);
            addedEnemy.Add(vol);
            enemyInfo.Add(addedEnemy);
        }
        private void LoadNextNPC()
        {
            //npcList.Add(new NPC()
        }
        enum SaveReading
        {
            player, enemy, npc, none
        }

        private void ReadFile()
        {
            SaveReading currentRead = SaveReading.player;
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

                        break;
                    case SaveReading.enemy:
                        LoadNextEnemy();
                        break;
                    case SaveReading.npc:
                        npcInfo.Add(line);
                        break;
                }

            }
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
