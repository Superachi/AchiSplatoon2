using AchiSplatoon2.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using AchiSplatoon2.Helpers;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.Emblems
{
    internal class AgentCloak : BaseAccessory
    {
        public static float specialChargeMultiplier = 0.5f;
        public static float subPowerMultiplier = 1f;
        public static float specialPowerMultiplier = 1f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(
            (int)(subPowerMultiplier * 100),
            (int)(specialPowerMultiplier * 100),
            (int)(specialChargeMultiplier * 100)
            );

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 26;
            Item.height = 32;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<InkAccessoryPlayer>();
                accMP.hasAgentCloak = true;
                accMP.specialChargeMultiplier   += specialChargeMultiplier;
                accMP.subPowerMultiplier        += subPowerMultiplier;
                accMP.specialPowerMultiplier    += specialPowerMultiplier;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.StarCloak, 1);
            recipe.AddIngredient(ModContent.ItemType<SpecialPowerEmblem>(), 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddIngredient(ItemID.SoulofFright, 1);
            recipe.Register();

            Recipe recipeB = CreateRecipe();
            recipeB.AddIngredient(ItemID.StarCloak, 1);
            recipeB.AddIngredient(ModContent.ItemType<SubPowerEmblem>(), 1);
            recipeB.AddIngredient(ItemID.SoulofSight, 1);
            recipeB.AddIngredient(ItemID.SoulofMight, 1);
            recipeB.AddIngredient(ItemID.SoulofFright, 1);
            recipeB.Register();

            Recipe recipeC = CreateRecipe();
            recipeC.AddIngredient(ItemID.StarCloak, 1);
            recipeC.AddIngredient(ModContent.ItemType<SpecialChargeEmblem>(), 1);
            recipeC.AddIngredient(ItemID.SoulofSight, 1);
            recipeC.AddIngredient(ItemID.SoulofMight, 1);
            recipeC.AddIngredient(ItemID.SoulofFright, 1);
            recipeC.Register();
        }
    }
}
