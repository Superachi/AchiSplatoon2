using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class InkDualiePlayer : ModPlayer
    {
        public bool isRolling;
        public bool isTurret;

        public bool postRoll;
        public int postRollCooldown;
        public static int postRollCooldownDefault = 30;

        private int maxRolls;
        private int rollsLeft;
        private int maxRollCooldown;
        private float rollDistance = 0f;
        private float rollDuration = 0f;
        private string rollSample = "";
        private bool slowMoveAfterRoll = true;
        private bool hasSquidClipOns;

        private int jumpInputBuffer = 0;

        private InventoryPlayer inventoryPlayer => Player.GetModPlayer<InventoryPlayer>();

        public void DisplayRolls()
        {
            if (maxRolls > 1)
            {
                CombatTextHelper.DisplayText($"{rollsLeft}/{maxRolls}", Player.Center);
            }
        }

        private void BlockJumps()
        {
            Player.jump = 0;
            Player.jumpHeight = 0;
            Player.StopExtraJumpInProgress();
            Player.blockExtraJumps = true;
        }

        public override void ResetEffects()
        {
            if (Player.HeldItem.ModItem is not BaseDualie)
            {
                isRolling = false;
                isTurret = false;
                postRoll = false;
                postRollCooldown = 0;

                maxRolls = 0;
                rollsLeft = 0;
                maxRollCooldown = 0;
            }
        }

        public void UpdateMaxRolls(BaseDualie dualieData)
        {
            bool rollsLeftWasMax = rollsLeft == maxRolls;

            maxRolls = dualieData.MaxRolls;
            if (hasSquidClipOns) maxRolls /= 2;
            if (maxRolls < 1) maxRolls = 1;
            if (rollsLeftWasMax) rollsLeft = maxRolls;
        }

        public void GetDualieStats(BaseDualie dualieData)
        {
            UpdateMaxRolls(dualieData);
            rollDistance = dualieData.RollDistance;
            rollDuration = dualieData.RollDuration;
            rollSample = dualieData.RollSample;
            slowMoveAfterRoll = dualieData.SlowMoveAfterRoll;

            rollsLeft = Math.Min(rollsLeft, maxRolls);
            maxRollCooldown = Math.Max(maxRollCooldown, 30);
        }

        public override void PreUpdate()
        {
            if (inventoryPlayer.HeldModItem() is not BaseDualie) return;

            if (inventoryPlayer.HasHeldItemChanged())
            {
                GetDualieStats((BaseDualie)inventoryPlayer.HeldModItem());
            }

            var accMP = Player.GetModPlayer<InkAccessoryPlayer>();
            if (hasSquidClipOns != accMP.hasSquidClipOns)
            {
                hasSquidClipOns = accMP.hasSquidClipOns;
                UpdateMaxRolls((BaseDualie)inventoryPlayer.HeldModItem());
            }

            if (jumpInputBuffer > 0) jumpInputBuffer--;
            if (InputHelper.GetInputRightClicked())
            {
                jumpInputBuffer = 6;
            }
        }

        public override void PreUpdateMovement()
        {
            int projType = ModContent.ProjectileType<DualieRollProjectile>();
            if (inventoryPlayer.HeldModItem() is BaseDualie)
            {
                var item = (BaseDualie)inventoryPlayer.HeldModItem();
                projType = item.RollProjectileType;
            }

            isRolling = Player.ownedProjectileCounts[projType] >= 1;

            if (!isRolling)
            {
                if (maxRollCooldown > 0)
                {
                    maxRollCooldown--;
                    if (maxRollCooldown == 0 && rollsLeft < maxRolls)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Dust.NewDust(Player.Center, 0, 0, DustID.GoldCoin, 0, 0, 0, default, 2);
                            SoundHelper.PlayAudio(SoundID.MaxMana, 0.3f);
                        }

                        rollsLeft = maxRolls;
                        if (maxRolls > 1) CombatTextHelper.DisplayText($"{rollsLeft}/{maxRolls}", Player.Center, color: Color.LimeGreen);
                    }
                }

                if (postRollCooldown > 0)
                {
                    postRollCooldown--;

                    if (Math.Abs(Player.velocity.X) > 4 && (slowMoveAfterRoll && !hasSquidClipOns))
                    {
                        Player.velocity.X *= 0.9f;
                    }
                }
            }

            if (isRolling)
            {
                BlockJumps();
                Player.wings = 0;
                postRoll = true;
                return;
            }

            int xDir = InputHelper.GetInputX();
            if ((postRollCooldown > 0 || (xDir == 0 && postRoll)) && jumpInputBuffer == 0 && Player.controlUseItem)
            {
                isTurret = true;
                return;
            }

            isTurret = false;
            postRoll = false;

            // If jumping while shooting...
            if (rollsLeft == 0 || xDir == 0 || Player.mount.Active) return;
            bool countJump = jumpInputBuffer > 0 && rollsLeft > 0;
            if (countJump && Player.controlUseItem)
            {
                // Check if we can roll in the given direction
                if (!Collision.SolidCollision(new Vector2(Player.position.X + xDir * 10, Player.position.Y), Player.width, Player.height))
                {
                    BlockJumps();

                    // Roll!
                    var proj = (DualieRollProjectile)ProjectileHelper.CreateProjectileWithWeaponProperties(Player, projType, (BaseWeapon)Player.HeldItem.ModItem, triggerAfterSpawn: false);
                    proj.rollDistance = rollDistance;
                    proj.rollDuration = rollDuration;
                    proj.AfterSpawn();

                    rollsLeft--;
                    maxRollCooldown = 30 + 15 * (maxRolls - rollsLeft);

                    if (hasSquidClipOns) {
                        maxRollCooldown = (int)(maxRollCooldown * SquidClipOns.RollCooldownMult);
                        Player.immuneTime = (int)rollDuration + 12;
                        Player.immune = true;
                        Player.immuneNoBlink = false;
                    };
                }
            }
        }
    }
}
