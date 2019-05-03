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

        protected AttackOptions attackName;

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

        public AttackOptions AttackName
        {
            get
            {
                return attackName;
            }
        }

        public Attack(int defaultChargeTime, int defaultDamage, AttackOptions attackName)
        {
            this.defaultChargeTime = defaultChargeTime;
            this.defaultDamage = defaultDamage;

            chargeTime = (int)(defaultChargeTime / player.LevelModifier) * 20;
            damage = (int)(defaultDamage * player.LevelModifier) * 5;

            this.attackName = attackName;
        }

        public virtual bool IsBasic()
        {
            return false;
        }

        public virtual bool IsMagic()
        {
            return false;
        }

        public static void LoadContent(Player player)
        {
            Attack.player = player;
            lunge = new BasicAttack(5, 4, AttackOptions.lunge);
            slash = new BasicAttack(4, 3, AttackOptions.slash);
            chop = new BasicAttack(8, 7, AttackOptions.chop);
            punch = new BasicAttack(1, 1, AttackOptions.punch);

            whirlwind = new MagicAttack(5, 5, AttackOptions.whirlwind, Element.Air);
            airSlash = new MagicAttack(3, 3, AttackOptions.airSlash, Element.Air);
            windStrike = new MagicAttack(4, 4, AttackOptions.windStrike, Element.Air);
            faldorsWind = new MagicAttack(4, 5, AttackOptions.faldorsWind, Element.Air);

            wallOfFire = new MagicAttack(8, 8, AttackOptions.wallOfFire, Element.Fire);
            fireBall = new MagicAttack(12, 12, AttackOptions.fireBall, Element.Fire);
            incendiaryCloud = new MagicAttack(6, 6, AttackOptions.incendiaryCloud, Element.Fire);
            ottosFireStorm = new MagicAttack(7, 9, AttackOptions.ottosFireStorm, Element.Fire);

            thornWhip = new MagicAttack(15, 15, AttackOptions.thornWhip, Element.Earth);
            stoneThrow = new MagicAttack(20, 20, AttackOptions.stoneThrow, Element.Earth);
            earthquake = new MagicAttack(25, 25, AttackOptions.earthquake, Element.Earth);
            otilukesWrath = new MagicAttack(22, 26, AttackOptions.otilukesWrath, Element.Earth);

            coneOfCold = new MagicAttack(12, 12, AttackOptions.coneOfCold, Element.Water);
            iceStorm = new MagicAttack(20, 20, AttackOptions.iceStorm, Element.Water);
            frostRay = new MagicAttack(16, 16, AttackOptions.frostRay, Element.Water);
            rarysTsunami = new MagicAttack(17, 20, AttackOptions.rarysTsunami, Element.Water);

            magicMissile = new MagicAttack(9, 9, AttackOptions.magicMissile, Element.Aether);
            eldritchBlast = new MagicAttack(15, 15, AttackOptions.eldritchBlast, Element.Aether);
            arcaneBeam = new MagicAttack(12, 12, AttackOptions.arcaneBeam, Element.Aether);
            tashasLaugh = new MagicAttack(13, 15, AttackOptions.tashasLaugh, Element.Aether);
        }

        public enum AttackOptions
        {
            lunge, slash, chop, punch,
            whirlwind, airSlash, windStrike, faldorsWind,
            wallOfFire, fireBall, incendiaryCloud, ottosFireStorm,
            thornWhip, stoneThrow, earthquake, otilukesWrath,
            coneOfCold, iceStorm, frostRay, rarysTsunami,
            magicMissile, eldritchBlast, arcaneBeam, tashasLaugh
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



        public static Attack Lunge
        {
            get
            {
                return lunge;
            }
        }
        public static Attack Slash
        {
            get
            {
                return slash;
            }
        }
        public static Attack Chop
        {
            get
            {
                return chop;
            }
        }
        public static Attack Punch
        {
            get
            {
                return punch;
            }
        }
        public static Attack Whirlwind
        {
            get
            {
                return whirlwind;
            }
        }
        public static Attack AirSlash
        {
            get
            {
                return airSlash;
            }
        }
        public static Attack WindStrike
        {
            get
            {
                return windStrike;
            }
        }
        public static Attack FaldorsWind
        {
            get
            {
                return faldorsWind;
            }
        }
        public static Attack WallOfFire
        {
            get
            {
                return wallOfFire;
            }
        }
        public static Attack FireBall
        {
            get
            {
                return fireBall;
            }
        }
        public static Attack IncendiaryCloud
        {
            get
            {
                return incendiaryCloud;
            }
        }

        public static Attack OttosFireStorm
        {
            get
            {
                return ottosFireStorm;
            }
        }

        public static Attack ThornWhip
        {
            get
            {
                return thornWhip;
            }
        }

        public static Attack StoneThrow
        {
            get
            {
                return stoneThrow;
            }
        }
        public static Attack Earthquake
        {
            get
            {
                return earthquake;
            }
        }
        public static Attack OtilukesWrath
        {
            get
            {
                return otilukesWrath;
            }
        }

        public static Attack ConeOfCold
        {
            get
            {
                return coneOfCold;
            }
        }

        public static Attack IceStorm
        {
            get
            {
                return iceStorm;
            }
        }

        public static Attack FrostRay
        {
            get
            {
                return frostRay;
            }
        }

        public static Attack RarysTsunami
        {
            get
            {
                return rarysTsunami;
            }
        }

        public static Attack MagicMissile
        {
            get
            {
                return magicMissile;
            }
        }

        public static Attack EldritchBlast
        {
            get
            {
                return eldritchBlast;
            }
        }

        public static Attack ArcaneBeam
        {
            get
            {
                return arcaneBeam;
            }
        }

        public static Attack TashasLaugh
        {
            get
            {
                return tashasLaugh;
            }
        }
    }
}
