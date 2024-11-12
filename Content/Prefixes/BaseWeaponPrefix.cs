using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;

namespace AchiSplatoon2.Content.Prefixes;

internal class BaseWeaponPrefix : BaseItemPrefix
{
    public virtual float PrefixValueModifier => 1f;
    public override PrefixCategory Category => PrefixCategory.Custom;

    // Weapon stat modifiers
    public virtual float DamageModifier => 1f;
    public virtual float UseTimeModifier => 1f;
    public virtual float KnockbackModifier => 1f;
    public virtual float VelocityModifier => 1f;
    public virtual int CritChanceBonus => 0;

    // Projectile stat modifiers
    public virtual int AimVariation => 0;
    public virtual int EnemyPierceBonus => 0;
    public virtual int ArmorPenetrationBonus => 0;
    public virtual float ChargeSpeedModifier => 1f;
    public virtual float ExplosionRadiusModifier => 1f;


    public override bool CanRoll(Item item)
    {
        return item.ModItem is BaseWeapon;
    }

    // Use this function to modify these stats for items which have this prefix:
    // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        damageMult *= DamageModifier;
        knockbackMult *= KnockbackModifier;
        shootSpeedMult *= VelocityModifier;
        critBonus += CritChanceBonus;
    }

    // Modify the cost of items with this modifier with this function.
    public override void ModifyValue(ref float valueMult)
    {
        valueMult *= 1f * PrefixValueModifier;
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        if (AimVariation != 0)
        {
            yield return new TooltipLine(Mod, "PrefixAimVariation", AimVariationTooltip.Format(AimVariation * 2))
            {
                IsModifier = true,
                IsModifierBad = true
            };
        }

        if (EnemyPierceBonus != 0)
        {
            yield return new TooltipLine(Mod, "PrefixPierce", PierceTooltip.Format(EnemyPierceBonus))
            {
                IsModifier = true,
                IsModifierBad = false
            };
        }

        if (ArmorPenetrationBonus != 0)
        {
            yield return new TooltipLine(Mod, "PrefixPenetration", PenetrationTooltip.Format(ArmorPenetrationBonus))
            {
                IsModifier = true,
                IsModifierBad = false
            };
        }
    }

    public static LocalizedText AimVariationTooltip { get; private set; }
    public static LocalizedText PierceTooltip { get; private set; }
    public static LocalizedText PenetrationTooltip { get; private set; }

    public override void Apply(Item item)
    {
        item.useTime = (int)(item.useTime * UseTimeModifier);
        item.useAnimation = (int)(item.useAnimation * UseTimeModifier);
    }

    public virtual void ApplyProjectileStats(BaseProjectile projectile)
    {
        projectile.Projectile.velocity = WoomyMathHelper.AddRotationToVector2(projectile.Projectile.velocity, -AimVariation, AimVariation);
        projectile.Projectile.penetrate += EnemyPierceBonus;
        projectile.Projectile.ArmorPenetration += ArmorPenetrationBonus;
    }

    public override void SetStaticDefaults()
    {
        AimVariationTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(AimVariationTooltip)}");
        PierceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(PierceTooltip)}");
        PenetrationTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(PenetrationTooltip)}");
    }
}
