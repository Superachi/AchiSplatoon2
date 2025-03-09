using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal class ShimmerItemList
    {
        public static Dictionary<int, int> ShimmerItemDictionary = new Dictionary<int, int> {
            // Regular Mains
            { ModContent.ItemType<SplooshOMatic>(),         ModContent.ItemType<SplashOMatic>() },

            // Cobalt <-> Palladium
            { ModContent.ItemType<ZinkMiniSplatling>(),     ModContent.ItemType<InkbrushNouveau>() },
            { ModContent.ItemType<KrakonSplatRoller>(),     ModContent.ItemType<CarbonRollerDeco>() },

            // Mythril <-> Orichalcum
            { ModContent.ItemType<DarkTetraDualie>(),       ModContent.ItemType<DappleDualie>() },
            { ModContent.ItemType<Octobrush>(),             ModContent.ItemType<RoyalHeavySplatling>() },
            { ModContent.ItemType<ReefluxDecoStringer>(),   ModContent.ItemType<TriStringerInkline>() },
            { ModContent.ItemType<BambooMk2Charger>(),      ModContent.ItemType<ZFSplatCharger>() },
            { ModContent.ItemType<SlosherDeco>(),           ModContent.ItemType<LunaBlaster>() },

            // Adamantite <-> Titanium
            { ModContent.ItemType<RapidBlasterDeco>(),      ModContent.ItemType<RangeBlaster>() },
            { ModContent.ItemType<UndercoverBrella>(),      ModContent.ItemType<SorellaSplatBrella>() },
            { ModContent.ItemType<GloogaDualie>(),          ModContent.ItemType<DouserDualie>() },
            { ModContent.ItemType<H3Nozzlenose>(),          ModContent.ItemType<L3Nozzlenose>() },

            // Reskins
            { ModContent.ItemType<ClassicSplattershotS1>(), ModContent.ItemType<ClassicSplattershotS2>() },

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

            // Order weapons
            { ModContent.ItemType<ShimmerOrderShot>(),      ModContent.ItemType<OrderShot>() },
            { ModContent.ItemType<ShimmerOrderBlaster>(),   ModContent.ItemType<OrderBlaster>() },
            { ModContent.ItemType<ShimmerOrderStringer>(),  ModContent.ItemType<OrderStringer>() },
            { ModContent.ItemType<ShimmerOrderBrella>(),    ModContent.ItemType<OrderBrella>() },
            { ModContent.ItemType<ShimmerOrderBrush>(),     ModContent.ItemType<OrderBrush>() },
            { ModContent.ItemType<ShimmerOrderCharger>(),   ModContent.ItemType<OrderCharger>() },
            { ModContent.ItemType<ShimmerOrderDualie>(),    ModContent.ItemType<OrderDualie>() },
            { ModContent.ItemType<ShimmerOrderRoller>(),    ModContent.ItemType<OrderRoller>() },
            { ModContent.ItemType<ShimmerOrderSlosher>(),   ModContent.ItemType<OrderSlosher>() },
            { ModContent.ItemType<ShimmerOrderSplatana>(),  ModContent.ItemType<OrderSplatana>() },
            { ModContent.ItemType<ShimmerOrderSplatling>(), ModContent.ItemType<OrderSplatling>() },
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
