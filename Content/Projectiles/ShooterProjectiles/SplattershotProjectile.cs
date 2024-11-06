using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles
{
    internal class SplattershotProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private readonly bool canFall = false;
        private readonly float terminalVelocity = 6f;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            BaseSplattershot weaponData = (BaseSplattershot)WeaponInstance;

            shootSample = weaponData.ShootSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;
        }

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayShootSound();
        }

        public override void AI()
        {
            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }

            Color dustColor = initialColor;

            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 2, newColor: dustColor, Scale: 1.4f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 5; i++)
            {
                float random = Main.rand.NextFloat(-2, 2);
                float velX = (Projectile.velocity.X + random) * -0.5f;
                float velY = (Projectile.velocity.Y + random) * -0.5f;
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: initialColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
            }
            return true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 20;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        protected override void PlayShootSound()
        {
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
        }
    }
}