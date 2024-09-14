using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using System;
using System.Collections.Generic;

namespace AchiSplatoon2.Helpers.WeaponKits
{
    internal static class WeaponKitList
    {
        public static Dictionary<Type, WeaponKit> WeaponKitDictionary = new Dictionary<Type, WeaponKit>
        {
            // Shooters
            { typeof(Splattershot),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(TentatekSplattershot), new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(OctoShot),             new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(ClassicSplattershotS1),new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(ClassicSplattershotS2),new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplashOMatic),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplashOMaticNeo),      new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplooshOMatic),        new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplooshOMaticNeo),     new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Dot52Gal),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(JetSquelcher),         new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(L3Nozzlenose),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(H3Nozzlenose),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },

            // Dualies
            { typeof(SplatDualie),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(EnperrySplatDualie),   new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(DappleDualie),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(DarkTetraDualie),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(LightTetraDualie),     new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(DouserDualie),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(CustomDouserDualie),   new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(GloogaDualie),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(GrizzcoDualie),        new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Chargers
            { typeof(SplatCharger),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(ZFSplatCharger),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(ClassicSquiffer),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(FreshSquiffer),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(BambooMk1Charger),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(BambooMk2Charger),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(EliterCharger),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(CustomEliterCharger),  new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(GrizzcoCharger),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Stringers
            { typeof(ReefluxStringer),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(TriStringer),          new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(ReefluxDecoStringer),  new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(TriStringerInkline),   new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(GrizzcoStringer),      new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Snipewriter5H),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Snipewriter5B),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },

            // Blasters
            { typeof(Blaster),              new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(RapidBlaster),         new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(LunaBlaster),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RangeBlaster),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RapidBlasterDeco),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SBlast92),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SBlast91),             new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },

            // Splatlings
            { typeof(MiniSplatling),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(HeavySplatling),       new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(ZinkMiniSplatling),    new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RoyalHeavySplatling),  new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(HydraSplatling),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Sloshers
            { typeof(Slosher),              new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(SlosherDeco),          new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SodaSlosher),          new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(TriSlosher),           new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(TriSlosherNouveau),    new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Bloblobber),           new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(BloblobberDeco),       new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },

            // Brushes
            { typeof(Inkbrush),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(InkbrushNouveau),      new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(Octobrush),            new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(OctobrushNouveau),     new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Splatana
            { typeof(SplatanaWiper),        new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(SplatanaWiperDeco),    new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(SplatanaStamper),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },

            // Brella
            { typeof(SplatBrella),          new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(SorellaSplatBrella),   new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(UndercoverBrella),     new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RecycleBrella),        new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(RecycleBrellaMk2),     new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },

            // Rollers
            { typeof(SplatRoller),          new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(KrakonSplatRoller),    new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(CarbonRoller),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount) },
            { typeof(CarbonRollerDeco),     new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(DynamoRoller),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount) },
            { typeof(GoldDynamoRoller),     new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },

            // Kensa weapons
            { typeof(KensaDot52Gal),            new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaDynamoRoller),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaGloogaDualie),        new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaL3Nozzlenose),        new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaLunaBlaster),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaMiniSplatling),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaOctobrush),           new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaRapidBlaster),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaReefluxStringer),     new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaSplatanaStamper),     new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaSplatCharger),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaSplatDualie),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaSplatRoller),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage) },
            { typeof(KensaUndercoverBrella),    new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage) },

            // Original
            { typeof(SlimeSplattershot),    new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount) },
            { typeof(StarfishedCharger),    new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage) },
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
