using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using Terraria.GameContent.ItemDropRules;
using AchiSplatoon2.Content.Items.Weapons.Shooters;

namespace AchiSplatoon2.Content.GlobalItems
{
    internal class GlobalTreasureBags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.KingSlimeBossBag)
            {
                itemLoot.Add(
                    ItemDropRule.Common(ModContent.ItemType<SlimeSplattershot>(), chanceDenominator: 5)
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

                itemLoot.Add(
                    ItemDropRule.OneFromOptions(
                        5,
                        ModContent.ItemType<SplooshOMatic>(),
                        ModContent.ItemType<SplashOMatic>()
                    )
                );
            }
        }
    }
}
