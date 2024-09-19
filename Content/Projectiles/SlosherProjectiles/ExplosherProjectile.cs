using AchiSplatoon2.Content.Dusts;
using System.IO;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using Microsoft.Xna.Framework;

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

        public override void AfterSpawn()
        {
            Initialize();
            ApplyWeaponInstanceData();
            enablePierceDamagefalloff = false;
            PlayAudio(shootSample, volume: 0.2f, pitchVariance: 0.2f, maxInstances: 3);
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
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: Vector2.Zero, newColor: dustColor, Scale: Main.rand.NextFloat(1.5f, 2f));
            Dust.NewDustPerfect(Position: Projectile.position + Main.rand.NextVector2Circular(10, 10), Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 5, newColor: dustColor, Scale: 1.2f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var p = CreateChildProjectile<BlastProjectile>(Projectile.Center, Vector2.Zero, (int)(originalDamage * 0.5f), false);
            p.SetProperties(200);
            p.AfterSpawn();
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
