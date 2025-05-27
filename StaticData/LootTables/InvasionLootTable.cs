using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.StaticData.LootTables
{
    internal class InvasionLootTable : ILootTable
    {
        public List<LootTableIndex> Indices()
        {
            var list = new List<LootTableIndex>()
            {
                // Pumpkin moon
                new LootTableIndex(
                    itemIdOptions:
                    [
                        ModContent.ItemType<GrimRangeBlaster>(),
                        ModContent.ItemType<CustomDouserDualie>(),
                    ],
                    npcId: NPCID.MourningWood,
                    chanceDenominator: 10),

                new LootTableIndex(
                    itemId: ModContent.ItemType<SpookyBrush>(),
                    npcId: NPCID.Pumpking,
                    chanceDenominator: 3),

                // Frost moon
                new LootTableIndex(
                    itemIdOptions:
                    [
                        ModContent.ItemType<Squeezer>(),
                        ModContent.ItemType<LightTetraDualie>(),
                    ],
                    npcId: NPCID.Everscream,
                    chanceDenominator: 10),

                new LootTableIndex(
                    itemIdOptions:
                    [
                        ModContent.ItemType<Explosher>(),
                        ModContent.ItemType<CustomHydraSplatling>(),
                    ],
                    npcId: NPCID.SantaNK1,
                    chanceDenominator: 10),

                new LootTableIndex(
                    itemId: ModContent.ItemType<IceStringer>(),
                    npcId: NPCID.IceQueen,
                    chanceDenominator: 2),

                // Solar eclipse
                new LootTableIndex(
                    itemId: ModContent.ItemType<LaserAddon>(),
                    npcId: NPCID.Eyezor,
                    chanceDenominator: 4),

                new LootTableIndex(
                    itemId: ModContent.ItemType<CustomWellstring>(),
                    npcId: NPCID.Nailhead,
                    chanceDenominator: 4),

                new LootTableIndex(
                    itemId: ModContent.ItemType<BloblobberDeco>(),
                    npcId: NPCID.DrManFly,
                    chanceDenominator: 4),

                new LootTableIndex(
                    itemId: ModContent.ItemType<SuperPaletteRightPart>(),
                    npcId: NPCID.Mothron,
                    chanceDenominator: 3),

                // Pirates
                new LootTableIndex(
                    itemId: ModContent.ItemType<SuperPaletteLeftPart>(),
                    npcId: NPCID.PirateShip,
                    chanceDenominator: 3),

                // Martians
                new LootTableIndex(
                    itemIdOptions:
                    [
                        ModContent.ItemType<MartianBrella>(),
                        ModContent.ItemType<SuperPaletteMiddlePart>(),
                    ],
                    npcId: NPCID.MartianSaucerCore,
                    chanceDenominator: 3),
            };

            return list;
        }
    }
}
