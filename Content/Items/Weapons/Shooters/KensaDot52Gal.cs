using AchiSplatoon2.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class KensaDot52Gal : Dot52Gal
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                 projectileType: ModContent.ProjectileType<SplattershotProjectile>(),
                 singleShotTime: 6,
                 shotVelocity: 10f);

            Item.damage = 52;
            Item.knockBack = 5;
            Item.crit = 15;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
