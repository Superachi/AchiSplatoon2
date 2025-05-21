using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using AchiSplatoon2.ExtensionMethods;

namespace AchiSplatoon2.Content.Items.Accessories
{
    internal class GoldHiHorses : BaseAccessory
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            WoomyMathHelper.FloatToPercentage(MoveAndAccelBonus)
            );

        public static float MoveAndAccelBonus => 0.4f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 28;
            Item.SetValuePreEvilBosses();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<AccessoryPlayer>();
                modPlayer.TryEquipAccessory<GoldHiHorses>();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ItemID.GoldBar, 5)
                .AddIngredient(ItemID.Silk, 5)
                .Register();

            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ItemID.PlatinumBar, 5)
                .AddIngredient(ItemID.Silk, 5)
                .Register();
        }
    }
}
