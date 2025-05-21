using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Terraria;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles
{
    internal class SqueezerProjectile : BaseProjectile
    {
        // Creation
        private int delayUntilFall;
        private float fallSpeed;
        private bool _aimedFar = false;

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
            shootAltSample = weaponData.ShootAltSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();

            var velocity = Projectile.velocity * GetOwner().direction;
            GetOwner().itemLocation += Vector2.Normalize(velocity) * 3;

            _aimedFar = true;
            if (IsThisClientTheProjectileOwner())
            {
                float distance = Vector2.Distance(Main.LocalPlayer.Center, Main.MouseWorld);

                if (distance < 200)
                {
                    _aimedFar = false;

                    Projectile.damage = (int)(Projectile.damage * 0.75f);
                    Owner.itemTime = (int)(Owner.itemTime * 0.75f);
                    delayUntilFall /= 3;

                    Projectile.velocity *= 0.5f;
                    Projectile.velocity = WoomyMathHelper.AddRotationToVector2(Projectile.velocity, -15, 15);

                    Owner.GetModPlayer<InkTankPlayer>().HealInk(currentInkCost * 0.8f, true);
                }
                else
                {
                    Projectile.velocity *= 1.1f;
                    Owner.itemTime += Main.rand.Next(-2, 2);
                }

                NetUpdate(ProjNetUpdateType.SyncMovement);
            }

            PlayShootSound();
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.4f;
            }

            Projectile.extraUpdates *= 2;
            Projectile.timeLeft *= 3;
            fallSpeed *= 0.04f;
            delayUntilFall *= 2;
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.ShooterSpawnVisual(this);
        }

        protected override void PlayShootSound()
        {
            if (_aimedFar)
            {
                if (Main.rand.NextBool(3))
                {
                    PlayAudio(shootAltSample, volume: 0.3f, pitchVariance: 0.2f, pitch: 0f, maxInstances: 3);
                }
                else
                {
                    PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, pitch: 0f, maxInstances: 3);
                }
            }
            else
            {
                PlayAudio(shootAltSample, volume: 0.2f, pitchVariance: 0.3f, pitch: 0.6f, maxInstances: 3);
            }
        }

        // Act

        public override void AI()
        {
            if (timeSpentAlive >= FrameSpeed(delayUntilFall))
            {
                Projectile.velocity.Y += fallSpeed;
            }

            Color dustColor = CurrentColor;

            DustHelper.NewSplatterBulletDust(
                position: Projectile.Center,
                velocity: Projectile.velocity / 4,
                color: dustColor,
                scale: 1.4f
            );

            if (Main.rand.NextBool(20))
            {
                DustHelper.NewDropletDust(
                    position: Projectile.Center,
                    velocity: Projectile.velocity / 4,
                    color: dustColor,
                    scale: 1f);
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
