using AchiSplatoon2.Content.Buffs;
using AchiSplatoon2.Content.Items.Accessories.General;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Projectiles.ThrowingProjectiles;
using AchiSplatoon2.DocGeneration;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Players
{
    internal class AccessoryPlayer : ModPlayer
    {
        public HashSet<int> equippedAccessories = new HashSet<int>();
        public Type? paletteType = null;

        public bool hasSpecialPowerEmblem;
        public bool hasSpecialChargeEmblem;
        public bool hasSubPowerEmblem;

        public float specialChargeMultiplier = 1f;
        public float subPowerMultiplier = 1f;
        public float specialPowerMultiplier = 1f;

        public bool hasFreshQuiver;
        public float freshQuiverArcMod = 0.5f;
        public float freshQuiverVelocityMod = 1.5f;

        public bool hasFieryPaintCan;
        public bool lastBlasterShotHit;

        public bool hasCrayonBox;
        public bool hasSteelCoil;
        public bool hasTentacleScope;
        public bool hasSquidClipOns;
        public bool hasPinkSponge;
        public bool hasMarinatedNecklace;
        public bool hasThermalInkTank;

        public bool hasChargedBattery;

        // Debug
        public bool hasDamageStabilizer;
        public bool hasNetcodeInspector;

        public override void PreUpdate()
        {
            var wepMP = Player.GetModPlayer<WeaponPlayer>();
            if (Player.HasBuff<BigBlastBuff>())
            {
                var w = Player.width;
                var h = Player.height;
                var pos = Player.position - new Vector2(w / 2, 0);
                int dustId;
                Dust dustInst;

                if (Main.rand.NextBool(20))
                {
                    dustId = Dust.NewDust(Position: pos,
                        Width: w,
                        Height: h,
                        Type: DustID.TheDestroyer,
                        SpeedX: 0f,
                        SpeedY: -2.5f,
                        newColor: Color.White,
                        Scale: Main.rand.NextFloat(0.8f, 1.2f));

                    dustInst = Main.dust[dustId];
                    dustInst.noGravity = true;
                    dustInst.fadeIn = 0.7f;
                }
            }
        }

        public bool TryEquipAccessory(int type)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(Player))
            {
                return equippedAccessories.Add(type);
            }

            return false;
        }

        public bool TryEquipAccessory<T>() where T : ModItem
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(Player))
            {
                return TryEquipAccessory(ModContent.ItemType<T>());
            }

            return false;
        }

        public bool HasAccessory(int type)
        {
            return equippedAccessories.Contains(type);
        }

        public bool HasAccessory<T>() where T : ModItem
        {
            return HasAccessory(ModContent.ItemType<T>());
        }

        public override void ResetEffects()
        {
            equippedAccessories.Clear();
            paletteType = null;

            hasSpecialPowerEmblem = false;
            hasSpecialChargeEmblem = false;
            hasSubPowerEmblem = false;

            specialChargeMultiplier = 1f;
            subPowerMultiplier = 1f;
            specialPowerMultiplier = 1f;

            // Main weapon boosting accessories
            hasFreshQuiver = false;

            if (!hasFieryPaintCan) lastBlasterShotHit = true;
            hasFieryPaintCan = false;

            hasCrayonBox = false;
            hasSteelCoil = false;
            hasTentacleScope = false;
            hasSquidClipOns = false;
            hasPinkSponge = false;
            hasThermalInkTank = false;
            hasMarinatedNecklace = false;

            hasChargedBattery = false;

            // Debug
            hasDamageStabilizer = false;
            hasNetcodeInspector = false;
        }

        public void SetBlasterBuff(bool hasHitTarget)
        {
            lastBlasterShotHit = hasHitTarget;
            if (lastBlasterShotHit)
            {
                Player.ClearBuff(ModContent.BuffType<BigBlastBuff>());
            }
            else
            {
                int buffType = ModContent.BuffType<BigBlastBuff>();
                Player.AddBuff(buffType, 2);
                Main.buffNoTimeDisplay[buffType] = true;
                Main.buffNoSave[buffType] = true;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            base.OnHitByNPC(npc, hurtInfo);
            TriggerBombAmulet(hurtInfo.Damage);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            base.OnHitByProjectile(proj, hurtInfo);
            TriggerBombAmulet(hurtInfo.Damage);
        }

        private void TriggerBombAmulet(int damageTaken)
        {
            if (!Player.HasAccessory<BombAmulet>()) return;

            int maxHP = Player.statLifeMax2;
            int lifeSlices = 8;
            float lifePortion = maxHP / (float)lifeSlices;
            int bombs = 1;

            for (int i = 0; i < lifeSlices; i++)
            {
                if (damageTaken > lifePortion * (i + 1))
                {
                    bombs++;
                }
            }

            if (bombs > 5) bombs = 5;
            var damage = Math.Max(20, damageTaken / 2);

            for (int b = 0; b < bombs; b ++)
            {
                var velocity = Main.rand.NextVector2CircularEdge(8, 8);
                if (velocity.Y < 0) velocity.Y *= -1;

                var p = ProjectileHelper.CreateProjectileWithWeaponProperties(
                    player: Player,
                    type: ModContent.ProjectileType<SplatBombProjectile>(),
                    damage: damage,
                    knockback: 8,
                    velocity: velocity,
                    weaponType: new SplatBomb(),
                    triggerSpawnMethods: false);

                p.Projectile.position = Player.Center;
                p.RunSpawnMethods();
                Main.NewText($"Bomb Amulet triggered! Spawned bomb with velocity {velocity} and damage {p.Projectile.damage}");
            }
        }
    }
}
