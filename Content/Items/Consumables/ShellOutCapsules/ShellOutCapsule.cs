using Terraria.ID;
using Terraria;
using AchiSplatoon2.Helpers;
using Terraria.ModLoader;
using AchiSplatoon2.Content.Projectiles.UnclassedWeaponProjectiles;
using AchiSplatoon2.Content.Items.CraftingMaterials;

namespace AchiSplatoon2.Content.Items.Consumables.ShellOutCapsules
{
    internal class ShellOutCapsule : BaseItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 10f;
            Item.useTime = 20;
            Item.useAnimation = Item.useTime;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.maxStack = Item.CommonMaxStack;
            Item.autoReuse = true;

            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                return false;
            }

            Item.stack--;
            ProjectileHelper.CreateProjectile(player, ModContent.ProjectileType<ShellOutCapsuleProjectile>());

            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<SheldonLicense>(), 1)
                .Register();

            CreateRecipe(3)
                .AddIngredient(ModContent.ItemType<SheldonLicenseSilver>(), 1)
                .Register();

            CreateRecipe(5)
                .AddIngredient(ModContent.ItemType<SheldonLicenseGold>(), 1)
                .Register();
        }
    }
}
