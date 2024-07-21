using AchiSplatoon2.Content.Items.Accessories.Palettes;
using AchiSplatoon2.Content.Items.CraftingMaterials;
using AchiSplatoon2.Content.Items.Weapons.Blasters;
using AchiSplatoon2.Content.Items.Weapons.Bows;
using AchiSplatoon2.Content.Items.Weapons.Brushes;
using AchiSplatoon2.Content.Items.Weapons.Chargers;
using AchiSplatoon2.Content.Items.Weapons.Dualies;
using AchiSplatoon2.Content.Items.Weapons.Shooters;
using AchiSplatoon2.Content.Items.Weapons.Sloshers;
using AchiSplatoon2.Content.Items.Weapons.Specials;
using AchiSplatoon2.Content.Items.Weapons.Splatana;
using AchiSplatoon2.Content.Items.Weapons.Splatling;
using AchiSplatoon2.Content.Items.Weapons.Throwing;
using AchiSplatoon2.Content.Players;
using AchiSplatoon2.Content.Projectiles;
using AchiSplatoon2.Helpers;
using AchiSplatoon2.Helpers.WeaponKits;
using Humanizer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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
        Sprinkler
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
        public virtual string ShootSample { get => "SplattershotShoot"; }
        public virtual string ShootWeakSample { get => "SplattershotShoot"; }
        public virtual string ShootAltSample { get => "SplattershotShoot"; }
        public virtual float MuzzleOffsetPx { get; set; } = 0f;

        // Main weapon stats
        public virtual float AimDeviation { get => 0f; }

        // Sub weapon stats
        public virtual MainWeaponStyle WeaponStyle { get; set; } = MainWeaponStyle.Other;
        public virtual bool IsSubWeapon { get => false; }
        public virtual bool AllowSubWeaponUsage { get => true; }
        public SubWeaponType BonusSub { get; private set; }
        public SubWeaponBonusType BonusType { get; private set; }

        public const float subDiscountChance = 0.5f;
        public const float subDamageBonus = 0.5f;
        protected static int[] subWeaponItemIDs = {
            ModContent.ItemType<SplatBomb>(),
            ModContent.ItemType<BurstBomb>(),
            ModContent.ItemType<AngleShooter>(),
            ModContent.ItemType<Sprinkler>(),
        };

        // Special weapon stats
        public virtual bool IsSpecialWeapon { get => false; }
        public virtual bool IsDurationSpecial { get => false; }
        public virtual float SpecialDrainPerUse { get => 0f; }
        public virtual float SpecialDrainPerTick { get => 0f; }

        public override void SetDefaults()
        {
            ItemIdentifier = Item.type;
            BonusSub = WeaponKitList.GetWeaponKitSubType(this.GetType());
            BonusType = WeaponKitList.GetWeaponKitSubBonusType(this.GetType());
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
            var accMP = player.GetModPlayer<InkAccessoryPlayer>();
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
                var wepMP = player.GetModPlayer<InkWeaponPlayer>();
                var accMP = player.GetModPlayer<InkAccessoryPlayer>();
                if (IsSpecialWeapon)
                {
                    damage *= accMP.specialPowerMultiplier;
                    return;
                }

                if (DoesPaletteBoostMainWeapon(this, player))
                {
                    damage *= wepMP.PaletteMainDamageMod;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
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
                    var tooltip = new TooltipLine(Mod, $"SubWeaponDiscountTooltip", $"{ColorHelper.TextWithBonusColor($"{(int)(subDiscountChance * 100f)}% chance to not consume {GetSubWeaponName(BonusSub)}")}") { OverrideColor = null };
                    tooltips.Add(tooltip);
                }

                if (BonusType == SubWeaponBonusType.Damage)
                {
                    var tooltip = new TooltipLine(Mod, $"SubWeaponDamageTooltip", $"{ColorHelper.TextWithBonusColor($"+{(int)(subDamageBonus * 100f)}% {GetSubWeaponName(BonusSub)} damage")}") { OverrideColor = null };
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

        public BaseProjectile CreateProjectileWithWeaponProperties(Player player, IEntitySource source, Vector2 velocity, bool triggerAfterSpawn = true, BaseWeapon weaponType = null)
        {
            var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
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
            proj.WeaponInstance = (BaseWeapon)Activator.CreateInstance(weaponType.GetType());
            proj.itemIdentifier = ItemIdentifier;

            // If throwing a sub weapon directly, apply damage modifiers
            if (this is BaseBomb)
            {
                BaseBomb bomb = (BaseBomb)this;
                proj.Projectile.damage = (int)(proj.Projectile.damage * bomb.CalculateDamageMod(player));
            }

            if (this is BaseSpecial && IsSpecialWeapon)
            {
                BaseSpecial special = (BaseSpecial)this;
                proj.Projectile.damage = (int)(proj.Projectile.damage * modPlayer.CalculateSpecialDamageBonusModifier());
            }

            if (triggerAfterSpawn) proj.AfterSpawn();
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
            if (!IsSpecialWeapon) {
                return base.CanUseItem(player);
            }
            else
            {
                var modPlayer = player.GetModPlayer<InkWeaponPlayer>();
                if (!modPlayer.SpecialReady
                    || (modPlayer.IsSpecialActive && IsDurationSpecial)
                    || (modPlayer.SpecialName != null && modPlayer.SpecialName != player.HeldItem.Name)
                    || player.altFunctionUse == 2)
                {
                    // DebugHelper.PrintWarning($"{player.name} | {modPlayer.SpecialReady} | {modPlayer.IsSpecialActive} | {modPlayer.SpecialName} | {player.altFunctionUse}");
                    player.itemTime = 30;
                    return false;
                }

                modPlayer.DrainSpecial(SpecialDrainPerUse);
                modPlayer.ActivateSpecial(SpecialDrainPerTick, player.HeldItem.Name);
                return true;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            if (!NetHelper.IsPlayerSameAsLocalPlayer(player)) return false;
            if (!player.ItemTimeIsZero) return false;
            if (!AllowSubWeaponUsage) return false;

            bool doneSearching = false;

            Type[] subWeaponType = {
                typeof(SplatBomb),
                typeof(BurstBomb),
                typeof(AngleShooter),
                typeof(Sprinkler)
            };

            // We use 4 here, as there are 4 ammo slots
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < subWeaponItemIDs.Length; j++)
                {
                    if (!doneSearching)
                    {
                        var item = player.inventory[54 + i];
                        // Ammo slots range from 54-57
                        // http://docs.tmodloader.net/docs/stable/class_player -> Player.inventory
                        if (item.type == subWeaponItemIDs[j])
                        {
                            // Check if the main weapon has a bonus that discounts sub weapons of a matching type
                            // Eg. Splattershot has a chance to not consume burst bombs
                            bool luckyDiscount = false;
                            float damageBonus = 1f;

                            bool hasMainWeaponBonus = false;
                            if (BonusSub != SubWeaponType.None)
                            {
                                SubWeaponType currentlyCheckedSub = (SubWeaponType)(j + 1);
                                if (BonusType == SubWeaponBonusType.Discount && currentlyCheckedSub == BonusSub)
                                {
                                    luckyDiscount = Main.rand.NextBool((int)(1f / subDiscountChance));
                                }

                                hasMainWeaponBonus = BonusType == SubWeaponBonusType.Damage && currentlyCheckedSub == BonusSub;
                            }
                            var mp = player.GetModPlayer<InkWeaponPlayer>();
                            damageBonus = mp.CalculateSubDamageBonusModifier(hasMainWeaponBonus);

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

                            var p = CreateProjectileWithWeaponProperties(
                                player: player,
                                source: source,
                                velocity: velocity * item.shootSpeed,
                                weaponType: (BaseWeapon)item.ModItem,
                                triggerAfterSpawn: false
                                );
                            p.Projectile.damage = (int)(p.Projectile.damage * damageBonus);
                            p.Projectile.position = player.Center;
                            p.itemIdentifier = item.type;
                            p.AfterSpawn();

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

            return false;
        }
    }
}
