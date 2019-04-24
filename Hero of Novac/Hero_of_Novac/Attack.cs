using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class Attack
    {
        protected static Player player;
        protected int defaultChargeTime;
        protected int defaultDamage;

        protected int chargeTime;
        protected int damage;

        public int ChargeTime
        {
            get
            {
                return chargeTime;
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }
        }

        public Attack(int defaultChargeTime, int defaultDamage)
        {
            this.defaultChargeTime = defaultChargeTime;
            this.defaultDamage = defaultDamage;

            chargeTime = (int)(defaultChargeTime / player.LevelModifier);
            damage = (int)(defaultDamage * player.LevelModifier);
        }

        public static void LoadContent()
        {
            //TODO
            //lunge = new BasicAttack();
            //slash = new BasicAttack();
            //chop = new BasicAttack();
            //punch = new BasicAttack();

            //whirlwind = new MagicAttack();
            //airSlash = new MagicAttack();
            //windStrike = new MagicAttack();
            //faldorsWind = new MagicAttack();

            //wallOfFire = new MagicAttack();
            //fireBall = new MagicAttack();
            //incendiaryCloud = new MagicAttack();
            //ottosFireStorm = new MagicAttack();

            //thornWhip = new MagicAttack();
            //stoneThrow = new MagicAttack();
            //earthquake = new MagicAttack();
            //otilukesWrath = new MagicAttack();

            //coneOfCold = new MagicAttack();
            //iceStorm = new MagicAttack();
            //frostRay = new MagicAttack();
            //rarysTsunami = new MagicAttack();

            //magicMissile = new MagicAttack();
            //eldritchBlast = new MagicAttack();
            //arcaneBeam = new MagicAttack();
            //tashasLaugh = new MagicAttack();
        }

        //Basic attacks
        private static Attack lunge;
        private static Attack slash;
        private static Attack chop;
        private static Attack punch;

        //Air Attacks
        private static Attack whirlwind;
        private static Attack airSlash;
        private static Attack windStrike;
        private static Attack faldorsWind;

        //Fire Attacks
        private static Attack wallOfFire;
        private static Attack fireBall;
        private static Attack incendiaryCloud;
        private static Attack ottosFireStorm;

        //Earth Attacks
        private static Attack thornWhip;
        private static Attack stoneThrow;
        private static Attack earthquake;
        private static Attack otilukesWrath;

        //Water Attacks
        private static Attack coneOfCold;
        private static Attack iceStorm;
        private static Attack frostRay;
        private static Attack rarysTsunami;

        //Aether Attack
        private static Attack magicMissile;
        private static Attack eldritchBlast;
        private static Attack arcaneBeam;
        private static Attack tashasLaugh;

       //BEAST Attacks
    }
}
