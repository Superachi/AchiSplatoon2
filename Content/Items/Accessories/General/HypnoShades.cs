using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AchiSplatoon2.Content.Items.Accessories.General
{
    internal class HypnoShades : BaseAccessory
    {
        public static float BombUseTimeMult => 0.7f;
        public static float BombInkCostMult => 0.5f;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            (int)((1f - BombInkCostMult) * 100)
            );

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 24;
            Item.SetValuePostMech();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                player.GetModPlayer<AccessoryPlayer>().TryEquipAccessory<HypnoShades>();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Sunglasses, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .Register();
        }
    }
}
