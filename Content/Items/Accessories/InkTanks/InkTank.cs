using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    [ItemCategory("Ink tanks", "InkTanks")]
    internal class InkTank : BaseAccessory
    {
        public virtual int CapacityBonus => 20;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CapacityBonus);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 32;

            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
                inkTankPlayer.InkAmountMaxBonus += CapacityBonus;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ItemID.Glass, 10)
                .AddIngredient(ModContent.ItemType<InkDroplet>(), 10)
                .Register();
        }
    }
}
