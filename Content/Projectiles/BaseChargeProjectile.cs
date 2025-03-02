using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
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
        protected override bool ConsumeInkAfterSpawn => false;

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

        private float chargeInkCost = 1f;

        // Boolean to check whether we've released the charge
        protected bool hasFired = false;

        private Texture2D? spriteChargeBar;
        private float chargeBarBrightness = 0f;

        protected SlotId? chargeStartAudio;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 1_000_000;
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
        }

        protected override void ApplyWeaponPrefixData()
        {
            base.ApplyWeaponPrefixData();
            var prefix = PrefixHelper.GetWeaponPrefixById(weaponSourcePrefix);

            if (prefix is BaseChargeWeaponPrefix chargeWeaponPrefix)
            {
                prefixChargeSpeedModifier = chargeWeaponPrefix.ChargeSpeedModifier.NormalizePrefixMod();
            }
        }

        protected override void AfterSpawn()
        {
            Initialize(isDissolvable: false);
            maxChargeTime = chargeTimeThresholds.Last();
            Projectile.velocity = Vector2.Zero;

            NetUpdate(ProjNetUpdateType.UpdateCharge);
        }

        protected void CalculateChargeInkCost()
        {
            chargeInkCost = WoomyMathHelper.CalculateChargeInkCost(currentInkCost, WeaponInstance, fullCharge: false);
        }

        public bool IsChargeMaxedOut()
        {
            return (chargeLevel >= chargeTimeThresholds.Length);
        }

        public float ChargeQuotient()
        {
            return ChargeTime / MaxChargeTime();
        }

        public float MaxChargeTime()
        {
            return chargeTimeThresholds[chargeTimeThresholds.Length - 1] * FrameSpeed();
        }

        protected virtual void IncrementChargeTime()
        {
            isPlayerGrounded = PlayerHelper.IsPlayerGrounded(Owner) || PlayerHelper.IsPlayerGrappled(Owner);
            float groundedSpeedModifier = !isPlayerGrounded && chargeSlowerInAir ? aerialChargeSpeedMod : 1f;

            var inkSpeedModifier = 1f;
            if (!GetOwnerModPlayer<InkTankPlayer>().HasEnoughInk(currentInkCost))
            {
                inkSpeedModifier = 0.3f;
            }

            var chargeIncrement = 1f * chargeSpeedModifier * groundedSpeedModifier * prefixChargeSpeedModifier * inkSpeedModifier;
            ChargeTime += 1f * chargeSpeedModifier * groundedSpeedModifier * prefixChargeSpeedModifier * inkSpeedModifier;

            CalculateChargeInkCost();
            ConsumeInk(inkCostOverride: chargeIncrement * chargeInkCost);
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
                        SoundHelper.StopSoundIfActive(chargeStartAudio);
                    }
                }
            }

            lastShotRadians = owner.DirectionTo(Main.MouseWorld).ToRotation();
            SyncProjectilePosWithPlayer(owner);
            PlayerItemAnimationFaceCursor(owner);

            if (timeSpentAlive > 0 && timeSpentAlive % 6 == 0)
            {
                NetUpdate(ProjNetUpdateType.UpdateCharge);
            }
        }

        protected void ChargeLevelUpEffect()
        {
            chargeBarBrightness = 1f;
            PlayAudio(SoundPaths.ChargeReady.ToSoundStyle(), volume: 0.3f, pitch: (chargeLevel - 1) * 0.2f, maxInstances: 1);
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

                if (owner.dead || owner.GetModPlayer<SquidPlayer>().IsSquid())
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
            var chipColor = Owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
            var lineCol = new Color(chipColor.R, chipColor.G, chipColor.B, ChargeTime / MaxChargeTime() * 0.5f);
            if (IsChargeMaxedOut())
            {
                lineCol = new Color(chipColor.R, chipColor.G, chipColor.B, 2f);
                linewidth = 4;
            }

            Utils.DrawLine(
                spriteBatch,
                GetOwner().Center + Vector2.Normalize(Main.MouseWorld - GetOwner().Center) * 50,
                GetOwner().Center + Vector2.Normalize(Main.MouseWorld - GetOwner().Center) * 1500,
                new Color(chipColor.R, chipColor.G, chipColor.B, 0) * sinMult,
                lineCol * sinMult,
                linewidth);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void PostDraw(Color lightColor)
        {
            if (!IsThisClientTheProjectileOwner()) return;
            if (hasFired) return;

            spriteChargeBar = TexturePaths.ChargeUpBar.ToTexture2D();
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
            Color chipColor = Owner.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
            Color color = new(chipColor.R + w.R, chipColor.G + w.G, chipColor.B + w.B);

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
            if (!isPlayerGrounded && !IsChargeMaxedOut() && chargeSlowerInAir && !playerHasChargedBattery)
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
