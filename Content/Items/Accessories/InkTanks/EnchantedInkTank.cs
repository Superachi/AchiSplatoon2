using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class EnchantedInkTank : InkTank
    {
        public override int CapacityBonus => 20;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();
                accessoryPlayer.hasThermalInkTank = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ModContent.ItemType<InkTank>())
                .AddIngredient(ItemID.FallenStar, 10)
                .Register();
        }
    }
}
