using AchiSplatoon2.Content.Items.Weapons;

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
