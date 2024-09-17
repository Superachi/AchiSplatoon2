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

        private Recipe RegisterRecipe(Recipe recipe, bool registerNow)
        {
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

        protected void AddRecipePostEOC()
        {
            AddRecipeWithSheldonLicenseBasic(false)
                .AddIngredient(ItemID.DemoniteBar, 5)
                .Register();

            AddRecipeWithSheldonLicenseBasic(false)
                .AddIngredient(ItemID.CrimtaneBar, 5)
                .Register();
        }

        protected void AddRecipeMeteorite()
        {
            AddRecipeWithSheldonLicenseBasic(false)
                .AddIngredient(ItemID.MeteoriteBar, 5)
                .Register();
        }

        protected void AddRecipePostBee()
        {
            AddRecipeWithSheldonLicenseBasic(false)
                .AddIngredient(ItemID.BeeWax, 5)
                .Register();
        }

        protected void AddRecipeHellstone()
        {
            AddRecipeWithSheldonLicenseBasic(false)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .Register();
        }

        protected void AddRecipePostSkeletron()
        {
            AddRecipeWithSheldonLicenseBasic(false)
                .AddIngredient(ItemID.Bone, 20)
                .Register();
        }

        protected void AddRecipeCobalt()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.CobaltBar, 10)
                .Register();
        }

        protected void AddRecipePalladium()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.PalladiumBar, 10)
                .Register();
        }

        protected void AddRecipeMythril()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.MythrilBar, 7)
                .Register();
        }

        protected void AddRecipeOrichalcum()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.OrichalcumBar, 7)
                .Register();
        }

        protected void AddRecipeAdamantite()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.AdamantiteBar, 5)
                .Register();
        }

        protected void AddRecipeTitanium()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.TitaniumBar, 5)
                .Register();
        }

        protected Recipe AddRecipePostMechBoss(bool registerNow)
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.HallowedBar, 8);

            return RegisterRecipe(recipe, registerNow);
        }

        protected void AddRecipePostPlanteraDungeon()
        {
            AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.Ectoplasm, 10)
                .AddIngredient(ItemID.Bone, 30)
                .Register();
        }

        protected Recipe AddRecipePostMechBoss(bool registerNow, int soulItemID)
        {
            var recipe = AddRecipeWithSheldonLicenseSilver(false)
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddIngredient(soulItemID, 5);

            return RegisterRecipe(recipe, registerNow);
        }

        protected Recipe AddRecipeGrizzco(int regularWeapon)
        {
            return AddRecipeWithSheldonLicenseGold(false)
                .AddIngredient(ItemID.ChlorophyteBar, 8)
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddIngredient(regularWeapon, 1)
                .Register();
        }

        protected Recipe AddRecipeKensa()
        {
            return AddRecipeWithSheldonLicenseGold(false)
                .Register();
        }

        protected void AddRecipeOrder()
        {
            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.IronBar, 5)
                .AddIngredient(ItemID.Gel, 10)
                .Register();

            CreateRecipe()
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.LeadBar, 5)
                .AddIngredient(ItemID.Gel, 10)
                .Register();
        }
    }
}
