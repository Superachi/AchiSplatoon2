using AchiSplatoon2.Content.Prefixes;
using AchiSplatoon2.Content.Prefixes.BlasterPrefixes;
using AchiSplatoon2.Content.Prefixes.BrellaPrefixes;
using AchiSplatoon2.Content.Prefixes.BrushPrefixes;
using AchiSplatoon2.Content.Prefixes.ChargerPrefixes;
using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using AchiSplatoon2.Content.Prefixes.DualiePrefixes;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes.InkCostPrefixes;
using AchiSplatoon2.Content.Prefixes.SlosherPrefixes;
using AchiSplatoon2.Content.Prefixes.SplatlingPrefixes;
using AchiSplatoon2.Content.Prefixes.StringerPrefixes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers;

internal class PrefixHelper : ModSystem
{
    public static BaseWeaponPrefix? GetWeaponPrefixById(int id)
    {
        if (id == 0) return null; // No prefix
        if (id == -1) return null; // Projectile spawned without the prefix being defined

        var value = WeaponPrefixDictionary.FirstOrDefault(x => x.Value == id).Key;
        var result = value != null ? Activator.CreateInstance(value) as BaseWeaponPrefix : null;

        if (result == null)
        {
            // DebugHelper.PrintWarning($"Called {nameof(GetWeaponPrefixById)} with prefix id {id}, but the result was null!");
        }

        return result;
    }

    public override void PostSetupContent()
    {
        WeaponPrefixDictionary = GenerateWeaponPrefixDictionary();
    }

    private static Dictionary<Type, int> WeaponPrefixDictionary { get; set; } = [];

    public static List<int> ListGenericPrefixes()
    {
        List<int> prefixes = new() {
            ModContent.PrefixType<AmpedUpPrefix>(),
            ModContent.PrefixType<RangedPrefix>(),
            ModContent.PrefixType<DeepCutPrefix>(),
            ModContent.PrefixType<PiercingPrefix>(),
            ModContent.PrefixType<SkilledPrefix>(),

            ModContent.PrefixType<SavouryPrefix>(),
            ModContent.PrefixType<HungryPrefix>(),
            ModContent.PrefixType<CheapPrefix>(),

            ModContent.PrefixType<DryPrefix>(),
            ModContent.PrefixType<TastelessPrefix>()
        };

        return prefixes;
    }

    public static List<int> ListShooterPrefixes()
    {
        var prefixes = ListGenericPrefixes();

        prefixes.Add(ModContent.PrefixType<ChaoticPrefix>());
        prefixes.Add(ModContent.PrefixType<FreshPrefix>());
        prefixes.Add(ModContent.PrefixType<HeavyDutyPrefix>());
        prefixes.Add(ModContent.PrefixType<SwiftPrefix>());
        prefixes.Add(ModContent.PrefixType<TurboPrefix>());

        return prefixes;
    }

    public static List<int> ListDualiePrefixes()
    {
        var prefixes = ListShooterPrefixes();

        prefixes.Add(ModContent.PrefixType<EvasivePrefix>());
        prefixes.Add(ModContent.PrefixType<PrincessPrefix>());
        prefixes.Add(ModContent.PrefixType<DivingPrefix>());

        return prefixes;
    }

    public static List<int> ListChargeWeaponsPrefixes()
    {
        var prefixes = ListGenericPrefixes();

        prefixes.Add(ModContent.PrefixType<QuickPrefix>());
        prefixes.Add(ModContent.PrefixType<BacklinePrefix>());
        prefixes.Add(ModContent.PrefixType<InstantPrefix>());

        return prefixes;
    }

    public static List<int> ListChargerPrefixes()
    {
        var prefixes = ListChargeWeaponsPrefixes();

        prefixes.Add(ModContent.PrefixType<GambitPrefix>());
        prefixes.Add(ModContent.PrefixType<TwinPrefix>());
        // prefixes.Add(ModContent.PrefixType<ExplosivePrefix>());

        return prefixes;
    }

    public static List<int> ListStringerPrefixes()
    {
        var prefixes = ListChargeWeaponsPrefixes();

        prefixes.Add(ModContent.PrefixType<CompactPrefix>());
        prefixes.Add(ModContent.PrefixType<WidePrefix>());

        return prefixes;
    }

    public static List<int> ListBlasterPrefixes()
    {
        var prefixes = ListShooterPrefixes();

        prefixes.Add(ModContent.PrefixType<BigBangPrefix>());
        prefixes.Add(ModContent.PrefixType<ConcentratedPrefix>());
        prefixes.Add(ModContent.PrefixType<ShortFusedPrefix>());

        return prefixes;
    }

    public static List<int> ListSlosherPrefixes()
    {
        var prefixes = ListShooterPrefixes();

        prefixes.Add(ModContent.PrefixType<ResonantPrefix>());
        prefixes.Add(ModContent.PrefixType<OversizedPrefix>());

        return prefixes;
    }

    public static List<int> ListSplatlingPrefixes()
    {
        var prefixes = ListChargeWeaponsPrefixes();

        prefixes.Add(ModContent.PrefixType<LoadedPrefix>());
        prefixes.Add(ModContent.PrefixType<CuratedPrefix>());
        prefixes.Add(ModContent.PrefixType<ChaoticSplatlingPrefix>());

        return prefixes;
    }

