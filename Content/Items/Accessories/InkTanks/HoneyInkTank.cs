using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class HoneyInkTank : InkTank
    {
        public override int CapacityBonus => 20;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accessoryPlayer = player.GetModPlayer<AccessoryPlayer>();
                accessoryPlayer.hasThermalInkTank = true;
                accessoryPlayer.TryEquipAccessory<HoneyInkTank>();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ModContent.ItemType<EnchantedInkTank>())
                .AddIngredient(ItemID.BottledHoney, 5)
                .AddIngredient(ItemID.BeeWax, 1)
                .Register();
        }
    }
}
