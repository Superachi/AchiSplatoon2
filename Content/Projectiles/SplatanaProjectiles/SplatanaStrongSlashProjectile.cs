using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaStrongSlashProjectile : SplatanaWeakSlashProjectile
    {
        protected override int FrameCount => 1;
        protected override int FrameDelay => 1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 12;
            Projectile.height = 12;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();

            Player owner = GetOwner();
            var accMP = owner.GetModPlayer<AccessoryPlayer>();
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
            if (Projectile.timeLeft <= timeLeftWhenFade && !fading)
            {
                fading = true;
            }

            if (ProjectileDust && timeSpentAlive > 16)
            {
                Color dustColor = GenerateInkColor();

                if (timeSpentAlive % 4 == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (Main.rand.NextBool(4))
                        {
                            Dust.NewDustPerfect(
                                Position: Projectile.Center + WoomyMathHelper.AddRotationToVector2(Projectile.velocity, 90) * i * Main.rand.NextFloat(-2, 2),
                                Type: ModContent.DustType<ChargerBulletDust>(),
                                Velocity: -Projectile.velocity / Main.rand.Next(2, 4),
                                newColor: dustColor,
                                Scale: Main.rand.NextFloat(1f, 1.5f));
                        }

                        if (i > 0 && Main.rand.NextBool(4))
                        {
                            Dust.NewDustPerfect(
                                Position: Projectile.Center + WoomyMathHelper.AddRotationToVector2(Projectile.velocity, -90) * i * Main.rand.NextFloat(-2, 2),
                                Type: ModContent.DustType<ChargerBulletDust>(),
                                Velocity: -Projectile.velocity / Main.rand.Next(2, 4),
                                newColor: dustColor,
                                Scale: Main.rand.NextFloat(1f, 1.5f));
                        }
                    }

                    if (Main.rand.NextBool(2))
                    {
                        var d = Dust.NewDustPerfect(
                            Position: Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 2, Projectile.height * 2),
                            Type: DustID.AncientLight,
                            Velocity: -Projectile.velocity,
                            newColor: ColorHelper.ColorWithAlpha255(bulletColor),
                            Scale: Main.rand.NextFloat(1.2f, 1.6f));
                        d.noGravity = true;
                    }
                }
            }

            if (!IsThisClientTheProjectileOwner()) return;

            Player owner = GetOwner();
            var accMP = owner.GetModPlayer<AccessoryPlayer>();
            if (accMP.hasPinkSponge)
            {
                var size = 80;
                Rectangle projectileRect = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
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
                }
            }
        }
    }
}
