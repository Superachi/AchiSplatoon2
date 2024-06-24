using AchiSplatoon2.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchiSplatoon2.Helpers.WeaponKits
{
    internal class WeaponKit
    {
        public SubWeaponType SubType;
        public SubWeaponBonusType BonusType;

        public WeaponKit(SubWeaponType subType, SubWeaponBonusType bonusType)
        {
            SubType = subType;
            BonusType = bonusType;
        }
    }
}
