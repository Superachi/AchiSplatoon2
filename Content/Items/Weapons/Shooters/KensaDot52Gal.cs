using AchiSplatoon2.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Items.Weapons.Shooters
{
    internal class KensaDot52Gal : Dot52Gal
    {
        public override float AimDeviation { get => 6f; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                 projectileType: ModContent.ProjectileType<Dot52GalProjectile>(),
                 singleShotTime: 7,
                 shotVelocity: 8f);

            Item.damage = 52;
            Item.knockBack = 7f;
            Item.crit = 10;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.Lime;
        }

        public override void AddRecipes() => AddRecipeKensa();
    }
}
