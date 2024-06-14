using AchiSplatoon2.Content.Projectiles.StringerProjectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseChargeProjectile : BaseProjectile
    {
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

        public override void OnSpawn(IEntitySource source)
        {
            Initialize();
            maxChargeTime = chargeTimeThresholds.Last();
            Projectile.velocity = Vector2.Zero;
            PlayAudio(soundPath: "ChargeStart");
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

        protected virtual void UpdateCharge(Player owner)
        {
            // Charge up mechanic
            var len = chargeTimeThresholds.Length;
            if (chargeLevel < len)
            {
                IncrementChargeTime();
                if (ChargeTime >= chargeTimeThresholds[chargeLevel] * FrameSpeed())
                {
                    chargeLevel++;

                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                    }

                    PlayAudio(soundPath: "ChargeReady", volume: 0.3f, pitch: (chargeLevel - 1) * 0.2f, maxInstances: 1);

                    if (chargeLevel == len)
                    {
                        PlayAudio(soundPath: "ChargeStart", volume: 0f, maxInstances: 1);
                    }
                }
            }
            else
            {
                if (Main.rand.NextBool(50 * FrameSpeed()))
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                }
            }

            SyncProjectilePosWithPlayer(owner);
            PlayerItemAnimationFaceCursor(owner);
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

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
    }
}
