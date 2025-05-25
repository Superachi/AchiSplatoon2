using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Consumables.LootBags;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.StaticData.LootTables
{
    internal class MimicLootTable : ILootTable
    {
        public List<LootTableIndex> Indices()
        {
            List<LootTableIndex> mimicLootList = new()
            {
                // Small mimics
                new LootTableIndex(
                    itemId: ModContent.ItemType<SheldonLicenseSilver>(),
                    chanceDenominator: 2,
                    npcId: NPCID.Mimic),

                new LootTableIndex(
                    itemId: ModContent.ItemType<MainSaverEmblem>(),
                    chanceDenominator: 5,
                    npcId: NPCID.Mimic),

                new LootTableIndex(
                    itemId: ModContent.ItemType<SubSaverEmblem>(),
                    chanceDenominator: 5,
                    npcId: NPCID.Mimic),

                // Big mimics
                new LootTableIndex(
                    itemId: ModContent.ItemType<HallowedLootBag>(),
                    chanceDenominator: 10,
                    npcId: NPCID.BigMimicHallow),

                new LootTableIndex(
                    itemId: ModContent.ItemType<CorruptLootBag>(),
                    chanceDenominator: 10,
                    npcId: NPCID.BigMimicCorruption),

                new LootTableIndex(
                    itemId: ModContent.ItemType<CrimsonLootBag>(),
                    chanceDenominator: 10,
                    npcId: NPCID.BigMimicCrimson),
            };

            // Last Ditch Effort Emblem
            var bigMimics = new Dictionary<int, int>()
            {
                { NPCID.BigMimicHallow, -1 },
                { NPCID.BigMimicCorruption, -1 },
                { NPCID.BigMimicCrimson, -1 },
            };

            var emblemDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<LastDitchEffortEmblem>(),
                npcIdAndBagId: bigMimics,
                chanceDenominator: 5);

            var licenseDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<SheldonLicenseSilver>(),
                npcIdAndBagId: bigMimics,
                chanceDenominator: 1);

            mimicLootList = mimicLootList
                .Concat(emblemDrops).ToList()
                .Concat(licenseDrops).ToList();

            return mimicLootList;
        }
    }
}
