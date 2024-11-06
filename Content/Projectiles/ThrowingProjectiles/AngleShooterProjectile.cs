using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class AngleShooterProjectile : BaseProjectile
    {
        protected override bool FallThroughPlatforms => true;

        private float previousVelocityX;
        private float previousVelocityY;
        private int maxBounces;
        private float bounceDamageMod = 1f;
        private readonly float bounceDamageModMax = 10f;
        private int baseDamage;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 120;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 40 * FrameSpeed();
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseBomb;

            maxBounces = weaponData.MaxBounces;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            baseDamage = Projectile.damage;

            PlayAudio("Throwables/AngleShooterThrow", volume: 0.3f, pitchVariance: 0.2f);
        }

        public override bool PreAI()
        {
            base.PreAI();
            previousVelocityX = Projectile.velocity.X;
            previousVelocityY = Projectile.velocity.Y;
            return true;
        }

        public override void AI()
        {
            Color dustColor = GenerateInkColor();
            var dust = Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
            dust.alpha = 64;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (bounceDamageMod > 5f)
            {
                DirectHitDustBurst(target.Center);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var bounced = false;
            if (Projectile.velocity.X != previousVelocityX)
            {
                bounced = true;
                Projectile.position.X += Projectile.velocity.X;
                Projectile.velocity.X = -previousVelocityX;
            }
            if (Projectile.velocity.Y != previousVelocityY)
            {
                bounced = true;
                Projectile.position.Y += Projectile.velocity.Y;
                Projectile.velocity.Y = -previousVelocityY;
            }

            if (bounced)
            {
                maxBounces--;
                if (bounceDamageMod == 1f)
                {
                    bounceDamageMod += 2f;
                }
                else
                {
                    bounceDamageMod += 1.5f;
                }

                bounceDamageMod = Math.Clamp(bounceDamageMod, 1, bounceDamageModMax);
                Projectile.damage = (int)(baseDamage * bounceDamageMod);
            }

            if (maxBounces == 0)
            {
                Projectile.Kill();
            }

            return false;
        }
    }
}
