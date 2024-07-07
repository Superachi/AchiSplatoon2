using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Helpers.WeaponKits;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using System.Linq;
using Terraria.ID;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Emblems;

namespace AchiSplatoon2.Helpers
{
    internal class ShimmerItemList
    {
        public static Dictionary<int, int> ShimmerItemDictionary = new Dictionary<int, int> {
            // Regular Mains
            { ModContent.ItemType<Blaster>(),               ModContent.ItemType<RapidBlaster>() },
            { ModContent.ItemType<ReefluxStringer>(),       ModContent.ItemType<TriStringer>() },
            { ModContent.ItemType<SplooshOMatic>(),         ModContent.ItemType<SplashOMatic>() },

            // Subs
            { ModContent.ItemType<SplatBomb>(),             ModContent.ItemType<BurstBomb>() },
            { ModContent.ItemType<BurstBomb>(),             ModContent.ItemType<AngleShooter>() },
            { ModContent.ItemType<AngleShooter>(),          ModContent.ItemType<Sprinkler>() },
            { ModContent.ItemType<Sprinkler>(),             ModContent.ItemType<SplatBomb>() },

            // Specials
            { ModContent.ItemType<TrizookaSpecial>(),       ModContent.ItemType<KillerWail>() },
            { ModContent.ItemType<KillerWail>(),            ModContent.ItemType<UltraStamp>() },
            { ModContent.ItemType<UltraStamp>(),            ModContent.ItemType<TrizookaSpecial>() },

            // Cobalt <-> Palladium
            { ModContent.ItemType<ZinkMiniSplatling>(),     ModContent.ItemType<InkbrushNouveau>() },

            // Mythril <-> Orichalcum
            { ModContent.ItemType<Octobrush>(),             ModContent.ItemType<RoyalHeavySplatling>() },
            { ModContent.ItemType<ReefluxDecoStringer>(),   ModContent.ItemType<TriStringerInkline>() },
            { ModContent.ItemType<BambooMk2Charger>(),      ModContent.ItemType<ZFSplatCharger>() },
            { ModContent.ItemType<SlosherDeco>(),           ModContent.ItemType<LunaBlaster>() },

            // Adamantite <-> Titanium
            { ModContent.ItemType<RapidBlasterDeco>(),      ModContent.ItemType<RangeBlaster>() },

            // Accessories
            { ModContent.ItemType<SpecialChargeEmblem>(),   ModContent.ItemType<SpecialPowerEmblem>() },
            { ModContent.ItemType<SpecialPowerEmblem>(),    ModContent.ItemType<SubPowerEmblem>() },
            { ModContent.ItemType<SubPowerEmblem>(),        ModContent.ItemType<SpecialChargeEmblem>() },

            // Color chips
            { ModContent.ItemType<ColorChipRed>(),          ModContent.ItemType<ColorChipBlue>() },
            { ModContent.ItemType<ColorChipBlue>(),         ModContent.ItemType<ColorChipYellow>() },
            { ModContent.ItemType<ColorChipYellow>(),       ModContent.ItemType<ColorChipPurple>() },
            { ModContent.ItemType<ColorChipPurple>(),       ModContent.ItemType<ColorChipGreen>() },
            { ModContent.ItemType<ColorChipGreen>(),        ModContent.ItemType<ColorChipAqua>() },
            { ModContent.ItemType<ColorChipAqua>(),         ModContent.ItemType<ColorChipRed>() },

        };

        public static int? GetShimmerItemType(int itemType)
        {
            var dict = ShimmerItemDictionary;
            int? shimmerResult = null;

            if (dict.ContainsKey(itemType))
            {
                shimmerResult = dict[itemType];
            }
            else if (dict.ContainsValue(itemType))
            {
                shimmerResult = dict.FirstOrDefault(x => x.Value == itemType).Key;
            }

            return shimmerResult;
        }
    }
}
