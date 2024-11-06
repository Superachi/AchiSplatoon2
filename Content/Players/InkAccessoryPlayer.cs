using AchiSplatoon2.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkAccessoryPlayer : ModPlayer
    {
        public Type? paletteType = null;

        public bool hasAgentCloak;
        public bool hasHypnoShades;
        public bool hasSpecialPowerEmblem;
        public bool hasSpecialChargeEmblem;
        public bool hasSubPowerEmblem;

        public float specialChargeMultiplier = 1f;
        public float subPowerMultiplier = 1f;
        public float specialPowerMultiplier = 1f;

        public bool hasFreshQuiver;
        public float freshQuiverArcMod = 0.5f;
        public float freshQuiverVelocityMod = 1.5f;

        public bool hasFieryPaintCan;
        public bool lastBlasterShotHit;

        public bool hasCrayonBox;
        public bool hasSteelCoil;
        public bool hasTentacleScope;
        public bool hasSquidClipOns;
        public bool hasPinkSponge;
        public bool hasMarinatedNecklace;
        public bool hasThermalInkTank;

        public bool hasChargedBattery;

        // Debug
        public bool hasDamageStabilizer;
        public bool hasNetcodeInspector;

        public override void PreUpdate()
        {
            var wepMP = Player.GetModPlayer<WeaponPlayer>();
            if (Player.HasBuff<BigBlastBuff>() && !wepMP.SpecialReady)
            {
                var w = Player.width;
                var h = Player.height;
                var pos = Player.position - new Vector2(w / 2, 0);
                int dustId;
                Dust dustInst;

                if (Main.rand.NextBool(20))
                {
                    dustId = Dust.NewDust(Position: pos,
                        Width: w,
                        Height: h,
                        Type: DustID.TheDestroyer,
                        SpeedX: 0f,
                        SpeedY: -2.5f,
                        newColor: Color.White,
                        Scale: Main.rand.NextFloat(0.8f, 1.2f));

                    dustInst = Main.dust[dustId];
                    dustInst.noGravity = true;
                    dustInst.fadeIn = 0.7f;
                }
            }
        }

        public override void ResetEffects()
        {
            paletteType = null;

            hasAgentCloak = false;
            hasHypnoShades = false;
            hasSpecialPowerEmblem = false;
            hasSpecialChargeEmblem = false;
            hasSubPowerEmblem = false;

            specialChargeMultiplier = 1f;
            subPowerMultiplier = 1f;
            specialPowerMultiplier = 1f;

            // Main weapon boosting accessories
            hasFreshQuiver = false;

            if (!hasFieryPaintCan) lastBlasterShotHit = true;
            hasFieryPaintCan = false;

            hasCrayonBox = false;
            hasSteelCoil = false;
            hasTentacleScope = false;
            hasSquidClipOns = false;
            hasPinkSponge = false;
            hasThermalInkTank = false;
            hasMarinatedNecklace = false;

            hasChargedBattery = false;

            // Debug
            hasDamageStabilizer = false;
            hasNetcodeInspector = false;
        }

        public void SetBlasterBuff(bool hasHitTarget)
        {
            lastBlasterShotHit = hasHitTarget;
            if (lastBlasterShotHit)
            {
                Player.ClearBuff(ModContent.BuffType<BigBlastBuff>());
            } else
            {
                int buffType = ModContent.BuffType<BigBlastBuff>();
                Player.AddBuff(buffType, 2);
                Main.buffNoTimeDisplay[buffType] = true;
                Main.buffNoSave[buffType] = true;
            }
        }
    }
}
