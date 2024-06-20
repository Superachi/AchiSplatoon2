using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Weapons
{
    enum SubWeaponType
    {
        None,
        SplatBomb,
        BurstBomb,
        AngleShooter,
        Sprinkler
    }

    enum SubWeaponBonusType
    {
        None,
        Discount,
        Damage
    }

    internal class BaseWeapon : ModItem
    {
        // Visual
        public virtual string ShootSample { get => "SplattershotShoot"; }
        public virtual string ShootWeakSample { get => "SplattershotShoot"; }
        public virtual float MuzzleOffsetPx { get; set; } = 0f;

        // Main weapon stats
        public virtual float AimDeviation { get => 0f; }

        // Sub weapon stats
        public virtual bool IsSubWeapon { get => false; }
        public virtual bool AllowSubWeaponUsage { get => true; }
        public virtual SubWeaponType BonusSub { get => SubWeaponType.None; }
        public virtual SubWeaponBonusType BonusType { get => SubWeaponBonusType.None; }

        // Special weapon stats
        public virtual bool IsSpecialWeapon { get => false; }
        public virtual bool IsDurationSpecial { get => false; }
        public virtual float SpecialDrainPerUse { get => 0f; }
        public virtual float SpecialDrainPerTick { get => 0f; }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (BonusType == SubWeaponBonusType.None || BonusSub == SubWeaponType.None)
            {
                return;
            }

            if (BonusType == SubWeaponBonusType.Discount)
            {
                
                TooltipLine tooltip = new TooltipLine(Mod, $"SubWeaponDiscountTooltip", $"33% chance not to consume {GetSubWeaponName(BonusSub)}") { OverrideColor = null };
                tooltips.Add(tooltip);
            }

            if (BonusType == SubWeaponBonusType.Damage)
            {
                TooltipLine tooltip = new TooltipLine(Mod, $"SubWeaponDamageTooltip", $"20% increased damage for {GetSubWeaponName(BonusSub)}") { OverrideColor = null };
                tooltips.Add(tooltip);
            }
        }

        private string GetSubWeaponName(SubWeaponType type)
        {
            string subname = "Sub weapon not found! (This is an error...)";
            switch (type)
            {
                case SubWeaponType.SplatBomb:
                    subname = "Splat Bomb";
                    break;
                case SubWeaponType.BurstBomb:
                    subname = "Burst Bomb";
                    break;
                case SubWeaponType.AngleShooter:
                    subname = "Angle Shooter";
                    break;
                case SubWeaponType.Sprinkler:
                    subname = "Sprinkler";
                    break;
            }
            return subname;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Track the item that is being used as the player is about to shoot
            // In BaseProjectile.cs -> Initialize(), this value is referenced in order to get information relevant to a weapon
            var modPlayer = player.GetModPlayer<ItemTrackerPlayer>();
            modPlayer.lastUsedWeapon = (BaseWeapon)Activator.CreateInstance(this.GetType());

            // Adjust the position of the projectile (that is about to spawn) to better match the weapon sprite
            Vector2 weaponOffset = HoldoutOffset() ?? new Vector2(0, 0);
            Vector2 muzzleOffset = Vector2.Add(Vector2.Normalize(velocity) * MuzzleOffsetPx, Vector2.Normalize(velocity) * weaponOffset);

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

        public override bool CanUseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
            if (!IsSpecialWeapon) { return base.CanUseItem(player); }
            if (!modPlayer.SpecialReady
                || (modPlayer.IsSpecialActive && IsDurationSpecial)
                || (modPlayer.SpecialName != null && modPlayer.SpecialName != player.HeldItem.Name))
            {
                player.itemTime = 30;
                return false;
            }
            modPlayer.DrainSpecial(SpecialDrainPerUse);
            modPlayer.ActivateSpecial(SpecialDrainPerTick, player.HeldItem.Name);
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            if (player.whoAmI != Main.myPlayer) return false;
            if (!player.ItemTimeIsZero) return false;
            if (!AllowSubWeaponUsage) return false;

            bool doneSearching = false;
            int[] idsToCheck = {
                ModContent.ItemType<SplatBomb>(),
                ModContent.ItemType<BurstBomb>(),
                ModContent.ItemType<AngleShooter>(),
                ModContent.ItemType<Sprinkler>(),
            };

            Type[] subWeaponType = {
                typeof(SplatBomb),
                typeof(BurstBomb),
                typeof(AngleShooter),
                typeof(Sprinkler)
            };

            // We use 4 here, as there are 4 ammo slots
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < idsToCheck.Length; j++)
                {
                    if (!doneSearching)
                    {
                        var item = player.inventory[54 + i];
                        // Ammo slots range from 54-57
                        // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory
                        if (item.type == idsToCheck[j])
                        {
                            // Check if the main weapon has a bonus that discounts sub weapons of a matching type
                            // Eg. Splattershot has a chance to not consume burst bombs
                            bool luckyDiscount = false;
                            float damageBonus = 1f;

                            if (BonusSub != SubWeaponType.None)
                            {
                                SubWeaponType currentlyCheckedSub = (SubWeaponType)(j + 1);
                                if (BonusType == SubWeaponBonusType.Discount && currentlyCheckedSub == BonusSub)
                                {
                                    luckyDiscount = Main.rand.NextBool(3);
                                }
                                if (BonusType == SubWeaponBonusType.Damage && currentlyCheckedSub == BonusSub)
                                {
                                    damageBonus = 1.2f;
                                }
                            }

                            if (!luckyDiscount)
                            {
                                item.stack--;
                            }
                            else
                            {
                                CombatTextHelper.DisplayText("Sub saved!", player.Center, new Color(140, 80, 255));
                            }

                            // Warn player if last sub weapon was used
                            if (item.stack == 0)
                            {
                                CombatTextHelper.DisplayText($"Used last {item.Name}!", player.Center);
                            }

                            // Specifically for the sprinkler, prevent usage if one is already active
                            if (item.type == ModContent.ItemType<Sprinkler>() && player.ownedProjectileCounts[item.shoot] >= 1)
                            {
                                CombatTextHelper.DisplayText("Sprinkler already active!", player.Center);
                                return false;
                            }

                            // Calculate throw angle and spawn projectile
                            float aimAngle = MathHelper.ToDegrees(
                                player.DirectionTo(Main.MouseWorld).ToRotation()
                            );

                            float radians = MathHelper.ToRadians(aimAngle);
                            Vector2 angleVector = radians.ToRotationVector2();
                            Vector2 velocity = angleVector;
                            var source = new EntitySource_ItemUse_WithAmmo(player, item, item.ammo);

                            var modPlayer = player.GetModPlayer<ItemTrackerPlayer>();
                            modPlayer.lastUsedWeapon = (BaseWeapon)Activator.CreateInstance(subWeaponType[j]);

                            Projectile.NewProjectile(
                                spawnSource: source,
                                position: player.Center,
                                velocity: velocity * item.shootSpeed,
                                Type: item.shoot,
                                Damage: (int)(item.damage * damageBonus),
                                KnockBack: item.knockBack,
                                Owner: Main.myPlayer);

                            player.itemTime = item.useTime;
                            doneSearching = true;
                            break;
                        }
                    }
                }
            }

            if (!doneSearching)
            {
                CombatTextHelper.DisplayText("No sub weapon equipped!", player.Center);
                return false;
            }

            return true;
        }
    }
}
