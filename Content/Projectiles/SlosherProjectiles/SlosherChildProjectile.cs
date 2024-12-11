using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class SlosherChildProjectile : BaseProjectile
    {
        private readonly float delayUntilFall = 3f;
        private float fallSpeed;
        private readonly float terminalVelocity = 12f;

        private Color bulletColor;
        private float drawScale = 0f;
        private float drawRotation = 0f;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseSlosher;

            fallSpeed = weaponData.ShotGravity;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            wormDamageReduction = true;

            var accMP = GetOwner().GetModPlayer<AccessoryPlayer>();
            if (accMP.hasSteelCoil)
            {
                Projectile.damage = (int)(Projectile.damage * AdamantiteCoil.DamageReductionMod);
            }


            // Set visuals
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            bulletColor = GenerateInkColor();
            drawRotation += MathHelper.ToRadians(Main.rand.Next(0, 359));
        }

        public override void AI()
        {
            if (isFakeDestroyed) return;
            Projectile.ai[0] += 1f;

            // Start falling eventually
            if (Projectile.ai[0] >= delayUntilFall * FrameSpeed())
            {
                Projectile.velocity.Y += fallSpeed;

                if (Projectile.velocity.Y >= 0)
                {
                    Projectile.velocity.X *= 0.99f;
                }
            }

            if (Projectile.velocity.Y > terminalVelocity)
            {
                Projectile.velocity.Y = terminalVelocity;
            }

            drawRotation += Math.Sign(Projectile.velocity.X) * 0.02f;
            if (drawScale <= 1f) drawScale += 0.1f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (isFakeDestroyed) return false;
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }

            if (IsThisClientTheProjectileOwner() && !NetHelper.IsSinglePlayer())
            {
                FakeDestroy();
                return false;
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var accMP = GetOwner().GetModPlayer<AccessoryPlayer>();
            if (accMP.hasSteelCoil)
            {
                target.immune[Projectile.owner] = 3;
            }
            else
            {
                target.immune[Projectile.owner] = 18;
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            DrawProjectile(inkColor: bulletColor, rotation: drawRotation, scale: drawScale, considerWorldLight: false);

            return false;
        }
    }
}
