using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Prefixes.BrushPrefixes;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Helpers.WeaponKits;
using AchiSplatoon2.Netcode;
using AchiSplatoon2.Netcode.DataTransferObjects;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static AchiSplatoon2.Content.Players.ColorChipPlayer;

namespace AchiSplatoon2.Content.Players
{
    internal class WeaponPlayer : ModPlayer
    {
        public int CustomWeaponCooldown = 0;

        public float moveSpeedModifier = 1f;
        public float moveAccelModifier = 1f;
        public float moveFrictionModifier = 1f;

        public bool isUsingRoller = false;
        public bool isBrushRolling = false;
        public bool isBrushAttacking = false;

        public bool hardmodeEnabled = false;
        public static float HardmodeSubWeaponDamageBonus => 0.5f;

        private ColorChipPlayer colorChipPlayer => Player.GetModPlayer<ColorChipPlayer>();

        public override void PreUpdate()
        {
            if (!hardmodeEnabled && Main.hardMode)
            {
                ChatHelper.SendChatToThisClient(
                    ColorHelper.TextWithSubWeaponColor($"Your sub weapons have been powered up permanently! (+{(int)(HardmodeSubWeaponDamageBonus * 100)}% base damage)"));
                hardmodeEnabled = true;
            }

            if (CustomWeaponCooldown > 0) CustomWeaponCooldown--;
        }

        // Between these two method calls, UpdateInventory is called

        public override void PostUpdateMiscEffects()
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;

            var oldMoveSpeedMod = moveSpeedModifier;
            var oldMoveAccelMod = moveAccelModifier;
            var oldMoveFrictionMod = moveFrictionModifier;
            moveSpeedModifier = 1f;
            moveAccelModifier = 1f;
            moveFrictionModifier = 1f;

            float mountSpeedMultMod = 1f;
            if (Player.mount.Active)
            {
                mountSpeedMultMod = 0.5f;
            }

            switch (Player.HeldItem.ModItem)
            {
                case BaseBrush:
                    BaseBrush brush = (BaseBrush)Player.HeldItem.ModItem;

                    if (isBrushRolling)
                    {
                        moveSpeedModifier = brush.RollMoveSpeedBonus;
                        moveAccelModifier = 2f * brush.RollMoveSpeedBonus;
                        moveFrictionModifier = 2f * brush.RollMoveSpeedBonus;

                        var prefix = PrefixHelper.GetWeaponPrefixById(Player.HeldItem.prefix);
                        if (prefix is BaseBrushPrefix brushPrefix)
                        {
                            moveSpeedModifier *= brushPrefix.DashSpeedModifier.NormalizePrefixMod();
                            moveAccelModifier *= brushPrefix.DashSpeedModifier.NormalizePrefixMod();
                            moveFrictionModifier *= brushPrefix.DashSpeedModifier.NormalizePrefixMod();
                        }
                    }
                    break;
                case BaseRoller:
                    var roller = (BaseRoller)Player.HeldItem.ModItem;
                    if (isUsingRoller)
                    {
                        moveAccelModifier = Math.Max(1, roller.RollingAccelModifier);
                        moveFrictionModifier = Math.Max(1, roller.RollingAccelModifier);
                    }
                    break;
            }

            if (Player.HeldItem.ModItem is BaseDualie)
            {
                moveAccelModifier = 2f;
                if (Player.GetModPlayer<DualiePlayer>().postRollCooldown > 0)
                {
                    moveFrictionModifier = 2f;
                }
            }

            // Move speed bonus from holding blue color chips
            if (colorChipPlayer.IsPaletteValid())
            {
                int blueChipCount = colorChipPlayer.ColorChipAmounts[(int)ChipColor.Blue];
                moveSpeedModifier += blueChipCount * colorChipPlayer.BlueChipBaseMoveSpeedBonus * mountSpeedMultMod;
                moveAccelModifier += blueChipCount * colorChipPlayer.BlueChipBaseMoveSpeedBonus * mountSpeedMultMod;
                moveFrictionModifier += blueChipCount * colorChipPlayer.BlueChipBaseMoveSpeedBonus * mountSpeedMultMod;
            }

            if (oldMoveSpeedMod != moveSpeedModifier
            || oldMoveAccelMod != moveAccelModifier
            || oldMoveFrictionMod != moveFrictionModifier)
            {
                SyncMoveSpeedData();
            }
        }

        public override void PostUpdateRunSpeeds()
        {
            Player.maxRunSpeed *= moveSpeedModifier;
            Player.runAcceleration *= moveAccelModifier;
            Player.runSlowdown *= moveFrictionModifier;
        }

        public float CalculateSubDamageBonusModifier(bool hasMainWeaponBonus)
        {
            var accMP = Player.GetModPlayer<AccessoryPlayer>();
            var heldItem = Player.HeldItem.ModItem;

            float damageMod = 1f;
            if (heldItem is BaseWeapon)
            {
                damageMod *= accMP.subPowerMultiplier;
                if (hasMainWeaponBonus) damageMod *= (1 + WeaponKitList.GetWeaponKitSubBonusAmount(heldItem.GetType()));
            }
            return damageMod;
        }

        public float CalculateSpecialDamageBonusModifier()
        {
            var accMP = Player.GetModPlayer<AccessoryPlayer>();

            float damageMod = 1f;
            damageMod *= accMP.specialPowerMultiplier;
            return damageMod;
        }

        #region Saving/loading

        public override void SaveData(TagCompound tag)
        {
            tag["hardmodeEnabled"] = hardmodeEnabled;
        }

        public override void LoadData(TagCompound tag)
        {
            hardmodeEnabled = tag.GetBool("hardmodeEnabled");
        }

        #endregion

        # region NetCode

        public override void OnEnterWorld()
        {
            SyncAllDataIfMultiplayer();
        }

        private void SendPacket(WeaponPlayerDTO dto)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;
            if (NetHelper.IsSinglePlayer()) return;

            NetHelper.SendModPlayerPacket(this, PlayerPacketType.WeaponPlayer, dto);
        }

        public void SyncAllDataManual()
        {
            SyncAllDataIfMultiplayer();
        }

        private void SyncAllDataIfMultiplayer()
        {
            if (NetHelper.IsSinglePlayer()) return;

            SyncMoveSpeedData();
        }

        private void SyncMoveSpeedData()
        {
            var dto = new WeaponPlayerDTO(
                moveSpeedMod: moveSpeedModifier,
                moveAccelMod: moveAccelModifier,
                moveFrictionMod: moveFrictionModifier);

            SendPacket(dto);
        }

        #endregion
    }
}
