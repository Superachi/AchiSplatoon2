using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Helpers;
using Humanizer;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items
{
    internal class BaseItem : ModItem
    {
        public virtual LocalizedText UsageHint { get; set; }
        public virtual LocalizedText Flavor { get; set; }
        protected virtual string UsageHintParamA => "";
        protected virtual string UsageHintParamB => "";

        public override void SetStaticDefaults()
        {
            UsageHint = this.GetLocalization(nameof(UsageHint));
            Flavor = this.GetLocalization(nameof(Flavor));

            int? shimmerResult = ShimmerItemList.GetShimmerItemType(Item.type);
            if (shimmerResult != null)
            {
                ItemID.Sets.ShimmerTransformToItem[Item.type] = (int)shimmerResult;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Usage hint 
            string usageVal = this.GetLocalizedValue("UsageHint").FormatWith(UsageHintParamA, UsageHintParamB);
            if (usageVal != this.GetLocalizationKey("UsageHint"))
            {
                var usageHint = new TooltipLine(Mod, "UsageHint", ColorHelper.TextWithFunctionalColor(usageVal)) { OverrideColor = null };
                tooltips.Add(usageHint);
            }

            // Flavor text
            string flavorVal = this.GetLocalizedValue("Flavor");
            if (flavorVal != this.GetLocalizationKey("Flavor"))
            {
                var flavor = new TooltipLine(Mod, "Flavor", $"{ColorHelper.TextWithFlavorColorAndQuotes(flavorVal)}") { OverrideColor = null };
                tooltips.Add(flavor);
            }
        }

        private Recipe AddRecipeWithSheldonLicense(int itemType, bool registerNow = true)
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(itemType);
            recipe.AddTile(TileID.Anvils);
            if (registerNow) { recipe.Register(); }
            return recipe;
        }

        protected Recipe AddRecipeWithSheldonLicenseBasic(bool registerNow = true)
        {
            return AddRecipeWithSheldonLicense(ModContent.ItemType<SheldonLicense>(), registerNow);
        }

        protected Recipe AddRecipeWithSheldonLicenseSilver(bool registerNow = true)
        {
            return AddRecipeWithSheldonLicense(ModContent.ItemType<SheldonLicenseSilver>(), registerNow);
        }

        protected Recipe AddRecipeWithSheldonLicenseGold(bool registerNow = true)
        {
            return AddRecipeWithSheldonLicense(ModContent.ItemType<SheldonLicenseGold>(), registerNow);
        }
    }
}
