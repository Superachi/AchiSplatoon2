using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.DualieProjectiles
{
    internal class DualieShotProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        public float aimDevOverride = -1f;

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
            BaseDualie weaponData = (BaseDualie)WeaponInstance;

            shootSample = weaponData.ShootSample;
            shootAltSample = weaponData.ShootAltSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;
        }

        protected override void AfterSpawn()
        {
            Initialize(aimDeviationOverride: aimDevOverride);
            ApplyWeaponInstanceData();
            PlayShootSound();
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.4f;
            }

            Projectile.extraUpdates *= 2;
            Projectile.timeLeft *= 2;
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

            Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 1.2f);

            if (Main.rand.NextBool(20))
            {
                Dust.NewDustPerfect(Position: Projectile.Center, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: 0.8f);
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
            var dualieMP = GetOwner().GetModPlayer<DualiePlayer>();
            if (dualieMP.isTurret)
            {
                PlayAudio(shootAltSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0f);
            }
            else
            {
                PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 5, pitch: 0.25f);
            }
        }
    }
}
