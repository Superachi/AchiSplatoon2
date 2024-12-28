using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles.BrushProjectiles
{
    internal class InkbrushProjectile : BaseProjectile
    {
        private float delayUntilFall;
        private float shotGravity = 0.05f;
        private readonly float airResist = 0.995f;
        private float drawScale = 0;
        protected float brightness = 0.001f;

        protected float damageFallOffMod = 0.75f;

        private bool _canRender;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 14;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBrush;

            shotGravity = weaponData.ShotGravity;
            delayUntilFall = weaponData.DelayUntilFall;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            // Set visuals
            _canRender = false;
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            Projectile.rotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
        }

        public override void AI()
        {
            _canRender = timeSpentAlive > 5 && Projectile.Distance(Owner.Center) > 10;

            if (timeSpentAlive > 3 * FrameSpeed())
            {
                if (drawScale < 1.4f)
                {
                    drawScale += 0.1f;
                }

                if (Main.rand.NextBool(timeSpentAlive / FrameSpeed()) && timeSpentAlive < FrameSpeedMultiply((int)delayUntilFall))
                {
                    DustHelper.NewDropletDust(
                        position: Projectile.Center + Main.rand.NextVector2Circular(5, 5),
                        velocity: Projectile.velocity / 2,
                        color: GenerateInkColor(),
                        minScale: 0.8f,
                        maxScale: 1.6f
                    );
                }
            }

            // Rotation increased by velocity.X 
            Projectile.rotation += Math.Sign(Projectile.velocity.X) * 0.05f;
            if (Math.Abs(Projectile.velocity.X) > 0)
            {
                Projectile.velocity.X *= airResist;
            }

            if (timeSpentAlive >= delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += shotGravity;
            }

            if (timeSpentAlive >= delayUntilFall / 2 * FrameSpeed() && damageFallOffMod > 0.5f)
            {
                damageFallOffMod -= 0.01f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this, true);
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!_canRender) return false;

            DrawProjectile(ColorHelper.ColorWithAlpha255(CurrentColor), Projectile.rotation, scale: 1.2f, alphaMod: 1, considerWorldLight: false);
            DrawTrailShrinking(scale: 1.2f, alpha: 0.2f, modulo: 0, considerWorldLight: false);

            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 30;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.SourceDamage *= damageFallOffMod;
        }
    }
}
