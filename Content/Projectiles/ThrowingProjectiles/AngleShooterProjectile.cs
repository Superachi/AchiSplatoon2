using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ThrowingProjectiles
{
    internal class AngleShooterProjectile : BaseProjectile
    {
        private float previousVelocityX;
        private float previousVelocityY;
        private int maxBounces;
        private float bounceDamageMod = 1f;
        private int baseDamage;

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 120;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.timeLeft = 20 * FrameSpeed();
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            BaseBomb weaponData = (BaseBomb)weaponSource;
            maxBounces = weaponData.MaxBounces;
            baseDamage = Projectile.damage;

            PlayAudio("Throwables/AngleShooterThrow", volume: 0.3f, pitchVariance: 0.2f);
        }

        public override bool PreAI()
        {
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
                bounceDamageMod++;
                bounceDamageMod = Math.Clamp(bounceDamageMod, 1, 5);
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
