using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.StaticData;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.GlobalItems
{
    internal class GlobalTreasureBags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            foreach (var loot in LootTables.List)
            {
                if (item.type == loot.TreasureBagId)
                {
                    loot.RegisterBagDrop(itemLoot);
                }
            }

            // License drops
            switch (item.type)
            {
                //case ItemID.KingSlimeBossBag:
                //case ItemID.EyeOfCthulhuBossBag:
                //case ItemID.EaterOfWorldsBossBag:
                //case ItemID.BrainOfCthulhuBossBag:
                //case ItemID.DeerclopsBossBag:
                //case ItemID.QueenBeeBossBag:
                //case ItemID.SkeletronBossBag:
                //    itemLoot.Add(
                //        ItemDropRule.Common(ModContent.ItemType<SheldonLicense>(), minimumDropped: 3, maximumDropped: 3)
                //    );
                //    break;

                case ItemID.WallOfFleshBossBag:
                    itemLoot.Add(
                        ItemDropRule.Common(ModContent.ItemType<SheldonLicenseSilver>(), minimumDropped: 1)
                    );
                    break;

                case ItemID.TwinsBossBag:
                case ItemID.DestroyerBossBag:
                case ItemID.SkeletronPrimeBossBag:
                case ItemID.PlanteraBossBag:
                    itemLoot.Add(
                        ItemDropRule.Common(ModContent.ItemType<SheldonLicenseSilver>(), minimumDropped: 3, maximumDropped: 3)
                    );
                    break;

                case ItemID.FishronBossBag:
                case ItemID.FairyQueenBossBag:
                case ItemID.GolemBossBag:
                case ItemID.CultistBossBag:
                    itemLoot.Add(
                        ItemDropRule.Common(ModContent.ItemType<SheldonLicenseGold>(), minimumDropped: 3, maximumDropped: 3)
                    );
                    break;
            }

            // Unique-per-boss drops
            if (item.type == ItemID.KingSlimeBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<SlimeSplattershot>())
                );
            }

            if (item.type == ItemID.EyeOfCthulhuBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<OrderEmblem>(), chanceDenominator: 3)
                );
            }

            if (item.type == ItemID.EaterOfWorldsBossBag || item.type == ItemID.BrainOfCthulhuBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<BombRush>())
                );
            }

            if (item.type == ItemID.QueenBeeBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<DroneDiscA>())
                );

                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<Tacticooler>())
                );
            }

            if (item.type == ItemID.SkeletronBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<Trizooka>())
                );
            }

            if (item.type == ItemID.WallOfFleshBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.OneFromOptions(
                        1,
                        ModContent.ItemType<SpecialChargeEmblem>(),
                        ModContent.ItemType<SpecialPowerEmblem>(),
                        ModContent.ItemType<SubPowerEmblem>()
                    )
                );
            }

            if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<DroneDiscC>())
                );
            }

            if (item.type == ItemID.GolemBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<GolemSplatana>())
                );
            }

            if (item.type == ItemID.FairyQueenBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<StarfishedCharger>(), chanceDenominator: 2)
                );

                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<EmpressInkTank>(), chanceDenominator: 2)
                );
            }

            if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<EelSplatana>())
                );
            }

            if (item.type == ItemID.GolemBossBag)
            {
            }

            // Empty color chip drops (specially post-evil boss, as thats when you can start crafting with meteorite)
            switch (item.type)
            {
                case ItemID.EaterOfWorldsBossBag:
                case ItemID.BrainOfCthulhuBossBag:
                case ItemID.DeerclopsBossBag:
                case ItemID.QueenBeeBossBag:
                case ItemID.SkeletronBossBag:
                case ItemID.WallOfFleshBossBag:
                    itemLoot.Add(
                        ItemDropRule.Common(ModContent.ItemType<ColorChipEmpty>(), minimumDropped: 1, maximumDropped: 3)
                    );
                    break;
            }
        }
    }
}
