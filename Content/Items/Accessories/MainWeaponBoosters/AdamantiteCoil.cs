using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Terraria.ID;
using Terraria;
using System;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class AdamantiteCoil : BaseWeaponBoosterAccessory
    {
        public static float DamageReductionMod = 0.6f;
        protected override string UsageHintParamA => $"{Math.Ceiling((1 - DamageReductionMod) * 100)}";

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 20;
            Item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var modPlayer = player.GetModPlayer<InkAccessoryPlayer>();
                modPlayer.hasSteelCoil = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteBar, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.Register();
        }
    }
}
