using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers.WeaponKits;
using Terraria;
using Terraria.ModLoader;

namespace AchiSplatoon2.Helpers
{
    internal static class StatCalculationHelper
    {
        public static float CalculateDamageModifiers(Player player, BaseWeapon weaponInstance, BaseProjectile projectile = null, bool debug = false)
        {
            float damageModifier = 1f;
            var wepMP = player.GetModPlayer<InkWeaponPlayer>();
            var accMP = player.GetModPlayer<InkAccessoryPlayer>();
            if (debug) DebugHelper.PrintInfo($"Start val: {damageModifier}");

            #region Additive

            // Red color chip bonus
            damageModifier += wepMP.CalculateAttackDamageBonus();
            if (debug) DebugHelper.PrintInfo($"Val after red chip: {damageModifier}");

            // Pallete bonus
            if (BaseWeapon.DoesPaletteBoostMainWeapon(weaponInstance, player))
            {
                damageModifier += wepMP.PaletteMainDamageMod  - 1;
                if (debug) DebugHelper.PrintInfo($"Val after palette: {damageModifier}");
            }

            // Sub power bonus
            if (weaponInstance.IsSubWeapon)
            {
                // Bonus from accessory
                damageModifier += accMP.subPowerMultiplier - 1;

                // Bonus from main weapon
                if (player.HeldItem.ModItem is BaseWeapon)
                {
                    var heldItem = (BaseWeapon)player.HeldItem.ModItem;

                    if (heldItem.BonusType == SubWeaponBonusType.Damage)
                    {
                        if ((heldItem.BonusSub == SubWeaponType.SplatBomb       && weaponInstance is SplatBomb)
                            || (heldItem.BonusSub == SubWeaponType.BurstBomb    && weaponInstance is BurstBomb)
                            || (heldItem.BonusSub == SubWeaponType.AngleShooter && weaponInstance is AngleShooter)
                            || (heldItem.BonusSub == SubWeaponType.Sprinkler    && weaponInstance is Sprinkler)
                            || (heldItem.BonusSub == SubWeaponType.InkMine      && weaponInstance is InkMine)) {
                            damageModifier += WeaponKitList.GetWeaponKitSubBonusAmount(heldItem.GetType());
                        }
                    }

                    if (heldItem is SplattershotJr)
                    {
                        damageModifier /= 2;
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
                else if (player.HeldItem.DamageType == DamageClass.Ranged)
                {
                    classMod = player.GetDamage(DamageClass.Ranged).ApplyTo(classMod);
                }
                   
                classMod = player.GetDamage(DamageClass.Generic).ApplyTo(classMod);
                damageModifier *= classMod;
                if (debug) DebugHelper.PrintInfo($"Val after class bonuses: {damageModifier}");
            }

            #endregion

            return damageModifier;
        }

        // NOTE! This data is based on the decompiled tmodloader source code. It may differ from in-game stats
        public static void GetPrefixStats(Item item, out float damage, out float knockback, out float speed, out float size, out float shootSpeed, out float manaCost, out float crit)
        {
            damage = 1f;
            knockback = 1f;
            speed = 1f;
            size = 1f;
            shootSpeed = 1f;
            manaCost = 1f;
            crit = 0;

            switch (item.prefix)
            {
                case 1:
                    size = 1.12f;
                    break;
                case 2:
                    size = 1.18f;
                    break;
                case 3:
                    damage = 1.05f;
                    crit = 2;
                    size = 1.05f;
                    break;
                case 4:
                    damage = 1.1f;
                    size = 1.1f;
                    knockback = 1.1f;
                    break;
                case 5:
                    damage = 1.15f;
                    break;
                case 6:
                    damage = 1.1f;
                    break;
                case 81:
                    knockback = 1.15f;
                    damage = 1.15f;
                    crit = 5;
                    speed = 0.9f;
                    size = 1.1f;
                    break;
                case 7:
                    size = 0.82f;
                    break;
                case 8:
                    knockback = 0.85f;
                    damage = 0.85f;
                    size = 0.87f;
                    break;
                case 9:
                    size = 0.9f;
                    break;
                case 10:
                    damage = 0.85f;
                    break;
                case 11:
                    speed = 1.1f;
                    knockback = 0.9f;
                    size = 0.9f;
                    break;
                case 12:
                    knockback = 1.1f;
                    damage = 1.05f;
                    size = 1.1f;
                    speed = 1.15f;
                    break;
                case 13:
                    knockback = 0.8f;
                    damage = 0.9f;
                    size = 1.1f;
                    break;
                case 14:
                    knockback = 1.15f;
                    speed = 1.1f;
                    break;
                case 15:
                    knockback = 0.9f;
                    speed = 0.85f;
                    break;
                case 16:
                    damage = 1.1f;
                    crit = 3;
                    break;
                case 17:
                    speed = 0.85f;
                    shootSpeed = 1.1f;
                    break;
                case 18:
                    speed = 0.9f;
                    shootSpeed = 1.15f;
                    break;
                case 19:
                    knockback = 1.15f;
                    shootSpeed = 1.05f;
                    break;
                case 20:
                    knockback = 1.05f;
                    shootSpeed = 1.05f;
                    damage = 1.1f;
                    speed = 0.95f;
                    crit = 2;
                    break;
                case 21:
                    knockback = 1.15f;
                    damage = 1.1f;
                    break;
                case 82:
                    knockback = 1.15f;
                    damage = 1.15f;
                    crit = 5;
                    speed = 0.9f;
                    shootSpeed = 1.1f;
                    break;
                case 22:
                    knockback = 0.9f;
                    shootSpeed = 0.9f;
                    damage = 0.85f;
                    break;
                case 23:
                    speed = 1.15f;
                    shootSpeed = 0.9f;
                    break;
                case 24:
                    speed = 1.1f;
                    knockback = 0.8f;
                    break;
                case 25:
                    speed = 1.1f;
                    damage = 1.15f;
                    crit = 1;
                    break;
                case 58:
                    speed = 0.85f;
                    damage = 0.85f;
                    break;
                case 26:
                    manaCost = 0.85f;
                    damage = 1.1f;
                    break;
                case 27:
                    manaCost = 0.85f;
                    break;
                case 28:
                    manaCost = 0.85f;
                    damage = 1.15f;
                    knockback = 1.05f;
                    break;
                case 83:
                    knockback = 1.15f;
                    damage = 1.15f;
                    crit = 5;
                    speed = 0.9f;
                    manaCost = 0.9f;
                    break;
                case 29:
                    manaCost = 1.1f;
                    break;
                case 30:
                    manaCost = 1.2f;
                    damage = 0.9f;
                    break;
                case 31:
                    knockback = 0.9f;
                    damage = 0.9f;
                    break;
                case 32:
                    manaCost = 1.15f;
                    damage = 1.1f;
                    break;
                case 33:
                    manaCost = 1.1f;
                    knockback = 1.1f;
                    speed = 0.9f;
                    break;
                case 34:
                    manaCost = 0.9f;
                    knockback = 1.1f;
                    speed = 1.1f;
                    damage = 1.1f;
                    break;
                case 35:
                    manaCost = 1.2f;
                    damage = 1.15f;
                    knockback = 1.15f;
                    break;
                case 52:
                    manaCost = 0.9f;
                    damage = 0.9f;
                    speed = 0.9f;
                    break;
                case 84:
                    knockback = 1.17f;
                    damage = 1.17f;
                    crit = 8;
                    break;
                case 36:
                    crit = 3;
                    break;
                case 37:
                    damage = 1.1f;
                    crit = 3;
                    knockback = 1.1f;
                    break;
                case 38:
                    knockback = 1.15f;
                    break;
                case 53:
                    damage = 1.1f;
                    break;
                case 54:
                    knockback = 1.15f;
                    break;
                case 55:
                    knockback = 1.15f;
                    damage = 1.05f;
                    break;
                case 59:
                    knockback = 1.15f;
                    damage = 1.15f;
                    crit = 5;
                    break;
                case 60:
                    damage = 1.15f;
                    crit = 5;
                    break;
                case 61:
                    crit = 5;
                    break;
                case 39:
                    damage = 0.7f;
                    knockback = 0.8f;
                    break;
                case 40:
                    damage = 0.85f;
                    break;
                case 56:
                    knockback = 0.8f;
                    break;
                case 41:
                    knockback = 0.85f;
                    damage = 0.9f;
                    break;
                case 57:
                    knockback = 0.9f;
                    damage = 1.18f;
                    break;
                case 42:
                    speed = 0.9f;
                    break;
                case 43:
                    damage = 1.1f;
                    speed = 0.9f;
                    break;
                case 44:
                    speed = 0.9f;
                    crit = 3;
                    break;
                case 45:
                    speed = 0.95f;
                    break;
                case 46:
                    crit = 3;
                    speed = 0.94f;
                    damage = 1.07f;
                    break;
                case 47:
                    speed = 1.15f;
                    break;
                case 48:
                    speed = 1.2f;
                    break;
                case 49:
                    speed = 1.08f;
                    break;
                case 50:
                    damage = 0.8f;
                    speed = 1.15f;
                    break;
                case 51:
                    knockback = 0.9f;
                    speed = 0.9f;
                    damage = 1.05f;
                    crit = 2;
                    break;
            }
        }
    }
}
