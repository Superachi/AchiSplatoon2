using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles.DualieProjectiles;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AchiSplatoon2.Content.Players
{
    internal class InkDualiePlayer : BaseModPlayer
    {
        public bool isHoldingDualie;
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

        private void BlockJumps()
        {
            Player.jump = 0;
            Player.jumpHeight = 0;
            Player.StopExtraJumpInProgress();
            Player.blockExtraJumps = true;
        }

        public override void ResetEffects()
        {
            if (Player.HeldItem.ModItem is not TestDualie)
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

        public override void PreUpdateMovement()
        {
            var heldItem = Player.HeldItem.ModItem;
            var dualieData = heldItem as TestDualie;

            if (heldItem is TestDualie)
            {
                if (!isHoldingDualie)
                {
                    isHoldingDualie = true;

                    maxRolls = dualieData.MaxRolls;
                    rollsLeft = dualieData.MaxRolls;
                    rollDistance = dualieData.RollDistance;
                    rollDuration = dualieData.RollDuration;
                    rollSample = dualieData.RollSample;
                }
                
            } else
            {
                isHoldingDualie = false;
                return;
            }

            int projType = ModContent.ProjectileType<DualieRollProjectile>();
            isRolling = Player.ownedProjectileCounts[projType] >= 1;

            if (!isRolling)
            {
                if (maxRollCooldown > 0)
                {
                    maxRollCooldown--;
                    if (maxRollCooldown == 0)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Dust.NewDust(Player.Center, 0, 0, DustID.SilverCoin, 0, 0, 0, default, 1);
                            SoundHelper.PlayAudio(SoundID.MaxMana, 0.2f);
                        }

                        rollsLeft = maxRolls;
                    }
                }

                if (postRollCooldown > 0)
                {
                    postRollCooldown--;
                }

                if (maxRollCooldown > 0 || postRollCooldown > 0)
                {
                    Player.velocity.X = Math.Clamp(Player.velocity.X, -3, 3);
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
            if ((postRollCooldown > 0 || (xDir == 0 && postRoll)) && !Player.controlJump)
            {
                isTurret = true;
                return;
            }

            isTurret = false;
            postRoll = false;

            // If jumping with a dualie while shooting...
            if (rollsLeft == 0 || xDir == 0) return;
            bool countJump = InputHelper.GetInputJumpPressed() || Player.controlJump && rollsLeft < maxRolls;
            if (countJump && isHoldingDualie && Player.controlUseItem)
            {
                // Check if we can roll in the given direction
                if (!Collision.SolidCollision(new Vector2(Player.position.X + xDir * 10, Player.position.Y), Player.width, Player.height))
                {
                    BlockJumps();

                    // Roll!
                    var p = CreateProjectileWithWeaponProperties(Player, projType, (BaseWeapon)Player.HeldItem.ModItem, triggerAfterSpawn: false);
                    var proj = p as DualieRollProjectile;
                    proj.rollDistance = rollDistance;
                    proj.rollDuration = rollDuration;
                    proj.AfterSpawn();

                    SoundHelper.PlayAudio(rollSample);

                    rollsLeft--;
                    maxRollCooldown = 60;
                }
            }
        }
    }
}
