using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class FlowerInkTank : InkTank
    {
        public override int CapacityBonus => 30;
        public static int ManaCost => 50;
        public static float InkCapacityPercentageToRecover => 0.05f;
        public static int ProcCooldown => 30;
        protected override string UsageHintParamA => $"{ManaCost}";

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
                accessoryPlayer.TryEquipAccessory<FlowerInkTank>();
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .AddIngredient(ModContent.ItemType<EnchantedInkTank>())
                .AddIngredient(ItemID.ManaFlower, 1)
                .Register();
        }
    }
}
