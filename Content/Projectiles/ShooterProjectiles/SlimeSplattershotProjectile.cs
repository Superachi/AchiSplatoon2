using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles
{
    internal class SlimeSplattershotProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private readonly bool canFall = false;
        private int bouncesLeft = 3;
        private int timeSinceLastBounce = 0;

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

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayShootSound();
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.5f;
            }
            
            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 3;
            fallSpeed *= 0.4f;
            delayUntilFall *= 2;
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.ShooterSpawnVisual(this);
        }

        public override void AI()
        {
            timeSinceLastBounce++;

            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }

            Color dustColor = initialColor;
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: 1.6f);

            if (Main.rand.NextBool(10))
            {
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1.2f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileBounce(oldVelocity, new Vector2(0.95f, 0.8f));

            if (timeSinceLastBounce > FrameSpeed(15))
            {
                timeSinceLastBounce = 0;
                bouncesLeft--;

                ProjectileDustHelper.ShooterTileCollideVisual(this, false);
                SoundHelper.PlayAudio(SoundID.ShimmerWeak2, volume: 0.1f, pitchVariance: 0.5f, maxInstances: 50, pitch: 1f);
            }

            return bouncesLeft < 1;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 20;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        protected override void PlayShootSound()
        {
            SoundHelper.PlayAudio(SoundID.DD2_SonicBoomBladeSlash, volume: 0.2f, pitchVariance: 0.3f, maxInstances: 50, pitch: 0.5f);
            SoundHelper.PlayAudio(SoundID.SplashWeak, volume: 0.3f, pitchVariance: 0.3f, maxInstances: 50, pitch: 0f);
            SoundHelper.PlayAudio(SoundID.SplashWeak, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 50, pitch: 0.5f);
            SoundHelper.PlayAudio(SoundID.ShimmerWeak1, volume: 0.5f, pitchVariance: 0.3f, maxInstances: 50, pitch: -0.5f);
        }

        protected override void CreateDustOnDespawn()
        {
            if (!IsThisClientTheProjectileOwner())
            {
                Projectile.position += Projectile.velocity;
                ProjectileDustHelper.ShooterTileCollideVisual(this);
            }
        }
    }
}
