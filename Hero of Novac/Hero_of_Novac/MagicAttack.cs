using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class MagicAttack : Attack
    {
        private Element element;
        public Element Element
        {
            get
            {
                return element;
            }
        }

        public MagicAttack(int defaultChargeTime, int defaultDamage, AttackOptions attackName, Element element) : base(defaultChargeTime, defaultDamage, attackName)
        {
            this.element = element;
            int elementlvl = player.Elementlvl(element);
            damage *= (int)(elementlvl * 1.25);
            chargeTime /= (int)(elementlvl * 1.25);
        }

        public override bool IsMagic()
        {
            return true;
        }
    }
}
