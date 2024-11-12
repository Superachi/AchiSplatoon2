using AchiSplatoon2.Content.Prefixes;
using AchiSplatoon2.Content.Prefixes.DualiePrefixes;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes;
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
        var value = WeaponPrefixDictionary.FirstOrDefault(x => x.Value == id).Key;
        return value != null ? Activator.CreateInstance(value) as BaseWeaponPrefix : null;
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
            ModContent.PrefixType<BacklinePrefix>(),
            ModContent.PrefixType<ChaoticPrefix>(),
            ModContent.PrefixType<DeepCutPrefix>(),

            ModContent.PrefixType<FreshPrefix>(),
            ModContent.PrefixType<HeavyDutyPrefix>(),
            ModContent.PrefixType<PiercingPrefix>(),
            ModContent.PrefixType<SkilledPrefix>(),

            ModContent.PrefixType<SwiftPrefix>(),
            ModContent.PrefixType<TurboPrefix>()
        };

        return prefixes;
    }

    public static List<int> ListDualiePrefixes()
    {
        var prefixes = ListGenericPrefixes();
        prefixes.Add(ModContent.PrefixType<SlipperyPrefix>());
        prefixes.Add(ModContent.PrefixType<PrincessPrefix>());
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
            // Dualies
            { typeof(SlipperyPrefix), ModContent.PrefixType<SlipperyPrefix>() },
            { typeof(PrincessPrefix), ModContent.PrefixType<PrincessPrefix>() },

            // Generic
            { typeof(AmpedUpPrefix), ModContent.PrefixType<AmpedUpPrefix>() },
            { typeof(BacklinePrefix), ModContent.PrefixType<BacklinePrefix>() },
            { typeof(ChaoticPrefix), ModContent.PrefixType<ChaoticPrefix>() },
            { typeof(DeepCutPrefix), ModContent.PrefixType<DeepCutPrefix>() },

            { typeof(FreshPrefix), ModContent.PrefixType<FreshPrefix>() },
            { typeof(HeavyDutyPrefix), ModContent.PrefixType<HeavyDutyPrefix>() },
            { typeof(PiercingPrefix), ModContent.PrefixType<PiercingPrefix>() },
            { typeof(SkilledPrefix), ModContent.PrefixType<SkilledPrefix>() },

            { typeof(SwiftPrefix), ModContent.PrefixType<SwiftPrefix>() },
            { typeof(TurboPrefix), ModContent.PrefixType<TurboPrefix>() },
        };

        return dict;
    }
}
