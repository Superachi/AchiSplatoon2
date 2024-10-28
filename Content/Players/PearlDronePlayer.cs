using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Consumables.DroneUpgradeDiscs;
using AchiSplatoon2.Content.Items.Weapons.Test;
using AchiSplatoon2.Content.Projectiles.Minions.PearlDrone;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static AchiSplatoon2.Content.Players.InkWeaponPlayer;

namespace AchiSplatoon2.Content.Players
{
    internal class PearlDronePlayer : BaseModPlayer
    {
        // Level mechanics
        public int PowerLevel { get; private set; } = 1;
        private int levelCap = 4;
        private List<string> usedDroneDiscs = new();

        // Attack stats
        public float DroneAttackCooldownReduction => GetDroneAttackCooldownReduction();
        public int SprinklerBaseDamage { get; private set; } = 5;
        public int BurstBombBaseDamage { get; private set; } = 30;
        public int KillerWailBaseDamage { get; private set; } = 30;
        public int InkStrikeBaseDamage { get; private set; } = 50;
        public int MinimumChipsForBurstBomb => 3;
        public int MinimumChipsForKillerWail => 6;
        public int MinimumChipsForInkStrike => 8;
        public bool IsBurstBombEnabled => GetDroneChipCount() >= MinimumChipsForBurstBomb;
        public bool IsKillerWailEnabled => GetDroneChipCount() >= MinimumChipsForKillerWail;
        public bool IsInkStrikeEnabled => GetDroneChipCount() >= MinimumChipsForInkStrike;

        // Misc.
        private bool isDroneActive = false;
        private string droneName = "Pearl Drone";

        // Usage stats
        public int DamageDealt => damageDealt;
        private int damageDealt = 0;

