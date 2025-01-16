using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Projectiles.Debug;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Test
{
    [DeveloperContent]
    internal class NetcodeInspector : BaseWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<NetcodeInspectorProjectile>(),
                singleShotTime: 30,
                shotVelocity: 0f);

            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Expert;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 1)
                .Register();
        }
    }
}
