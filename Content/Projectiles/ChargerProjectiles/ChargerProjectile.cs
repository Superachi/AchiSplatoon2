using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles.ChargerProjectiles
{
    internal class ChargerProjectile : BaseProjectile
    {
        protected override float DamageModifierAfterPierce => 0.95f;
        private bool firstHit = false;

        protected bool isPiercingTile = false;
        protected Vector2 velocityBeforeTilePierce;
        protected int tilePiercesLeft;

        public int penetrateOverride = 1;
        public bool wasParentChargeMaxed = false;
        public bool enableDirectHitEffect = false;
        protected bool hasTentacleScope = false;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.extraUpdates = 96;
            Projectile.width = 1;
            Projectile.height = 1;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = (BaseCharger)WeaponInstance;

            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
        }

        protected override void AfterSpawn()
        {
            Initialize();
            Projectile.penetrate = penetrateOverride;

            ApplyWeaponInstanceData();
            PlayShootSound();

            wormDamageReduction = false;
            velocityBeforeTilePierce = Projectile.velocity;

            tilePiercesLeft = TentacularOcular.TerrainMaxPierceCount;

            if (parentProjectile != null)
            {
                var accMP = GetOwnerModPlayer<AccessoryPlayer>();
                hasTentacleScope = accMP.hasTentacleScope;

                if (hasTentacleScope && wasParentChargeMaxed)
                {
                    Projectile.tileCollide = false;
                }
                else
                {
                    Projectile.tileCollide = true;
                }
            }
        }

        protected override void CreateDustOnSpawn()
        {
            ProjectileDustHelper.ShooterSpawnVisual(this, velocityMod: 2f);
        }

        public override void AI()
        {
            if (IsThisClientTheProjectileOwner())
            {
                PierceTile();
            }

            if (!isPiercingTile)
            {
                DustTrail();
            }
        }

        private void PierceTile()
        {
            bool CheckSolid()
            {
                return Framing.GetTileSafely(Projectile.Center).HasTile && Collision.SolidCollision(Projectile.Center, Projectile.width, Projectile.height);
            }

            if (Projectile.tileCollide == false)
            {
                if (!isPiercingTile)
                {
                    if (CheckSolid() && tilePiercesLeft > 0)
                    {
                        isPiercingTile = true;
                        tilePiercesLeft--;

                        Projectile.velocity *= 0.1f;
                        Projectile.damage = MultiplyProjectileDamage(0.8f);
                        Projectile.friendly = false;
                        NetUpdate(ProjNetUpdateType.SyncMovement, true);
                    }
                }
                else
                {
                    Projectile.timeLeft++;

                    if (!CheckSolid())
                    {
                        isPiercingTile = false;

                        TilePierceDustEffect();
                        Projectile.velocity = velocityBeforeTilePierce;
                        Projectile.friendly = true;
                        NetUpdate(ProjNetUpdateType.SyncMovement, true);
                    }
                }
            }

            if (tilePiercesLeft == 0)
            {
                Projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (tilePiercesLeft < TentacularOcular.TerrainMaxPierceCount)
            {
                modifiers.DisableCrit();
            }
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            if (wasParentChargeMaxed && enableDirectHitEffect && tilePiercesLeft == TentacularOcular.TerrainMaxPierceCount)
            {
                if (!firstHit)
                {
                    firstHit = true;
                    DirectHitDustBurst(target.Center);
                }

                for (int i = 0; i < 10; i++)
                {
                    DustHelper.NewChargerBulletDust(
                        position: Projectile.Center,
                        velocity: WoomyMathHelper.AddRotationToVector2(Projectile.velocity, Main.rand.Next(-15, 15)) * Main.rand.NextFloat(3, 6),
                        color: CurrentColor,
                        minScale: 1f,
                        maxScale: 2f);
                }

                for (int i = 0; i < 5; i++)
                {
                    DustHelper.NewDropletDust(
                        position: Projectile.Center,
                        velocity: Main.rand.NextVector2Circular(5, 5),
                        color: CurrentColor,
                        scale: Main.rand.NextFloat(0.8f, 1.6f));
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            var size = 20;
            hitbox = new Rectangle((int)Projectile.Center.X - size / 2, (int)Projectile.Center.Y - size / 2, size, size);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ProjectileDustHelper.ShooterTileCollideVisual(this);
            return true;
        }

        protected override void PlayShootSound()
        {
            if (wasParentChargeMaxed)
            {
                PlayAudio(shootSample, volume: 0.4f, maxInstances: 1);
            }
            else
            {
                PlayAudio(shootWeakSample, volume: 0.4f, maxInstances: 1);
            }
        }

        protected virtual void DustTrail()
        {
            var bigShotScale = wasParentChargeMaxed ? Main.rand.NextFloat(1.2f, 1.8f) : 1.2f;

            DustHelper.NewSplatterBulletDust(
                position: Projectile.Center,
                velocity: Projectile.velocity / 2,
                color: CurrentColor,
                scale: bigShotScale);

            DustHelper.NewChargerBulletDust(
                position: Projectile.Center,
                velocity: Projectile.velocity / 2,
                color: CurrentColor,
                minScale: 0.5f,
                maxScale: 1f);

            if (Main.rand.NextBool(10))
            {
                DustHelper.NewDropletDust(
                    position: Projectile.Center,
                    velocity: Projectile.velocity / 2,
                    color: CurrentColor,
                    minScale: 0.5f,
                    maxScale: 1f);
            }
        }

        protected void TilePierceDustEffect()
        {
            var position = Projectile.Center;

            void spawnDust(Vector2 velocity, float scale, Color? newColor = null)
            {
                Color color;
                if (newColor == null)
                {
                    color = new Color(255, 255, 255);
                }
                else
                {
                    color = ColorHelper.LerpBetweenColors((Color)newColor, new Color(255, 255, 255), 0.8f);
                }

                var dust = Dust.NewDustPerfect(position, 306,
                    velocity,
                    0, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.noLight = true;
                dust.noLightEmittence = true;
                dust.rotation = Main.rand.NextFloatDirection();
            }

            PlayAudio(SoundID.SplashWeak, volume: 1f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);

            var vel = Vector2.Normalize(Projectile.velocity);
            var velX = vel.X;
            var velY = vel.Y;

            for (int i = 0; i < 8; i++)
            {
                float speed = i * 2f;
                float scale = 2 - i / 10 * 2;
                spawnDust(new Vector2(velY * speed, -velX * speed), scale, CurrentColor);
                spawnDust(new Vector2(-velY * speed, velX * speed), scale, CurrentColor);
            }
        }

        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
            writer.WriteVector2(Projectile.position);
            writer.Write(isPiercingTile);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            Projectile.velocity = reader.ReadVector2();
            Projectile.position = reader.ReadVector2();
            isPiercingTile = reader.ReadBoolean();
            if (!isPiercingTile) TilePierceDustEffect();
        }

        protected override void NetSendInitialize(BinaryWriter writer)
        {
            writer.Write(wasParentChargeMaxed);
            base.NetSendInitialize(writer);
        }

        protected override void NetReceiveInitialize(BinaryReader reader)
        {
            wasParentChargeMaxed = reader.ReadBoolean();
            base.NetReceiveInitialize(reader);
        }
    }
}
