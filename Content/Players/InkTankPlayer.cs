using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.Debug;
using AchiSplatoon2.Content.Items.Accessories.Emblems;
using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.Content.Items.Accessories.InkTanks;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AchiSplatoon2.Content.Players
{
    internal class InkTankPlayer : ModPlayer
    {
        public float InkAmount = 0f;
        public float InkAmountBaseMax = 0f;
        public float InkAmountMaxBonus = 0f;
        public float InkAmountFinalMax => CalculateInkCapacity();

        public float InkRecoveryRate => 0.05f;
        public float InkRecoveryStillMult => 3f;
        public float InkRecoverySwimMult => 8f;

        public float InkRecoveryDelay = 0;

        public int DropletCooldown = 0;
        public int DropletCooldownMax = 300;

        private bool _isSubmerged = false;
        private int _lowInkMessageCooldown = 0;
        private int _flowerInkTankProcCooldown = 0;

        private int _inkCrystalsUsed = 0;

        private bool _loadedWorld = false;

        public static int InkCrystalsMax => 10;
        public static int ValuePerCrystal => 5;

        #region Built-in hooks

        public override void PreUpdate()
        {
            if (_lowInkMessageCooldown > 0) _lowInkMessageCooldown--;
            if (DropletCooldown > 0) DropletCooldown--;
            if (_flowerInkTankProcCooldown > 0) _flowerInkTankProcCooldown--;

            if (!_loadedWorld && Player.miscCounter > 3)
            {
                _loadedWorld = true;
                InkAmount = InkAmountFinalMax;
            }

            _isSubmerged = Player.GetModPlayer<SquidPlayer>().IsSquid();

            RecoverInk();

            InkAmount = Math.Clamp(InkAmount, 0, InkAmountFinalMax);
        }

        public override void PostUpdate()
        {
            // Last Ditch Effort
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;

            if (Player.HasAccessory<LastDitchEffortEmblem>()
                || Player.HasAccessory<SuperSaverEmblem>()
                || Player.HasAccessory<SquidbeakCloak>())
            {
                if ((float)Player.statLife / (float)Player.statLifeMax2 <= LastDitchEffortEmblem.LifePercentageThreshold)
                {
                    Player.AddBuff(ModContent.BuffType<LastDitchEffortBuff>(), 2);
                }
            }
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

        private float CalculateInkCapacity()
        {
            var result = InkAmountBaseMax + InkAmountMaxBonus + _inkCrystalsUsed * ValuePerCrystal;
            if (Player.GetModPlayer<AccessoryPlayer>().HasAccessory<LaserAddon>())
            {
                result *= LaserAddon.InkCapacityMult;
            }
            return result;
        }

        private void RecoverInk()
        {
            if (InkRecoveryDelay > 0)
            {
                InkRecoveryDelay--;
                return;
            }

            if (!Player.ItemTimeIsZero && Player.HeldItem.ModItem is BaseWeapon)
            {
                return;
            }

            if (InkAmount < InkAmountFinalMax)
            {
                var stillMult = Player.velocity.Length() < 1 ? InkRecoveryStillMult : 1f;
                var swimMult = _isSubmerged ? InkRecoverySwimMult : 1f;
                var starterInkTankValue = 100;

                if (Player.HasBuff<InkRegenerationBuff>())
                {
                    InkAmount += (InkAmountFinalMax / starterInkTankValue) * InkRecoveryRate * InkRecoveryStillMult * InkRecoverySwimMult;
                }
                else
                {
                    InkAmount += (InkAmountFinalMax / starterInkTankValue) * InkRecoveryRate * stillMult * swimMult;
                }

                CheckAndActivateFlowerInkTank();
            }
        }

        public void HealInk(float amount, bool hideText = false)
        {
            InkAmount += amount;
            if (hideText) return;

            var color = Player.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer();
            CombatTextHelper.DisplayText($"+{Math.Ceiling(amount)}%", Player.Center, ColorHelper.ColorWithAlpha255(ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.5f)));
        }

        public void HealInkFull(bool hideText = false)
        {
            var diff = InkAmountFinalMax - InkAmount;
            InkAmount = InkAmountFinalMax;
            if (hideText) return;

            var color = Player.GetModPlayer<ColorChipPlayer>().GetColorFromInkPlayer();
            CombatTextHelper.DisplayText($"+{Math.Ceiling(diff)}%", Player.Center, ColorHelper.ColorWithAlpha255(ColorHelper.LerpBetweenColorsPerfect(color, Color.White, 0.5f)));
        }

        public void ConsumeInk(float amount)
        {
            if (Player.HasAccessory<InkfinityEmblem>()) return;
            InkAmount -= amount;
        }

        public float InkQuotient()
        {
            return Math.Clamp(InkAmount / InkAmountFinalMax, 0, 1);
        }

        public bool HasEnoughInk(float inkCost, bool createPopUp = true)
        {
            var finalCost = inkCost;
            if (InkAmount < finalCost && createPopUp)
            {
                CreateLowInkPopup();
            }

            return InkAmount >= finalCost;
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

        public bool HasUsedAllCrystals()
        {
            return _inkCrystalsUsed >= InkCrystalsMax;
        }

        public int CrystalUseCount()
        {
            return _inkCrystalsUsed;
        }

        private void CheckAndActivateFlowerInkTank()
        {
            if (!Player.HasAccessory<FlowerInkTank>()) return;
            if (Player.statMana < FlowerInkTank.ManaCost) return;
            if (Player.HasBuff(BuffID.ManaSickness)) return;

            if (_flowerInkTankProcCooldown == 0 && InkAmount < InkAmountFinalMax / 2)
            {
                Player.statMana -= FlowerInkTank.ManaCost;
                _flowerInkTankProcCooldown = FlowerInkTank.ProcCooldown;

                HealInk(InkAmountFinalMax * FlowerInkTank.InkCapacityPercentageToRecover);
                SoundHelper.PlayAudio(SoundID.Item112, 0.5f, 0.2f, 10, 0.8f, Main.LocalPlayer.Center);
            }
        }

        #endregion

        #region Interface

        public void CreateLowInkPopup()
        {
            if (_lowInkMessageCooldown == 0)
            {
                if (InkAmountFinalMax <= 0f)
                {
                    _lowInkMessageCooldown = 210;
                    Player.GetModPlayer<HudPlayer>().SetOverheadText("Low ink! (Do you have an ink tank?)", 180, Color.Yellow);
                }
                else
                {
                    _lowInkMessageCooldown = 120;
                    Player.GetModPlayer<HudPlayer>().SetOverheadText("Low ink!", 90, Color.Yellow);
                }

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
