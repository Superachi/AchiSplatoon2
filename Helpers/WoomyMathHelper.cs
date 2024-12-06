using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.ExtensionMethods;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;

namespace AchiSplatoon2.Helpers
{
    public static class WoomyMathHelper
    {
        public static Vector2 DegreesToVector(float degrees)
        {
            float radians = MathHelper.ToRadians(degrees);
            return radians.ToRotationVector2();
        }

        public static int FloatToPercentage(float value)
        {
            return (int)(value * 100f);
        }

        public static Vector2 AddRotationToVector2(Vector2 inputVector, float degreesToRotateBy)
        {
            float inputVectorLength = inputVector.Length();
            float originalRadians = inputVector.ToRotation();
            float newRadians = originalRadians + MathHelper.ToRadians(degreesToRotateBy);

            Vector2 newVector = newRadians.ToRotationVector2() * inputVectorLength;
            return newVector;
        }

        public static Vector2 AddRotationToVector2(Vector2 inputVector, float degreesMin, float degreesMax)
        {
            var degrees = Main.rand.NextFloat(degreesMin, degreesMax);
            return AddRotationToVector2(inputVector, degrees);
        }

        public static Vector2 RoundVector2(Vector2 inputVector)
        {
            return new Vector2((int)inputVector.X, (int)inputVector.Y);
        }

        internal static float CalculateWeaponInkCost(BaseWeapon weapon, Player player, int? prefixOverride = null)
        {
            float baseInkCost = weapon.InkCost;
            float inkCostModifier = 1f;

            var accMP = player.GetModPlayer<AccessoryPlayer>();

            if (player.HasBuff<LastDitchEffortBuff>())
            {
                inkCostModifier *= LastDitchEffortEmblem.InkSaverAmount;
            }

            if (weapon.IsSubWeapon)
            {
                if (accMP.hasHypnoShades)
                {
                    inkCostModifier *= HypnoShades.BombInkCostMult;
                }
            }

            if (prefixOverride != null || (weapon.Item != null && weapon.Item.prefix > 0))
            {
                var prefix = PrefixHelper.GetWeaponPrefixById(weapon.Item.prefix);

                if (prefix != null)
                {
                    inkCostModifier *= prefix.InkCostModifier.NormalizePrefixMod();
                }
            }

            // DebugHelper.PrintInfo($"Cost modifier: {(int)(inkCostModifier * 100)}. Reduction: {baseInkCost} -> {baseInkCost * inkCostModifier}");
            return baseInkCost * inkCostModifier;
        }

        internal static float CalculateChargeInkCost(float baseInkCost, BaseWeapon weaponInstance, bool fullCharge)
        {
            float maxChargeTime;

            switch (weaponInstance)
            {
                case BaseCharger charger:
                    maxChargeTime = charger.ChargeTimeThresholds.Last();
                    break;
                case BaseStringer stringer:
                    maxChargeTime = stringer.ChargeTimeThresholds.Last();
                    break;
                case BaseSplatana splatana:
                    maxChargeTime = splatana.ChargeTimeThresholds.Last();
                    break;
                case BaseSplatling splatling:
                    maxChargeTime = splatling.ChargeTimeThresholds.Last();
                    break;
                default:
                    maxChargeTime = 60;
                    break;
            }

            if (fullCharge)
            {
                return baseInkCost * 10;
            }

            return baseInkCost / maxChargeTime * 10;
        }
    }
}
