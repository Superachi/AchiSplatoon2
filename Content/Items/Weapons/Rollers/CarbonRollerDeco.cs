using AchiSplatoon2.Content.Projectiles.RollerProjectiles.SwingProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Rollers
{
    internal class CarbonRollerDeco : CarbonRoller
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 95;
            Item.knockBack = 4;
            Item.shoot = ModContent.ProjectileType<CarbonDecoSwingProjectile>();

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes() => AddRecipePalladium();
    }
}
