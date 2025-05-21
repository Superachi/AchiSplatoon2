using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.SlosherProjectiles
{
    internal class ExplosherProjectile : BaseProjectile
    {
        private int delayUntilFall;
        private float fallSpeed;
        private bool canFall = false;

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
            Projectile.penetrate = -1;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as Explosher;

            shootSample = weaponData.ShootSample;
            fallSpeed = weaponData.ShotGravity;
            delayUntilFall = weaponData.ShotGravityDelay;
            Projectile.extraUpdates = weaponData.ShotExtraUpdates;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
        }

        protected override void AdjustVariablesOnShoot()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Projectile.velocity *= 0.4f;
            }

            Projectile.extraUpdates *= 3;
            Projectile.timeLeft *= 3;
            fallSpeed *= 0.4f;
            delayUntilFall *= 2;
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

            DustHelper.NewDust(
                position: Projectile.position,
                dustType: ModContent.DustType<SplatterBulletDust>(),
                velocity: Projectile.velocity / 3,
                CurrentColor,
                scale: 2f,
                data: new(scaleIncrement: -0.25f));

            if (Main.rand.NextBool(20))
            {
                DustHelper.NewDust(
                    position: Projectile.position,
                    dustType: ModContent.DustType<SplatterBulletDust>(),
                    velocity: Projectile.velocity / 5,
                    CurrentColor,
                    scale: 1f,
                    data: new(scaleIncrement: -0.03f, gravity: 0.2f));
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, (int)(originalDamage * 0.5f), false);
            p.SetProperties(200, new(SoundPaths.Silence));
            p.RunSpawnMethods();

            if (Projectile.Center.Distance(Owner.Center) < 200)
            {
                GameFeelHelper.ShakeScreenNearPlayer(Owner, true, strength: 3, speed: 4, duration: 15);
            }

            PlayAudio(SoundPaths.BlasterExplosion.ToSoundStyle(), volume: 0.2f, maxInstances: 5);
            PlayAudio(SoundID.Item167, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 0.5f);
            PlayAudio(SoundID.Item38, volume: 0.4f, pitchVariance: 0.3f, maxInstances: 5, pitch: 0.5f);
            PlayAudio(SoundID.Splash, volume: 0.6f, pitchVariance: 0.3f, maxInstances: 5, pitch: 0.5f);

            return true;
        }

        // Netcode
        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.Write((bool)canFall);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            canFall = reader.ReadBoolean();
        }
    }
}
