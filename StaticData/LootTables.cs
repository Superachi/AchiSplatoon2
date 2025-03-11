using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.StaticData
{
    internal class LootTables
    {
        public static List<LootTableIndex> List { get; private set; }
        public static void UpdateStaticData()
        {
            List = AllLootIndices();
        }

        public static List<LootTableIndex> AllLootIndices()
        {
            var list = new List<LootTableIndex>();

            foreach (var loot in BossLoot()) list.Add(loot);
            foreach (var loot in MimicLoot()) list.Add(loot);

            return list;
        }

        public static List<LootTableIndex> BossLoot()
        {
            List<LootTableIndex> bossLootList = new();

            var bossesDroppingSheldonLicense = new Dictionary<int, int>
            {
                { NPCID.KingSlime, ItemID.KingSlimeBossBag },
                { NPCID.EyeofCthulhu, ItemID.EyeOfCthulhuBossBag },
                { NPCID.EaterofWorldsHead, ItemID.EaterOfWorldsBossBag },
                { NPCID.BrainofCthulhu, ItemID.BrainOfCthulhuBossBag },
                { NPCID.Deerclops, ItemID.DeerclopsBossBag },
                { NPCID.QueenBee, ItemID.QueenBeeBossBag },
                { NPCID.SkeletronHead, ItemID.SkeletronBossBag }
            };

            var indices = LootTableIndex.CreateLootTableIndices(modItemId: ModContent.ItemType<SheldonLicense>(), bossesDroppingSheldonLicense, 1, 3, 1);
            foreach (var index in indices) bossLootList.Add(index);

            return bossLootList;
        }

        public static List<LootTableIndex> MimicLoot()
        {
            List<LootTableIndex> mimicLootList = new();

            mimicLootList.Add(
                new LootTableIndex(
                    modItemId: ModContent.ItemType<SheldonLicenseSilver>(),
                    chanceDenominator: 2,
                    npcId: NPCID.Mimic)
            );

            mimicLootList.Add(
                new LootTableIndex(
                    modItemId: ModContent.ItemType<MainSaverEmblem>(),
                    chanceDenominator: 5,
                    npcId: NPCID.Mimic)
            );

            mimicLootList.Add(
                new LootTableIndex(
                    modItemId: ModContent.ItemType<SubSaverEmblem>(),
                    chanceDenominator: 5,
                    npcId: NPCID.Mimic)
            );

            return mimicLootList;
        }
    }
}
