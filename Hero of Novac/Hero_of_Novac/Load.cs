
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
        private List<List<string>> npcInfo;
        private List<List<string>> enemyInfo;
        private List<string> playerInfo;
        private List<string> areaInfo;

        public List<List<string>> NpcInfo
        {
            get { return npcInfo; }
        }
        public List<List<string>> EnemyInfo
        {
            get { return enemyInfo; }
        }
        public List<string> PlayerInfo
        {
            get { return playerInfo; }
        }
        public List<string> AreaInfo
        {
            get { return areaInfo; }
        }       

        StreamReader reader;
        public Load(int selectedSave)
        {
            npcInfo = new List<List<string>>();
            enemyInfo = new List<List<string>>();
            playerInfo = new List<string>();
            areaInfo = new List<string>();
            LoadAll();
        }
        private void LoadAll()
        {
            reader = new StreamReader(@"Content/SaveData.save");
            ReadFile();
            reader.Close();
        }
        private void LoadPlayer()
        {
            string health = reader.ReadLine();
            string level = reader.ReadLine();
            string position = reader.ReadLine();
            string hitbox = reader.ReadLine();
            string xp = reader.ReadLine();
            playerInfo.Add(health);
            playerInfo.Add(level);
            playerInfo.Add(position);
            playerInfo.Add(hitbox);
            playerInfo.Add(xp);
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
            string name = reader.ReadLine();
            string rec = reader.ReadLine();
            string texName = reader.ReadLine();
            string space = reader.ReadLine();
            string headshotName = reader.ReadLine();
            string interact = reader.ReadLine();
            List<string> addedNPC = new List<string>();
            addedNPC.Add(name);
            addedNPC.Add(rec);
            addedNPC.Add(texName);
            addedNPC.Add(space);
            addedNPC.Add(headshotName);
            addedNPC.Add(interact);
            npcInfo.Add(addedNPC);
        }
        private void LoadArea()
        {
            string window = reader.ReadLine();
            string areaRec = reader.ReadLine();
            int count;
            Int32.TryParse(reader.ReadLine(), out count);
            areaInfo.Add(window);
            areaInfo.Add(areaRec);
            for (int i = 0; i < count; i++)
            {
                areaInfo.Add(reader.ReadLine());
            }
        }
        enum SaveReading
        {
            player, enemy, npc, area, none
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
                    case Save.areaStart:
                        currentRead = SaveReading.area;
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
                        LoadPlayer();
                        break;
                    case SaveReading.enemy:
                        LoadNextEnemy();
                        break;
                    case SaveReading.npc:
                        LoadNextNPC();
                        break;
                    case SaveReading.area:
                        LoadArea();
                        break;
                }

            }
        }
    }
}
