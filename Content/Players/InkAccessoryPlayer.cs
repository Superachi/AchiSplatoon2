using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkAccessoryPlayer : ModPlayer
    {
        public bool hasSpecialPowerEmblem;
        public bool hasSpecialChargeEmblem;
        public bool hasSubPowerEmblem;
        public static float specialChargeMultiplier = 1.5f;
        public static float subPowerMultiplier = 2f;
        public static float specialPowerMultiplier = 2f;

        public bool hasFreshQuiver;
        public float freshQuiverArcMod = 0.5f;
        public float freshQuiverVelocityMod = 1.5f;

        public bool hasCrayonBox;
        public bool hasSteelCoil;
        public bool hasTentacleScope;
        public bool hasFieryPaintCan;
        public bool lastBlasterShotHit;

        public override void ResetEffects()
        {
            hasSpecialPowerEmblem = false;
            hasSpecialChargeEmblem = false;
            hasSubPowerEmblem = false;
            hasFreshQuiver = false;
            hasCrayonBox = false;
            hasSteelCoil = false;
            hasTentacleScope = false;

            if (!hasFieryPaintCan) lastBlasterShotHit = true;
            hasFieryPaintCan = false;
        }
    }
}
