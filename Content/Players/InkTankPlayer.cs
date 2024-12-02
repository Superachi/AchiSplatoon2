using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AchiSplatoon2.Content.Players
{
    internal class InkTankPlayer : ModPlayer
    {
        public float InkAmount = 0f;
        public float InkAmountBaseMax = 100f;
        public float InkAmountMaxBonus = 0f;
        public float InkAmountFinalMax => InkAmountBaseMax + InkAmountMaxBonus + _inkCrystalsUsed * ValuePerCrystal;

        public float InkRecoveryRate = 0.1f;
        public float InkRecoveryStillMult = 1.5f;
        public float InkRecoverySwimMult = 5f;
        public float InkRecoveryDelay = 0f;

        public int DropletCooldown = 0;
        public int DropletCooldownMax = 300;

        private bool _isSubmerged = false;
        private int _lowInkMessageCooldown = 0;

        private int _inkCrystalsUsed = 0;
        public static int InkCrystalsMax => 10;
        public static int ValuePerCrystal => 5;

        #region Built-in hooks

        public override void OnEnterWorld()
        {
            InkAmount = InkAmountFinalMax;
        }

        public override void PreUpdate()
        {
            if (_lowInkMessageCooldown > 0) _lowInkMessageCooldown--;
            if (DropletCooldown > 0) DropletCooldown--;

            _isSubmerged = Player.GetModPlayer<SquidPlayer>().IsSquid();

            RecoverInk();

            InkAmount = Math.Clamp(InkAmount, 0, InkAmountFinalMax);
        }

        public override void ResetEffects()
        {
            InkAmountMaxBonus = 0f;
        }

        public override void PostUpdateEquips()
        {
            if (Player.HeldItem.ModItem is SplattershotJr jr)
            {
                InkAmountMaxBonus += jr.InkTankCapacityBonus;
            }
        }

        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff<InkCapacityBuff>())
            {
                InkAmountMaxBonus += InkCapacityBuff.InkCapacityBonus;
            }
        }

        #endregion

        #region Ink management

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

                if (Player.HasBuff<InkRegenerationBuff>())
                {
                    InkAmount += (InkAmountFinalMax / InkAmountBaseMax) * InkRecoveryRate * InkRecoveryStillMult * InkRecoverySwimMult;
                }
                else
                {
                    InkAmount += (InkAmountFinalMax / InkAmountBaseMax) * InkRecoveryRate * stillMult * swimMult;
                }
            }
        }

        public void HealInk(float amount, bool hideText = false)
        {
            InkAmount += amount;
            if (hideText) return;

            var color = Player.GetModPlayer<ColorChipPlayer>().GetColorFromChips();
            CombatTextHelper.DisplayText($"+{Math.Ceiling(amount)}%", Player.Center, ColorHelper.ColorWithAlpha255(ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.5f)));
        }

        public void ConsumeInk(float amount, float inkSaverModifier = 0f)
        {
            InkAmount -= amount / (1 + inkSaverModifier);
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

        public void ResetDropletCooldown()
        {
            DropletCooldown = DropletCooldownMax;
        }

        public bool ConsumeInkCrystal()
        {
            if (_inkCrystalsUsed >= InkCrystalsMax)
            {
                _inkCrystalsUsed = InkCrystalsMax;

                var hudPlayer = Player.GetModPlayer<HudPlayer>();

                if (!hudPlayer.IsTextActive())
                {
                    hudPlayer.SetOverheadText("You can't use any more Liquid Crystals!", 90, Color.White);
                    SoundHelper.PlayAudio(SoundPaths.EmptyInkTank.ToSoundStyle(), volume: 0.5f);
                }
                return false;
            }

            _inkCrystalsUsed++;
            HealInk(ValuePerCrystal);
            return true;
        }

        #endregion

        #region Interface

        public void CreateLowInkPopup()
        {
            if (_lowInkMessageCooldown == 0)
            {
                _lowInkMessageCooldown = 120;
                Player.GetModPlayer<HudPlayer>().SetOverheadText("Low ink!", 90, Color.Yellow);
                SoundHelper.PlayAudio(SoundPaths.EmptyInkTank.ToSoundStyle(), volume: 0.5f);
            }
        }

        #endregion

        #region Saving/loading

        public override void SaveData(TagCompound tag)
        {
            tag[nameof(_inkCrystalsUsed)] = _inkCrystalsUsed;
        }

        public override void LoadData(TagCompound tag)
        {
            _inkCrystalsUsed = tag.GetInt(nameof(_inkCrystalsUsed));
        }

        #endregion
    }
}
