using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using System.IO;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AchiSplatoon2.Content.Projectiles.ShooterProjectiles
{
    internal class SlimeSplattershotProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private bool canFall = false;
        private int bouncesLeft = 3;

        protected float Timer
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

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
            BaseSplattershot weaponData = WeaponInstance as BaseSplattershot;

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
            Timer++;
            if (Timer >= FrameSpeed(delayUntilFall))
            {
                if (!canFall)
                {
                    canFall = true;
                    NetUpdate(ProjNetUpdateType.SyncMovement, true);
                }
            }

            if (canFall)
            {
                Projectile.velocity.Y += FrameSpeedDivide(fallSpeed);
            }

            Color dustColor = GenerateInkColor();
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: 1.5f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustPerfect(
                Position: Projectile.Center,
                Type: ModContent.DustType<SplatterBulletLastingDust>(),
                Velocity: Main.rand.NextVector2CircularEdge(3, 3),
                newColor: initialColor,
                Scale: 0.8f);
            }

            ProjectileBounce(oldVelocity, new Vector2(0.95f, 0.8f));
            bouncesLeft--;

            return bouncesLeft < 1;
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

        // Netcode
        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.Write(canFall);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            canFall = reader.ReadBoolean();
        }
    }
}
