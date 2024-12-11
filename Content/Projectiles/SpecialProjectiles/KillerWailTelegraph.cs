using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class KillerWailTelegraph : KillerWailProjectile
    {
        protected override bool enableDust => false;
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 3;
            Projectile.width = 18;
            Projectile.height = 160;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            AIType = ProjectileID.Bullet;
        }

        protected override void AfterSpawn()
        {
            Initialize(ignoreAimDeviation: true);
            Projectile.alpha = 128;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = 0;
        }
    }
}
