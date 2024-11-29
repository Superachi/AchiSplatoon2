using AchiSplatoon2.Content.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Consumables.Potions
{
    internal class InkRegenerationPotion : BaseItem
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
            Item.buffType = ModContent.BuffType<InkRegenerationBuff>();

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.Coral, 1)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddIngredient(ItemID.Fireblossom, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
