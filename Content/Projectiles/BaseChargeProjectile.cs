using Microsoft.Xna.Framework;
using System.IO;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseChargeProjectile : BaseProjectile
    {
        protected float lastShotRadians; // Used for networking

        // Charge mechanic
        protected int chargeLevel = 0;
        protected float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        protected float maxChargeTime;
        protected float[] chargeTimeThresholds = { 60f };

        // Boolean to check whether we've released the charge
        protected bool hasFired = false;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 36000;
            Projectile.penetrate = -1;
            AIType = ProjectileID.Bullet;
        }

        public override void AfterSpawn()
        {
            Initialize();
            maxChargeTime = chargeTimeThresholds.Last();
            Projectile.velocity = Vector2.Zero;
            PlayAudio(soundPath: "ChargeStart");
        }

        protected bool IsChargeMaxedOut()
        {
            return (chargeLevel >= chargeTimeThresholds.Length);
        }

        protected virtual void IncrementChargeTime()
        {
            ChargeTime += 1f * chargeSpeedModifier;
        }

        protected float MaxChargeTime()
        {
            return chargeTimeThresholds[chargeTimeThresholds.Length - 1] * FrameSpeed();
        }

        protected virtual void ReleaseCharge(Player owner)
        {
            hasFired = true;
            Projectile.Kill();
        }

        protected virtual void StartCharge()
        {
            // You can do something like playing a sound effect here
        }

        protected virtual void UpdateCharge(Player owner)
        {
            if (ChargeTime == 0)
            {
                StartCharge();
            }

            // Charge up mechanic
            var len = chargeTimeThresholds.Length;
            if (chargeLevel < len)
            {
                IncrementChargeTime();
                if (ChargeTime >= chargeTimeThresholds[chargeLevel] * FrameSpeed())
                {
                    chargeLevel++;
                    ChargeLevelDustBurst();

                    PlayAudio(soundPath: "ChargeReady", volume: 0.3f, pitch: (chargeLevel - 1) * 0.2f, maxInstances: 1);

                    if (chargeLevel == len)
                    {
                        StopAudio(soundPath: "ChargeStart");
                    }
                }
            }
            else
            {
                MaxChargeDustStream();
            }

            lastShotRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
            SyncProjectilePosWithPlayer(owner);
            PlayerItemAnimationFaceCursor(owner);
            NetUpdate(ProjNetUpdateType.UpdateCharge);
        }

        protected void ChargeLevelDustBurst()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
            }
        }

        protected void MaxChargeDustStream()
        {
            if (Main.rand.NextBool(50 * FrameSpeed()))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
            }
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (owner.dead) { Projectile.Kill(); return; }

            if (IsThisClientTheProjectileOwner())
            {
                if (owner.channel)
                {
                    UpdateCharge(owner);
                    return;
                }

                if (!hasFired)
                {
                    ReleaseCharge(owner);
                }
            }
        }

        #region Netcode
        protected override void NetSendUpdateCharge(BinaryWriter writer)
        {
            Player owner = Main.player[Projectile.owner];

            writer.Write((double)lastShotRadians);
            writer.Write((Int16)owner.itemAnimationMax);
            writer.Write((byte)chargeLevel);
        }

        protected override void NetReceiveUpdateCharge(BinaryReader reader)
        {
            Player owner = Main.player[Projectile.owner];

            // Make weapon face client's cursor
            lastShotRadians = (float)reader.ReadDouble();
            Vector2 rotationVector = lastShotRadians.ToRotationVector2();
            PlayerItemAnimationFaceCursor(owner, null, lastShotRadians);

            // Set the animation time
            owner.itemAnimationMax = reader.ReadInt16();
            owner.itemTimeMax = owner.itemAnimationMax;

            // Render dusts based on charge level
            var newChargeLevel = reader.ReadByte();
            if (chargeLevel != newChargeLevel)
            {
                chargeLevel = newChargeLevel;
                ChargeLevelDustBurst();
            }
            if (IsChargeMaxedOut())
            {
                MaxChargeDustStream();
            }
        }
        #endregion
    }
}
