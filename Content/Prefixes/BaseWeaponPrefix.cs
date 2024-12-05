using AchiSplatoon2.Content.Items.Weapons;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Prefixes;

internal class BaseWeaponPrefix : BaseItemPrefix
{
    public virtual float PrefixValueModifier => 1f;
    public override PrefixCategory Category => PrefixCategory.Custom;

    // Weapon stat modifiers
    public virtual float DamageModifier => 0f;
    public virtual float UseTimeModifier => 0f;
    public virtual float KnockbackModifier => 0f;
    public virtual float VelocityModifier => 0f;
    public virtual int CritChanceBonus => 0;
    public virtual float InkCostModifier => 0f;

    // Projectile stat modifiers
    public virtual int AimVariation => 0;
    public virtual int EnemyPierceBonus => 0;
    public virtual int ArmorPenetrationBonus => 0;
    public virtual float ChargeSpeedModifier => 0f;
    public virtual float ExplosionRadiusModifier => 0f;
    public virtual int ExtraProjectileBonus => 0;
    public virtual float MeleeDamageModifier => 0f;

    #region Tooltips

    // Localization
    public static LocalizedText VelocityTooltip { get; private set; }
    public static LocalizedText AimVariationTooltip { get; private set; }
    public static LocalizedText EnemyPierceTooltip { get; private set; }
    public static LocalizedText ArmorPenetrationTooltip { get; private set; }
    public static LocalizedText ExplosionRadiusTooltip { get; private set; }
    public static LocalizedText ExtraProjectileTooltip { get; private set; }
    public static LocalizedText MeleeDamageTooltip { get; private set; }
    public static LocalizedText InkCostTooltip { get; private set; }

    public override void SetStaticDefaults()
    {
        VelocityTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(VelocityTooltip)}");
        AimVariationTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(AimVariationTooltip)}");
        EnemyPierceTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(EnemyPierceTooltip)}");
        ArmorPenetrationTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ArmorPenetrationTooltip)}");
        ExplosionRadiusTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ExplosionRadiusTooltip)}");
        ExtraProjectileTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(ExtraProjectileTooltip)}");
        MeleeDamageTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(MeleeDamageTooltip)}");
        InkCostTooltip = Mod.GetLocalization($"{LocalizationCategory}.{nameof(InkCostTooltip)}");
    }

    public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
    {
        if (VelocityModifier != 0f)
        {
            yield return CreateTooltip(VelocityTooltip, VelocityModifier, false);
        }

        if (AimVariation != 0)
        {
            yield return CreateTooltip(AimVariationTooltip, AimVariation, true);
        }

        if (EnemyPierceBonus != 0)
        {
            yield return CreateTooltip(EnemyPierceTooltip, EnemyPierceBonus, false);
        }

        if (ArmorPenetrationBonus != 0)
        {
            yield return CreateTooltip(ArmorPenetrationTooltip, ArmorPenetrationBonus, false);
        }

        if (ExplosionRadiusModifier != 0)
        {
            yield return CreateTooltip(ExplosionRadiusTooltip, ExplosionRadiusModifier, false);
        }

        if (ExtraProjectileBonus != 0)
        {
            yield return CreateTooltip(ExtraProjectileTooltip, ExtraProjectileBonus, false);
        }

        if (MeleeDamageModifier != 0)
        {
            yield return CreateTooltip(MeleeDamageTooltip, MeleeDamageModifier, false);
        }

        if (InkCostModifier != 0)
        {
            yield return CreateTooltip(InkCostTooltip, InkCostModifier, true);
        }
    }

    private string FormatIntModifier(float modifier)
    {
        var symbol = "";
        if (modifier > 0)
        {
            symbol = "+";
        }

        return $"{symbol}{modifier}";
    }

    private string FormatPercentageModifier(float modifier)
    {
        var symbol = "";
        if (modifier > 0)
        {
            symbol = "+";
        }

        return $"{symbol}{(int)(modifier * 100)}%";
    }

    protected TooltipLine CreateTooltip(LocalizedText localizedText, int modifier, bool isModifierAboveZeroBad)
    {
        var text = FormatIntModifier(modifier);
        return new TooltipLine(Mod, $"Prefix{localizedText.Key}", localizedText.Format(text))
        {
            IsModifier = true,
            IsModifierBad = (isModifierAboveZeroBad && modifier > 0) || (!isModifierAboveZeroBad && modifier < 0)
        };
    }

    protected TooltipLine CreateTooltip(LocalizedText localizedText, float modifier, bool isModifierAboveZeroBad)
    {
        var text = FormatPercentageModifier(modifier);
        return new TooltipLine(Mod, $"Prefix{localizedText.Key}", localizedText.Format(text))
        {
            IsModifier = true,
            IsModifierBad = (isModifierAboveZeroBad && modifier > 0) || (!isModifierAboveZeroBad && modifier < 0)
        };
    }

    #endregion

    #region Stat application

    // Use this function to modify these stats for items which have this prefix:
    // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
    public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
    {
        damageMult *= (1 + DamageModifier);
        knockbackMult *= (1 + KnockbackModifier);
        critBonus += (CritChanceBonus);
    }

    public override void Apply(Item item)
    {
        item.useTime = (int)(item.useTime * (1 + UseTimeModifier));
        item.useAnimation = (int)(item.useAnimation * (1 + UseTimeModifier));
    }

    public virtual void ApplyProjectileStats(BaseProjectile projectile)
    {
        projectile.Projectile.velocity *= (1 + VelocityModifier);
        projectile.Projectile.velocity = WoomyMathHelper.AddRotationToVector2(projectile.Projectile.velocity, -AimVariation, AimVariation);

        // If this if-statement isn't here,
        // the projectile will immediately despawn if the value increases from -1 (infinite penetrate) to 0
        if (projectile.Projectile.penetrate > 0)
        {
            projectile.Projectile.penetrate += EnemyPierceBonus;
            projectile.UpdatePierceDamageModifiers();
        }

        projectile.Projectile.ArmorPenetration += ArmorPenetrationBonus;

        projectile.currentInkCost *= (1 + InkCostModifier);
    }

    #endregion

    // Modify the cost of items with this modifier with this function.
    public override void ModifyValue(ref float valueMult)
    {
        valueMult *= 1f * PrefixValueModifier;
    }

    public override bool CanRoll(Item item)
    {
        return item.ModItem is BaseWeapon;
    }
}
