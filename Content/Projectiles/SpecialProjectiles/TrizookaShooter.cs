using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SpecialProjectiles
{
    internal class TrizookaShooter : BaseProjectile
    {
        private const float recoilAmount = 5f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = false;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            AIType = ProjectileID.Bullet;
        }

        public override void AfterSpawn()
        {
            Initialize();

            TrizookaSpecial weaponData = (TrizookaSpecial)weaponSource;
            shootSample = weaponData.ShootSample;
            EmitShotBurstDust();
            PlayAudio(shootSample, volume: 2f, pitchVariance: 0.05f, maxInstances: 3);

            if (IsThisClientTheProjectileOwner())
            {
                Player owner = Main.LocalPlayer;
                owner.velocity -= Vector2.Normalize(Projectile.velocity) * recoilAmount;

                PunchCameraModifier modifier = new PunchCameraModifier(
                    startPosition: owner.Center,
                    direction: (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(),
                    strength: 4f,
                    vibrationCyclesPerSecond: 6f,
                    frames: 30, 80f, FullName);
                Main.instance.CameraModifiers.Add(modifier);

                for (int i = 0; i < 3; i++)
                {
                    CreateChildProjectile(
                        position: Projectile.Center,
                        velocity: Projectile.velocity,
                        type: ModContent.ProjectileType<TrizookaProjectile>(),
                        damage: Projectile.damage);
                }
            }

            Projectile.Kill();
        }

        private void EmitShotBurstDust()
        {
            for (int i = 0; i < 15; i++)
            {
                Color dustColor = GenerateInkColor();

                float random = Main.rand.NextFloat(-5, 5);
                float velX = ((Projectile.velocity.X + random) * 0.5f);
                float velY = ((Projectile.velocity.Y + random) * 0.5f);

                Dust.NewDust(Projectile.position, 1, 1, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.2f));
            }
        }
    }
}
