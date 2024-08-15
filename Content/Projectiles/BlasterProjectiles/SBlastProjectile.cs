using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            var baseMP = GetOwner().GetModPlayer<BaseModPlayer>();
            if (baseMP.IsPlayerGrounded())
            {
                Projectile.velocity *= 2f;
                Projectile.extraUpdates = 6;
                explosionRadiusAir /= 2;
            }
        }
    }
}
