using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Consumables.Potions
{
    internal class InkCapacityPotion : BaseItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(InkCapacityBuff.InkCapacityBonus);
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 20;
            Item.useTime = Item.useAnimation;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;

            Item.buffTime = 60 * 180;
            Item.buffType = ModContent.BuffType<InkCapacityBuff>();

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 3)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddIngredient(ItemID.GlowingMushroom, 5)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
