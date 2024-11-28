using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkTankPlayer : ModPlayer
    {
        public float InkAmount = 0f;
        public float InkAmountBaseMax = 100f;
        public float InkAmountMaxBonus = 0f;
        public float InkAmountFinalMax => InkAmountBaseMax + InkAmountMaxBonus;

        public float InkRecoveryRate = 0.05f;
        public float InkRecoveryStillMult = 2f;
        public float InkRecoverySwimMult = 5f;
        public float InkRecoveryEquipMult = 1f;
        public float InkRecoveryDelay = 0f;

        private bool _isSubmerged = false;
        private int _lowInkMessageCooldown = 0;

        public override void OnEnterWorld()
        {
            InkAmount = InkAmountBaseMax;
        }

        public override void PreUpdate()
        {
            if (_lowInkMessageCooldown > 0) _lowInkMessageCooldown--;

            _isSubmerged = Player.GetModPlayer<SquidPlayer>().IsSquid();

            RecoverInk();

            InkAmount = Math.Clamp(InkAmount, 0, InkAmountFinalMax);
        }

        public override void ResetEffects()
        {
            InkAmountMaxBonus = 0f;
            InkRecoveryEquipMult = 1f;
        }

        public override void PostUpdateEquips()
        {
            if (Player.HeldItem.ModItem is SplattershotJr jr)
            {
                InkAmountMaxBonus += jr.InkTankCapacityBonus;
            }
        }

        private void RecoverInk()
        {
            if (InkRecoveryDelay > 0)
            {
                InkRecoveryDelay--;
                return;
            }

            if (!Player.ItemTimeIsZero)
            {
                return;
            }

            if (InkAmount < InkAmountFinalMax)
            {
                var stillMult = Player.velocity.Length() < 1 ? InkRecoveryStillMult : 1f;
                var swimMult = _isSubmerged ? InkRecoverySwimMult : 1f;

                InkAmount += (InkAmountFinalMax / InkAmountBaseMax) * InkRecoveryRate * stillMult * swimMult;
            }
        }

        public void HealInk(float amount, bool hideText = false)
        {
            InkAmount += amount;
            if (hideText) return;

            var color = Player.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
            CombatTextHelper.DisplayText($"+{Math.Ceiling(amount)}%", Player.Center, ColorHelper.ColorWithAlpha255(ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.2f)));
        }

        public void ConsumeInk(float amount)
        {
            InkAmount -= amount;
        }

        public float InkQuotient()
        {
            return Math.Clamp(InkAmount / InkAmountFinalMax, 0, 1);
        }

        public bool HasEnoughInk(float inkCost)
        {
            if (InkAmount < inkCost)
            {
                CreateLowInkPopup();
            }

            return InkAmount >= inkCost;
        }

        public bool HasMaxInk()
        {
            return InkAmount >= InkAmountFinalMax;
        }

        public bool HasNoInk()
        {
            return InkAmount <= 0;
        }

        public void CreateLowInkPopup()
        {
            if (_lowInkMessageCooldown == 0)
            {
                _lowInkMessageCooldown = 120;
                Player.GetModPlayer<HudPlayer>().SetOverheadText("Low ink!", 90, Color.Yellow);
            }
        }
    }
}
