using AchiSplatoon2.Content.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Content.GlobalProjectiles;
using Terraria.ID;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;
using AchiSplatoon2.Content.Players;

namespace AchiSplatoon2.Content.Projectiles.BrellaProjectiles
{
    internal class BrellaMeleeProjectile : BaseProjectile
    {
        private Vector2 shieldAngle;
        private float shieldAngleOffsetMult = 30f;
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

        public override void AfterSpawn()
        {
            base.AfterSpawn();
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;

            var owner = GetOwner();
            Projectile.timeLeft = owner.itemTimeMax;
            shieldAngle = Projectile.velocity * shieldAngleOffsetMult;
            Projectile.velocity = Vector2.Zero;

            var brellaMP = owner.GetModPlayer<InkBrellaPlayer>();
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
                        newColor: GenerateInkColor(),
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
                var brellaMP = owner.GetModPlayer<InkBrellaPlayer>();
                brellaMP.DamageShield(deflectedProj.damage);

                // Ink
                for (int i = 0; i < 15; i++)
                {
                    Color dustColor = GenerateInkColor();
                    Dust.NewDustPerfect(deflectedProj.Center, ModContent.DustType<SplatterDropletDust>(),
                        Vector2.Normalize(deflectedProj.velocity) * 8 + Main.rand.NextVector2Circular(3, 3),
                        255, dustColor, Main.rand.NextFloat(0.5f, 1f));
                }

                // Firework
                for (int i = 0; i < 15; i++)
                {
                    Color dustColor = GenerateInkColor();
                    Dust.NewDustPerfect(deflectedProj.Center, DustID.FireworksRGB,
                        Vector2.Normalize(deflectedProj.velocity) * 8 + Main.rand.NextVector2Circular(3, 3),
                        255, dustColor, Main.rand.NextFloat(0.5f, 1f));
                }
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

            var brellaMP = GetOwner().GetModPlayer<InkBrellaPlayer>();
            brellaMP.DamageShield((int)(target.damage * 0.25f));
        }

        public override void PostDraw(Color lightColor)
        {
            var brellaMP = GetOwner().GetModPlayer<InkBrellaPlayer>();
            var brellaLifePercentage = Math.Max(0, (int)(brellaMP.shieldLife / brellaMP.shieldLifeMax * 100));

            Utils.DrawBorderString(Main.spriteBatch, $"{brellaLifePercentage}%", GetOwner().Center - Main.screenPosition + new Vector2(0, 60 + GetOwner().gfxOffY) , Color.White, anchorx: 0.5f);
        }
    }
}
