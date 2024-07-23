using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles;
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
                            || (heldItem.BonusSub == SubWeaponType.Sprinkler    && weaponInstance is Sprinkler)) {
                            damageModifier += BaseWeapon.subDamageBonus;
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
    }
}
