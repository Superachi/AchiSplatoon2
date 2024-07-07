using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkAccessoryPlayer : ModPlayer
    {
        public bool hasAgentCloak;
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

        public override void ResetEffects()
        {
            hasAgentCloak = false;
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
        }
    }
}
