using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaStrongSlashProjectile : SplatanaWeakSlashProjectile
    {
        protected override bool Animate => false;
        protected override int FrameCount => 1;
        protected override int FrameDelay => 1;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 12;
            Projectile.height = 12;
        }

        public override void AfterSpawn()
        {
            base.AfterSpawn();

            Player owner = GetOwner();
            var accMP = owner.GetModPlayer<InkAccessoryPlayer>();
            if (accMP.hasPinkSponge)
            {
                Projectile.timeLeft = (int)(Projectile.timeLeft * PinkSponge.ChargeSlashLifetimeModifier);
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 64;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override void AI()
        {
            base.AI();
            if (!IsThisClientTheProjectileOwner()) return;

            Player owner = GetOwner();
            var accMP = owner.GetModPlayer<InkAccessoryPlayer>();
            if (accMP.hasPinkSponge)
            {
                Rectangle projectileRect = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Projectile deflectedProj = DeflectProjectileWithinRectangle(projectileRect);

                if (deflectedProj != null)
                {
                    TripleHitDustBurst(Projectile.Center, false);
                    PlayAudio("HitNoDamage", volume: 0.5f, pitchVariance: 0.4f, maxInstances: 5);

                    deflectedProj.velocity = Vector2.Normalize(deflectedProj.velocity) * PinkSponge.DeflectVelocityModifier;
                    deflectedProj.friendly = true;
                    deflectedProj.hostile = false;
                    deflectedProj.damage = (int)(deflectedProj.damage * PinkSponge.DeflectDamageModifier);

                    if (Main.masterMode)
                    {
                        deflectedProj.damage *= 4;
                    }
                    else if (Main.expertMode)
                    {
                        deflectedProj.damage *= 2;
                    }

                    Projectile.timeLeft = (int)MathHelper.Min(Projectile.timeLeft, timeLeftWhenFade);
                }
            }
        }
    }
}
