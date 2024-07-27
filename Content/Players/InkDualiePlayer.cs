using AchiSplatoon2.Content.Dusts;
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
    internal class InkDualiePlayer : BaseModPlayer
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
            base.PreUpdate();

            if (HeldModItem() is not BaseDualie) return;

            var accMP = Player.GetModPlayer<InkAccessoryPlayer>();
            if (hasSquidClipOns != accMP.hasSquidClipOns)
            {
                hasSquidClipOns = accMP.hasSquidClipOns;
                UpdateMaxRolls(HeldModItem() as BaseDualie);
            }

            if (jumpInputBuffer > 0) jumpInputBuffer--;
            if (InputHelper.GetInputRightClicked())
            {
                jumpInputBuffer = 6;
            }
        }

        protected override void HeldItemChangeTrigger()
        {
            if (HeldModItem() is not BaseDualie) return;
            GetDualieStats(HeldModItem() as BaseDualie);
        }

        public override void PreUpdateMovement()
        {
            int projType = ModContent.ProjectileType<DualieRollProjectile>();
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

                    if (Math.Abs(Player.velocity.X) > 4 && slowMoveAfterRoll)
                    {
                        Player.velocity.X *= 0.9f;
                    }
                }
            }

            if (isRolling)
            {
                DodgeRollDustStream();
                BlockJumps();
                Player.wings = 0;
                postRoll = true;
                return;
            }

            int xDir = InputHelper.GetInputX();
            if ((postRollCooldown > 0 || (xDir == 0 && postRoll)) && !Player.controlJump && Player.controlUseItem)
            {
                isTurret = true;
                return;
            }

            isTurret = false;
            postRoll = false;

            // If jumping while shooting...
            if (rollsLeft == 0 || xDir == 0) return;
            bool countJump = jumpInputBuffer > 0 && rollsLeft > 0;
            if (countJump && Player.controlUseItem)
            {
                // Check if we can roll in the given direction
                if (!Collision.SolidCollision(new Vector2(Player.position.X + xDir * 10, Player.position.Y), Player.width, Player.height))
                {
                    BlockJumps();
                    DodgeRollDustBurst(xDir);

                    // Roll!
                    var p = CreateProjectileWithWeaponProperties(Player, projType, (BaseWeapon)Player.HeldItem.ModItem, triggerAfterSpawn: false);
                    var proj = p as DualieRollProjectile;
                    proj.rollDistance = rollDistance;
                    proj.rollDuration = rollDuration;
                    if (hasSquidClipOns) proj.rollDuration *= SquidClipOns.RollDistanceMult;
                    proj.AfterSpawn();

                    SoundHelper.PlayAudio(rollSample, volume: 0.3f, pitchVariance: 0.1f, maxInstances: 3);
                    SoundHelper.PlayAudio(SoundID.Splash, volume: 0.5f, pitchVariance: 0.3f, maxInstances: 5, pitch: 2f);

                    rollsLeft--;
                    maxRollCooldown = 30 + 15 * (maxRolls - rollsLeft);

                    if (hasSquidClipOns) {
                        maxRollCooldown = (int)(maxRollCooldown * SquidClipOns.RollCooldownMult);
                        Player.immuneTime = (int)rollDuration;
                        Player.immune = true;
                        Player.immuneNoBlink = true;
                    };
                }
            }
        }

        private void DodgeRollDustStream()
        {
            for (int i = 0; i < 2; i++)
            {
                Rectangle rect = new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height);

                Color color = Player.GetModPlayer<InkWeaponPlayer>().ColorFromChips;
                Dust d = Dust.NewDustPerfect(
                    Position: Main.rand.NextVector2FromRectangle(rect),
                    Type: ModContent.DustType<SplatterBulletDust>(),
                    Velocity: new Vector2(Player.velocity.X / Main.rand.NextFloat(2, 6), 0),
                    Alpha: 96,
                    newColor: color,
                    Scale: 1f);
            }
        }

        private void DodgeRollDustBurst(int xDirection)
        {
            for (int i = 0; i < 30; i++)
            {
                Rectangle rect = new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height);

                Color color = Player.GetModPlayer<InkWeaponPlayer>().ColorFromChips;
                Dust d = Dust.NewDustPerfect(
                    Position: Main.rand.NextVector2FromRectangle(rect),
                    Type: ModContent.DustType<SplatterDropletDust>(),
                    Velocity: new Vector2(-xDirection * Main.rand.NextFloat(2, 8), Main.rand.NextFloat(0, -3)),
                    Alpha: 0,
                    newColor: color,
                    Scale: Main.rand.NextFloat(1f, 2f));
            }
        }
    }
}
