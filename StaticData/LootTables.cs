using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs;
using AchiSplatoon2.Content.Items.Consumables.LootBags;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using System.Collections.Generic;
using System.Linq;
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

        public static List<LootTableIndex> FindIndicesById(int modItemId)
        {
            return List.Where(index => index.itemIds.Contains(modItemId)).ToList();
        }

        public static List<LootTableIndex> BossLoot()
        {
            List<LootTableIndex> bossLootList = new();

            // Licenses
            var basicLicenseDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<SheldonLicense>(),
                npcIdAndBagId: BossBagAssociations.PreHardmodeBosses(),
                maximumDropped: 3);

            foreach (var index in basicLicenseDrops)
            {
                bossLootList.Add(index);
            }

            var silverLicenseDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<SheldonLicenseSilver>(),
                npcIdAndBagId: BossBagAssociations.BossesDroppingSilverLicense(),
                maximumDropped: 3);

            foreach (var index in silverLicenseDrops) bossLootList.Add(index);

            var goldLicenseDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<SheldonLicenseGold>(),
                npcIdAndBagId: BossBagAssociations.BossesDroppingGoldLicense(),
                maximumDropped: 3);

            foreach (var index in goldLicenseDrops) bossLootList.Add(index);

            // Color chips
            var colorChipDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<ColorChipEmpty>(),
                npcIdAndBagId: BossBagAssociations.PostEvilBosses(),
                maximumDropped: 3);

            foreach (var index in colorChipDrops) bossLootList.Add(index);

            // Unique drops - PRE-HARDMODE
            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<SlimeSplattershot>(),
                npcId: NPCID.KingSlime,
                treasureBagId: ItemID.KingSlimeBossBag)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<OrderEmblem>(),
                npcId: NPCID.EyeofCthulhu,
                treasureBagId: ItemID.EyeOfCthulhuBossBag,
                chanceDenominator: 3)
            );

            var bombRushDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<BombRush>(),
                npcIdAndBagId: BossBagAssociations.EvilBosses());

            foreach (var index in bombRushDrops) bossLootList.Add(index);

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<DroneDiscA>(),
                npcId: NPCID.QueenBee,
                treasureBagId: ItemID.QueenBeeBossBag)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<Trizooka>(),
                npcId: NPCID.SkeletronHead,
                treasureBagId: ItemID.SkeletronBossBag)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<Tacticooler>(),
                npcId: NPCID.WallofFlesh,
                treasureBagId: ItemID.WallOfFleshBossBag)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemIdOptions: [ModContent.ItemType<SpecialChargeEmblem>(), ModContent.ItemType<SpecialPowerEmblem>(), ModContent.ItemType<SubPowerEmblem>()],
                npcId: NPCID.WallofFlesh,
                treasureBagId: ItemID.WallOfFleshBossBag)
            );

            // Unique drops - HARDMODE
            var droneDiscBDrops = LootTableIndex.CreateLootTableIndicesSingleItem(
                itemId: ModContent.ItemType<DroneDiscB>(),
                npcIdAndBagId: BossBagAssociations.MechBosses(),
                chanceDenominator: 5);

            foreach (var index in droneDiscBDrops) bossLootList.Add(index);

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<GolemSplatana>(),
                npcId: NPCID.Golem,
                treasureBagId: ItemID.GolemBossBag)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<StarfishedCharger>(),
                npcId: NPCID.HallowBoss,
                treasureBagId: ItemID.FairyQueenBossBag,
                chanceDenominator: 2)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<EmpressInkTank>(),
                npcId: NPCID.HallowBoss,
                treasureBagId: ItemID.FairyQueenBossBag,
                chanceDenominator: 2)
            );

            bossLootList.Add(
                new LootTableIndex(
                itemId: ModContent.ItemType<EelSplatana>(),
                npcId: NPCID.DukeFishron,
                treasureBagId: ItemID.FishronBossBag)
            );

            return bossLootList;
        }

        public static List<LootTableIndex> MimicLoot()
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
