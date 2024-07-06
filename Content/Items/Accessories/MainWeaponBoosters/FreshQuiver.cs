using AchiSplatoon2.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using AchiSplatoon2.Helpers;
using Terraria.ID;

namespace AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters
{
    internal class FreshQuiver : BaseWeaponBoosterAccessory
    {
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
                modPlayer.hasFreshQuiver = true;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagicQuiver, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.Register();
        }
    }
}
