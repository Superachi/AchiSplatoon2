using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
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

            // Hallowed mimic drops
            mimicLootList.Add(
                new LootTableIndex(
                itemIdOptions: [ModContent.ItemType<FreshQuiver>(), ModContent.ItemType<SquidClipOns>(), ModContent.ItemType<PinkSponge>(), ModContent.ItemType<CrayonBox>()],
                npcId: NPCID.BigMimicHallow)
            );

            // Corrupt/Crimson mimic drops
            mimicLootList.Add(
                new LootTableIndex(
                itemIdOptions: [ModContent.ItemType<TentacularOcular>(), ModContent.ItemType<AdamantiteCoil>(), ModContent.ItemType<FieryPaintCan>(), ModContent.ItemType<MarinatedNecklace>()],
                npcId: NPCID.BigMimicCorruption)
            );

            mimicLootList.Add(
                new LootTableIndex(
                itemIdOptions: [ModContent.ItemType<TentacularOcular>(), ModContent.ItemType<AdamantiteCoil>(), ModContent.ItemType<FieryPaintCan>(), ModContent.ItemType<MarinatedNecklace>()],
                npcId: NPCID.BigMimicCrimson)
            );

            return mimicLootList;
        }
    }
}
