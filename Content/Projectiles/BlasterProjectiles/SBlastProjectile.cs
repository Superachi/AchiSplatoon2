using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Players;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var baseMP = GetOwner().GetModPlayer<BaseModPlayer>();
            return baseMP.IsPlayerGrounded();
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();

            if (IsPlayerGrounded())
            {
                Projectile.damage = MultiplyProjectileDamage(1.5f);
                Projectile.velocity *= 1.5f;
                Projectile.extraUpdates = 6;
                explosionRadiusAir /= 2;
            }
        }

        protected override void PlayShootSound()
        {
            if (IsPlayerGrounded())
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
