using AchiSplatoon2.Content.Dusts;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class SplatChargerProjectile : BaseChargeProjectile
    {
        private const int timeLeftAfterFiring = 30;
        private bool firstHit = false;

        private bool isPiercingTile = false;
        private Vector2 velocityBeforeTilePierce;
        private int tilePiercesLeft;

        protected virtual bool ShakeScreenOnChargeShot { get; private set; }
        protected virtual int MaxPenetrate { get; private set; }
        protected float RangeModifier { get; private set; }
        protected float MinPartialRange { get; private set; }
        protected float MaxPartialRange { get; private set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.extraUpdates = 32;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            var weaponData = WeaponInstance as BaseCharger;

            chargeTimeThresholds = weaponData.ChargeTimeThresholds;
            shootSample = weaponData.ShootSample;
            shootWeakSample = weaponData.ShootWeakSample;
            ShakeScreenOnChargeShot = weaponData.ScreenShake;
            MaxPenetrate = weaponData.MaxPenetrate;

            RangeModifier = weaponData.RangeModifier;
            MinPartialRange = weaponData.MinPartialRange;
            MaxPartialRange = weaponData.MaxPartialRange;
        }

        public override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            ApplyWeaponInstanceData();

            Projectile.velocity = Vector2.Zero;
            tilePiercesLeft = TentacularOcular.TerrainMaxPierceCount;

            if (IsThisClientTheProjectileOwner())
            {
                PlayAudio("ChargeStart", volume: 0.2f, pitchVariance: 0.1f, maxInstances: 1);
            }
        }

        protected override void ReleaseCharge(Player owner)
        {
            // Release the attack
            hasFired = true;
            Projectile.friendly = true;
            dissolvable = true;

            // Adjust behaviour depending on the charge amount
            if (chargeLevel > 0)
            {
                Projectile.penetrate = MaxPenetrate + piercingModifier;
                wormDamageReduction = true;

                Projectile.timeLeft = timeLeftAfterFiring * Projectile.extraUpdates;

                if (ShakeScreenOnChargeShot)
                {
                    GameFeelHelper.ShakeScreenNearPlayer(owner, true);
                }
            }
            else
            {
                Projectile.penetrate = 1 + piercingModifier;
                if (Projectile.penetrate > 1) wormDamageReduction = true;

                int chargeTimeNormalized = Convert.ToInt32(ChargeTime / Projectile.extraUpdates);

                // Deal a min. of 10% damage and a max. of 40% damage
                Projectile.damage = Convert.ToInt32(
                    (Projectile.damage * 0.1) + ((Projectile.damage * 0.3) * chargeTimeNormalized / chargeTimeThresholds[0])
                );

                // Similar for the range (min. 10% range, max. 40%)
                Projectile.timeLeft = Convert.ToInt32(
                    (timeLeftAfterFiring * MinPartialRange) + ((timeLeftAfterFiring * (MaxPartialRange - MinPartialRange)) * chargeTimeNormalized / chargeTimeThresholds[0])
                ) * Projectile.extraUpdates;
            }

            Projectile.timeLeft = (int)(Projectile.timeLeft * RangeModifier);

            var accMP = owner.GetModPlayer<InkAccessoryPlayer>();
            if (accMP.hasTentacleScope && IsChargeMaxedOut())
            {
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }

            PlayShootSample();

            Projectile.velocity = owner.DirectionTo(Main.MouseWorld) * 3f;
            velocityBeforeTilePierce = Projectile.velocity;

            SyncProjectilePosWithWeaponBarrel(Projectile.position, Projectile.velocity, new SplatCharger());
            PlayAudio("ChargeStart", volume: 0.0f, maxInstances: 1);
            NetUpdate(ProjNetUpdateType.ReleaseCharge);
            return;
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

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (IsThisClientTheProjectileOwner())
            {
                if (owner.channel && !hasFired)
                {
                    UpdateCharge(owner);
                    return;
                }

                if (!hasFired)
                {
                    ReleaseCharge(owner);
                    return;
                }

                PierceTile();
            }

            if (hasFired && !isPiercingTile)
            {
                DustTrail();
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (chargeLevel == 0) return;
            for (int i = 0; i < 15; i++)
            {
                float random = Main.rand.NextFloat(-5, 5);
                float velX = ((Projectile.velocity.X + random) * -0.5f);
                float velY = ((Projectile.velocity.Y + random) * -0.5f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<SplatterBulletDust>(), velX, velY, newColor: GenerateInkColor(), Scale: Main.rand.NextFloat(0.8f, 1.6f));
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

            if (!firstHit && IsChargeMaxedOut() && tilePiercesLeft == TentacularOcular.TerrainMaxPierceCount)
            {
                firstHit = true;
                DirectHitDustBurst(target.Center);
            }
        }

        private void PlayShootSample()
        {
            if (chargeLevel > 0)
            {
                PlayAudio(shootSample, volume: 0.4f, maxInstances: 1);
            }
            else
            {
                PlayAudio(shootWeakSample, volume: 0.4f, maxInstances: 1);
            }
        }

        private void DustTrail()
        {
            Color dustColor = GenerateInkColor();
            var randomDustVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterBulletDust>(), Velocity: randomDustVelocity, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
            Dust.NewDustPerfect(Position: Projectile.position, Type: ModContent.DustType<SplatterDropletDust>(), Velocity: Projectile.velocity / 4, newColor: dustColor, Scale: Main.rand.NextFloat(0.8f, 1.6f));
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

                var dust = Dust.NewDustPerfect((Vector2)position, 306,
                    velocity,
                    0, color, scale);
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.noLight = true;
                dust.noLightEmittence = true;
                dust.rotation = Main.rand.NextFloatDirection();
            }

            PlayAudio(SoundID.SplashWeak, volume: 1f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);

            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            Color inkColor = modPlayer.ColorFromChips;

            var vel = Vector2.Normalize(Projectile.velocity);
            var velX = vel.X;
            var velY = vel.Y;

            for (int i = 0; i < 8; i++)
            {
                float speed = i * 2f;
                float scale = 2 - (i / 10) * 2;
                spawnDust(new Vector2(velY * speed, -velX * speed), scale, inkColor);
                spawnDust(new Vector2(-velY * speed, velX * speed), scale, inkColor);
            }
        }

        protected override void NetSendReleaseCharge(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
            writer.WriteVector2(Projectile.position);
            writer.Write((byte)chargeLevel);
            writer.Write((string)shootSample);
            writer.Write((string)shootWeakSample);
        }

        protected override void NetReceiveReleaseCharge(BinaryReader reader)
        {
            Projectile.velocity = reader.ReadVector2();
            Projectile.position = reader.ReadVector2();
            chargeLevel = reader.ReadByte();
            shootSample = reader.ReadString();
            shootWeakSample = reader.ReadString();

            PlayShootSample();
            hasFired = true;
        }

        protected override void NetSendSyncMovement(BinaryWriter writer)
        {
            writer.WriteVector2(Projectile.velocity);
            writer.WriteVector2(Projectile.position);
            writer.Write((bool)isPiercingTile);
        }

        protected override void NetReceiveSyncMovement(BinaryReader reader)
        {
            Projectile.velocity = reader.ReadVector2();
            Projectile.position = reader.ReadVector2();
            isPiercingTile = reader.ReadBoolean();
            if (!isPiercingTile) TilePierceDustEffect();
        }
    }
}