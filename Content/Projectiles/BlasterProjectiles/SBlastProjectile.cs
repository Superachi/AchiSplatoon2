using AchiSplatoon2.Helpers;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.BlasterProjectiles
{
    internal class SBlastProjectile : BlasterProjectileV2
    {

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        private bool IsPlayerGrounded()
        {
            return PlayerHelper.IsPlayerGrounded(GetOwner());
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            var owner = GetOwner();
            if (IsPlayerGrounded() || owner.velocity.Length() < 0.005f)
            {
                Projectile.damage = MultiplyProjectileDamage(1.5f);
                Projectile.velocity *= 1.5f;
                Projectile.extraUpdates = 6;
                explosionRadiusAir /= 2;
            }
        }

        protected override void PlayShootSound()
        {
            var owner = GetOwner();
            if (IsPlayerGrounded() || owner.velocity.Length() < 0.005f)
            {
                PlayAudio("SBlastShoot", volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
            }
            else
            {
                PlayAudio(shootSample, volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
            }
        }
    }
}
