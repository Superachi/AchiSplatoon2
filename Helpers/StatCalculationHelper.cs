using AchiSplatoon2.Attributes;
using AchiSplatoon2.Content.Items.Accessories;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.ExtensionMethods;
using AchiSplatoon2.Helpers.WeaponKits;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal static class StatCalculationHelper
    {
        public static float CalculateDamageModifiers(Player player, BaseWeapon weaponInstance, BaseProjectile projectile = null, bool debug = false)
        {
            float damageModifier = 1f;
            var colorChipPlayer = player.GetModPlayer<ColorChipPlayer>();
            var accMP = player.GetModPlayer<AccessoryPlayer>();
            if (debug) DebugHelper.PrintInfo($"Start val: {damageModifier}");

            #region Additive

            // Red color chip bonus
            damageModifier += colorChipPlayer.CalculateAttackDamageBonus();
            if (debug) DebugHelper.PrintInfo($"Val after red chip: {damageModifier}");

            // Pallete bonus
            if (BaseWeapon.DoesPaletteBoostMainWeapon(weaponInstance, player))
            {
                damageModifier += colorChipPlayer.PaletteMainDamageMod - 1;
                if (debug) DebugHelper.PrintInfo($"Val after palette: {damageModifier}");
            }

            if (Attribute.GetCustomAttribute(weaponInstance.GetType(), typeof(OrderWeaponAttribute)) != null && player.HasAccessory<OrderEmblem>())
            {
                damageModifier += OrderEmblem.OrderWeaponDamageBonus;
            }

            // Sub power bonus
            if (weaponInstance.IsSubWeapon)
            {
                // Bonus from hardmode
                damageModifier += Main.hardMode ? WeaponPlayer.HardmodeSubWeaponDamageBonus : 0f;

                // Bonus from accessory
                damageModifier += accMP.subPowerMultiplier - 1;

                // Bonus from main weapon
                if (player.HeldItem.ModItem is BaseWeapon)
                {
                    var heldItem = (BaseWeapon)player.HeldItem.ModItem;

                    if (heldItem.BonusType == SubWeaponBonusType.Damage)
                    {
                        if ((heldItem.BonusSub == SubWeaponType.SplatBomb && weaponInstance is SplatBomb)
                            || (heldItem.BonusSub == SubWeaponType.BurstBomb && weaponInstance is BurstBomb)
                            || (heldItem.BonusSub == SubWeaponType.AngleShooter && weaponInstance is AngleShooter)
                            || (heldItem.BonusSub == SubWeaponType.Sprinkler && weaponInstance is Sprinkler)
                            || (heldItem.BonusSub == SubWeaponType.InkMine && weaponInstance is InkMine)
                            || (heldItem.BonusSub == SubWeaponType.Torpedo && weaponInstance is Torpedo))
                        // Skip point sensor here, as it deals no damage
                        {
                            damageModifier += WeaponKitList.GetWeaponKitSubBonusAmount(heldItem.GetType());
                        }
                    }
                }
                if (debug) DebugHelper.PrintInfo($"Val after sub power bonuses: {damageModifier}");
            }

            // Special power bonus
            if (weaponInstance.IsSpecialWeapon)
            {
                damageModifier += accMP.specialPowerMultiplier - 1;
                if (debug) DebugHelper.PrintInfo($"Val after special power bonuses: {damageModifier}");
            }

            #endregion

            #region Multiplicative

            // Class bonus (eg. from vanilla armors, accessories, etc.)
            if (projectile != null || weaponInstance.IsSubWeapon)
            {
                float classMod = 1f;
                if (player.HeldItem.DamageType == DamageClass.Melee)
                {
                    classMod = player.GetDamage(DamageClass.Melee).ApplyTo(classMod);
                }

                if (player.HeldItem.DamageType == DamageClass.Ranged)
                {
                    classMod = player.GetDamage(DamageClass.Ranged).ApplyTo(classMod);
                }

                classMod = player.GetDamage(DamageClass.Generic).ApplyTo(classMod);
                damageModifier *= classMod;
                if (debug) DebugHelper.PrintInfo($"Val after class bonuses: {damageModifier}");
            }

            if (player.HasAccessory<DropletLocket>() || player.HasAccessory<SorcererLocket>() || player.HeldItem.DamageType == DamageClass.Magic)
            {
                damageModifier = player.GetDamage(DamageClass.Magic).ApplyTo(damageModifier);
            }

            #endregion

            return damageModifier;
        }
    }
}
