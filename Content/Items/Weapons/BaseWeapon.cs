using AchiSplatoon2.Content.EnumsAndConstants;
using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Accessories.Palettes;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Rollers;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Prefixes.ChargeWeaponPrefixes;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes.InkCostPrefixes;
using AchiSplatoon2.Content.Prefixes.SlosherPrefixes;
using AchiSplatoon2.Content.Prefixes.StringerPrefixes;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Helpers.WeaponKits;
using Humanizer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AchiSplatoon2.Content.Items.Weapons
{
    enum MainWeaponStyle
    {
        Other,
        Shooter,
        Roller,
        Charger,
        Slosher,
        Splatling,
        Dualies,
        Brella,
        Blaster,
        Brush,
        Stringer,
        Splatana
    }

    enum SubWeaponType
    {
        None,
        SplatBomb,
        BurstBomb,
        AngleShooter,
        Sprinkler,
        InkMine,
        Torpedo,
        PointSensor
    }

    enum SubWeaponBonusType
    {
        None,
        Discount,
        Damage
    }

    internal class BaseWeapon : BaseItem
    {
        public int ItemIdentifier { get; private set; }

        // Visual
        public virtual SoundStyle ShootSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public virtual SoundStyle ShootWeakSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public virtual SoundStyle ShootAltSample { get => SoundPaths.SplattershotShoot.ToSoundStyle(); }
        public virtual Vector2 MuzzleOffset => Vector2.Zero;

        // Main weapon stats
        public virtual float AimDeviation { get => 0f; }
        public virtual bool SlowAerialCharge { get => true; }

        // Sub weapon stats
        public virtual MainWeaponStyle WeaponStyle { get; set; } = MainWeaponStyle.Other;
        public virtual bool IsSubWeapon { get => false; }
        public SubWeaponType BonusSub { get; private set; }
        public SubWeaponBonusType BonusType { get; private set; }

        public float SubBonusAmount { get; private set; }

        // Ink stats
        public virtual float InkCost { get => 0f; }
        public virtual float InkRecoveryDelay { get => 0f; }

        // Special weapon stats
        public virtual bool IsSpecialWeapon { get => false; }
        public virtual bool IsDurationSpecial { get => false; }
        public virtual float SpecialDrainPerUse { get => 0f; }
        public virtual float SpecialDrainPerTick { get => 0f; }

        // Determines the points you need to charge the next special
        public virtual float RechargeCostPenalty { get => 0f; }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<StatisticsPlayer>().attacksUsed++;
            return base.UseItem(player);
        }

        public override void SetDefaults()
        {
            ItemIdentifier = Item.type;
            BonusSub = WeaponKitList.GetWeaponKitSubType(this.GetType());
            BonusType = WeaponKitList.GetWeaponKitSubBonusType(this.GetType());
            SubBonusAmount = WeaponKitList.GetWeaponKitSubBonusAmount(this.GetType());
        }

        public void RangedWeaponDefaults(int projectileType, int singleShotTime, float shotVelocity)
        {
            Item.DefaultToRangedWeapon(
                baseProjType: projectileType,
                ammoID: AmmoID.None,
                singleShotTime: singleShotTime,
                shotVelocity: shotVelocity,
                hasAutoReuse: true);
        }

        public static bool DoesPaletteBoostMainWeapon(BaseWeapon usedWeapon, Player player)
        {
            var accMP = player.GetModPlayer<AccessoryPlayer>();
            if (accMP.paletteType == null) return false;

            ChipPalette palette = (ChipPalette)Activator.CreateInstance(accMP.paletteType);
            MainWeaponStyle paletteWeaponStyle = palette.PaletteWeaponStyle();
            MainWeaponStyle usedWeaponStyle = usedWeapon.WeaponStyle;
            if (paletteWeaponStyle == MainWeaponStyle.Other) return false;

            return usedWeaponStyle == paletteWeaponStyle;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            base.ModifyWeaponDamage(player, ref damage);

            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                float damageMod = StatCalculationHelper.CalculateDamageModifiers(player, this);
                damage *= damageMod;
            }
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            if (NetHelper.IsPlayerSameAsLocalPlayer(player))
            {
                var accMP = player.GetModPlayer<AccessoryPlayer>();
                if (accMP.hasTentacleScope && WeaponStyle == MainWeaponStyle.Charger)
                {
                    crit += TentacularOcular.BaseCritChance;
                }

                crit += StatCalculationHelper.CalculateCritModifiers(player, this);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Item != null)
            {
                var baseInkCost = WoomyMathHelper.CalculateWeaponInkCost(this, Main.LocalPlayer, Item.prefix);
                if (IsChargingWeapon(this))
                {
                    baseInkCost = WoomyMathHelper.CalculateChargeInkCost(baseInkCost, this, true);
                }

                if (baseInkCost > 0)
                {
                    tooltips.Add(new TooltipLine(Mod, $"InkCost", $"Uses {baseInkCost.ToString("0.0")}% ink") { OverrideColor = null });
                }
            }

            // Usage hint 
            string usageVal = this.GetLocalizedValue("UsageHint").FormatWith(UsageHintParamA, UsageHintParamB);
            if (usageVal != this.GetLocalizationKey("UsageHint"))
            {
                var usageHint = new TooltipLine(Mod, "UsageHint", ColorHelper.TextWithFunctionalColor(usageVal)) { OverrideColor = null };
                tooltips.Add(usageHint);
            }

            // Weapon hint + sub bonus
            if (IsSubWeapon)
            {
                tooltips.Add(new TooltipLine(Mod, $"SubWeaponUsageHintSub", $"{ColorHelper.TextWithSubWeaponColor("Sub weapon:")} equipable in ammo slot, usable via right-click") { OverrideColor = null });
            }
            else if (IsSpecialWeapon)
            {
                tooltips.Add(new TooltipLine(Mod, $"SpecialWeaponUsageHint", $"{ColorHelper.TextWithSpecialWeaponColor("Special weapon:")} Defeat enemies and fill your special gauge, then middle-click when ready") { OverrideColor = null });

                if (RechargeCostPenalty > 0)
                {
                    var tip = $"When used, your next special requires " + ColorHelper.TextWithSpecialWeaponColor($"{(int)RechargeCostPenalty} points ") + "to charge";
                    tooltips.Add(new TooltipLine(Mod, "SpecialCost", tip) { OverrideColor = null });
                }
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, $"SubWeaponUsageHintMain", $"{ColorHelper.TextWithMainWeaponColor("Main weapon:")} enables sub weapon usage via right-click") { OverrideColor = null });
            }

            if (BonusSub != SubWeaponType.None)
            {
                if (BonusType == SubWeaponBonusType.Discount)
                {
                    var tooltip = new TooltipLine(Mod, $"SubWeaponDiscountTooltip", $"{ColorHelper.TextWithBonusColor($"{GetSubWeaponName(BonusSub)} uses {(int)(SubBonusAmount * 100f)}% less ink")}") { OverrideColor = null };
                    tooltips.Add(tooltip);
                }

                if (BonusType == SubWeaponBonusType.Damage)
                {
                    var tooltip = new TooltipLine(Mod, $"SubWeaponDamageTooltip", $"{ColorHelper.TextWithBonusColor($"+{(int)(SubBonusAmount * 100f)}% {GetSubWeaponName(BonusSub)} damage")}") { OverrideColor = null };
                    tooltips.Add(tooltip);
                }
            }

            // Flavor text
            string flavorVal = this.GetLocalizedValue("Flavor");
            if (flavorVal != this.GetLocalizationKey("Flavor"))
            {
                var flavor = new TooltipLine(Mod, "Flavor", $"{ColorHelper.TextWithFlavorColorAndQuotes(flavorVal)}") { OverrideColor = null };
                tooltips.Add(flavor);
            }
        }

        public static bool IsChargingWeapon(BaseWeapon weapon)
        {
            return weapon.WeaponStyle == MainWeaponStyle.Charger || weapon.WeaponStyle == MainWeaponStyle.Stringer || weapon.WeaponStyle == MainWeaponStyle.Splatana || weapon.WeaponStyle == MainWeaponStyle.Splatling;
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
                case SubWeaponType.InkMine:
                    subname = "Ink Mine";
                    break;
                case SubWeaponType.Torpedo:
                    subname = "Torpedo";
                    break;
                case SubWeaponType.PointSensor:
                    subname = "Point Sensor";
                    break;
            }
            return subname;
        }

        public BaseProjectile CreateProjectileWithWeaponProperties(Player player, IEntitySource source, Vector2 velocity, bool triggerSpawnMethods = true, BaseWeapon weaponType = null)
        {
            if (weaponType == null) weaponType = this;

            // Offset the projectile's position to match the weapon
            Vector2 weaponOffset = HoldoutOffset() ?? new Vector2(0, 0);

            Vector2 finalMuzzleOffset = Vector2.Zero;
            if (MuzzleOffset != Vector2.Zero)
            {
                var shotAngle = velocity.ToRotation();
                shotAngle = MathHelper.ToDegrees(shotAngle);

                var baseMuzzleOffset = MuzzleOffset;
                if (player.direction == -1)
                {
                    baseMuzzleOffset.Y *= player.direction;
                }

                finalMuzzleOffset += WoomyMathHelper.AddRotationToVector2(baseMuzzleOffset, shotAngle) + WoomyMathHelper.AddRotationToVector2(weaponOffset, shotAngle);
            }

            Vector2 position = player.Center;
            bool canHit = Collision.CanHit(position, 0, 0, position + finalMuzzleOffset, 0, 0);
            if (canHit)
            {
                position += finalMuzzleOffset;
            }

            // Spawn the projectile
            var p = Projectile.NewProjectileDirect(
                spawnSource: source,
                position: position,
                velocity: velocity,
                type: weaponType.Item.shoot,
                damage: weaponType.Item.damage,
                knockback: weaponType.Item.knockBack,
                owner: player.whoAmI);
            var proj = p.ModProjectile as BaseProjectile;

            // Config variables after spawning
            proj.weaponSourcePrefix = Item.prefix;
            proj.WeaponInstance = (BaseWeapon)Activator.CreateInstance(weaponType.GetType());
            proj.itemIdentifier = ItemIdentifier;
            proj.SetInkCost(WoomyMathHelper.CalculateWeaponInkCost(weaponType, player));

            if (triggerSpawnMethods) proj.RunSpawnMethods();
            return proj;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CreateProjectileWithWeaponProperties(
            player: player,
            source: source,
            velocity: velocity);

            return false;
        }

        public override bool CanUseItem(Player player)
        {
            var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
            if (inkTankPlayer.InkAmount < WoomyMathHelper.CalculateWeaponInkCost(this, player))
            {
                inkTankPlayer.CreateLowInkPopup();
                inkTankPlayer.InkRecoveryDelay = Math.Max(inkTankPlayer.InkRecoveryDelay, 30);
                return false;
            }

            var weaponPlayer = player.GetModPlayer<WeaponPlayer>();
            if (weaponPlayer.CustomWeaponCooldown > 0) return false;

            if (!IsSpecialWeapon)
            {
                return base.CanUseItem(player);
            }
            else
            {
                return false;
            }
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            var possiblePrefixes = PrefixHelper.ListGenericPrefixes();

            switch (this)
            {
                case BaseSplattershot:
                    possiblePrefixes = PrefixHelper.ListShooterPrefixes();

                    if (this is Dot52Gal)
                    {
                        possiblePrefixes = new()
                        {
                            ModContent.PrefixType<RangedPrefix>(),
                            ModContent.PrefixType<PiercingPrefix>(),
                            ModContent.PrefixType<SkilledPrefix>(),
                            ModContent.PrefixType<SwiftPrefix>(),
                            ModContent.PrefixType<SavouryPrefix>(),

                            ModContent.PrefixType<TastelessPrefix>()
                        };
                    }

                    break;

                case BaseBlaster:
                    possiblePrefixes = PrefixHelper.ListBlasterPrefixes();
                    break;

                case BaseDualie:
                    possiblePrefixes = PrefixHelper.ListDualiePrefixes();
                    break;

                case BaseCharger:
                    possiblePrefixes = PrefixHelper.ListChargerPrefixes();
                    break;

                case BaseSlosher:
                    possiblePrefixes = PrefixHelper.ListSlosherPrefixes();


                    if (this is Bloblobber)
                    {
                        possiblePrefixes.Remove(ModContent.PrefixType<OversizedPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<ResonantPrefix>());
                    }
                    else if (this is Explosher)
                    {
                        possiblePrefixes.Remove(ModContent.PrefixType<OversizedPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<ResonantPrefix>());

                        possiblePrefixes.Remove(ModContent.PrefixType<DeepCutPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<PiercingPrefix>());
                    }
                    else
                    {
                        possiblePrefixes.Remove(ModContent.PrefixType<DeepCutPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<PiercingPrefix>());

                        possiblePrefixes.Remove(ModContent.PrefixType<HeavyDutyPrefix>());
                    }
                    break;

                case BaseSplatana:

                    possiblePrefixes = PrefixHelper.ListChargeWeaponsPrefixes();

                    if (this is GolemSplatana)
                    {
                        possiblePrefixes.Remove(ModContent.PrefixType<BacklinePrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<RangedPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<PiercingPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<DeepCutPrefix>());
                        possiblePrefixes.Remove(ModContent.PrefixType<TastelessPrefix>());
                    }

                    break;

                case BaseSplatling:
                    possiblePrefixes = PrefixHelper.ListSplatlingPrefixes();
                    if (WeaponStyle == MainWeaponStyle.Charger)
                    {
                        possiblePrefixes = PrefixHelper.ListChargeWeaponsPrefixes();
                    }
                    break;

                case BaseStringer:
                    possiblePrefixes = PrefixHelper.ListStringerPrefixes();

                    if (this is IceStringer)
                    {
                        possiblePrefixes.Remove(ModContent.PrefixType<CompactPrefix>());
                    }

                    break;

                case BaseBrush:
                    possiblePrefixes = PrefixHelper.ListBrushPrefixes();
                    break;

                case BaseBrella:
                    possiblePrefixes = PrefixHelper.ListBrellaPrefixes();
                    break;

                case BaseRoller:
                    possiblePrefixes.Remove(ModContent.PrefixType<DeepCutPrefix>());
                    possiblePrefixes.Remove(ModContent.PrefixType<PiercingPrefix>());
                    break;
            }

            if (InkCost == 0)
            {
                if (possiblePrefixes.Contains(ModContent.PrefixType<SavouryPrefix>()))
                {
                    possiblePrefixes.Remove(ModContent.PrefixType<SavouryPrefix>());
                }

                if (possiblePrefixes.Contains(ModContent.PrefixType<CheapPrefix>()))
                {
                    possiblePrefixes.Remove(ModContent.PrefixType<CheapPrefix>());
                }
            }

            return rand.NextFromCollection(possiblePrefixes);
        }
    }
}
