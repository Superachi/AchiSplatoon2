using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class ThermalInkTank : BaseWeaponBoosterAccessory
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 30;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasThermalInkTank = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ItemID.Glass, 5)
                .AddIngredient(ItemID.Glowstick, 25)
                .Register();
        }
    }
}
