using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class FieryPaintCan : BaseWeaponBoosterAccessory
    {
        public static float MissDamageModifier = 1.5f;
        public static float MissRadiusModifier = 1.25f;
        protected override string UsageHintParamA => ((MissDamageModifier - 1) * 100).ToString();

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 24;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasFieryPaintCan = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.Register();
        }
    }
}