        public override void PreUpdate()
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(Player)) return;
            UpdateDroneExistence();
        }

        #region Saving/loading

        public override void SaveData(TagCompound tag)
        {
            tag["dronePowerLevel"] = PowerLevel;
            tag["usedDroneDiscs"] = usedDroneDiscs;
        }

        public override void LoadData(TagCompound tag)
        {
            PowerLevel = tag.GetInt("dronePowerLevel");
            usedDroneDiscs = (List<string>)tag.GetList<string>("usedDroneDiscs");
        }

        #endregion

        #region Leveling

        public bool CheckIfDiscCanBeUsed(DroneDiscBase disc)
        {
            if (GetPlayerDrone() == null)
            {
                Main.NewText($"You need to summon {droneName} first!", Color.Pink);
                return false;
            }

            if (PowerLevel + 1 > levelCap)
            {
                Main.NewText($"{droneName}'s level is maxed out (level {PowerLevel}).", Color.Pink);
                return false;
            }

            if (usedDroneDiscs.Contains(disc.Name))
            {
                Main.NewText($"{droneName} (level {PowerLevel}) has already used this type of disc.", Color.Pink);
                return false;
            }

            return true;
        }

        public void LevelUp(DroneDiscBase usedDisc)
        {
            var discName = usedDisc.Name;
            {
                usedDroneDiscs.Add(discName);
                PowerLevel++;

                Main.NewText($"{droneName} has been upgraded to level {PowerLevel}!", Color.Pink);
                // Todo: apply visual effect to pearl and make her say something
            }
        }

        public void ResetLevel()
        {
            PowerLevel = 1;
            usedDroneDiscs.Clear();

            Main.NewText($"{droneName}'s level has been set back to {PowerLevel}!", Color.Pink);
            // Todo: apply visual effect to pearl and make her say something
        }

        #endregion

        public bool DoesPlayerHavePearlDrone()
        {
            return Player.ownedProjectileCounts[ModContent.ProjectileType<PearlDroneMinion>()] > 0;
        }

        public PearlDroneMinion? GetPlayerDrone()
        {
            if (!DoesPlayerHavePearlDrone()) return null;

            foreach(Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.type == ModContent.ProjectileType<PearlDroneMinion>() && projectile.owner == Player.whoAmI)
                {
                    return (PearlDroneMinion)projectile.ModProjectile;
                }
            }

            return null;
        }

        public int GetDroneChipCount()
        {
            var wepMP = Player.GetModPlayer<InkWeaponPlayer>();
            return wepMP.ColorChipAmounts[(int)ChipColor.Aqua];
        }

        public void AddDamageDealtStatistic(int damage)
        {
            damageDealt += damage;
        }

        #region Damage calcs

        public int GetSprinklerDamage()
        {
            float baseDamage = SprinklerBaseDamage;
            switch (PowerLevel)
            {
                case 1:
                    baseDamage *= 1;
                    break;
                case 2:
                    baseDamage *= 2;
                    break;
                case 3:
                    baseDamage *= 4;
                    break;
                case 4:
                    baseDamage *= 6;
                    break;
            }

            baseDamage *= GetSummonDamageModifier();
            return (int)baseDamage;
        }

        public int GetBurstBombDamage()
        {
            float baseDamage = BurstBombBaseDamage;
            switch (PowerLevel)
            {
                case 1:
                    baseDamage *= 1;
                    break;
                case 2:
                    baseDamage *= 2;
                    break;
                case 3:
                    baseDamage *= 5;
                    break;
                case 4:
                    baseDamage *= 8;
                    break;
            }

            baseDamage *= GetSummonDamageModifier();
            return (int)baseDamage;
        }

        public float GetSummonDamageModifier()
        {
            var baseVal = 1f;
            baseVal = Player.GetDamage(DamageClass.Summon).ApplyTo(baseVal);
            baseVal = Player.GetDamage(DamageClass.Generic).ApplyTo(baseVal);
            return baseVal;
        }

        public float GetAttackCooldownModifier()
        {
            var modifier = 1f - DroneAttackCooldownReduction;
            modifier = Math.Max(0.25f, modifier);

            return modifier;
        }

        private float GetDroneAttackCooldownReduction()
        {
            var wepMP = Player.GetModPlayer<InkWeaponPlayer>();
            return wepMP.CalculateDroneAttackCooldownReduction();
        }

        #endregion

        #region Dialogue triggers

        public void TriggerDialoguePlayerKillsNpc(NPC npc)
        {
            if (!DroneExists(out var drone)) return;
            drone!.TriggerDialoguePlayerKillsNpc(npc);
        }

        public void TriggerDialoguePearlKillsNpc(NPC npc)
        {
            if (!DroneExists(out var drone)) return;
            drone!.TriggerDialoguePearlKillsNpc(npc);
        }

        public void TriggerDialoguePlayerActivatesSpecial(int heldItemId)
        {
            if (!DroneExists(out var drone)) return;
            drone!.TriggerDialoguePlayerActivatesSpecial(heldItemId);
        }

        #endregion

        private bool DroneExists(out PearlDroneMinion? drone)
        {
            drone = GetPlayerDrone();
            return drone != null;
        }

        private void UpdateDroneExistence()
        {
            if (Player.dead) isDroneActive = false;

            var wepMP = Player.GetModPlayer<InkWeaponPlayer>();
            if (!isDroneActive)
            {
                if (wepMP.IsPaletteValid() && wepMP.ColorChipAmounts[(int)ChipColor.Aqua] > 0)
                {
                    if (GetPlayerDrone() == null)
                    {
                        isDroneActive = true;
                        Player.AddBuff(ModContent.BuffType<PearlDroneBuff>(), 2);

                        var projectile = Projectile.NewProjectileDirect(
                            spawnSource: Player.GetSource_None(),
                            position: Player.Center,
                            velocity: Vector2.Zero,
                            type: ModContent.ProjectileType<PearlDroneMinion>(),
                            damage: 0,
                            knockback: 0,
                            owner: Player.whoAmI);

                        var drone = projectile.ModProjectile as PearlDroneMinion ??
                            throw new NullReferenceException($"Tried to cast drone projectile to type {nameof(PearlDroneMinion)}, but the result was null");

                        drone.WeaponInstance = (PearlDroneStaff)Activator.CreateInstance(typeof(PearlDroneStaff))!;
                        drone.itemIdentifier = ModContent.ItemType<PearlDroneStaff>();
                        drone.AfterSpawn();
                    }
                }
            }
            else
            {
                if (!wepMP.IsPaletteValid() || wepMP.ColorChipAmounts[(int)ChipColor.Aqua] == 0)
                {
                    var drone = GetPlayerDrone();
                    if (drone != null)
                    {
                        drone.Projectile.Kill();
                    }

                    damageDealt = 0;
                    isDroneActive = false;
                }
            }
        }
    }
}
