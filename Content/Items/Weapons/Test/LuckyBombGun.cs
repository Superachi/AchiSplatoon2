using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Projectiles.LuckyBomb;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons.Test
{
    internal class LuckyBombGun : BaseSplattershot
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            RangedWeaponDefaults(
                projectileType: ModContent.ProjectileType<LuckyBombProjectile>(),
                singleShotTime: 6,
                shotVelocity: 6f);

            Item.damage = 50;
        }
    }
}
