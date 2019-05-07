using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hero_of_Novac
{
    public class BasicAttack : Attack
    {
        public BasicAttack(int defaultChargeTime, int defaultDamage, string attackName) :base(defaultChargeTime, defaultDamage, attackName)
        {
        }

        public new bool IsBasic()
        {
            return true;
        }
    }
}
