using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles
{
    internal class SplattershotProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;

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
            Projectile.extraUpdates *= 2;
            Projectile.timeLeft *= 2;
            Projectile.velocity *= 0.4f;
            fallSpeed *= 0.04f;
            delayUntilFall *= 2;
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.ShooterSpawnVisual(this);
        }

        public override void AI()
        {
            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += fallSpeed;
            }

            Color dustColor = initialColor;

            Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1.4f);

            if (Main.rand.NextBool(20))
            {
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this);
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