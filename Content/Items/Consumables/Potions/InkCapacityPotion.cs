using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Buffs;

namespace AchiSplatoon2.Content.Items.Consumables.Potions
{
    internal class InkCapacityPotion : BaseItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 20;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;

            Item.buffTime = 60 * 120;
            Item.buffType = ModContent.BuffType<InkCapacityBuff>();

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
        }
    }
}
