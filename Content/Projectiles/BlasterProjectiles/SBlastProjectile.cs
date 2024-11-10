using AchiSplatoon2.Helpers;
using Terraria;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BlasterProjectiles
{
    internal class SBlastProjectile : BlasterProjectile
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
        }

        protected override void AdjustVariablesOnShoot()
        {
            // Apply the modifiers for standing still
            if (CanUseLongRange())
            {
                if (IsThisClientTheProjectileOwner())
                {
                    Projectile.damage = MultiplyProjectileDamage(1.5f);
                    Projectile.velocity *= 1.5f;
                    explosionRadiusAir /= 2;
                }

                Projectile.extraUpdates = 6;
            }

            // Apply the regular modifiers used to make the dust stream look nicer
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.4f;
            }

            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 3;
        }

        protected override void PlayShootSound()
        {
            if (CanUseLongRange())
            {
                PlayAudio("SBlastShoot", volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
            }
            else
            {
                PlayAudio(shootSample, volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
            }

            PlayAudio(SoundID.Item38, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: -0.5f);
            PlayAudio(SoundID.Item45, volume: 0.3f, pitchVariance: 0.2f, maxInstances: 5, pitch: 1f);
        }

        private bool CanUseLongRange()
        {
            return IsPlayerGrounded() || GetOwner().velocity.Length() < 0.005f;
        }
    }
}
