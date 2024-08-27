using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class MarinatedNecklace : BaseWeaponBoosterAccessory
    {
        public static float RecoverTimeModifier = 1.5f;
        public static float RecoverAttackSpeedModifier = 0.7f;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 32;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasMarinatedNecklace = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.Register();
        }
    }
}
