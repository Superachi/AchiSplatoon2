using AchiSplatoon2.Content.Items.Accessories.ColorChips;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
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
            if (item.type == ItemID.KingSlimeBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<SlimeSplattershot>())
                );
            }

            if (item.type == ItemID.SkeletronBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<DroneDiscA>())
                );
            }

            if (item.type == ItemID.WallOfFleshBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<DroneDiscB>())
                );

                itemLoot.Add(
                    ItemDropRule.OneFromOptions(
                        1,
                        ModContent.ItemType<SpecialChargeEmblem>(),
                        ModContent.ItemType<SpecialPowerEmblem>(),
                        ModContent.ItemType<SubPowerEmblem>()
                    )
                );

                itemLoot.Add(
                    ItemDropRule.OneFromOptions(
                        5,
                        ModContent.ItemType<SplooshOMatic>(),
                        ModContent.ItemType<SplashOMatic>()
                    )
                );
            }

            if (item.type == ItemID.PlanteraBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<DroneDiscC>())
                );
            }

            if (item.type == ItemID.FairyQueenBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<StarfishedCharger>())
                );
            }

            if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<EelSplatanaWeapon>())
                );
            }

            if (item.type == ItemID.GolemBossBag)
            {
            }

            switch (item.type)
            {
                case ItemID.KingSlimeBossBag:
                case ItemID.EyeOfCthulhuBossBag:
                case ItemID.EaterOfWorldsBossBag:
                case ItemID.BrainOfCthulhuBossBag:
                case ItemID.SkeletronBossBag:
                case ItemID.QueenBeeBossBag:
                case ItemID.DeerclopsBossBag:
                case ItemID.WallOfFleshBossBag:
                    itemLoot.Add(
                        ItemDropRule.Common(ModContent.ItemType<ColorChipEmpty>())
                    );
                    break;
            }
        }
    }
}
