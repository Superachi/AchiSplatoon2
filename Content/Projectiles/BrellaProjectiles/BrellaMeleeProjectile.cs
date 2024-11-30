using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Players;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaMeleeProjectile : BaseProjectile
    {
        private Vector2 shieldAngle;
        private readonly float shieldAngleOffsetMult = 30f;
        private bool canShield;

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;

            var owner = GetOwner();
            Projectile.timeLeft = owner.itemTimeMax;
            shieldAngle = Projectile.velocity * shieldAngleOffsetMult;
            Projectile.velocity = Vector2.Zero;

            var brellaMP = owner.GetModPlayer<BrellaPlayer>();
            canShield = brellaMP.shieldAvailable;
            Projectile.friendly = canShield;
        }

        private void BrellaProtectDustStream()
        {
            if (canShield)
            {
                if (Main.rand.NextBool(8))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.RainbowTorch,
                        newColor: CurrentColor,
                        Scale: Main.rand.NextFloat(0.5f, 1f)
                    );
                    dust.noGravity = true;
                }
            }
            else
            {
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.Torch,
                        Scale: Main.rand.NextFloat(1f, 2f)
                    );
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.noLightEmittence = true;

                    Dust smoke = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.Smoke,
                        Scale: Main.rand.NextFloat(1f, 2f)
                    );
                    smoke.noGravity = true;
                }
            }
        }

        public override void AI()
        {
            Player owner = GetOwner();
            SyncProjectilePosWithPlayer(owner, shieldAngle.X - Projectile.width / 2, shieldAngle.Y - Projectile.height / 2);
            BrellaProtectDustStream();

            if (!IsThisClientTheProjectileOwner()) return;
            if (!canShield) return;

            Rectangle projectileRect = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            Projectile deflectedProj = DeflectProjectileWithinRectangle(projectileRect);

            if (deflectedProj != null)
            {
                var brellaMP = owner.GetModPlayer<BrellaPlayer>();
                brellaMP.DamageShield(deflectedProj.damage);

                BlockProjectileEffect(deflectedProj);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            var p = GetOwner();
            if (Collision.CanHitLine(p.Center, 1, 1, target.Center, 1, 1) && canShield)
            {
                if (!target.friendly) return true;
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            Projectile.knockBack += 5;
            modifiers.HitDirectionOverride = GetOwner().direction;

            var brellaMP = GetOwner().GetModPlayer<BrellaPlayer>();
            brellaMP.DamageShield((int)(target.damage * 0.25f));
        }

        public override void PostDraw(Color lightColor)
        {
            var brellaMP = GetOwner().GetModPlayer<BrellaPlayer>();
            var brellaLifePercentage = Math.Max(0, (int)(brellaMP.shieldLife / brellaMP.shieldLifeMax * 100));

            var playerPos = GetOwner().Center;
            Utils.DrawBorderString(
                Main.spriteBatch, $"{brellaLifePercentage}%", new Vector2((int)playerPos.X, (int)playerPos.Y) - Main.screenPosition + new Vector2(0, 60 + GetOwner().gfxOffY),
                Color.White,
                anchorx: 0.5f);
        }

        protected virtual void BlockProjectileEffect(Projectile deflectedProjectile)
        {
            // Ink
            for (int i = 0; i < 15; i++)
            {
                Color dustColor = CurrentColor;
                Dust.NewDustPerfect(deflectedProjectile.Center, ModContent.DustType<SplatterDropletDust>(),
                    Vector2.Normalize(deflectedProjectile.velocity) * 8 + Main.rand.NextVector2Circular(3, 3),
                    255, dustColor, Main.rand.NextFloat(0.5f, 1f));
            }

            // Firework
            for (int i = 0; i < 15; i++)
            {
                Color dustColor = CurrentColor;
                Dust.NewDustPerfect(deflectedProjectile.Center, DustID.FireworksRGB,
                    Vector2.Normalize(deflectedProjectile.velocity) * 8 + Main.rand.NextVector2Circular(3, 3),
                    255, dustColor, Main.rand.NextFloat(0.5f, 1f));
            }
        }
    }
}
