using AchiSplatoon2.Content.Items.Weapons.Shooters;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.IO;
using System;

namespace AchiSplatoon2.Content.Projectiles.NozzlenoseProjectiles
{
    internal class NozzlenoseShooter : BaseProjectile
    {
        private L3Nozzlenose weaponData;
        private int shotsLeft = 3;
        private int shotSpeed;
        private float shotVelocity = 10f;
        public int NPCTarget = -1;

        private float muzzleDistance;

        protected float lastShotRadians; // Used for networking

        protected float ShotTimer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = 300;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
        }

        public override void AfterSpawn()
        {
            Initialize();
            Projectile.velocity = Vector2.Zero;

            weaponData = (L3Nozzlenose)weaponSource;
            shotVelocity = weaponData.ShotVelocity;
            shotSpeed = weaponData.BurstShotTime;
            muzzleDistance = weaponData.MuzzleOffsetPx;
            ShotTimer = 4;
        }

        protected Vector2 GetVelocityTimesAngle(float aimRadians, float shotVelocity)
        {
            Vector2 angleVector = aimRadians.ToRotationVector2();
            return angleVector * shotVelocity;
        }

        protected Vector2 CalcBulletSpawnOffset(Vector2 aimVelocity, float distance)
        {
            var spawnPositionOffset = Vector2.Normalize(aimVelocity) * muzzleDistance;
            if (!Collision.CanHit(Projectile.position, 0, 0, Projectile.position + spawnPositionOffset, 0, 0))
            {
                spawnPositionOffset = Vector2.Zero;
            }
            return spawnPositionOffset;
        }

        public override void AI()
        {
            var owner = Main.player[Projectile.owner];
            Projectile.position = owner.Center;

            ShotTimer++;
            if (ShotTimer >= shotSpeed && shotsLeft > 0)
            {
                ShotTimer = 0;
                shotsLeft--;

                var recoilVector = owner.DirectionTo(Main.MouseWorld) * -3;
                if (IsThisClientTheProjectileOwner())
                {
                    PlayerItemAnimationFaceCursor(owner, recoilVector);

                    // Calculate angle/velocity
                    lastShotRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
                    Vector2 velocity = GetVelocityTimesAngle(lastShotRadians, shotVelocity);
                    var spawnPositionOffset = CalcBulletSpawnOffset(velocity, muzzleDistance);

                    var p = CreateChildProjectile(Projectile.Center + spawnPositionOffset, velocity, ModContent.ProjectileType<NozzlenoseProjectile>(), Projectile.damage, false);

                    p.weaponSource = weaponSource;
                    p.Projectile.ai[2] = Projectile.whoAmI;
                    p.AfterSpawn();

                    NetUpdate(ProjNetUpdateType.ShootAnimation);
                }
            }
        }

        protected override void NetSendShootAnimation(BinaryWriter writer)
        {
            Player owner = Main.player[Projectile.owner];

            writer.Write((double)lastShotRadians);
            writer.Write((Int16)owner.itemAnimationMax);
        }

        protected override void NetReceiveShootAnimation(BinaryReader reader)
        {
            Player owner = Main.player[Projectile.owner];

            // Make weapon face client's cursor
            lastShotRadians = (float)reader.ReadDouble();
            PlayerItemAnimationFaceCursor(owner, null, lastShotRadians);

            // Set the animation time
            owner.itemAnimation = reader.ReadInt16();
            owner.itemTime = owner.itemAnimation;
        }
    }
}
