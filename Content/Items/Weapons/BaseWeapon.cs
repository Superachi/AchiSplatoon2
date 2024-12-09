using AchiSplatoon2.Content.Items.Accessories.MainWeaponBoosters;
using AchiSplatoon2.Content.Items.Accessories.Palettes;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brellas;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes.InkCostPrefixes;
using AchiSplatoon2.Content.Prefixes.GeneralPrefixes;
using AchiSplatoon2.Content.Prefixes.StringerPrefixes;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Helpers.WeaponKits;
using Humanizer;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using AchiSplatoon2.Content.EnumsAndConstants;
using Terraria.Audio;
using AchiSplatoon2.Content.Items.Accessories;

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
        Torpedo
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
        public virtual float MuzzleOffsetPx { get; set; } = 0f;
        public virtual Vector2 MuzzleOffset => Vector2.Zero;

        // Main weapon stats
        public virtual float AimDeviation { get => 0f; }
        public virtual bool SlowAerialCharge { get => true; }

        // Sub weapon stats
        public virtual MainWeaponStyle WeaponStyle { get; set; } = MainWeaponStyle.Other;
        public virtual bool IsSubWeapon { get => false; }
        public virtual bool AllowSubWeaponUsage { get => true; }
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
                tooltips.Add(new TooltipLine(Mod, $"SubWeaponUsageHintSub", $"{ColorHelper.TextWithSubWeaponColor("Sub weapon:")} equip in ammo slot to be used by main weapon") { OverrideColor = null });
            }
            else if (IsSpecialWeapon)
            {
                tooltips.Add(new TooltipLine(Mod, $"SpecialWeaponUsageHint", $"{ColorHelper.TextWithSpecialWeaponColor("Special weapon:")} can't be reforged and is only usable when special gauge is filled") { OverrideColor = null });
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, $"SubWeaponUsageHintMain", $"{ColorHelper.TextWithMainWeaponColor("Main weapon:")} can use sub weapons with right-click") { OverrideColor = null });
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
            }
            return subname;
        }

        public BaseProjectile CreateProjectileWithWeaponProperties(Player player, IEntitySource source, Vector2 velocity, bool triggerSpawnMethods = true, BaseWeapon weaponType = null)
        {
            if (weaponType == null) weaponType = this;

            // Offset the projectile's position to match the weapon
            Vector2 weaponOffset = HoldoutOffset() ?? new Vector2(0, 0);
            Vector2 muzzleOffset = Vector2.Add(Vector2.Normalize(velocity) * MuzzleOffsetPx, Vector2.Normalize(velocity) * weaponOffset);
            Vector2 position = player.Center;
            bool canHit = Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0);
            if (canHit)
            {
                position += muzzleOffset;
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
                // SPEC-TODO: Change special activation system

                return true;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return false;
            if (!player.ItemTimeIsZero) return false;

            if (player.HasBuff(BuffID.Cursed)) return false;
            if (player.HasBuff(BuffID.Frozen)) return false;
            if (player.HasBuff(BuffID.Stoned)) return false;

            if (!AllowSubWeaponUsage) return false;

            SearchAndUseSubWeapon(player);

            return false;
        }

        private void SearchAndUseSubWeapon(Player player)
        {
            bool doneSearching = false;

            int[] subWeaponItemIDs = {
                ModContent.ItemType<StarterSplatBomb>(),
                ModContent.ItemType<SplatBomb>(),
                ModContent.ItemType<BurstBomb>(),
                ModContent.ItemType<AngleShooter>(),
                ModContent.ItemType<Sprinkler>(),
                ModContent.ItemType<InkMine>(),
                ModContent.ItemType<Torpedo>()
            };

            // We use 4 here, as there are 4 ammo slots
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < subWeaponItemIDs.Length; j++)
                {
                    if (doneSearching) continue;

                    var item = player.inventory[54 + i];
                    // Ammo slots range from 54-57
                    // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory
                    if (item.type == subWeaponItemIDs[j])
                    {
                        var subWeapon = (BaseBomb)item.ModItem;
                        var baseInkCost = WoomyMathHelper.CalculateWeaponInkCost(subWeapon, player);
                        var discount = 0f;

                        if (BonusSub != SubWeaponType.None)
                        {
                            SubWeaponType currentlyCheckedSub = (SubWeaponType)(j + 1);
                            if (BonusType == SubWeaponBonusType.Discount && currentlyCheckedSub == BonusSub)
                            {
                                discount = baseInkCost * SubBonusAmount;
                            }
                        }

                        var inkTankPlayer = player.GetModPlayer<InkTankPlayer>();
                        if (!inkTankPlayer.HasEnoughInk(baseInkCost - discount))
                        {
                            doneSearching = true;
                            break;
                        }

                        if (discount > 0)
                        {
                            inkTankPlayer.HealInk(discount, false);
                        }

                        // Calculate throw angle and spawn projectile
                        float aimAngle = MathHelper.ToDegrees(
                            player.DirectionTo(Main.MouseWorld).ToRotation()
                        );

                        float radians = MathHelper.ToRadians(aimAngle);
                        Vector2 angleVector = radians.ToRotationVector2();
                        Vector2 velocity = angleVector;
                        var source = new EntitySource_ItemUse_WithAmmo(player, item, item.ammo);

                        var p = CreateProjectileWithWeaponProperties(
                            player: player,
                            source: source,
                            velocity: velocity * item.shootSpeed,
                            weaponType: (BaseWeapon)item.ModItem,
                            triggerSpawnMethods: false
                            );
                        p.Projectile.position = player.Center;
                        p.itemIdentifier = item.type;
                        p.weaponSourcePrefix = item.prefix;
                        p.RunSpawnMethods();

                        player.itemTime = item.useTime;

                        doneSearching = true;
                        break;
                    }
                }
            }

            if (!doneSearching)
            {
                player.GetModPlayer<HudPlayer>().SetOverheadText("No sub weapon equipped!", displayTime: 90);
                SoundHelper.PlayAudio(SoundPaths.EmptyInkTank.ToSoundStyle(), volume: 0.5f);
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
                case BaseSplatana:
                    possiblePrefixes = PrefixHelper.ListChargeWeaponsPrefixes();
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
            }

            return rand.NextFromCollection(possiblePrefixes);
        }
    }
}
