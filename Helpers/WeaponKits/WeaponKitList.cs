using System;
using System.Collections.Generic;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Brushes;

namespace AchiSplatoon2.Helpers.WeaponKits
{
    internal static class WeaponKitList
    {
        public static Dictionary<Type, WeaponKit> WeaponKitDictionary = new Dictionary<Type, WeaponKit>
        {
            // Shooters
            { typeof(Splattershot),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(TentatekSplattershot), new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(OctoShot),             new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplashOMatic),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplooshOMatic),        new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Dot52Gal),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(JetSquelcher),         new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },

            // Chargers
            { typeof(SplatCharger),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(ZFSplatCharger),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(BambooMk1Charger),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(BambooMk2Charger),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(EliterCharger),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(GrizzcoCharger),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Stringers
            { typeof(ReefluxStringer),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(TriStringer),          new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(ReefluxDecoStringer),  new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(TriStringerInkline),   new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(GrizzcoStringer),      new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Blasters
            { typeof(Blaster),              new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(RapidBlaster),         new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(LunaBlaster),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RangeBlaster),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RapidBlasterDeco),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },

            // Splatlings
            { typeof(MiniSplatling),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(HeavySplatling),       new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(ZinkMiniSplatling),    new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RoyalHeavySplatling),  new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },

            // Sloshers
            { typeof(Slosher),              new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(SlosherDeco),          new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },

            // Brushes
            { typeof(Inkbrush),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(InkbrushNouveau),      new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Octobrush),            new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(OctobrushNouveau),     new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
        };

        public static SubWeaponType GetWeaponKitSubType(Type weaponType)
        {
            if (!WeaponKitDictionary.ContainsKey(weaponType))
            {
                return SubWeaponType.None;
            }

            return WeaponKitDictionary[weaponType].SubType;
        }

        public static SubWeaponBonusType GetWeaponKitSubBonusType(Type weaponType)
        {
            if (!WeaponKitDictionary.ContainsKey(weaponType))
            {
                return SubWeaponBonusType.None;
            }

            return WeaponKitDictionary[weaponType].BonusType;
        }
    }
}
