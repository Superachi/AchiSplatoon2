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
        private static readonly float _defaultSubSaverBonus = 0.3f;
        private static readonly float _defaultSubDamageBonus = 0.5f;

        public static Dictionary<Type, WeaponKit> WeaponKitDictionary = new Dictionary<Type, WeaponKit>
        {
            // Shooters
            { typeof(Splattershot),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SplattershotJr),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SplattershotPro),      new WeaponKit(subType: SubWeaponType.PointSensor, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },

            { typeof(CustomSplattershotJr), new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(ForgeSplattershotPro), new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            { typeof(TentatekSplattershot), new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(OctoShot),             new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(ClassicSplattershotS1),new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(ClassicSplattershotS2),new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SplashOMatic),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SplashOMaticNeo),      new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SplooshOMatic),        new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SplooshOMaticNeo),     new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Dot52Gal),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Dot96Gal),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(JetSquelcher),         new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(L3Nozzlenose),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(H3Nozzlenose),         new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(HeroShot),             new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },

            // Dualies
            { typeof(SplatDualie),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(EnperrySplatDualie),   new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(DappleDualie),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(DappleDualieNouveau),  new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(DarkTetraDualie),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(LightTetraDualie),     new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(DouserDualie),         new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(CustomDouserDualie),   new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(DualieSquelcher),      new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(GloogaDualie),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(GrizzcoDualie),        new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Chargers
            { typeof(SplatCharger),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(ZFSplatCharger),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(ClassicSquiffer),      new WeaponKit(subType: SubWeaponType.PointSensor, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(FreshSquiffer),        new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(BambooMk1Charger),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(BambooMk2Charger),     new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(EliterCharger),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(CustomEliterCharger),  new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(GrizzcoCharger),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Stringers
            { typeof(ReefluxStringer),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(TriStringer),          new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(ReefluxDecoStringer),  new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(TriStringerInkline),   new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(GrizzcoStringer),      new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Snipewriter5H),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Snipewriter5B),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Wellstring),           new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Blasters
            { typeof(Blaster),              new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(RapidBlaster),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(LunaBlaster),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(RangeBlaster),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(RapidBlasterDeco),     new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(ClashBlaster),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(ClashBlasterNeo),      new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SBlast92),             new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SBlast91),             new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Splatlings
            { typeof(MiniSplatling),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(HeavySplatling),       new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(ZinkMiniSplatling),    new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(RoyalHeavySplatling),  new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(HydraSplatling),       new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(CustomHydraSplatling), new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Sloshers
            { typeof(Slosher),              new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SlosherDeco),          new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SodaSlosher),          new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(TriSlosher),           new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(TriSlosherNouveau),    new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Bloblobber),           new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(BloblobberDeco),       new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Explosher),            new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },

            // Brushes
            { typeof(Inkbrush),             new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(InkbrushNouveau),      new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(Octobrush),            new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(OctobrushNouveau),     new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(PainBrush),            new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(PainBrushNouveau),     new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Splatana
            { typeof(SplatanaWiper),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SplatanaWiperDeco),    new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SplatanaStamper),      new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Brella
            { typeof(SplatBrella),          new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SorellaSplatBrella),   new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(UndercoverBrella),     new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(RecycleBrella),        new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(RecycleBrellaMk2),     new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Rollers
            { typeof(SplatRoller),          new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(KrakonSplatRoller),    new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(CarbonRoller),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(CarbonRollerDeco),     new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(DynamoRoller),         new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(GoldDynamoRoller),     new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

            // Kensa weapons
            { typeof(KensaSplattershotJr),      new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaDot52Gal),            new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaDynamoRoller),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaGloogaDualie),        new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaL3Nozzlenose),        new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaLunaBlaster),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaMiniSplatling),       new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaOctobrush),           new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaRapidBlaster),        new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaReefluxStringer),     new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaSplatanaStamper),     new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaSplatCharger),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaSplatDualie),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaSplatRoller),         new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(KensaUndercoverBrella),    new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },

            // Original
            { typeof(SlimeSplattershot),    new WeaponKit(subType: SubWeaponType.PointSensor, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(PainterDualie),        new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(DesertBrush),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(CoralStringer),        new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(CorruptionBrella),     new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SkySplatana),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Discount, bonusAmount: _defaultSubSaverBonus) },
            { typeof(SpookyBrush),          new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(IceStringer),          new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(MartianBrella),        new WeaponKit(subType: SubWeaponType.InkMine, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(StarfishedCharger),    new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(VortexDualie),         new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },
            { typeof(NebulaStringer),       new WeaponKit(subType: SubWeaponType.Torpedo, bonusType: SubWeaponBonusType.Damage, bonusAmount: 1f) },

            { typeof(SlimeSplattershotB),   new WeaponKit(subType: SubWeaponType.AngleShooter, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(CoralStringerB),       new WeaponKit(subType: SubWeaponType.Sprinkler, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(CorruptionBrellaB),    new WeaponKit(subType: SubWeaponType.SplatBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },
            { typeof(SkySplatanaB),          new WeaponKit(subType: SubWeaponType.BurstBomb, bonusType: SubWeaponBonusType.Damage, bonusAmount: _defaultSubDamageBonus) },

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

        public static float GetWeaponKitSubBonusAmount(Type weaponType)
        {
            if (!WeaponKitDictionary.ContainsKey(weaponType))
            {
                return 0f;
            }

            return WeaponKitDictionary[weaponType].BonusAmount;
        }
    }
}
