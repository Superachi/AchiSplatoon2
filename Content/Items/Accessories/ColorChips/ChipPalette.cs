using AchiSplatoon2.Content.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ChipPalette : BaseAccessory
    {
        protected virtual int PaletteCapacity { get => 4; }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe altRecipe = CreateRecipe();
            altRecipe.AddIngredient(ItemID.CrimtaneBar, 8);
            altRecipe.AddTile(TileID.Anvils);
            altRecipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            if (modPlayer.isPaletteEquipped)
            {
                modPlayer.conflictingPalettes = true;
                return;
            }

            modPlayer.isPaletteEquipped = true;
            modPlayer.paletteCapacity = PaletteCapacity;

            // Note that disabling the buffs here doesn't disable ALL the buffs
            // See also the calculations in BaseProjectile.cs
            if (!modPlayer.IsPaletteValid()) return;

            var chips = modPlayer.ColorChipAmounts;
            player.GetDamage(DamageClass.Generic) +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Red] * modPlayer.RedChipBaseAttackDamageBonus;

            player.GetKnockback(DamageClass.Generic) +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Purple] * modPlayer.PurpleChipBaseKnockbackBonus;

            player.GetCritChance(DamageClass.Generic) +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Green] * modPlayer.GreenChipBaseCritBonus;

            player.moveSpeed +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Blue] * modPlayer.BlueChipBaseMoveSpeedBonus;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var textColorEffect = "c/ff8e2c:";
            var textColorWhite = "c/ffffff:";
            var textColorGray = "c/a8a8a8:";
            var textColorWarn = "c/ed3a4a:";
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            int index = tooltips.FindIndex(l => l.Name == "ItemName");
            if (index != -1)
            {
                var t = tooltips[index];
                if (modPlayer.isPaletteEquipped)
                {
                    if (modPlayer.conflictingPalettes)
                    {
                        t.Text = $"{Item.Name}" +
                            $"\n[{textColorWarn}ERROR! Palette inactive!]" +
                            $"\n[{textColorGray}You cannot equip multiple palettes at the same time.]";
                        return;
                    }

                    int chipCount = modPlayer.CalculateColorChipTotal();
                    if (modPlayer.DoesPlayerHaveTooManyChips())
                    {
                        t.Text = $"{Item.Name}" +
                            $"\n[{textColorWarn}ERROR! Palette inactive!]" +
                            $"\n[{textColorGray}Remove Color Chips from your inventory until you have {PaletteCapacity} or less.]" +
                            $"\n[{textColorGray}Currently you have {chipCount} Color Chips in your inventory.]";
                        return;
                    }

                    var chips = modPlayer.ColorChipAmounts;
                    var red = (float)chips[(int)InkWeaponPlayer.ChipColor.Red];
                    var blue = (float)chips[(int)InkWeaponPlayer.ChipColor.Blue];
                    var yellow = (float)chips[(int)InkWeaponPlayer.ChipColor.Yellow];
                    var purple = (float)chips[(int)InkWeaponPlayer.ChipColor.Purple];
                    var green = (float)chips[(int)InkWeaponPlayer.ChipColor.Green];
                    var aqua = (float)chips[(int)InkWeaponPlayer.ChipColor.Aqua];

                    t.Text = $"{Item.Name}" +
                        $"\n[{textColorEffect}Activates the effects of Color Chips]" +
                        $"\n[{textColorEffect}held in your inventory ({chipCount}/{PaletteCapacity})]" +
                        $"\n[c/ffffff:Currently active bonuses:]";
                    if (red > 0)
                    {
                        t.Text += $"\n[{textColorWhite}Power ({red}) >]" +
                            $"\n[{textColorGray}Damage: +{(int)(red * modPlayer.RedChipBaseAttackDamageBonus * 100)}%]" +
                            $"\n[{textColorGray}Armor penetration: {(int)(red * modPlayer.RedChipBaseArmorPierceBonus)} Defense]";
                    }
                    if (blue > 0)
                    {
                        t.Text += $"\n[{textColorWhite}Mobility ({blue}) >]" +
                            $"\n[{textColorGray}Move speed: +{(int)(blue * modPlayer.BlueChipBaseMoveSpeedBonus * 100)}%]" +
                            $"\n[{textColorGray}Special charge while moving: +{(int)(blue * modPlayer.BlueChipBaseChargeBonus * 100)}%]";
                    }
                    if (yellow > 0)
                    {
                        t.Text += $"\n[{textColorWhite}Range ({yellow}) >]" +
                            $"\n[{textColorGray}Explosion radius: +{(int)(yellow * modPlayer.YellowChipExplosionRadiusBonus * 100)}%]" +
                            $"\n[{textColorGray}Projectile piercing: +{(int)(yellow * modPlayer.YellowChipPiercingBonus)}]";
                    }
                    if (purple > 0)
                    {
                        t.Text += $"\n[{textColorWhite}Support ({purple}) >]" +
                            $"\n[{textColorGray}Knockback: +{purple * modPlayer.PurpleChipBaseKnockbackBonus} unit(s)]" +
                            $"\n[{textColorGray}Weapon charge speed: +{(int)(purple * modPlayer.PurpleChipBaseChargeSpeedBonus * 100)}%]";
                    }
                    if (green > 0)
                    {
                        t.Text += $"\n[{textColorWhite}Lucky ({green}) > Critical strike: +{(int)(green * modPlayer.GreenChipBaseCritBonus)}%]";
                    }
                }
                else
                {
                    t.Text = $"{Item.Name}" +
                        $"\n[{textColorEffect}When equipped, it activates the effects of Color Chips held in your inventory (up to {PaletteCapacity})]";
                }
            }
        }
    }
}
