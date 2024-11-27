using AchiSplatoon2.Helpers;
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

                InkAmount += InkRecoveryRate * stillMult * swimMult;
            }
        }

        public float InkQuotient()
        {
            return InkAmount / InkAmountFinalMax;
        }

        public bool HasEnoughInk(float inkCost)
        {
            if (InkAmount < inkCost)
            {
                CreateLowInkPopup();
            }

            return InkAmount >= inkCost;
        }

        public void CreateLowInkPopup()
        {
            if (_lowInkMessageCooldown == 0)
            {
                _lowInkMessageCooldown = 120;
                CombatTextHelper.DisplayText("Low ink!", Player.Center);
            }
        }
    }
}
