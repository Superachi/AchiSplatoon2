using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SplatanaProjectiles
{
    internal class SplatanaMeleeProjectile : BaseProjectile
    {
        private Color bulletColor;
        private int swingDirection;

        public bool wasFullyCharged;
        private bool firstHit = false;

        private readonly int baseAnimationTime = 22;

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 12;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        protected override void AfterSpawn()
        {
            base.AfterSpawn();
            Initialize(isDissolvable: false);
            enablePierceDamagefalloff = false;

            Projectile.velocity = Vector2.Zero;

            swingDirection = GetOwner().direction;
            Projectile.timeLeft = GetOwner().itemAnimationMax * FrameSpeed();
        }

        public override void AI()
        {
            Player p = GetOwner();
            float meleeSpeedMod = p.GetAttackSpeed(DamageClass.Melee);

            float swingDirOffset = swingDirection == -1 ? 180 : 0;
            float deg = (Projectile.ai[1] + 30 + swingDirOffset) * swingDirection;
            float rad = MathHelper.ToRadians(deg);
            float distanceFromPlayer = 64f;

            var oldPos = Projectile.position;
            Projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * distanceFromPlayer) - Projectile.width / 2;
            Projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * distanceFromPlayer) - Projectile.height / 2;

            Projectile.ai[1] += 0.7f * (float)baseAnimationTime / (float)p.itemAnimationMax;

            Vector2 offsetFromPlayer = Projectile.Center.DirectionFrom(p.Center) * 30;

            if (WeaponInstance is GolemSplatana) return;

            if (wasFullyCharged && timeSpentAlive % 4 == 0)
            {
                DustHelper.NewChargerBulletDust(
                    position: Projectile.Center + offsetFromPlayer + Main.rand.NextVector2Circular(Projectile.width * 0.25f, Projectile.height * 0.25f),
                    velocity: GetOwner().direction * WoomyMathHelper.AddRotationToVector2(offsetFromPlayer / 15, 90),
                    color: CurrentColor,
                    minScale: 1.0f,
                    1.6f);

                if (Main.rand.NextBool(4))
                {
                    DustHelper.NewDust(
                        position: Projectile.Center + offsetFromPlayer / 4 + Main.rand.NextVector2Circular(Projectile.width * 0.25f, Projectile.height * 0.25f),
                        dustType: DustID.AncientLight,
                        velocity: GetOwner().direction * WoomyMathHelper.AddRotationToVector2(offsetFromPlayer / 15, 90),
                        color: ColorHelper.ColorWithAlpha255(CurrentColor),
                        scale: Main.rand.NextFloat(0.8f, 1.2f),
                        data: new(gravity: 0));
                }
            }

            if (!wasFullyCharged && timeSpentAlive % 6 == 0)
            {
                DustHelper.NewChargerBulletDust(
                    position: Projectile.Center + offsetFromPlayer + Main.rand.NextVector2Circular(Projectile.width * 0.25f, Projectile.height * 0.25f),
                    velocity: GetOwner().direction * WoomyMathHelper.AddRotationToVector2(offsetFromPlayer / 15, 90),
                    color: CurrentColor,
                    minScale: 0.8f,
                    maxScale: 1.4f);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            var p = GetOwner();
            if (Collision.CanHitLine(p.Center, 1, 1, target.Center, 1, 1))
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
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (!firstHit && wasFullyCharged)
            {
                firstHit = true;

                if (target.life > 0)
                {
                    WeakDirectDustBurst(target.Center);
                }
                else
                {
                    DirectHitDustBurst(target.Center);
                }
            }
        }
    }
}
