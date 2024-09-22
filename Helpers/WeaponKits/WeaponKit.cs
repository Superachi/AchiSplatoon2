using AchiSplatoon2.Content.Items.Weapons;

namespace AchiSplatoon2.Helpers.WeaponKits
{
    internal class WeaponKit
    {
        public SubWeaponType SubType;
        public SubWeaponBonusType BonusType;
        public float BonusAmount;

        public WeaponKit(SubWeaponType subType, SubWeaponBonusType bonusType, float bonusAmount = 0.5f)
        {
            SubType = subType;
            BonusType = bonusType;
            BonusAmount = bonusAmount;
        }
    }
}
