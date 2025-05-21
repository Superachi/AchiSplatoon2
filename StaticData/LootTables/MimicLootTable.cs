using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Consumables.LootBags;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using System.Collections.Generic;
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
                    npcId: NPCID.BigMimicHallow),

                new LootTableIndex(
                    itemId: ModContent.ItemType<CorruptLootBag>(),
                    npcId: NPCID.BigMimicCorruption),

                new LootTableIndex(
                    itemId: ModContent.ItemType<CrimsonLootBag>(),
                    npcId: NPCID.BigMimicCrimson),
            };

            return mimicLootList;
        }
    }
}
