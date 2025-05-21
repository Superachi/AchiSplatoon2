using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.InkTanks
{
    internal class FlowerInkTank : InkTank
    {
        public override int CapacityBonus => 30;
        public static int ManaCost => 100;
        public static float InkCapacityPercentageToRecover => 0.1f;
        public static int ProcCooldown => 30;
        protected override string UsageHintParamA => $"{ManaCost}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.SetValuePostEvilBosses();
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
                .AddTile(TileID.TinkerersWorkbench)
                .AddIngredient(ModContent.ItemType<EnchantedInkTank>())
                .AddIngredient(ItemID.ManaFlower, 1)
                .Register();
        }
    }
}
