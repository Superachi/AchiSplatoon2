using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaStrongSlashProjectile : SplatanaWeakSlashProjectile
    {
        protected override bool Animate => false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.tileCollide = false;
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

        public override void AI()
        {
            base.AI();
            bool CheckSolid()
            {
                return Framing.GetTileSafely(Projectile.Center).HasTile && Collision.SolidCollision(Projectile.Center, 16, 16);
            }

            if (CheckSolid())
            {
                Projectile.Kill();
            }

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