    public static List<int> ListBrushPrefixes()
    {
        var prefixes = ListShooterPrefixes();

        prefixes.Add(ModContent.PrefixType<SlipperyPrefix>());
        prefixes.Add(ModContent.PrefixType<SteadfastPrefix>());
        prefixes.Add(ModContent.PrefixType<SeepingPrefix>());

        return prefixes;
    }

    public static List<int> ListBrellaPrefixes()
    {
        var prefixes = ListShooterPrefixes();

        prefixes.Add(ModContent.PrefixType<BurstingPrefix>());
        prefixes.Add(ModContent.PrefixType<CrispyPrefix>());
        prefixes.Add(ModContent.PrefixType<ShelledPrefix>());
        prefixes.Add(ModContent.PrefixType<SturdyPrefix>());

        return prefixes;
    }

    public static bool IsPrefixOfType<T>(Item item)
    where T : BaseItemPrefix
    {
        var prefix = GetWeaponPrefixById(item.prefix);
        return prefix is T;
    }

    public static bool IsPrefixOfType<T>(BaseItemPrefix prefix)
    where T : BaseItemPrefix
    {
        return prefix is T;
    }

    public static bool IsPrefixOfType<T>(int prefixId)
    where T : BaseItemPrefix
    {
        var prefix = GetWeaponPrefixById(prefixId);
        return prefix is T;
    }

    private Dictionary<Type, int> GenerateWeaponPrefixDictionary()
    {
        Dictionary<Type, int> dict = new()
        {
            // Generic
            { typeof(AmpedUpPrefix), ModContent.PrefixType<AmpedUpPrefix>() },
            { typeof(RangedPrefix), ModContent.PrefixType<RangedPrefix>() },
            { typeof(ChaoticPrefix), ModContent.PrefixType<ChaoticPrefix>() },
            { typeof(DeepCutPrefix), ModContent.PrefixType<DeepCutPrefix>() },

            { typeof(FreshPrefix), ModContent.PrefixType<FreshPrefix>() },
            { typeof(HeavyDutyPrefix), ModContent.PrefixType<HeavyDutyPrefix>() },
            { typeof(PiercingPrefix), ModContent.PrefixType<PiercingPrefix>() },
            { typeof(SkilledPrefix), ModContent.PrefixType<SkilledPrefix>() },

            { typeof(SavouryPrefix), ModContent.PrefixType<SavouryPrefix>() },
            { typeof(HungryPrefix), ModContent.PrefixType<HungryPrefix>() },
            { typeof(CheapPrefix), ModContent.PrefixType<CheapPrefix>() },

            { typeof(SwiftPrefix), ModContent.PrefixType<SwiftPrefix>() },
            { typeof(TurboPrefix), ModContent.PrefixType<TurboPrefix>() },

            { typeof(DryPrefix), ModContent.PrefixType<DryPrefix>() },
            { typeof(TastelessPrefix), ModContent.PrefixType<TastelessPrefix>() },

            // Dualies
            { typeof(EvasivePrefix), ModContent.PrefixType<EvasivePrefix>() },
            { typeof(PrincessPrefix), ModContent.PrefixType<PrincessPrefix>() },
            { typeof(DivingPrefix), ModContent.PrefixType<DivingPrefix>() },

            // Charge weapons
            { typeof(QuickPrefix), ModContent.PrefixType<QuickPrefix>() },
            { typeof(BacklinePrefix), ModContent.PrefixType<BacklinePrefix>() },
            { typeof(InstantPrefix), ModContent.PrefixType<InstantPrefix>() },

            // Chargers
            { typeof(GambitPrefix), ModContent.PrefixType<GambitPrefix>() },
            { typeof(TwinPrefix), ModContent.PrefixType<TwinPrefix>() },
            { typeof(ExplosivePrefix), ModContent.PrefixType<ExplosivePrefix>() },

            // Stringers
            { typeof(CompactPrefix), ModContent.PrefixType<CompactPrefix>() },
            { typeof(WidePrefix), ModContent.PrefixType<WidePrefix>() },

            // Blasters
            { typeof(ConcentratedPrefix), ModContent.PrefixType<ConcentratedPrefix>() },
            { typeof(BigBangPrefix), ModContent.PrefixType<BigBangPrefix>() },
            { typeof(ShortFusedPrefix), ModContent.PrefixType<ShortFusedPrefix>() },

            // Sloshers
            { typeof(ResonantPrefix), ModContent.PrefixType<ResonantPrefix>() },
            { typeof(OversizedPrefix), ModContent.PrefixType<OversizedPrefix>() },

            // Splatlings
            { typeof(LoadedPrefix), ModContent.PrefixType<LoadedPrefix>() },
            { typeof(CuratedPrefix), ModContent.PrefixType<CuratedPrefix>() },
            { typeof(ChaoticSplatlingPrefix), ModContent.PrefixType<ChaoticSplatlingPrefix>() },

            // Brushes
            { typeof(SlipperyPrefix), ModContent.PrefixType<SlipperyPrefix>() },
            { typeof(SteadfastPrefix), ModContent.PrefixType<SteadfastPrefix>() },
            { typeof(SeepingPrefix), ModContent.PrefixType<SeepingPrefix>() },

            // Brellas
            { typeof(BurstingPrefix), ModContent.PrefixType<BurstingPrefix>() },
            { typeof(CrispyPrefix), ModContent.PrefixType<CrispyPrefix>() },
            { typeof(ShelledPrefix), ModContent.PrefixType<ShelledPrefix>() },
            { typeof(SturdyPrefix), ModContent.PrefixType<SturdyPrefix>() },

        };

        return dict;
    }
}
