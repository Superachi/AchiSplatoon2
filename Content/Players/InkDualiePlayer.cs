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
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AchiSplatoon2.Content.Players
{
    internal class InkDualiePlayer : BaseModPlayer
    {
        public bool isRolling;
        public bool isTurret;
        public bool postRoll;
        public int postRollCooldown;

        public int maxRolls = 2;
        public int rollsLeft = 2;
        public int maxRollCooldown;

        public override void PreUpdateMovement()
        {
            int projType = ModContent.ProjectileType<DualieRollProjectile>();
            isRolling = Player.ownedProjectileCounts[projType] >= 1;

            if (!isRolling)
            {
                if (maxRollCooldown > 0)
                {
                    maxRollCooldown--;
                    if (maxRollCooldown == 0) rollsLeft = maxRolls;
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
                Player.blockExtraJumps = true;
                Player.jump = 0;
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

            var heldItem = Player.HeldItem.ModItem;

            // If jumping with a dualie while shooting...
            if (rollsLeft == 0 || xDir == 0) return;
            bool countJump = InputHelper.GetInputJumpPressed() || Player.controlJump && rollsLeft < maxRolls;
            if (countJump &&  heldItem is TestDualie && !Player.ItemTimeIsZero)
            {
                // Check if we can roll in the given direction
                if (!Collision.SolidCollision(new Vector2(Player.position.X + xDir * 10, Player.position.Y), Player.width, Player.height))
                {
                    // Blue color chips make dodge rolls go faster
                    var weaponMP = Player.GetModPlayer<InkWeaponPlayer>();
                    int blueChips = weaponMP.ColorChipAmounts[(int)InkWeaponPlayer.ChipColor.Blue];
                    float rollSpeedBonus = Math.Max(0.7f, 1f - blueChips * weaponMP.BlueChipBaseDualieRollSpeedBonus);

                    // Roll!
                    var dualieData = heldItem as TestDualie;
                    var p = CreateProjectileWithWeaponProperties(Player, projType, (BaseWeapon)Player.HeldItem.ModItem, triggerAfterSpawn: false);
                    var proj = p as DualieRollProjectile;
                    proj.rollDistance = 18 / rollSpeedBonus;
                    proj.rollDuration = 30 * rollSpeedBonus;
                    proj.AfterSpawn();

                    SoundHelper.PlayAudio(dualieData.RollSample);

                    rollsLeft--;
                    maxRollCooldown = 60;
                }
            }
        }
    }
}
