using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Projectiles
{
    internal class BaseChargeProjectile : BaseProjectile
    {
        protected float lastShotRadians; // Used for networking

        // Charge mechanic
        protected int chargeLevel = 0;
        protected bool chargeCanceled = false;
        protected float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        protected float maxChargeTime;
        protected float[] chargeTimeThresholds = { 60f };
        private bool chargeSlowerInAir = true;
        private float aerialChargeSpeedMod = 0.6f;
        private bool isPlayerGrounded = true;
        private float prefixChargeSpeedModifier = 1f;
        private bool playerHasChargedBattery = false;

        // Boolean to check whether we've released the charge
        protected bool hasFired = false;

        private Texture2D? spriteChargeBar;
        private float chargeBarBrightness = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 36000;
            Projectile.penetrate = -1;
            AIType = ProjectileID.Bullet;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
        }

        public override void ApplyWeaponInstanceData()
        {
            base.ApplyWeaponInstanceData();
            chargeSlowerInAir = WeaponInstance.SlowAerialCharge;
            playerHasChargedBattery = GetOwner().GetModPlayer<AccessoryPlayer>().hasChargedBattery;
            if (playerHasChargedBattery)
            {
                chargeSpeedModifier += ChargedBattery.ChargeSpeedFlatBonus;
                aerialChargeSpeedMod = ChargedBattery.AerialChargeSpeedModOverride;
            }

            if (IsThisClientTheProjectileOwner())
            {
                StatCalculationHelper.GetPrefixStats(GetOwner().HeldItem, out float _, out float _, out float speed, out float _, out float _, out float _, out float _);
                prefixChargeSpeedModifier = 1 + (1f - speed);
            }
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            maxChargeTime = chargeTimeThresholds.Last();
            Projectile.velocity = Vector2.Zero;
        }

        protected bool IsChargeMaxedOut()
        {
            return (chargeLevel >= chargeTimeThresholds.Length);
        }

        protected float ChargeQuotient()
        {
            return ChargeTime / MaxChargeTime();
        }

        protected virtual void IncrementChargeTime()
        {
            isPlayerGrounded = PlayerHelper.IsPlayerGrounded(GetOwner());

            float groundedSpeedModifier = !isPlayerGrounded && chargeSlowerInAir ? aerialChargeSpeedMod : 1f;
            ChargeTime += 1f * chargeSpeedModifier * groundedSpeedModifier * prefixChargeSpeedModifier;
        }

        protected float MaxChargeTime()
        {
            return chargeTimeThresholds[chargeTimeThresholds.Length - 1] * FrameSpeed();
        }

        protected virtual void ReleaseCharge(Player owner)
        {
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            hasFired = true;
            Projectile.Kill();
        }

        protected virtual void StartCharge()
        {
            // You can do something like playing a sound effect here
        }

        protected virtual void AllowChargeCancel()
        {
            if (InputHelper.GetInputRightClicked()) CancelCharge();
        }

        protected virtual void CancelCharge()
        {
            chargeCanceled = true;
            Projectile.Kill();
        }

        protected virtual void UpdateCharge(Player owner)
        {
            if (ChargeTime == 0)
            {
                StartCharge();
            }

            if (playerHasChargedBattery && IsChargeMaxedOut() && !hasFired)
            {
                GetOwner().channel = false;
                ReleaseCharge(owner);
                return;
            }

            // Charge up mechanic
            var len = chargeTimeThresholds.Length;
            if (chargeLevel < len)
            {
                IncrementChargeTime();
                if (ChargeTime >= chargeTimeThresholds[chargeLevel] * FrameSpeed())
                {
                    chargeLevel++;
                    ChargeLevelUpEffect();

                    if (chargeLevel == len)
                    {
                        StopAudio(soundPath: "ChargeStart");
                    }
                }
            }

            lastShotRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
            SyncProjectilePosWithPlayer(owner);
            PlayerItemAnimationFaceCursor(owner);
            NetUpdate(ProjNetUpdateType.UpdateCharge);
        }

        protected void ChargeLevelUpEffect()
        {
            chargeBarBrightness = 1f;

            if (!playerHasChargedBattery)
            {
                PlayAudio(soundPath: "ChargeReady", volume: 0.3f, pitch: (chargeLevel - 1) * 0.2f, maxInstances: 1);
            }
        }

        protected void MaxChargeDustStream()
        {
            if (Main.rand.NextBool(50 * FrameSpeed()))
            {
                var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0, 0, 0, default, 1);
                d.noLight = true;
                d.noLightEmittence = true;
            }
        }

        public override void AI()
        {
            if (IsThisClientTheProjectileOwner())
            {
                Player owner = Main.player[Projectile.owner];
                if (owner.dead)
                {
                    Projectile.Kill();
                    return;
                }

                if (owner.channel)
                {
                    UpdateCharge(owner);
                    return;
                }

                if (!hasFired)
                {
                    ReleaseCharge(owner);
                }

                AllowChargeCancel();
            }
        }

        protected void DrawStraightTrajectoryLine()
        {
            if (hasFired) return;

            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            var sinMult = 0.75f + (float)Math.Sin(timeSpentAlive / (FrameSpeed() * 8f)) / 4;

            int linewidth = 2;
            var lineCol = new Color(initialColor.R, initialColor.G, initialColor.B, ChargeTime / MaxChargeTime() * 0.5f);
            if (IsChargeMaxedOut())
            {
                lineCol = new Color(initialColor.R, initialColor.G, initialColor.B, 2f);
                linewidth = 4;
            }

            Utils.DrawLine(
                spriteBatch,
                GetOwner().Center + Vector2.Normalize(Main.MouseWorld - GetOwner().Center) * 50,
                GetOwner().Center + Vector2.Normalize(Main.MouseWorld - GetOwner().Center) * 1500,
                new Color(initialColor.R, initialColor.G, initialColor.B, 0) * sinMult,
                lineCol * sinMult,
                linewidth);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void PostDraw(Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner()) return;
            if (hasFired) return;

            spriteChargeBar = ModContent.Request<Texture2D>("AchiSplatoon2/Content/UI/WeaponCharge/ChargeUpBar").Value;
            if (spriteChargeBar == null) return;

            // Draw gauge, animate a white flash when charge thresholds are met
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 position = GetOwner().Center - Main.screenPosition + new Vector2(0, 50 + GetOwner().gfxOffY);
            Vector2 origin = spriteChargeBar.Size() / 2;

            if (chargeBarBrightness > 0)
            {
                chargeBarBrightness -= 0.1f;
            }

            Color w = new Color(255, 255, 255) * (chargeBarBrightness * 0.8f);
            Color color = new(initialColor.R + w.R, initialColor.G + w.G, initialColor.B + w.B);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(spriteChargeBar, new Vector2((int)position.X, (int)position.Y), null, Color.White, 0, origin, 1f, SpriteEffects.None);

            var quotient = Math.Min(ChargeQuotient(), 1);
            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                new Rectangle((int)position.X - (int)origin.X + 2,
                (int)position.Y - 2,
                (int)((spriteChargeBar.Size().X - 4) * quotient),
                (int)spriteChargeBar.Size().Y - 4),
                color);

            // Darken the gauge when charge speed is reduced
            if (!isPlayerGrounded && !IsChargeMaxedOut() && chargeSlowerInAir)
            {
                spriteBatch.Draw(
                    TextureAssets.MagicPixel.Value,
                    new Rectangle((int)position.X - (int)origin.X + 2,
                    (int)position.Y - 2,
                    (int)((spriteChargeBar.Size().X - 4) * quotient),
                    (int)spriteChargeBar.Size().Y - 4),
                    new Color(0, 0, 0, 0.5f));
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
            }
        }
        #endregion
    }
}
