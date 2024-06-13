using AchiSplatoon2.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AchiSplatoon2.Content.Items.Accessories.ColorChips
{
    internal class ChipPalette : BaseAccessory
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            modPlayer.isPaletteEquipped = true;

            var chips = modPlayer.ColorChipAmounts;
            player.GetAttackSpeed(DamageClass.Generic) +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Red] * modPlayer.RedChipBaseAttackSpeedBonus;

            player.GetKnockback(DamageClass.Generic) +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Purple] * modPlayer.PurpleChipBaseKnockbackBonus;

            player.GetCritChance(DamageClass.Generic) +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Green] * modPlayer.GreenChipBaseCritBonus;

            player.moveSpeed +=
                (float)chips[(int)InkWeaponPlayer.ChipColor.Blue] * modPlayer.BlueChipBaseMoveSpeedBonus;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<InkWeaponPlayer>();
            int index = tooltips.FindIndex(l => l.Name == "ItemName");
            if (index != -1)
            {
                if (modPlayer.isPaletteEquipped)
                {
                    var chips = modPlayer.ColorChipAmounts;
                    var red = (float)chips[(int)InkWeaponPlayer.ChipColor.Red];
                    var blue = (float)chips[(int)InkWeaponPlayer.ChipColor.Blue];
                    var yellow = (float)chips[(int)InkWeaponPlayer.ChipColor.Yellow];
                    var purple = (float)chips[(int)InkWeaponPlayer.ChipColor.Purple];
                    var green = (float)chips[(int)InkWeaponPlayer.ChipColor.Green];
                    var aqua = (float)chips[(int)InkWeaponPlayer.ChipColor.Aqua];

                    var t = tooltips[index];
                    t.Text = $"{Item.Name}" +
                        $"\n[c/ff8e2c:Activates the effects of Color Chips]" +
                        $"\n[c/ff8e2c:held in your inventory (up to 8)]" +
                        $"\n[c/ffffff:Currently active:]";
                    if (red > 0)
                    {
                        t.Text += $"\n[c/a8a8a8:Power ({red}) > Attack speed bonus: {(int)(red * modPlayer.RedChipBaseAttackSpeedBonus * 100)}%]";
                    }
                    if (blue > 0)
                    {
                        t.Text += $"\n[c/a8a8a8:Mobility ({blue}) > Move speed bonus: {(int)(blue * modPlayer.BlueChipBaseMoveSpeedBonus * 100)}%]";
                    }
                    if (yellow > 0)
                    {
                        t.Text += $"\n[c/a8a8a8:Range ({yellow}) > Explosion radius bonus: {(int)(yellow * modPlayer.YellowChipExplosionRadiusBonus * 100)}%]";
                    }
                    if (purple > 0)
                    {
                        t.Text += $"\n[c/a8a8a8:Support ({purple}) >]" +
                            $"\n[c/a8a8a8:Knockback bonus: {(int)(purple * modPlayer.PurpleChipBaseKnockbackBonus)} unit(s)]" +
                            $"\n[c/a8a8a8:Weapon charge speed bonus: {(int)(purple * modPlayer.PurpleChipBaseChargeSpeedBonus * 100)}%]";
                    }
                    if (green > 0)
                    {
                        t.Text += $"\n[c/a8a8a8:Lucky ({green}) > Critical strike bonus: {(int)(green * modPlayer.GreenChipBaseCritBonus)}%]";
                    }
                } else
                {
                    var t = tooltips[index];
                    t.Text = $"{Item.Name}" +
                        $"\n[c/ff8e2c:When worn, it activates the effects of Color Chips held in your inventory (up to 8)]" +
                        $"\n[c/a8a8a8:Currently inactive]";
                }
            }
        }
    }
}
